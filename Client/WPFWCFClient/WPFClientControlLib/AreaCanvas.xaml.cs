using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ArchorUDPTool.Models;
using DbModel.Engine;
using DbModel.Location.Settings;
using DbModel.Tools;
using IModel.Enums;
using Location.IModel;
using Location.TModel.Location.Alarm;
using TModel.Location.AreaAndDev;
using TModel.Location.Nodes;
using TModel.Tools;
using WPFClientControlLib.AreaCanvaItems;
//using AreaEntity= DbModel.Location.AreaAndDev.Area;
using AreaEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
//using DevEntity=DbModel.Location.AreaAndDev.DevInfo;
using DevEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using PersonEntity = Location.TModel.Location.Person.Personnel;
using WPFClientControlLib.Extensions;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for AreaCanvas.xaml
    /// </summary>
    public partial class AreaCanvas : UserControl
    {
        public Shape zeroPoint;
        public double Scale = 1;
        public double ZeroX;
        public double ZeroY;
        public double OffsetX;
        public double OffsetY;
        public int ViewMode { get; set; }
        public double DevSize { get; set; }

        public double CanvasMargin = 20;

        public bool ShowDev { get; set; }

        public bool ShowPerson { get; set; }

        public Shape SelectedRect { get; set; }
        public List<Shape> SelectedRects = new List<Shape>();

        public AreaEntity SelectedArea { get; set; }

        public List<AreaEntity> SelectedAreas = new List<AreaEntity>();

        public Rectangle SelectedDev { get; set; }

        public Dictionary<int, Shape> AreaDict = new Dictionary<int, Shape>();
        public Dictionary<int, DevShape> DevDict = new Dictionary<int, DevShape>();
        public Dictionary<int, Ellipse> PersonDict = new Dictionary<int, Ellipse>();

        private IList<PersonEntity> _persons;
        public List<PersonShape> PersonShapeList = new List<PersonShape>();

        public event Action<Rectangle, DevEntity> DevSelected;

        public ContextMenu DevContextMenu { get; set; }

        public ContextMenu AreaContextMenu { get; set; }

        public ContextMenu CanvasContextMenu { get; set; }

        public AreaCanvas()
        {
            InitializeComponent();
            if(CanvasContextMenu!=null)
                Canvas1.ContextMenu = CanvasContextMenu;
        }

        public void SelectArea<T>(T entity) where T :IEntity
        {
            if (entity == null) return;
            if (AreaDict.ContainsKey(entity.Id))
            {
                ClearSelect();
                FocusRectangle(AreaDict[entity.Id]);
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到区域:" + entity.Name;
            }
        }

        public void SelectAreas(List<AreaEntity> list)
        {
            if (list == null) return;
            ClearSelect();
            LbState.Content = "";
            SelectedRects.Clear();
            SelectedAreas.Clear();
            foreach (var entity in list)
            {
                if (AreaDict.ContainsKey(entity.Id))
                {
                    SelectedRects.Add(AreaDict[entity.Id]);
                    SelectedAreas.Add(entity);
                    SetFocusStyle(AreaDict[entity.Id]);
                    LbState.Content += entity.Name + ";";
                }
                else
                {
                    LbState.Content += "[" + entity.Name + "];";
                }
            }
        }

        public void SelectDevs(List<DevEntity> list)
        {
            if (list == null) return;
            ClearSelect();
            //LbState.Content = "";
            //SelectedRects.Clear();
            foreach (var entity in list)
            {
                if (DevDict.ContainsKey(entity.Id))
                {
                    SelectedRects.Add(DevDict[entity.Id].Rect);
                    SetFocusStyle(DevDict[entity.Id].Rect);
                    LbState.Content += entity.Name + ";";
                }
                else
                {
                    LbState.Content += "[" + entity.Name + "];";
                }
            }
        }

        public Rectangle GetDev(int id)
        {
            if (DevDict.ContainsKey(id))
            {
                return DevDict[id].Rect;
            }
            else
            {
                return null;
            }
        }

        public List<int> selectedDevs = new List<int>();

        public void SelectDevsById(List<int> list)
        {
            selectedDevs = list;
            if (list == null) return;
            ClearSelect();
            //LbState.Content = "";
            //SelectedRects.Clear();
            foreach (var id in list)
            {
                if (DevDict.ContainsKey(id))
                {
                    var shape = DevDict[id];
                    SelectedRects.Add(shape.Rect);
                    SetFocusStyle(shape.Rect);
                    ScrollViewer1.ScrollTo(shape.Rect);

                    shape.ReAdd();
                }
                else
                {
                    LbState.Content += "[" + id + "];";
                }
            }
        }

        public void SelectDevsByIdEx(List<int> list)
        {
            if (list == null) return;
            ClearSelect();
            foreach (var item in DevDict)
            {
                var shape = item.Value;
                var id = item.Key;
                if (list.Contains(id))
                {
                    SelectedRects.Add(shape.Rect);
                    SetFocusStyle(shape.Rect);
                }
                else
                {
                    shape.Remove();
                }
            }
        }

        public void SelectDev<T>(T entity) where T : IEntity
        {
            if (entity == null) return;
            if (DevDict.ContainsKey(entity.Id))
            {
                ClearSelect();
                FocusRectangle(DevDict[entity.Id].Rect);
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到设备:" + entity.Name;
            }
        }

        public void SelectDevById(int id) 
        {
            if (DevDict.ContainsKey(id))
            {
                ClearSelect();
                FocusRectangle(DevDict[id].Rect);
                ScrollViewer1.ScrollTo(DevDict[id].Rect);
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到设备:" + id;
            }
        }


        public void RemoveArea(int id)
        {
            if (AreaDict.ContainsKey(id))
            {
                var dev = AreaDict[id];
                Canvas1.Children.Remove(dev);
                AreaDict.Remove(id);
            }
        }

        public void RemoveDev(int id)
        {
            if (DevDict.ContainsKey(id))
            {
                var dev = DevDict[id];
                dev.Remove();
                DevDict.Remove(id);
            }
        }

        private void SetAllShapeStrokeDash(Shape rect)
        {
            foreach (var item in Canvas1.Children)
            {
                Shape shape = item as Shape;
                if (shape == null) continue;
                if (shape != rect)
                    SetShapeStrokeDash(shape);
            }
        }

        public void FocusRectangle(Shape rect)
        {
            SetAllShapeStrokeDash(rect);

            SelectedRect = rect;
            SetFocusStyle(rect);

            double left = Canvas.GetLeft(SelectedRect);
            double top= Canvas.GetTop(SelectedRect);
            //ScrollViewer1.ScrollToHorizontalOffset(100);
            //ScrollViewer1.ver
            //ScrollViewer1.ScrollToVerticalOffset(top);
        }


        private void SetFocusStyle(Shape rect)
        {
            rect.Stroke = Brushes.Red;
            rect.StrokeThickness = 2;
            rect.Focus();
        }

        private void InitCbScale(double scale)
        {
            CbScale.SelectionChanged -= CbScale_SelectionChanged;
            CbScale.SelectedItem = scale;
            CbScale.SelectionChanged += CbScale_SelectionChanged;
        }

        private void InitCbDevSize(double[] list,double item)
        {
            CbDevSize.SelectionChanged -= CbDevSize_SelectionChanged;
            CbDevSize.ItemsSource = list;
            CbDevSize.SelectedItem = item;
            CbDevSize.SelectionChanged += CbDevSize_SelectionChanged;
        }

        public AreaEntity CurrentArea;

        List<bus_anchor_switch_area> _switchAreas;

        public void ShowArea(AreaEntity area, List<bus_anchor_switch_area> switchAreas=null)
        {
            try
            {
                _switchAreas = switchAreas;
                CurrentArea = area;
                CbView.SelectionChanged -= CbView_OnSelectionChanged;
                CbView.SelectionChanged += CbView_OnSelectionChanged;
                if (area == null) return;
                if (area.IsPark()) //电厂
                {
                    SelectedArea = area;
                    double scale = 0;
                    DevSize = 0;
                    double[] devSizeList = new double[] {0.5, 1, 2, 3, 4, 5};
                    if (area.Name == "宝钢园区")
                    {
                        scale = 2;
                        floorIndex = 1;
                        CbFloor.SelectedIndex = 1;

                        devSizeList= new double[] { 1,2,4,8,16,32,64 };
                        DevSize = 16;
                    }

                    if (area.Name == "淄博电厂")
                    {
                        scale = 0.2;
                    }

                    Clear();
                    DrawPark(area, ref scale, DevSize);
                    InitCbScale(scale);
                    InitCbDevSize(devSizeList, DevSize);
                    //ShowPersons(area.Persons);
                    ShowSwitchArea(area, switchAreas, scale);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    //GetSettingFunc = null;
                    SelectedArea = area;
                    double scale = 0;
                    if (area.Name.Contains("燃料准备车间"))
                    {
                        scale = 2;
                    }

                    double[] devSizeList = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6 };
                    DevSize = 0.4;

                    //if (area.Name == "宝钢园区")
                    //{
                    //    //scale = 2;
                    //    //ShowFloor = 1;
                    //    //CbFloor.SelectedIndex = 1;

                    //    devSizeList = new double[] { 1,2,3,4,5,6,7,8,9,10 };
                    //    DevSize = 4;
                    //}
                    
                    DrawFloor(area,ref scale, DevSize);
                    InitCbScale(scale);
                    InitCbDevSize(devSizeList, DevSize);
                    //ShowPersons(area.Persons);
                }
                else if (area.Type == AreaTypes.分组)
                {
                    SelectAreas(area.Children);
                }
                else
                {
                    SelectArea(area);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ShowSwitchArea(AreaEntity area, List<bus_anchor_switch_area> switchAreas, double scale)
        {
            if (IsShowSwitchArea == false) return;
            if (switchAreas != null)
            {
                var subSwitchAreas = switchAreas;
                if (floorIndex == 1)//1层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z == 0 || i.min_z == 150);
                }
                else if (floorIndex == 2)//2层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z == 450 || i.min_z == 600);
                }
                else if (floorIndex == 3)//3层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z == 880);
                }
                else if (floorIndex == 4)//4层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z >880);
                }
                else
                {

                }
                foreach (var item in subSwitchAreas)
                {
                    AreaEntity switchArea = new AreaEntity();
                    float x1 = item.start_x / 100.0f + 2059;
                    float x2 = item.end_x / 100.0f + 2059;
                    float y1 = item.start_y / 100.0f + 1565;
                    float y2 = item.end_y / 100.0f + 1565;
                    switchArea.InitBound = new Location.TModel.Location.AreaAndDev.Bound(x1, y1, x2, y2, item.min_z, item.max_z, false);
                    switchArea.Parent = area;
                    switchArea.Name = item.area_id;
                    switchArea.Type = AreaTypes.SwitchArea;

                    AddAreaRect(switchArea, null, scale);
                }
            }
        }

        public void ClearSelect()
        {
            foreach (var item in Canvas1.Children)
            {
                Shape shape = item as Shape;
                if (shape == null) continue;
                shape.StrokeDashArray = null;
            }
            if (SelectedRect != null)
            {
                SelectedRect.Stroke = Brushes.Black;
                SelectedRect.StrokeThickness = 1;
                SelectedRect = null;
            }
            foreach (var shape in SelectedRects)
            {
                shape.Stroke = Brushes.Black;
                shape.StrokeThickness = 1;
            }

            LbState.Content = "";
            SelectedRects.Clear();
        }

        public Shape ShowPoint(double x, double y)
        {
            if (zeroPoint != null)
            {
                Canvas1.Children.Remove(zeroPoint);
            }
            zeroPoint=AddPoint(Scale,new Vector(x,y));
            return zeroPoint;
        }

        private void AddZeroPoint(double scale, Vector vec)
        {
            ZeroX = vec.X;
            ZeroY = vec.Y;
            /*
             * <Ellipse Canvas.Left="60" Canvas.Top="80" Width="100" Height="100"

　　Fill="Blue" Opacity="0.5" Stroke="Black" StrokeThickness="3"/>
             */
            double size = 20;
            var ellipse = new XYZero();
            ellipse.Tag = vec;
            Canvas1.Children.Add(ellipse);

            double left = (vec.X - OffsetX) * scale - size / 2;
            double top = (vec.Y - OffsetY) * scale - size / 2;

            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);

            ellipse.ToolTip = string.Format("坐标({0:F2},{1:F2})", vec.X, vec.Y);
        }

        private Ellipse AddPoint(double scale,Vector vec)
        {
            ZeroX = vec.X;
            ZeroY = vec.Y;

            double size = 10;
            Ellipse ellipse = new Ellipse();
            ellipse.Width = size;
            ellipse.Height = size;
            ellipse.Fill = Brushes.Transparent;
            ellipse.Stroke = Brushes.Red;
            ellipse.StrokeThickness = 2;
            ellipse.Tag = vec;
            Canvas1.Children.Add(ellipse);

            double left = (vec.X - OffsetX) * scale- size/2;
            double top = (vec.Y - OffsetY) * scale- size / 2;

            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);

            ellipse.ToolTip = string.Format("坐标({0:F2},{1:F2})", vec.X, vec.Y);

            return ellipse;
        }


        private Location.TModel.Location.AreaAndDev.Point GetOriginalPoint(System.Windows.Point point)
        {

            if (CurrentArea==null || CurrentArea.InitBound == null) return null;
            double x = point.X / Scale + OffsetX- CurrentArea.InitBound.GetZeroX();
            double y= point.Y / Scale + OffsetX - CurrentArea.InitBound.GetZeroY();
            return new Location.TModel.Location.AreaAndDev.Point((float)x, (float)y,0);
        }


        private void DrawFloor(AreaEntity area,ref double scale,double devSize)
        {
            Clear();
            var bound = area.InitBound;
            if (bound == null) return;
            if(bound.MaxX==0)
                bound.SetMinMaxXY();
            if (scale == 0)
            {
                scale = getAutoScale(bound);
            }

            Scale = scale;
            CanvasMargin = 10;
            OffsetX = -CanvasMargin/2;
            OffsetY = -CanvasMargin/2;
            Canvas1.Width = (bound.MaxX+ CanvasMargin) * scale ;
            Canvas1.Height = (bound.MaxY+ CanvasMargin) * scale;
            DrawMode = 2;
            AddAreaRect(area, null, scale);
            if (area.Children != null)
            {
                area.Children.Sort();
                foreach (var level1Item in area.Children) //机房
                {
                    if (IsShowAlarmArea==false&&level1Item.Type == AreaTypes.范围) continue;
                    level1Item.Parent = area;
                    AddAreaRect(level1Item, area, scale, true);

                    ShowDevs(level1Item.GetLeafNodes(), scale, devSize);//机房内设备
                }
            }

            ShowDevs(area.GetLeafNodes(), scale, devSize);//楼层内设备

            AddZeroPoint(scale,new Vector(0,0));
        }

        private void Clear()
        {
            Canvas1.Children.Clear();
            AreaDict.Clear();
            SelectedRect = null;
            SelectedRects.Clear();
            OffsetX = 0;
            OffsetY = 0;
            SelectedDev = null;
            SelectedAreas.Clear();
            SelectedArea = null;
        }

        private double getAutoScale(Location.TModel.Location.AreaAndDev.Bound bound)
        {
            double scaleX = 0;
            double sizeX = bound.GetSizeX();
            if (sizeX > 0)
            {
                scaleX = this.ActualWidth / sizeX;
            }
            double scaleY = 0;
            double sizeY = bound.GetSizeY();
            if (sizeY > 0)
            {
                scaleY = this.ActualHeight / sizeY;
            }
            double scale1 = scaleX < scaleY ? scaleX : scaleY;
            double scale = Math.Round(scale1, 2);//四舍五入
            if (scale == 0)
            {
                scale = Math.Round(scale1, 3);//四舍五入
            }
            if (scale == 0)
            {
                scale = Math.Round(scale1, 4);//四舍五入
            }
            if (scale == 0)
            {
                scale = Math.Round(scale1, 5);//四舍五入
            }
            /*
Math.Round(45.367,2)     //Returns   45.37
Math.Round(45.365,2)     //Returns   45.36
             */

            SetAutoScale(scale);
            return scale;
        }

        private void DrawPark(AreaEntity area,ref double scale,double devSize)
        {
            
            var bound = area.InitBound;
            //if (bound == null)
            //{
            //    bound=area.CreateBoundByChildren();
            //}
            if (bound == null) return;
            //bound=area.SetBoundByDevs();

            if (scale == 0)
            {
                scale = getAutoScale(bound);
              
            }

            Scale = scale;
            CanvasMargin = 20;
            OffsetX = bound.MinX - CanvasMargin;
            OffsetY = bound.MinY - CanvasMargin;
       

            Canvas1.Width = (bound.MaxX - OffsetX + CanvasMargin) * scale;
            Canvas1.Height =(bound.MaxY - OffsetY + CanvasMargin) *scale;

            DrawMode = 1;

            AddAreaRect(area,null, scale);

            if (area.Children != null)
                foreach (var level1Item in area.Children) //建筑群
                {
                    level1Item.Parent = area;
                    AddAreaRect(level1Item, area, scale);
                    if (level1Item.Children != null)
                        foreach (Location.TModel.Location.AreaAndDev.PhysicalTopology level2Item in level1Item.Children) //建筑
                        {
                            if (IsShowAlarmArea == false && level2Item.Type == AreaTypes.范围) continue;

                            level2Item.Parent = level1Item;
                            AddAreaRect(level2Item, level1Item, scale);

                            if (floorIndex > 0)
                            {
                                Location.TModel.Location.AreaAndDev.PhysicalTopology floor = null;
                                if (level2Item.Type == AreaTypes.楼层)
                                {
                                    floor = level2Item;
                                }
                                else
                                {
                                    floor = level2Item.GetChild(floorIndex - 1);//楼层
                                }
                                
                                if (floor != null)
                                {
                                    if(level2Item.InitBound!=null)
                                        AddAreaRect(floor, level2Item, scale);//楼层画出来

                                    if (floor.Children!=null)
                                        foreach (var room in floor.Children)//机房
                                        {
                                            if (room == null) continue;
                                            room.Parent = floor;
                                            AddAreaRect(room, floor, scale);
                                            ShowDevs(room.GetLeafNodes(), scale, devSize / 5);//机房内设备
                                        }

                                    ShowDevs(floor.GetLeafNodes(), scale, devSize/5);//楼层内设备
                                }
                            }
                        }
                }
            ShowDevs(area.GetLeafNodes(), scale, devSize);
            AddZeroPoint(scale,new Vector(bound.GetZeroX(), bound.GetZeroY()));
        }

        public void ShowLocationAlarms(LocationAlarm[] alarms)
        {
            foreach (var item in alarms)
            {
                var personId = item.PersonnelId;
                
                if (PersonDict.ContainsKey(personId))
                {
                    var person = PersonDict[personId];
                    person.Fill = Brushes.Red;
                }

                var areaId = item.AreaId;
                if (AreaDict.ContainsKey(areaId))
                {
                    var area = AreaDict[areaId];
                    area.Fill = Brushes.Red;
                }
            }
        }

        /// <summary>
        /// 基站TypeCode
        /// </summary>
        private int ArchorTypeCode = TypeCodes.Archor;

        private Rectangle AddDevRectEx(DevEntity dev, double scale, double devSize)
        {
            if (dev == null) return null;
            if (dev.TypeCode == ArchorTypeCode)
            {
                if (IsShowAnchor)
                {
                    return AddDevRect(dev, scale, devSize, ShowDevName);
                }
            }
            else if (dev.TypeCode == TypeCodes.TrackPoint)
            {
                if (IsShowTrackPoint)
                {
                    return AddDevRect(dev, scale, devSize * 0.5, false);
                }
            }
            else if (dev.TypeCode == TypeCodes.Camera)
            {
                if (IsShowCamera)
                {
                    return AddDevRect(dev, scale, devSize * 0.8, false);
                }
            }
            else
            {
                if (IsShowOtherDev)
                {
                    return AddDevRect(dev, scale, devSize * 1.5, false);
                }
            }
            return null;
        }

        private void ShowDevs(List<DevEntity> devs, double scale, double devSize)
        {
            if (ShowDev)
                if (devs != null)
                {
                    int count = 0;
                    foreach (var dev in devs)
                    {
                        if (dev == null) continue;
                        if (dev.TypeCode == ArchorTypeCode)
                        {
                            if (IsShowAnchor)
                            {
                                count++;
                            }
                        }
                        AddDevRectEx(dev, scale, devSize);
                    }
                    
                    LbState.Content = "基站设备数量:" + count;
                }
        }

        public Func<DevEntity, ArchorSetting> GetSettingFunc;

        private Rectangle AddDevRect(DevEntity dev,double scale, double size,bool showDevName)
        {
            if (DevDict.ContainsKey(dev.Id))
            {
                DevDict[dev.Id].Remove();
            }

            AreaEntity parent = dev.Parent;

            double roomOffX = 0;
            double roomOffY = 0;
            GetRoomOffXY(parent, ref roomOffX, ref roomOffY);

            double ax = dev.Pos.PosX + roomOffX;
            double ay = dev.Pos.PosZ + roomOffY;

            if (DrawMode == 1 && GetSettingFunc != null)//大图模式
            {
                ArchorSetting setting = GetSettingFunc(dev);
                if (setting != null)
                {
                    setting.CalAbsolute();//检测配置的数据是否正确
                    ax = setting.AbsoluteX.ToDouble();
                    ay = setting.AbsoluteY.ToDouble();
                }
            }


            double x = (ax - OffsetX) * scale - size * scale / 2;
            double y = (ay - OffsetY) * scale - size * scale / 2;

            DevShape devShape = new DevShape(Canvas1);
            if (showDevName)
            {
                if (udpArchorList == null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
                    udpArchorList = XmlSerializeHelper.LoadFromFile<UDPArchorList>(path);
                }
                Label lb = new Label();
                lb.Content = GetDevName(dev);
                Canvas.SetLeft(lb, x + size * scale);
                Canvas.SetTop(lb, y);
                Canvas1.Children.Add(lb);
                lb.LayoutTransform = ScaleTransform1;

                var udpArchor = udpArchorList.Find(i => i.Id == GetArchorCode(dev));
                if (udpArchor != null)
                {
                    lb.Foreground = Brushes.Blue;
                }

                devShape.Label = lb;
            }


            //if (ViewMode == 0)
            //    y = Canvas1.Height - size * scale - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
            Rectangle devRect = new Rectangle()
            {
                //Margin = new Thickness(x, y, 0, 0),
                Width = size * scale,
                Height = size * scale,
                Fill = GetDevRectFillColor(dev),
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = dev,
                ToolTip = GetDevNameEx(dev)
            };

            devShape.Rect = devRect;
            devShape.Id = dev.Id;

            devRect.ContextMenu = DevContextMenu;

            Canvas.SetLeft(devRect, x);
            Canvas.SetTop(devRect, y);

            DevDict[dev.Id] = devShape;
            devRect.MouseDown += DevRect_MouseDown;
            devRect.MouseEnter += DevRect_MouseEnter;
            devRect.MouseLeave += DevRect_MouseLeave;
            Canvas1.Children.Add(devRect);

            return devRect;
        }

        private void GetRoomOffXY(AreaEntity parent, ref double roomOffX, ref double roomOffY)
        {
            if (DrawMode == 1)//大图模式
            {
                if (parent != null)
                {
                    if (parent.Type == AreaTypes.楼层 && parent.Parent != null)
                    {
                        roomOffX = parent.InitBound.GetZeroX() + parent.Parent.InitBound.GetZeroX();
                        roomOffY = parent.InitBound.GetZeroY() + parent.Parent.InitBound.GetZeroY();
                    }
                    else if (parent.Type == AreaTypes.机房 && parent.Parent != null)
                    {
                        roomOffX = parent.Parent.Parent.InitBound.GetZeroX() + parent.Parent.InitBound.GetZeroX();
                        roomOffY = parent.Parent.Parent.InitBound.GetZeroY() + parent.Parent.InitBound.GetZeroY();
                    }
                }
            }
            else //小图模式
            {
                if (parent != null)
                {
                    if (parent.Type == AreaTypes.楼层)
                    {
                        roomOffX = parent.InitBound.GetZeroX();
                        roomOffY = parent.InitBound.GetZeroY();
                    }
                    else if (parent.Type == AreaTypes.机房)
                    {
                        roomOffX = parent.Parent.InitBound.GetZeroX();
                        roomOffY = parent.Parent.InitBound.GetZeroY();
                    }
                }
            }
        }

        public UDPArchorList udpArchorList;

        private void DevRect_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            if (SelectedDev == rect) return;
            var dev = rect.Tag as DevEntity;
            LbState.Content = "";

            rect.Fill = GetDevRectFillColor(dev);
            rect.Stroke = Brushes.Black;
        }

        private Brush GetDevRectFillColor(DevEntity dev)
        {
            if (dev.DevDetail is Archor)
            {
                if (GetSettingFunc != null)
                {
                    ArchorSetting setting = GetSettingFunc(dev);
                    if (setting != null )
                    {
                        if(setting.RelativeHeight == 2)
                        {
                            return Brushes.LightGreen;
                        }
                        else
                        {
                            return Brushes.Green;
                        }
                        
                    }
                    else
                    {
                        return Brushes.DeepSkyBlue;
                    }
                }
                else
                {
                    return Brushes.DeepSkyBlue;
                }

                //Archor archor = dev.DevDetail as Archor;
                //if (!string.IsNullOrEmpty(archor.Code) && !archor.Code.StartsWith("Code"))
                //{
                //    return Brushes.Green;
                //}
                //else
                //{
                //    return Brushes.DeepSkyBlue;
                //}
            }
            else
            {
                return Brushes.CadetBlue;
            }
        }

        private string GetDevNameEx(DevEntity dev)
        {
            if (dev.DevDetail is Archor)
            {
                Archor archor = dev.DevDetail as Archor;
                return archor.Name+"("+archor.Code + "|" + archor.Ip+")";
                //return archor.Code;// + "|" + archor.Ip;
            }
            else
            {
                return dev.Name;
            }
        }

        private string GetDevName(DevEntity dev)
        {
            if (dev.DevDetail is Archor)
            {
                Archor archor = dev.DevDetail as Archor;
                //return archor.Name+"("+archor.Code + "|" + archor.Ip+")";
                return archor.Code;// + "|" + archor.Ip;
            }
            else
            {
                return dev.Name;
            }
        }

        private string GetArchorCode(DevEntity dev)
        {
            if (dev.DevDetail is Archor)
            {
                Archor archor = dev.DevDetail as Archor;
                if (archor.Code == null)
                {
                    return "NULL";
                }
                if (archor.Code.Contains("Code"))
                {
                    return "[C]";
                }
                return archor.Code;
            }
            else
            {
                return dev.Name;
            }
        }

        private void DevRect_MouseEnter(object sender, MouseEventArgs e)
        {
            //SelectRectangle(sender as Rectangle);
            Rectangle rect = sender as Rectangle;
            SelectDev(rect);
        }

        private void SelectDev(Rectangle rect)
        {
            var dev = rect.Tag as DevEntity;
            LbState.Content = GetDevText(dev);

            rect.Fill = Brushes.Blue;
            rect.Stroke = Brushes.Red;
        }

        private string GetDevText(DevEntity dev)
        {
            return string.Format("[{0}]({1},{2})", dev.Name, dev.Pos.PosX, dev.Pos.PosZ); 
        }

        private void DevRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            if (SelectedDev != null && SelectedDev != rect)
            {
                var dev = rect.Tag as DevEntity;
                SelectedDev.Fill = GetDevRectFillColor(dev);
                SelectedDev.Stroke = Brushes.Black;
            }
            var dev2 = rect.Tag as DevEntity;
            LbState.Content = GetDevText(dev2);
            SelectedDev = rect;

            if (DevSelected != null)
            {
                DevSelected(rect, dev2);
            }
        }



        private void AddAreaRect(AreaEntity area, AreaEntity parent, double scale = 1, bool isTransparent = false)
        {
            if (area == null) return;
            var bound = area.InitBound;
            if (bound == null) return;
            {
                if (parent == null)
                {
                    parent = area.Parent;
                }

                double roomOffX = 0;
                double roomOffY = 0;
                SetAreaRoomOffset(area, parent, ref roomOffX, ref roomOffY);
                List<Point> points = GetPoints(scale, bound, roomOffX, roomOffY);

                Shape areaShape = null;
                if (bound.Shape == 2)//圆形
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = bound.GetSizeX() * scale;
                    ellipse.Height = bound.GetSizeY() * scale;

                    double left = (bound.GetCenterX() - OffsetX - roomOffX - bound.GetSizeX() / 2) * scale;
                    double top = (bound.GetCenterY() - OffsetY - roomOffY - bound.GetSizeY() / 2) * scale;

                    Canvas.SetLeft(ellipse, left);
                    Canvas.SetTop(ellipse, top);

                    areaShape = ellipse;
                }
                else
                {
                    Polygon polygon = new Polygon();
                    foreach (var item in points)
                    {
                        polygon.Points.Add(item);
                    }
                    areaShape = polygon;
                }

                AreaDict[area.Id] = areaShape;

                Canvas1.Children.Add(areaShape);
                SetAreaStyle(area, areaShape);
                areaShape.Tag = area;
                areaShape.ToolTip = area.Name;
                areaShape.ContextMenu = AreaContextMenu;

                areaShape.MouseUp += Area_MouseDown;
                areaShape.MouseEnter += Area_MouseEnter;
                areaShape.MouseLeave += Area_MouseLeave;

                double mX = 0;
                double mY = 0;
                int c = 0;
                foreach (var item in points)
                {
                    mX += item.X;
                    mY += item.Y;
                    c++;
                }
                mX /= c;
                mY /= c;
                if (area.Type==AreaTypes.大楼)
                {
                    ShowAreaName(area, mX, mY,Brushes.Green,30);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    ShowAreaName(area, mX, mY, Brushes.Gray, 25);
                }
                else if (area.Type == AreaTypes.机房)
                {
                    ShowAreaName(area, mX, mY, Brushes.Black, 14);
                }
                else
                {
                    ShowAreaName(area, mX, mY);
                }
                
            }
        }

        private List<Point> GetPoints(double scale, Location.TModel.Location.AreaAndDev.Bound bound, double roomOffX, double roomOffY)
        {
            List<Point> points = new List<Point>();
            foreach (var item in bound.GetPoints2D())
            {
                double x = (item.X - OffsetX + roomOffX) * scale;
                double y = (item.Y - OffsetY + roomOffY) * scale;
                points.Add(new Point(x, y));
            }

            return points;
        }

        private Label ShowAreaName(AreaEntity area, double mX, double mY)
        {
            return ShowAreaName(area, mX, mY, Brushes.Gray,0 );
        }

        private Label ShowAreaName(AreaEntity area, double mX, double mY, Brush color,double size)
        {
            if (IsShowAreaName && area.Type != AreaTypes.CAD && !string.IsNullOrEmpty(area.Name))
            {
                Label lb = new Label();
                if(size>0)
                    lb.FontSize = size;
                lb.Content = area.Name;
                var ft = MeasureText(area.Name, lb.FontSize, lb.FontFamily.ToString());
                var w = ft.WidthIncludingTrailingWhitespace;
                var h = ft.Height;
                Canvas.SetLeft(lb, mX - w / 2);
                Canvas.SetTop(lb, mY - h / 2);
                Canvas1.Children.Add(lb);
                lb.Foreground = color;
                lb.LayoutTransform = ScaleTransform1;
                lb.Focusable = false;
                return lb;
            }

            return null;
        }

        private void SetAreaRoomOffset(AreaEntity area, AreaEntity parent, ref double roomOffX, ref double roomOffY)
        {
            if (DrawMode == 1)//大图模式
            {
                if (parent != null && parent.Type == AreaTypes.楼层 && parent.Parent != null)//当前是机房
                {
                    if (parent.InitBound.IsRelative)
                    {
                        roomOffX = parent.InitBound.GetZeroX()+ parent.Parent.InitBound.GetZeroX();
                        roomOffY = parent.InitBound.GetZeroY() + parent.Parent.InitBound.GetZeroY();
                    }
                }
                if (area.Type == AreaTypes.楼层 && parent != null && parent.InitBound != null)//当前是楼层
                {
                    if (area.InitBound.IsRelative)
                    {
                        roomOffX = /*area.InitBound.GetZeroX() + */ parent.InitBound.GetZeroX();
                        roomOffY = /*area.InitBound.GetZeroY() +*/ parent.InitBound.GetZeroY();
                    } 
                }
            }
            else//小图模式
            {
                if (parent != null && parent.Type == AreaTypes.楼层)//当前是机房
                {
                    if (area.InitBound.IsRelative)
                    {
                        roomOffX = parent.InitBound.GetZeroX();
                        roomOffY = parent.InitBound.GetZeroY();
                    }
                }
            }
        }

        private void SetAreaStyle(AreaEntity area, Shape shape)
        {
            if (area.Type == AreaTypes.CAD)
            {
                if (area.Name.StartsWith("Block") || area.Name == "Polyline")
                {
                    var brush = new SolidColorBrush(Color.FromArgb(128, 80, 80, 80));
                    shape.Fill = brush;
                    shape.Stroke = Brushes.Gray;
                }
                else if (area.Name == "Line")
                {
                    shape.Fill = Brushes.Transparent;
                    shape.Stroke = Brushes.Gray;
                }
                else
                {
                    shape.Fill = Brushes.Transparent;
                    shape.Stroke = Brushes.Gray;
                }
            }
            else if (area.Type == AreaTypes.SwitchArea)
            {
                shape.Fill = Brushes.Transparent;
                shape.Stroke = Brushes.Green;
            }
            else if (area.Type == AreaTypes.大楼)
            {
                shape.Fill = Brushes.AliceBlue;
                shape.Stroke = Brushes.Blue;
            }
            else if(area.Type == AreaTypes.范围)
            {
                shape.Fill = Brushes.Transparent;
                //SetShapeStrokeDash(polygon);
                shape.Stroke = Brushes.Orange;
            }
            else
            {
                shape.Fill = Brushes.Transparent;
                shape.Stroke = Brushes.Black;
            }

            shape.StrokeThickness = 1;
        }

        private FormattedText MeasureText(string text, double fontSize, string fontFamily)
        {
            FormattedText formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface(fontFamily.ToString()),
            fontSize,
            Brushes.Black
            );
            return formattedText;
        }

        private void Area_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var shape=sender as Shape;
            if (SelectedRect != null && SelectedRect != shape)
            {
                UnSelectRectangle(SelectedRect);
            }
            SelectedRect = shape;
            SelectRectangle(shape);
        }

        private void SetShapeStrokeDash(Shape shape)
        {
            shape.StrokeDashArray = new DoubleCollection() { 2, 3 };
            shape.StrokeDashCap = PenLineCap.Triangle;
            shape.StrokeEndLineCap = PenLineCap.Square;
            shape.StrokeStartLineCap = PenLineCap.Round;
        }

        private void Area_MouseLeave(object sender, MouseEventArgs e)
        {
            var shape = sender as Shape;
            var area = shape.Tag as AreaEntity;
            if (area == null) return;

            if (SelectedRect != shape)
            {
                //UnSelectRectangle(shape);
                SetAreaStyle(area, shape);
            }
        }

        private void Area_MouseEnter(object sender, MouseEventArgs e)
        {
            //SelectRectangle(sender as Shape);

            Shape rect = sender as Shape;
            rect.Stroke = Brushes.Red;
            rect.StrokeThickness = 2;
        }

        private void SelectRectangle(Shape rect)
        {
            SelectedArea = rect.Tag as AreaEntity;
            if (SelectedArea == null) return;
            LbState.Content = "" + SelectedArea.Name;
            rect.Stroke = Brushes.Red;
            rect.StrokeThickness = 2;
        }

        private void UnSelectRectangle(Shape rect)
        {
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1;
            //SelectedRect = null;
        }

        private void CbScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            if (CbScale.SelectedItem == null) return;//还没初始化
            try
            {
                var scale = (double)CbScale.SelectedItem;
                var area = CurrentArea;
                if (area == null) return;
                if (area.IsPark()) //电厂
                {
                    Clear();
                    
                    DrawPark(area, ref scale, DevSize);

                    ShowSwitchArea(area, _switchAreas, scale);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    DrawFloor(area, ref scale, DevSize);
                }
                else
                {
                    SelectArea(area);
                }

                ShowPersons();

                SelectDevsById(selectedDevs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        int DrawMode = 1;

        public void RefreshDev(DevEntity dev)
        {
            var scale = (double)CbScale.SelectedItem;

            //var rect=AddDevRect(dev, scale, DevSize,ShowDevName);
            var rect=AddDevRectEx(dev, scale, DevSize);
            SelectDev(rect);
        }

        private void CbDevSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DevSize = (double)CbDevSize.SelectedItem;
            
            RefreshDevs();
        }

        private void RefreshDevs()
        {
            Refresh();
        }

        private void AreaCanvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private List<double> scaleList= new List<double>()
        {0,
            0.01,0.02,0.04,0.08,
            0.1, 0.2, 0.4, 0.8,
            1, 2, 4, 8,
            10, 20, 40, 60, 80};

        public void Init()
        {
            InitScale();
        }

        private void InitScale()
        {
            CbScale.ItemsSource = scaleList;
            CbScale.SelectedIndex = 0;
        }

        private double autoScale = 0;

        private void SetAutoScale(double newScale)
        {
            if (autoScale != 0)
            {
                scaleList.Remove(autoScale);
            }

            autoScale = newScale;
            scaleList.Add(autoScale);
            scaleList.Sort();
            CbScale.ItemsSource = scaleList;
            CbScale.SelectedItem = newScale;
        }

        public void ShowDevs(DevEntity[] devs)
        {
            
        }


        public void ShowPersons()
        {
            ShowPersons(_persons);
        }

        public void ShowPersons(IList<PersonEntity> persons)
        {
            PersonShapeList.Clear();
            _persons = persons;//这个要放到前面

            if (IsShowPerson == false) return;

           
            if (persons == null) return;
            foreach (var person in persons)
            {
                PersonShape ps = AddPersonRect(person, Scale, 2);
                PersonShapeList.Add(ps);
            }
        }

        public void ShowPersons(IList<PersonNode> persons)
        {
            PersonShapeList.Clear();
            //_persons = persons;
            if (persons == null) return;
            foreach (var person in persons)
            {
                PersonShape ps = AddPersonRect(person, Scale, 2);
                PersonShapeList.Add(ps);
            }
        }

        private PersonShape AddPersonRect(PersonNode person, double scale, double size = 2)
        {
            PersonShape ps = new PersonShape(this, person.Id, person.Name, person.Tag.Pos, scale, size);
            ps.Moved += Ps_Moved;
            ps.Show();
            return ps;
        }

        private PersonShape AddPersonRect(PersonEntity person, double scale, double size = 2)
        {
            PersonShape ps = new PersonShape(this,person.Id,person.Name,person.Pos, scale, size);
            ps.Moved += Ps_Moved;
            ps.Show();
            return ps;
        }

        private void Ps_Moved(PersonShape obj)
        {
            
        }

        public void RemovePerson(int id)
        {
            if (PersonDict.ContainsKey(id))
            {
                Canvas1.Children.Remove(PersonDict[id]);
            }
        }

        public void AddPerson(int id, Ellipse personShape)
        {
            PersonDict[id] = personShape;
            Canvas1.Children.Add(personShape);
        }

        private void CbView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode = CbView.SelectedIndex;
            if (ViewMode == 0)
            {
                ScaleTransform1.ScaleY = -1;
            }
            else
            {
                ScaleTransform1.ScaleY = 1;
            }
            Refresh();
        }

        private void CbAreaName_Checked(object sender, RoutedEventArgs e)
        {
            IsShowAreaName = (bool)CbAreaName.IsChecked;
            Refresh();
        }

        public bool IsShowAreaName = true;

        private void CbAreaName_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowAreaName = (bool)CbAreaName.IsChecked;
            Refresh();
        }

        public bool ShowDevName = true;

        private void CbDevName_Checked(object sender, RoutedEventArgs e)
        {
            ShowDevName = (bool)CbDevName.IsChecked;
            Refresh();
        }

        private void CbDevName_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowDevName = (bool)CbDevName.IsChecked;
            Refresh();
        }

        private void CbFloor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (floorIndex == CbFloor.SelectedIndex) return;
            floorIndex = CbFloor.SelectedIndex;
            Refresh();
        }

        private int floorIndex;//当前显示的楼层

        private void MenuSaveImage_Click(object sender, RoutedEventArgs e)
        {
            string name = CurrentArea.Name;
            if (CurrentArea.IsPark())
            {
                if (CbFloor.SelectedIndex > 0)
                {
                    name += "(" + CbFloor.Text + ")";
                }
            }
            else
            {

            }
            SaveToImage(Canvas1, name);
        }

        private void SaveToImage(Canvas canvas,string fileName)
        {
            string path = string.Format("{0}\\Data\\Images\\{1}.png", AppDomain.CurrentDomain.BaseDirectory, fileName);
            //保存到特定路径
            FileStream fs = new FileStream(path, FileMode.Create);
            //对象转换成位图
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)(canvas.ActualWidth*1.1), (int)(canvas.ActualHeight * 1.1), 100, 100, PixelFormats.Pbgra32);
            bmp.Render(canvas);
            //对象的集合编码转成图像流
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            //保存到路径中
            encoder.Save(fs);
            //释放资源
            fs.Close();
            fs.Dispose();
        }

        public bool IsShowPerson = false;

        private void CbShowPerson_Checked(object sender, RoutedEventArgs e)
        {
            IsShowPerson = (bool)CbShowPerson.IsChecked;
            Refresh();
        }

        private void CbShowPerson_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowPerson = (bool)CbShowPerson.IsChecked;
            Refresh();
        }

        public bool IsShowSwitchArea = false;

        private void CbShowSwitchArea_Checked(object sender, RoutedEventArgs e)
        {
            IsShowSwitchArea = (bool)CbShowSwitchArea.IsChecked;
            Refresh();
        }

        private void CbShowSwitchArea_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowSwitchArea = (bool)CbShowSwitchArea.IsChecked;
            Refresh();
        }

        private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentArea == null) return;
            var p1 = e.GetPosition(Canvas1);
            SelectedPoint1 = p1;
            //Ellipse p = new Ellipse()
            //{
            //    Margin = new Thickness(p1.X, p1.Y, 0, 0),
            //    Width = 2,
            //    Height = 2,
            //    Fill = Brushes.Red,
            //    Stroke = Brushes.Black,
            //    StrokeThickness = 1,
            //    ToolTip = Name
            //};
            //Canvas1.Children.Add(p);
            var p2=GetOriginalPoint(p1);
            if (CurrentArea.InitBound!=null )
            {
                var p3 = CurrentArea.InitBound.GetLeftBottomPoint();
            }
          
            //SelectedPoint2 = new Location.TModel.Location.AreaAndDev.Point(p3.X - p2.X, p3.Y - p2.Y, 0);
            SelectedPoint2 = p2;
        }

        public System.Windows.Point SelectedPoint1;

        public Location.TModel.Location.AreaAndDev.Point SelectedPoint2;

        public bool IsShowAnchor = true;

        private void CbDevAnchor_Checked(object sender, RoutedEventArgs e)
        {
            IsShowAnchor = (bool)CbDevAnchor.IsChecked;
            Refresh();
        }

        private void CbDevAnchor_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowAnchor = (bool)CbDevAnchor.IsChecked;
            Refresh();
        }

        public bool IsShowOtherDev = false;

        private void CbDevOther_Checked(object sender, RoutedEventArgs e)
        {
            IsShowOtherDev = (bool)CbDevOther.IsChecked;
            Refresh();
        }

        private void CbDevOther_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowOtherDev = (bool)CbDevOther.IsChecked;
            Refresh();
        }

        public bool IsShowTrackPoint = true;

        private void CbDevTrackPoint_Checked(object sender, RoutedEventArgs e)
        {
            IsShowTrackPoint = (bool)CbDevTrackPoint.IsChecked;
            Refresh();
        }

        private void CbDevTrackPoint_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowTrackPoint = (bool)CbDevTrackPoint.IsChecked;
            Refresh();
        }

        bool IsShowCamera = true;

        private void CbDevCamera_Checked(object sender, RoutedEventArgs e)
        {
            IsShowCamera = (bool)CbDevCamera.IsChecked;
            Refresh();
        }

        private void CbDevCamera_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowCamera = (bool)CbDevCamera.IsChecked;
            Refresh();
        }

        private void Canvas1_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        bool IsShowAlarmArea = false;

        private void CbShowAlarmArea_Checked(object sender, RoutedEventArgs e)
        {
            IsShowAlarmArea = (bool)CbShowAlarmArea.IsChecked;
            Refresh();
        }

        private void CbShowAlarmArea_Unchecked(object sender, RoutedEventArgs e)
        {
            IsShowAlarmArea = (bool)CbShowAlarmArea.IsChecked;
            Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using DbModel.Tools;
using Location.IModel;
using Location.TModel.Location.Alarm;
using TModel.Location.Nodes;
using WPFClientControlLib.AreaCanvaItems;
//using AreaEntity= DbModel.Location.AreaAndDev.Area;
using AreaEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
//using DevEntity=DbModel.Location.AreaAndDev.DevInfo;
using DevEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using PersonEntity = Location.TModel.Location.Person.Personnel;

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
        public Dictionary<int, Rectangle> DevDict = new Dictionary<int, Rectangle>();
        public Dictionary<int, Ellipse> PersonDict = new Dictionary<int, Ellipse>();

        private IList<PersonEntity> _persons;
        public List<PersonShape> PersonShapeList = new List<PersonShape>();

        public event Action<Rectangle, DevEntity> DevSelected;

        public ContextMenu DevContextMenu { get; set; }

        public ContextMenu AreaContextMenu { get; set; }

        public AreaCanvas()
        {
            InitializeComponent();
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

        public void SelectDev<T>(T entity) where T : IEntity
        {
            if (entity == null) return;
            if (DevDict.ContainsKey(entity.Id))
            {
                ClearSelect();
                FocusRectangle(DevDict[entity.Id]);
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到设备:" + entity.Name;
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

            ScrollViewer1.ScrollToHorizontalOffset(SelectedRect.Margin.Left);
            ScrollViewer1.ScrollToVerticalOffset(SelectedRect.Margin.Top);
        }

        private void SetFocusStyle(Shape rect)
        {
            rect.Stroke = Brushes.Red;
            rect.StrokeThickness = 2;
            rect.Focus();
        }

        private void InitCbScale(int scale)
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

        public void ShowArea(AreaEntity area)
        {
            try
            {
                CbView.SelectionChanged -= CbView_OnSelectionChanged;
                CbView.SelectionChanged += CbView_OnSelectionChanged;
                if (area == null) return;
                if (area.IsPark()) //电厂
                {
                    SelectedArea = area;
                    int scale = 3;
                    DevSize = 3;
                    DrawPark(area, scale, DevSize);
                    InitCbScale(scale);
                    InitCbDevSize(new double[] { 0.5, 1, 2, 3,4,5 }, DevSize);
                    //ShowPersons(area.Persons);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    SelectedArea = area;
                    int scale = 20;
                    DevSize = 0.3;
                    DrawFloor(area, scale, DevSize);
                    InitCbScale(scale);
                    InitCbDevSize(new double[] { 0.1, 0.2, 0.3, 0.4, 0.5,0.6 }, DevSize);
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

        private void ClearSelect()
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
            }
            foreach (var shape in SelectedRects)
            {
                shape.Stroke = Brushes.Black;
                shape.StrokeThickness = 1;
            }
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
            /*
             * <Ellipse Canvas.Left="60" Canvas.Top="80" Width="100" Height="100"

　　Fill="Blue" Opacity="0.5" Stroke="Black" StrokeThickness="3"/>
             */
            double size = 10;
            Ellipse ellipse = new Ellipse();
            //ellipse.Margin = new Thickness(Margin/2, Margin/2, 0, 0);
            ellipse.Width = size;
            ellipse.Height = size;
            ellipse.Fill = Brushes.Transparent;
            ellipse.Stroke = Brushes.Red;
            ellipse.StrokeThickness = 2;
            //SetShapeStrokeDash(ellipse);
            ellipse.Tag = vec;
            Canvas1.Children.Add(ellipse);

            double left = (vec.X - OffsetX) * scale- size/2;
            double top = (vec.Y - OffsetY) * scale- size / 2;

            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);

            ellipse.ToolTip = string.Format("坐标({0:F2},{1:F2})", vec.X, vec.Y);

            return ellipse;
        }

        private void DrawFloor(AreaEntity area,double scale,double devSize)
        {
            Clear();
            var bound = area.InitBound;
            if (bound == null) return;
            Scale = scale;
            CanvasMargin = 10;
            OffsetX = -CanvasMargin/2;
            OffsetY = -CanvasMargin/2;
            Canvas1.Width = (bound.MaxX+ CanvasMargin) * scale ;
            Canvas1.Height = (bound.MaxY+ CanvasMargin) * scale;
            AddAreaRect(area, null, scale);
            if (area.Children != null)
                foreach (var level1Item in area.Children) //机房
                {
                    AddAreaRect(level1Item, null, scale, true);
                }

            ShowDevs(area.LeafNodes, scale, devSize);

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

        private void DrawPark(AreaEntity area,int scale,double devSize)
        {
            Clear();
            var bound = area.InitBound;
            //if (bound == null)
            //{
            //    bound=area.CreateBoundByChildren();
            //}
            if (bound == null) return;
            //bound=area.SetBoundByDevs();
            Scale = scale;
            CanvasMargin = 20;
            OffsetX = bound.MinX - CanvasMargin;
            OffsetY = bound.MinY - CanvasMargin;
            Canvas1.Width = (bound.MaxX - OffsetX + CanvasMargin) * scale;
            Canvas1.Height =(bound.MaxY - OffsetY + CanvasMargin) *scale;

            AddAreaRect(area,null, scale);

            if (area.Children != null)
                foreach (var level1Item in area.Children) //建筑群
                {
                    AddAreaRect(level1Item, area, scale);
                    if (level1Item.Children != null)
                        foreach (var level2Item in level1Item.Children) //建筑
                        {
                            AddAreaRect(level2Item, level1Item, scale);
                        }
                }

            ShowDevs(area.LeafNodes, scale, devSize);

            AddZeroPoint(scale,new Vector(bound.MinX, bound.MinY));
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

        private void ShowDevs(List<DevEntity> devs, double scale, double devSize)
        {
            if (ShowDev)
                if (devs != null)
                    foreach (var dev in devs)
                    {
                        AddDevRect(dev, scale, devSize);
                    }
        }

        private Rectangle AddDevRect(DevEntity dev,double scale, double size = 2)
        {
            if (DevDict.ContainsKey(dev.Id))
            {
                Canvas1.Children.Remove(DevDict[dev.Id]);
            }

            double x = (dev.Pos.PosX - OffsetX) * scale-size*scale/2;
            double y = (dev.Pos.PosZ - OffsetY) * scale - size * scale / 2;
            //if (ViewMode == 0)
            //    y = Canvas1.Height - size * scale - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
            Rectangle devRect = new Rectangle()
            {
                //Margin = new Thickness(x, y, 0, 0),
                Width = size * scale,
                Height = size * scale,
                Fill = Brushes.DeepSkyBlue,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = dev,
                ToolTip = dev.Name
            };
            devRect.ContextMenu = DevContextMenu;

            Canvas.SetLeft(devRect, x );
            Canvas.SetTop(devRect, y);

            DevDict[dev.Id] = devRect;
            devRect.MouseDown += DevRect_MouseDown;
            devRect.MouseEnter += DevRect_MouseEnter;
            devRect.MouseLeave += DevRect_MouseLeave;
            Canvas1.Children.Add(devRect);
            return devRect;
        }

        private void DevRect_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            if (SelectedDev == rect) return;
            var dev = rect.Tag as DevEntity;
            LbState.Content = "";

            rect.Fill = Brushes.DeepSkyBlue;
            rect.Stroke = Brushes.Black;
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
                SelectedDev.Fill = Brushes.DeepSkyBlue;
                SelectedDev.Stroke = Brushes.Black;
            }
            var dev = rect.Tag as DevEntity;
            LbState.Content = GetDevText(dev);
            SelectedDev = rect;

            if (DevSelected != null)
            {
                DevSelected(rect, dev);
            }
        }

        private void AddAreaRect(AreaEntity area, AreaEntity parent, double scale = 1,bool isTransparent = false)
        {
            if (area == null) return;
            var bound = area.InitBound;
            if (bound == null) return;
            {
                //< Polygon Fill = "AliceBlue" StrokeThickness = "5" Stroke = "Green" Points = "40,10 70,80 10,50" />

                Polygon polygon = new Polygon();
                if (isTransparent)
                {
                    polygon.Fill = Brushes.Transparent;
                }
                else
                {
                    polygon.Fill = Brushes.AliceBlue;
                }
                
                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = 1;
                polygon.MouseDown += Polygon_MouseDown;
                polygon.MouseEnter += Polygon_MouseEnter;
                polygon.MouseLeave += Polygon_MouseLeave;
                polygon.Tag = area;
                polygon.ContextMenu = AreaContextMenu;

                if (area.Type == AreaTypes.范围)
                {
                    polygon.Fill = Brushes.Transparent;
                    SetShapeStrokeDash(polygon);
                }

                foreach (var item in bound.GetPoints2D())
                {
                    double x = (item.X - OffsetX) * scale;
                    double y = (item.Y - OffsetY) * scale;
                    polygon.Points.Add(new System.Windows.Point(x, y));
                }

                AreaDict[area.Id] = polygon;
                Canvas1.Children.Add(polygon);
            }
        }

        private void Polygon_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Polygon_MouseLeave(object sender, MouseEventArgs e)
        {
            var shape = sender as Shape;
            if (SelectedRect != shape)
            {
                UnSelectRectangle(shape);
            }
        }

        private void Polygon_MouseEnter(object sender, MouseEventArgs e)
        {
            SelectRectangle(sender as Shape);
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
            try
            {
                int scale = (int)CbScale.SelectedItem;
                var area = SelectedArea;
                if (area == null) return;
                if (area.ParentId == 1) //电厂
                {
                    DrawPark(area, scale, DevSize);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    DrawFloor(area, scale, DevSize);
                }
                else
                {
                    SelectArea(area);
                }
                ShowPersons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RefreshDev(DevEntity dev)
        {
            int scale = (int)CbScale.SelectedItem;
            var rect=AddDevRect(dev, scale, DevSize);
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

        public void Init()
        {
            CbScale.ItemsSource = new int[] { 1, 2, 3, 4, 5, 10, 20, 30, 40, 50 };
            CbScale.SelectedIndex = 0;

            
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
            _persons = persons;
            if (persons == null) return;
            foreach (var person in persons)
            {
                PersonShape ps=AddPersonRect(person,Scale,2);
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
    }
}

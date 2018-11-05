using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Tools;
using Location.IModel;
using WPFClientControlLib.Behaviors;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for AreaCanvas.xaml
    /// </summary>
    public partial class AreaCanvas : UserControl
    {
        public AreaCanvas()
        {
            InitializeComponent();
        }

        public void SelectArea(Area area)
        {
            if (area == null) return;
            if (AreaDict.ContainsKey(area.Id))
            {
                ClearSelect();
                FocusRectangle(AreaDict[area.Id]);
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到区域:" + area.Name;
            }
        }

        public void SelectDev(DevInfo dev)
        {
            if (dev == null) return;
            if (DevDict.ContainsKey(dev.Id))
            {
                ClearSelect();
                FocusRectangle(DevDict[dev.Id]);
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到设备:" + dev.Name;
            }
        }

        public void FocusRectangle(Shape rect)
        {
            foreach(var item in Canvas1.Children)
            {
                Shape shape = item as Shape;
                if (shape == null) continue;
                if(shape!= rect)
                    SetShapeStrokeDash(shape);
            }

            SelectedRect = rect;
            SelectedRect.Stroke = Brushes.Red;
            SelectedRect.StrokeThickness = 2;
            SelectedRect.Focus();
            //VisualTreeHelper.sc
            ScrollViewer1.ScrollToHorizontalOffset(SelectedRect.Margin.Left);
            ScrollViewer1.ScrollToVerticalOffset(SelectedRect.Margin.Top);
        }

        public Shape SelectedRect { get; set; }

        public Area Current { get; set; }

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

        public bool ShowDev { get; set; }

        public bool ShowPerson { get; set; }

        public void ShowArea(Area area)
        {
            try
            {
                CbView.SelectionChanged -= CbView_OnSelectionChanged;
                CbView.SelectionChanged += CbView_OnSelectionChanged;

                
                if (area == null) return;

                if (area.Parent.Name == "根节点") //电厂
                {
                    Current = area;
                    int scale = 3;
                    DevSize = 3;
                    DrawPark(area, scale, DevSize);
                    InitCbScale(scale);

                    InitCbDevSize(new double[] { 0.5, 1, 2, 3,4,5 }, DevSize);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    Current = area;
                    int scale = 20;
                    DevSize = 0.3;
                    DrawFloor(area, scale, DevSize);
                    InitCbScale(scale);

                    InitCbDevSize(new double[] { 0.1, 0.2, 0.3, 0.4, 0.5,0.6 }, DevSize);
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
        }

        public Dictionary<int, Shape> AreaDict = new Dictionary<int, Shape>();
        public Dictionary<int, Rectangle> DevDict = new Dictionary<int, Rectangle>();
        public Dictionary<int, Ellipse> PersonDict = new Dictionary<int, Ellipse>();

        public double Scale = 1;

        public Shape zeroPoint;
        public Shape ShowPoint(double x, double y)
        {
            if (zeroPoint != null)
            {
                Canvas1.Children.Remove(zeroPoint);
            }
            zeroPoint=AddPoint(Scale,new Vector(x,y));
            return zeroPoint;
        }

        public double ZeroX;
        public double ZeroY;

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

        private void DrawFloor(Area area,double scale,double devSize)
        {
            Clear();

            Bound bound = area.InitBound;
            if (bound == null) return;

            Scale = scale;

            Margin = 10;
            OffsetX = -Margin/2;
            OffsetY = -Margin/2;
            Canvas1.Width = (bound.MaxX+ Margin) * scale ;
            Canvas1.Height = (bound.MaxY+ Margin) * scale;

            

            AddAreaRect(area, null, scale);

            if (area.Children != null)
                foreach (var level1Item in area.Children) //机房
                {
                    AddAreaRect(level1Item, null, scale, true);
                }

            ShowDevs(area, scale, devSize);

            AddZeroPoint(scale,new Vector(0,0));
        }

        private void Clear()
        {
            Canvas1.Children.Clear();
            AreaDict.Clear();
            SelectedRect = null;
            OffsetX = 0;
            OffsetY = 0;
        }

        public double OffsetX = 0;
        public double OffsetY = 0;

        public double Margin = 20;

        private void DrawPark(Area area,int scale,double devSize)
        {
            Clear();

            Bound bound = area.InitBound;
            //if (bound == null)
            //{
            //    bound=area.CreateBoundByChildren();
            //}
            if (bound == null) return;

            //bound=area.SetBoundByDevs();

            Scale = scale;

            Margin = 20;

            OffsetX = bound.MinX - Margin;
            OffsetY = bound.MinY - Margin;

            Canvas1.Width = (bound.MaxX - OffsetX + Margin) * scale;
            Canvas1.Height =(bound.MaxY - OffsetY + Margin) *scale;

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

            ShowDevs(area, scale, devSize);

            AddZeroPoint(scale,new Vector(bound.MinX, bound.MinY));
        }

        private void ShowDevs(Area area, double scale, double devSize)
        {
            if (ShowDev)
                if (area.LeafNodes != null)
                    foreach (var dev in area.LeafNodes)
                    {
                        AddDevRect(dev, scale, devSize);
                    }
        }

        private Rectangle AddDevRect(DevInfo dev,double scale, double size = 2)
        {
            if (DevDict.ContainsKey(dev.Id))
            {
                Canvas1.Children.Remove(DevDict[dev.Id]);
            }

            double x = (dev.PosX - OffsetX) * scale-size*scale/2;
            double y = (dev.PosZ - OffsetY) * scale - size * scale / 2;
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
            DevInfo dev = rect.Tag as DevInfo;
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
            DevInfo dev = rect.Tag as DevInfo;
            LbState.Content = GetDevText(dev);

            rect.Fill = Brushes.Blue;
            rect.Stroke = Brushes.Red;
        }

        private Rectangle SelectedDev;

        private string GetDevText(DevInfo dev)
        {
            return string.Format("[{0}]({1},{2})", dev.Name, dev.PosX, dev.PosZ); 
        }

        private void DevRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedDev != null)
            {
                SelectedDev.Fill = Brushes.DeepSkyBlue;
                SelectedDev.Stroke = Brushes.Black;
            }
            Rectangle rect = sender as Rectangle;
            DevInfo dev = rect.Tag as DevInfo;
            LbState.Content = GetDevText(dev);
            SelectedDev = rect;

            if (DevSelected != null)
            {
                DevSelected(rect,dev);
            }
        }

        public event Action<Rectangle,DevInfo> DevSelected;

        

        private void AddAreaRect(Area area, Area parent, double scale = 1,bool isTransparent = false)
        {
            if (area == null) return;
            Bound bound = area.InitBound;
            if (bound == null) return;

            //{
            //    double x = (bound.MinX - OffsetX) * scale;
            //    double y = (bound.MinY - OffsetY) * scale;
            //    //if (ViewMode == 0)
            //    //    y = Canvas1.Height - bound.GetHeight() * scale - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
            //    if (bound.IsRelative && parent != null)
            //    {
            //        x += parent.InitBound.MinX * scale;
            //        y -= parent.InitBound.MinY * scale;
            //    }

            //    Rectangle areaRect = new Rectangle()
            //    {
            //        Margin = new Thickness(x, y, 0, 0),
            //        Width = bound.GetWidth() * scale,
            //        Height = bound.GetHeight() * scale,
            //        Fill = Brushes.LightGoldenrodYellow,
            //        Stroke = Brushes.Black,
            //        StrokeThickness = 1,
            //        Tag = area,
            //        ToolTip = area.Name
            //    };
            //    AreaDict[area.Id] = areaRect;
            //    areaRect.MouseEnter += AreaRect_MouseEnter;
            //    areaRect.MouseLeave += AreaRect_MouseLeave;
            //    //Canvas1.Children.Add(areaRect);
            //}

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
                polygon.MouseEnter += Polygon_MouseEnter;
                polygon.MouseLeave += Polygon_MouseLeave;
                polygon.Tag = area;

                if (area.Type == AreaTypes.范围)
                {
                    polygon.Fill = Brushes.Transparent;
                    SetShapeStrokeDash(polygon);
                }

                foreach (var item in bound.GetPoints2D())
                {
                    double x = (item.X - OffsetX) * scale;
                    double y = (item.Y - OffsetY) * scale;
                    //if (ViewMode == 0)
                    //    y = Canvas1.Height - /*bound.GetHeight() * scale*/ - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
                    //if (bound.IsRelative && parent != null)
                    //{
                    //    x += parent.InitBound.MinX * scale;
                    //    y -= parent.InitBound.MinY * scale;
                    //}
                    polygon.Points.Add(new System.Windows.Point(x, y));
                }

                AreaDict[area.Id] = polygon;
                Canvas1.Children.Add(polygon);
            }
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
            UnSelectRectangle(sender as Shape);
        }

        private void Polygon_MouseEnter(object sender, MouseEventArgs e)
        {
            SelectRectangle(sender as Shape);
        }

        private void AreaRect_MouseLeave(object sender, MouseEventArgs e)
        {
            UnSelectRectangle(sender as Shape);
        }

        private void AreaRect_MouseEnter(object sender, MouseEventArgs e)
        {
            SelectRectangle(sender as Shape);
        }

        private void SelectRectangle(Shape rect)
        {
            IName entity = rect.Tag as IName;
            LbState.Content = "" + entity.Name;
            rect.Stroke = Brushes.Red;
            rect.StrokeThickness = 2;

            //Canvas1.Children.Remove(rect);
            //Canvas1.Children.Add(rect);

            SelectedRect = rect;
        }

        private void UnSelectRectangle(Shape rect)
        {
            IName entity = rect.Tag as IName;
            LbState.Content = "" + entity.Name;

            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1;

            SelectedRect = null;
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
                Area area = Current;
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

        public void RefreshDev(DevInfo dev)
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

        public double DevSize { get; set; }

        private void AreaCanvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void Init()
        {
            CbScale.ItemsSource = new int[] { 1, 2, 3, 4, 5, 10, 20, 30, 40, 50 };
            CbScale.SelectedIndex = 0;

            
        }

        public void ShowArchors(Archor[] archors)
        {
            
        }

        public void ShowDevs(DevInfo[] devs)
        {
            
        }

        private IList<Location.TModel.Location.Person.Personnel> _persons;

        public void ShowPersons()
        {
            ShowPersons(_persons);
        }

        public void ShowPersons(IList<Location.TModel.Location.Person.Personnel> persons)
        {
            _persons = persons;
            if (persons == null) return;
            foreach (var person in persons)
            {
                AddPersonRect(person,Scale,2);
            }
        }

        private Ellipse AddPersonRect(Location.TModel.Location.Person.Personnel person, double scale, double size = 2)
        {
            if (PersonDict.ContainsKey(person.Id))
            {
                Canvas1.Children.Remove(PersonDict[person.Id]);
            }

            double x = (person.Tag.Pos.X - OffsetX) * scale - size * scale / 2;
            double y = (person.Tag.Pos.Z - OffsetY) * scale - size * scale / 2;
            //if (ViewMode == 0)
            //    y = Canvas1.Height - size * scale - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
            Ellipse personShape = new Ellipse()
            {
                //Margin = new Thickness(x, y, 0, 0),
                Width = size * scale,
                Height = size * scale,
                Fill = Brushes.GreenYellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = person,
                ToolTip = person.Name
            };


            DragInCanvasBehavior behavior1 = new DragInCanvasBehavior();
            behavior1.Moved += Behavior1_Moved;
            Interaction.GetBehaviors(personShape).Add(behavior1);

            Canvas.SetLeft(personShape, x);
            Canvas.SetTop(personShape, y);

            PersonDict[person.Id] = personShape;
            personShape.MouseDown += PersonShape_MouseDown;
            personShape.MouseEnter += PersonShape_MouseEnter;
            personShape.MouseLeave += PersonShape_MouseLeave;
            Canvas1.Children.Add(personShape);
            return personShape;
        }

        private void Behavior1_Moved(object arg1, System.Windows.Point arg2)
        {
            //double left = Canvas.GetLeft(this);
            //double top = Canvas.GetTop(this);

            //this.XPos = left;
            //this.YPos = top;
            //Position.XPos = (int)left;
            //Position.YPos = (int)top;
        }

        private void PersonShape_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse rect = sender as Ellipse;
            if (SelectedPerson == rect) return;
            var dev = rect.Tag as Location.TModel.Location.Person.Personnel;
            LbState.Content = "";

            rect.Fill = Brushes.GreenYellow;
            rect.Stroke = Brushes.Black;
        }

        private void PersonShape_MouseEnter(object sender, MouseEventArgs e)
        {
            //SelectRectangle(sender as Rectangle);
            Ellipse rect = sender as Ellipse;
            var person = rect.Tag as Location.TModel.Location.Person.Personnel;
            LbState.Content = person.Name;

            rect.Fill = Brushes.Blue;
            rect.Stroke = Brushes.Red;
        }

        public event Action<Ellipse, Location.TModel.Location.Person.Personnel> PersonSelected;

        public Ellipse SelectedPerson;

        private void PersonShape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPerson != null)
            {
                SelectedPerson.Fill = Brushes.GreenYellow;
                SelectedPerson.Stroke = Brushes.Black;
            }
            Ellipse rect = sender as Ellipse;
            var person = rect.Tag as Location.TModel.Location.Person.Personnel;
            LbState.Content = person.Name;
            SelectedPerson = rect;

            if (PersonSelected != null)
            {
                PersonSelected(rect, person);
            }
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

        public int ViewMode { get; set; }
    }
}

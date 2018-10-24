using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;

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

        public void Select(Area area)
        {
            if (AreaDict.ContainsKey(area.Id))
            {
                ClearSelect();
                SelectedRect = AreaDict[area.Id];
                SelectedRect.Stroke = Brushes.Red;
                SelectedRect.Focus();
                LbState.Content = "";
            }
            else
            {
                ClearSelect();
                LbState.Content = "未找到区域:" + area.Name;
            }
        }

        public Rectangle SelectedRect { get; set; }

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

        public void ShowArea(Area area)
        {
            try
            {
                CbView.SelectionChanged -= CbView_OnSelectionChanged;
                CbView.SelectionChanged += CbView_OnSelectionChanged;

                Current = area;
                if (area == null) return;

                if (area.ParentId == 1) //电厂
                {
                    int scale = 2;
                    DevSize = 2;
                    DrawPark(area, scale, DevSize);
                    InitCbScale(scale);

                    InitCbDevSize(new double[] { 0.5, 1, 2, 3 }, DevSize);
                }
                else if (area.Type == AreaTypes.楼层)
                {
                    int scale = 20;
                    DevSize = 0.2;
                    DrawFloor(area, scale, DevSize);
                    InitCbScale(scale);

                    InitCbDevSize(new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 }, DevSize);
                }
                else
                {
                    Select(area);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }


        private void ClearSelect()
        {
            if (SelectedRect != null)
            {
                SelectedRect.Stroke = Brushes.Black;
            }
        }

        public Dictionary<int, Rectangle> AreaDict = new Dictionary<int, Rectangle>();
        public Dictionary<int, Rectangle> DevDict = new Dictionary<int, Rectangle>();

        public double Scale = 1;

        private void DrawFloor(Area area,double scale,double devSize)
        {
            Clear();

            Bound bound = area.InitBound;
            if (bound == null) return;

            Scale = scale;

            Canvas1.Width = bound.MaxX * scale ;
            Canvas1.Height = bound.MaxY * scale;

            AddAreaRect(area, null, scale);

            if (area.Children != null)
                foreach (var level1Item in area.Children) //机房
                {
                    AddAreaRect(level1Item, null, scale);
                }

            if(area.LeafNodes!=null)
                foreach (var dev in area.LeafNodes)
                {
                    AddDevRect(dev, scale, devSize);
                }
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

        private void DrawPark(Area area,int scale,double devSize)
        {
            Clear();

            Bound bound = area.InitBound;
            if (bound == null) return;

            OffsetX = bound.MinX;
            OffsetY = bound.MinY;

            Canvas1.Width = (bound.MaxX - OffsetX)* scale;
            Canvas1.Height =(bound.MaxY - OffsetY)*scale;

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

            foreach (var dev in area.LeafNodes)
            {
                AddDevRect(dev, scale, devSize);
            }
        }

        private void AddDevRect(DevInfo dev,double scale, double size = 2)
        {
            double x = (dev.PosX - OffsetX) * scale;
            double y = (dev.PosZ - OffsetY) * scale;
            if (ViewMode == 0)
                y = Canvas1.Height - size * scale - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
            Rectangle devRect = new Rectangle()
            {
                Margin = new Thickness(x, y, 0, 0),
                Width = size * scale,
                Height = size * scale,
                Fill = Brushes.Blue,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = dev,
                ToolTip = dev.Name
            };
            DevDict[dev.Id] = devRect;
            devRect.MouseDown += DevRect_MouseDown;
            Canvas1.Children.Add(devRect);
        }

        private void DevRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            DevInfo dev = rect.Tag as DevInfo;
            LbState.Content = "" + dev.Name;
        }

        private void AddAreaRect(Area area, Area parent, double scale = 1)
        {
            if (area == null) return;
            Bound bound = area.InitBound;
            if (bound == null) return;

            double x = (bound.MinX - OffsetX)*scale;
            double y = (bound.MinY - OffsetY)*scale;
            if(ViewMode==0)
                y = Canvas1.Height - bound.GetHeight()*scale - y; //上下颠倒一下，不然就不是CAD上的上北下南的状况了
            if (bound.IsRelative && parent != null)
            {
                x += parent.InitBound.MinX*scale;
                y -= parent.InitBound.MinY*scale;
            }

            Rectangle areaRect = new Rectangle()
            {
                Margin = new Thickness(x, y, 0, 0),
                Width = bound.GetWidth()*scale,
                Height = bound.GetHeight()*scale,
                Fill = Brushes.LightGoldenrodYellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = area,
                ToolTip = area.Name
            };

            AreaDict[area.Id] = areaRect;
            areaRect.MouseEnter += AreaRect_MouseEnter;
            areaRect.MouseLeave += AreaRect_MouseLeave;
            Canvas1.Children.Add(areaRect);
        }

        private void AreaRect_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            Area area = rect.Tag as Area;
            LbState.Content = "" + area.Name;

            rect.Stroke = Brushes.Black;
            SelectedRect = null;
        }

        private void AreaRect_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            Area area = rect.Tag as Area;
            LbState.Content = "" + area.Name;
            rect.Stroke = Brushes.Red;
            SelectedRect = rect;
        }

        private void CbScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
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
                    Select(area);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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

        public void ShowPersons()
        {
            
        }

        private void CbView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode = CbView.SelectedIndex;
            Refresh();
        }

        public int ViewMode { get; set; }
    }
}

using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Point = DbModel.Location.AreaAndDev.Point;

namespace LocationServer.Models.EngineTool
{
    /// <summary>
    /// PointSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PointSelectWindow : Window
    {
        public PointSelectWindow()
        {
            InitializeComponent();
        }

        Area area;
        DevInfo dev;
        int mode;
        public PointSelectWindow(Area area,DevInfo dev,int mode)
        {
            InitializeComponent();
            this.area = area;
            this.dev = dev;
            this.mode = mode;
        }

        private void List1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var area = List1.SelectedItem as Area;
            if (area == null) return;
            var bound = area.InitBound;
            var points = new List<Point>();
            points.AddRange(bound.Points);
            points.Add(bound.GetCenter());
            List2.ItemsSource = points;
            if (dev != null)
            {
                List2.SelectedItem = bound.GetClosePoint(dev.PosX, dev.PosZ);
            }

            if (SelectedAreaChanged != null)
            {
                SelectedAreaChanged(area);
            }
        }

        public event Action<Area> SelectedAreaChanged;

        private void List2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentPoint = List2.SelectedItem as Point;
            if (CurrentPoint == null) return;
            if (SelectedPointChanged!=null)
            {
                SelectedPointChanged(CurrentPoint);
            }
        }

        public event Action<Point> SelectedPointChanged;

        public Point CurrentPoint;

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (dev != null)
            {
                List1.ItemsSource = area.SortByPoint(dev.PosX, dev.PosZ);
            }
            else
            {
                if (area.Children == null || area.Children.Count == 0)
                {
                    List1.ItemsSource = new List<Area>() { area };
                }
                else
                {
                    if(mode == 1)
                    {
                        var areas = new List<Area>();
                        areas.Add(area);
                        areas.AddRange(area.Children);
                        List1.ItemsSource = areas;
                    }
                    else
                    {
                        List1.ItemsSource = area.Children;
                    }
                    
                }
                
            }
            List1.SelectedIndex = 0;
        }
    }
}

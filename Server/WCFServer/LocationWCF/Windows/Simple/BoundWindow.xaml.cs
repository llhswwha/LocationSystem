using Location.TModel.Location.AreaAndDev;
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

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// BoundWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BoundWindow : Window
    {
        public BoundWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        public Bound Bound { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Bound == null) return;
            ListBox1.ItemsSource = Bound.Points;
            BoundCanvas1.Show(Bound);

        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            //var point = ListBox1.SelectedItem as Location.TModel.Location.AreaAndDev.Point;
            var point = new DbModel.Location.AreaAndDev.Point();
            var win = new ItemInfoWindow(point);
            win.Show();
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            var point = ListBox1.SelectedItem as Location.TModel.Location.AreaAndDev.Point;
            //var point = new DbModel.Location.AreaAndDev.Point();
            var win = new ItemInfoWindow(point);
            win.Show();
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            var point = ListBox1.SelectedItem as Location.TModel.Location.AreaAndDev.Point;
            //var point = new DbModel.Location.AreaAndDev.Point();
            //var win = new ItemInfoWindow(point);
            //win.Show();
            Bound.Points.Remove(point);


        }
    }
}

using DbModel.LocationHistory.Data;
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

namespace LocationServer.Windows
{
    /// <summary>
    /// PosHistoryBrowserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PosHistoryBrowserWindow : Window
    {
        public PosHistoryBrowserWindow()
        {
            InitializeComponent();
        }

        double minX = double.MaxValue;
        double minY = double.MaxValue;
        double maxX = double.MinValue;
        double maxY = double.MinValue;

        public void Draw(List<PosInfo> posInfoList)
        {
            //AddPoint(0, 0);
            //AddPoint(10, 10);
            //AddPoint(100, 100);
            if (posInfoList == null) return;
            foreach (PosInfo pos in posInfoList)
            {
                Ellipse ellipse = AddPoint(pos.X, pos.Z);
                ellipse.Tag = pos;

            }

            //double width = maxX - minX;
            //double height = maxY - minY;
            Canvas1.Width = maxX;
            Canvas1.Height = maxY;
        }

        private Ellipse AddPoint(double x, double y)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 15;
            ellipse.Height = 15;
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            Canvas1.Children.Add(ellipse);
            if (x < minX)
            {
                minX = x;
            }
            if (y < minY)
            {
                minY = y;
            }

            if (x > maxX)
            {
                maxX = x;
            }
            if (y > maxY)
            {
                maxY = y;
            }

            return ellipse;
        }
    }
}

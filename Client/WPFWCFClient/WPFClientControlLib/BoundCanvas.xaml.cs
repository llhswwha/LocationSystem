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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Location.TModel.Location.AreaAndDev;
using Point = Location.TModel.Location.AreaAndDev.Point;

namespace WPFClientControlLib
{
    /// <summary>
    /// BoundCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class BoundCanvas : UserControl
    {
        public BoundCanvas()
        {
            InitializeComponent();
        }

        public double CanvasMargin = 10;

        public Bound Bound;

        public void DrawBound(Bound bound)
        {
            PointEllipse.Clear();
            if (bound == null) return;
            if (bound.GetSizeX() == 0) return;
            double scale1 = (Canvas1.ActualWidth - CanvasMargin) / bound.GetSizeX();
            double scale2 = (Canvas1.ActualHeight - CanvasMargin) / bound.GetSizeY();
            double scale = scale1 < scale2 ? scale1 : scale2;

            //this.Width = bound.GetWidth();
            //this.Height = bound.GetLength();

            Canvas1.Children.Clear();
            Polygon polygon = new Polygon();
            polygon.Fill = Brushes.AliceBlue;
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 1;
            var points = bound.Points;
            var ps = new List<System.Windows.Point>();
            foreach (var item in points)
            {
                double x = (item.X - bound.MinX) * scale + CanvasMargin / 2;
                double y = (item.Y - bound.MinY) * scale + CanvasMargin / 2;
                //double x = (item.X - OffsetX) * scale;
                //double y = (item.Y - OffsetY) * scale;
                var p = new System.Windows.Point(x, y);
                ps.Add(p);
                polygon.Points.Add(p);
            }
            Canvas1.Children.Add(polygon);

            foreach (var item in points)
            {
                double x = (item.X - bound.MinX) * scale + CanvasMargin / 2;
                double y = (item.Y - bound.MinY) * scale + CanvasMargin / 2;

                double size = scale;
                if (size > 0)
                {
                    size = 10;
                }
                Ellipse ellipse = new Ellipse();
                //ellipse.Margin = new Thickness(Margin/2, Margin/2, 0, 0);
                ellipse.Width = size;
                ellipse.Height = size;
                ellipse.Fill = Brushes.Transparent;
                ellipse.Stroke = Brushes.Blue;
                ellipse.StrokeThickness = 2;
                Canvas1.Children.Add(ellipse);

                double left = x - size / 2;
                double top = y - size / 2;

                Canvas.SetLeft(ellipse, left);
                Canvas.SetTop(ellipse, top);

                PointEllipse[item] = ellipse;
            }
        }

        public void Show(Bound bound)
        {
            this.Bound = bound;
            DrawBound(Bound);
        }

        public void RefreshBound()
        {
            if (Bound == null) return;
            DrawBound(Bound);
            SelectPoint(SelectedPoint);
        }

        public Dictionary<Point, Ellipse> PointEllipse = new Dictionary<Point, Ellipse>();

        public Ellipse SelectedEllipse = null;

        public Point SelectedPoint = null;

        public void SelectPoint(Point point)
        {
            if (SelectedEllipse != null)
            {
                SelectedEllipse.Stroke = Brushes.Blue;
            }
            if (point == null) return;
            if (PointEllipse.ContainsKey(point))
            {
                Ellipse ellipse = PointEllipse[point];
                ellipse.Stroke = Brushes.Red;
                SelectedEllipse = ellipse;
                SelectedPoint = point;
            }
        }
    }
}

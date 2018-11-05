using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace WPFClientControlLib.Behaviors
{
    public enum HitPoint
    {
        None,Left, Top, Right, Bottom,Center
    }

    public static class RectangleHelper
    {
        public static bool IsHitBorder(Point point, Rectangle rect)
        {
            double thickness = rect.StrokeThickness;
            return point.X <= thickness || point.Y <= thickness
                   || point.X > rect.ActualWidth - thickness
                   || point.Y > rect.ActualHeight - thickness;
        }

        public static HitPoint HitTest(Point point, Rectangle rect)
        {
            double thickness = rect.StrokeThickness;
            HitPoint hitPoint = HitPoint.None;
            if (point.X <= thickness && point.X >= 0)
            {
                hitPoint = HitPoint.Left;
            }
            else if (point.Y <= thickness && point.X >= 0)
            {
                hitPoint = HitPoint.Top;
            }
            else if (point.X >= rect.ActualWidth - thickness && point.X <= rect.ActualWidth + thickness)
            {
                hitPoint = HitPoint.Right;
            }
            else if (point.Y >= rect.ActualHeight - thickness && point.Y <= rect.ActualHeight + thickness)
            {
                hitPoint = HitPoint.Bottom;
            }
            else if (point.X > thickness && point.X < rect.ActualWidth - thickness
                     && point.Y > thickness && point.Y < rect.ActualHeight - thickness)
            {
                hitPoint = HitPoint.Center;
            }
            return hitPoint;
        }

        public static void SetCursor(HitPoint hitPoint)
        {
            Cursor cursor = Cursors.Arrow;
            if (hitPoint== HitPoint.Left)
            {
                cursor = Cursors.SizeWE;
            }
            if (hitPoint == HitPoint.Top)
            {
                cursor = Cursors.SizeNS;
            }
            if (hitPoint == HitPoint.Right)
            {
                cursor = Cursors.SizeWE;
            }
            if (hitPoint == HitPoint.Bottom)
            {
                cursor = Cursors.SizeNS;
            }
            if (hitPoint == HitPoint.Center)
            {
                cursor = Cursors.Hand;
            }
            Mouse.SetCursor(cursor);
            //Log.Debug("SetCursor", "cursor:" + cursor);
        }

    }
}

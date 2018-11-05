using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFClientControlLib.Behaviors
{
    public class MoveRectangleBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove -= new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonUp -= new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
                rect = null;
                OnMoved(e);
            }
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (rect == null) return;
            if (isDragging)
            {
                //double left = Canvas.GetLeft(AssociatedObject);
                //double top = Canvas.GetTop(AssociatedObject);
                //double right = Canvas.GetRight(AssociatedObject);
                //double bottom = Canvas.GetBottom(AssociatedObject);
                Point point = e.GetPosition(canvas);
                double x = point.X - mouseOffset.X;
                if (x < 0)
                {
                    x = 0;
                }
                AssociatedObject.SetValue(Canvas.LeftProperty, x);

                double y = point.Y - mouseOffset.Y;
                if (y < 0)
                {
                    y = 0;
                }
                AssociatedObject.SetValue(Canvas.TopProperty, y);
            }
        }

        private Rectangle rect;

        void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ClickCount>1)return;
            if (canvas == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
            }
            rect = GetRectangle(AssociatedObject);
            Point point = e.GetPosition(rect);
            HitPoint hitPoint = RectangleHelper.HitTest(point, rect);

            if (hitPoint == HitPoint.Center || hitPoint == HitPoint.None)
            {
                isDragging = true;
                mouseOffset = e.GetPosition(AssociatedObject);
                AssociatedObject.CaptureMouse();
            }
        }

        public event Action<object, Point> Moved;

        protected void OnMoved(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Moved != null)
            {
                Point point = e.GetPosition(canvas);
                Moved(this, point);
            }
        }
         

        protected virtual Rectangle GetRectangle(UIElement uiElement)
        {
             return uiElement as Rectangle;
        }

        private Point mouseOffset;
        private bool isDragging = false;
        private Canvas canvas;
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace WPFClientControlLib.Behaviors
{
    public class DragInCanvasBehavior:Behavior<UIElement>
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
                OnMoved(e);
            }
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                Point point = e.GetPosition(canvas);
                AssociatedObject.SetValue(Canvas.TopProperty, point.Y - mouseOffset.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);

                OnMoving(e);
            }
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (canvas == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
            }
            isDragging = true;
            mouseOffset = e.GetPosition(AssociatedObject);
            AssociatedObject.CaptureMouse();
        }

        private Point mouseOffset;
        private bool isDragging = false;
        private Canvas canvas;

        public event Action<object, Point> Moving;

        protected void OnMoving(System.Windows.Input.MouseEventArgs e)
        {
            if (Moving != null)
            {
                Point point = e.GetPosition(canvas);
                Moving(this, point);
            }
        }

        public event Action<object, Point> Moved;

        protected void OnMoved(System.Windows.Input.MouseEventArgs e)
        {
            if (Moved != null)
            {
                Point point = e.GetPosition(canvas);
                Moved(this, point);
            }
        }

    }
}

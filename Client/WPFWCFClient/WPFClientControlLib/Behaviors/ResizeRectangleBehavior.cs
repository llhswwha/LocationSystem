using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFClientControlLib.Behaviors
{
    public class ResizeRectangleBehavior : Behavior<UIElement>
    {
        protected virtual Rectangle GetRectangle(UIElement uiElement)
        {
            return uiElement as Rectangle;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            rect = GetRectangle(AssociatedObject);
            borderBrush = rect.Stroke;
            borderThickness = rect.StrokeThickness;

            this.AssociatedObject.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseLeave += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseLeave);
        }

        private Canvas canvas;

        void AssociatedObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            rect.Stroke = borderBrush;
            rect.StrokeThickness = borderThickness;
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseMove -= new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
            this.AssociatedObject.MouseLeftButtonUp -= new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseLeave -= new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseLeave);
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
                OnSizeChanged(rect, true);
            }
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Rectangle rect = GetRectangle(AssociatedObject);
            FrameworkElement element = AssociatedObject as FrameworkElement;
            if (element.Width < 10 || element.Height < 10)
            {
                return;
            }
            Point point = e.GetPosition(element);
            if (isDragging)
            {
                isChanged = false;
                double newWidth = width;
                double newHeight = height;

                if (hitPoint == HitPoint.Right)
                {
                    newWidth = point.X + 1;
                }
                else if (hitPoint == HitPoint.Bottom)
                {
                    newHeight = point.Y + 1;
                }
                else
                {
                    Canvas canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
                    Point point2 = e.GetPosition(canvas);
                    if (hitPoint == HitPoint.Top)
                    {
                        AssociatedObject.SetValue(Canvas.TopProperty, point2.Y - mouseOffset.Y);
                        newHeight = height - (point2.Y - mouseOffset2.Y);
                        isChanged = true;
                    }
                    else if (hitPoint == HitPoint.Left)
                    {
                        AssociatedObject.SetValue(Canvas.LeftProperty, point2.X - mouseOffset.X);
                        newWidth = width - (point2.X - mouseOffset2.X);
                        isChanged = true;
                    }
                }
                if (newHeight != height && newHeight>10)
                {
                    SetHeight(element, newHeight);
                    isChanged = true;
                }
                if (newWidth != width && newWidth>10)
                {
                    SetWidth(element, newWidth);
                    isChanged = true;
                }
                if (isChanged)
                {
                    OnSizeChanged(element,false);
                }
            }
            else
            {
                hitPoint = RectangleHelper.HitTest(point, rect);
                if (hitPoint != HitPoint.None)
                {
                    rect.Stroke = new SolidColorBrush(Color);
                    if (rect.StrokeThickness<= 0 && Thickness > 0)
                    {
                        rect.StrokeThickness = Thickness;
                    }
                }
                else
                {
                    rect.Stroke = borderBrush;
                    rect.StrokeThickness = borderThickness;
                }
            }

            RectangleHelper.SetCursor(hitPoint);
        }

        protected virtual void SetHeight(FrameworkElement element,double newHeight)
        {
            element.Height = newHeight + 25;
        }

        protected virtual void SetWidth(FrameworkElement element, double newWidth)
        {
            element.Width = newWidth;
        }

        private bool isChanged = false;

        public event Action<object, FrameworkElement, bool> SizeChanged;

        protected void OnSizeChanged(FrameworkElement element,bool isFinished)
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, element, isFinished);
            }
        }

        private Rectangle rect;
        private Brush borderBrush;
        private double borderThickness;
        public double Thickness = 0;
        public Color Color = Colors.DeepSkyBlue;
        private HitPoint hitPoint;
        private double height;
        private double width;

        void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(rect);

            hitPoint = RectangleHelper.HitTest(point, rect);

            if (canvas == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
            }

            FrameworkElement element = AssociatedObject as FrameworkElement;

            if (hitPoint!=HitPoint.None)
            {
                height = rect.ActualHeight;
                width = rect.ActualWidth;

                rect.Stroke = new SolidColorBrush(Color);
                if (rect.StrokeThickness <= 0 && Thickness > 0)
                {
                    rect.StrokeThickness = Thickness;
                }

                isDragging = true;
                mouseOffset = e.GetPosition(AssociatedObject);
                mouseOffset2 = e.GetPosition(canvas);
                AssociatedObject.CaptureMouse();
            }
            else
            {
                rect.Stroke = borderBrush;
                rect.StrokeThickness = borderThickness;
            }
        }

        private Point mouseOffset;
        private Point mouseOffset2;
        private bool isDragging = false;
    }
}

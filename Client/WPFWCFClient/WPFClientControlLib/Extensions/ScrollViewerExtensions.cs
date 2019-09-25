using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFClientControlLib.Extensions
{
    public static class ScrollViewerExtensions
    {
        public static void ScrollToVerticalTop(this ScrollViewer sv, FrameworkElement element)
        {
            try
            {
                if (element == null) return;
                var offset = sv.VerticalOffset;
                var p = new Point(0, offset);
                var tarPos = element.TransformToVisual(sv).Transform(p);
                sv.ScrollToVerticalOffset(tarPos.Y);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

        }

        public static void ScrollToHorizontalRight(this ScrollViewer sv, FrameworkElement element)
        {
            try
            {
                if (element == null) return;
                var offset = sv.HorizontalOffset; ;
                var p = new Point(offset, 0);
                var tarPos = element.TransformToVisual(sv).Transform(p);
                sv.ScrollToHorizontalOffset(tarPos.X);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

        }

        public static void ScrollTo(this ScrollViewer sv, FrameworkElement element)
        {
            try
            {
                return;//最终发现不行，还要调试先放着吧。
                sv.ScrollToHorizontalOffset(0);
                sv.ScrollToVerticalOffset(0);

                if (element == null) return;
                var p = new Point(sv.HorizontalOffset, sv.VerticalOffset);
                var tarPos = element.TransformToVisual(sv).Transform(p);
                sv.ScrollToHorizontalOffset(tarPos.X);
                sv.ScrollToVerticalOffset(tarPos.Y);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}

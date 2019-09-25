using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace WPFClientControlLib.Extensions
{
    public static class TreeViewExtensions
    {
        public static void ExpandLevel(this TreeView treeView1,int level)
        {
            ExpandChildren(treeView1, level);
            treeView1.UpdateLayout();
        }

        public static void ExpandChildren(ItemsControl control, int level)
        {
            if (level <= 0) return;
            foreach (var item in control.Items)
            {
                var node = item as TreeViewItem;
                if (node == null) continue;
                node.IsExpanded = true;
                ExpandChildren(node, level - 1);
            }
            control.UpdateLayout();
        }

        public static ScrollViewer GetScrollViewer(this TreeView treeView1)
        {
            TreeViewAutomationPeer vap = new TreeViewAutomationPeer(treeView1);
            var svap = vap.GetPattern(PatternInterface.Scroll) as ScrollViewerAutomationPeer;
            var scroller = svap.Owner as ScrollViewer;
            return scroller;
        }

        public static void ScrollTo(this TreeView treeView1,TreeViewItem item)
        {
            var scroller = treeView1.GetScrollViewer();
            scroller.ScrollTo(item);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

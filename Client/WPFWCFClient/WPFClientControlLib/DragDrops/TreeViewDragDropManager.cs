using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using WpfDragAndDropSmorgasbord.DragDropFramework;
using WpfDragAndDropSmorgasbord.DragDropFrameworkData;

namespace WpfFramework.Common.DragDrops
{
    public class TreeViewDragDropManager
    {
        public TreeViewDragDropManager(TreeView treeView)
        {
            TreeViewDataProvider<ItemsControl, TreeViewItem> treeViewDataProvider = new TreeViewDataProvider<ItemsControl, TreeViewItem>("TreeViewItemObject");
            // Data Consumer
            TreeViewDataConsumer<ItemsControl, TreeViewItem> treeViewDataConsumer = new TreeViewDataConsumer<ItemsControl, TreeViewItem>(new string[] { "TreeViewItemObject" });
            // Drag Managers
            DragManager dragHelperTreeView0 = new DragManager(treeView, treeViewDataProvider);
            // Drop Managers
            DropManager dropHelperTreeView0 = new DropManager(treeView, new IDataConsumer[] { treeViewDataConsumer, });
        }
    }
}

using BLL;
using DbModel.Location.AreaAndDev;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
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

            DropManager dropHelperTreeView0 = new DropManager(treeView, new IDataConsumer[] { treeViewDataConsumer, });
            dropHelperTreeView0.DragDrop_Drop += DropHelperTreeView0_DragDrop_Over;
            dropHelperTreeView0.DragDrop_Enter += DropHelperTreeView0_DragDrop_Enter;
        }

        private void DropHelperTreeView0_DragDrop_Enter(object arg1, System.Windows.DragEventArgs arg2)
        {
            TreeViewItem _TreeViewItem = arg2.Source as TreeViewItem;
            if (_TreeViewItem!=null )
            {
                _PhysicalTopology_Drag = _TreeViewItem.Tag as PhysicalTopology;
            }
         

        }
        public  object GetData(DragEventArgs e)
        {
            object data = null;
            string[] dataFormats = e.Data.GetFormats();
            foreach (string dataFormat in dataFormats)
            {
                        try
                        {
                            data = e.Data.GetData(dataFormat);
                        }
                        catch /*(COMException e2)*/
                        {
                            ;
                        }
                    
                    if (data != null)
                        return data;
                }
            return null;
        }
        PhysicalTopology _PhysicalTopology_Drag; //拖拽的原始节点
        PhysicalTopology _PhysicalTopology_DragDrop; //拖拽进入的父节点
        private void DropHelperTreeView0_DragDrop_Over(object arg1, System.Windows.DragEventArgs arg2)
        {
            TreeViewDataProvider <ItemsControl, TreeViewItem> dataProvider = GetData(arg2) as TreeViewDataProvider<ItemsControl, TreeViewItem>;
            TreeViewItem _TreeViewItem= dataProvider.SourceObject as TreeViewItem;
            _PhysicalTopology_Drag = _TreeViewItem.Tag as PhysicalTopology;
            TreeViewItem _TreeViewItemP = arg2.Source as TreeViewItem;
            //   Area _Area = _TreeViewItem.Tag as TD;
            if (_TreeViewItemP == null)
            {
                return;
            }
            if (_TreeViewItemP.Tag != null)
            {
                _PhysicalTopology_DragDrop = _TreeViewItemP.Tag as PhysicalTopology;
            }
            if (_TreeViewItemP != null)
            {
                if (_PhysicalTopology_Drag.ParentId!=_PhysicalTopology_DragDrop.Id) // 平行移动就不需要跟新数据了
                {
                    _PhysicalTopology_Drag.ParentId = _PhysicalTopology_DragDrop.Id;
                    Area area = _PhysicalTopology_Drag.ToDbModel();
                    Bll _bll = new Bll();
                    _bll.Areas.Edit(area);
                }
            }
        }
    }
}

using Location.TModel.Location.AreaAndDev;
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
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.IModel;
using WPFClientControlLib.Extensions;
using System.Windows.Automation.Peers;
using WpfFramework.Common.DragDrops;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for TopoTreeView.xaml
    /// </summary>
    public partial class TopoTreeView : UserControl
    {
        public TopoTreeView()
        {
            InitializeComponent();
            
        }
        public void LoadDataEx<TD,TF>(TD root, bool onlyBuilding = false) where TD : ITreeNodeEx<TD,TF> where TF : IId
        {
            AreaNodeDict.Clear();
            DevNodeDict.Clear();
            if (root == null)
            {
                ShowTreeEx<TD,TF>(TreeView1, null);
            }
            else
            {
                if (onlyBuilding)
                {
                    ShowTreeEx<TD, TF>(TreeView1, root.GetAllChildren((int)AreaTypes.大楼));

                }
                else
                {
                    ShowTreeEx<TD, TF>(TreeView1, root.Children);
                }
            }
            TreeViewDragDropManager dragdrop = new TreeViewDragDropManager(TreeView1);
        }

        private void ShowTreeEx<TD, TF>(ItemsControl control, List<TD> list) where TD : ITreeNodeEx<TD, TF> where TF : IId
        {
            control.Items.Clear();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                TreeViewItem node = new TreeViewItem();
                node.Header = item.Name;
                node.Tag = item;
                node.ContextMenu = AreaMenu;
                control.Items.Add(node);
                AreaNodeDict[item.Id] = node;

                ShowTreeEx<TD, TF>(node, item.Children);

                if (item.LeafNodes != null)
                {
                    foreach (var leaf in item.LeafNodes)
                    {
                        TreeViewItem subNode = new TreeViewItem();
                        subNode.Header = leaf.ToString();
                        subNode.Tag = leaf;
                        subNode.Foreground = Brushes.Blue;
                        subNode.ContextMenu = DevMenu;
                        node.Items.Add(subNode);
                        DevNodeDict[leaf.Id] = subNode;
                    }
                }
            }
        }

        private void ShowTreeEx<TD, TF>(ItemsControl control, TD root) where TD : ITreeNodeEx<TD, TF> where TF : IId
        {
            control.Items.Clear();

            if (root.Children != null)
            {
                for (int i = 0; i < root.Children.Count; i++)
                {
                    var item = root.Children[i];
                    TreeViewItem node = new TreeViewItem();
                    node.Header = item.Name;
                    node.Tag = item;
                    node.ContextMenu = AreaMenu;
                    control.Items.Add(node);
                    AreaNodeDict[item.Id] = node;
                    ShowTreeEx<TD, TF>(node, item.Children);
                    ShowLeafNodesEx<TD, TF>(item, node);
                }
            }

            ShowLeafNodesEx<TD, TF>(root, control);

        }

        public void SetCheckByIds(List<int> areas)
        {
            ClearCheckState();
            foreach (var id in areas)
            {
                var r = FindNode(id,null);
                if (r == null) continue;
                CheckBox cb = r.Header as CheckBox;
                if (cb != null)
                {
                    cb.IsChecked = true;
                }
            }
        }

        private void ClearCheckState()
        {
            ClearCheckState(TreeView1.Items);
        }

        public void ClearCheckState(ItemCollection nodes)
        {
            foreach (TreeViewItem node in nodes)
            {
                CheckBox cb = node.Header as CheckBox;
                if (cb != null)
                {
                    cb.IsChecked = false;
                }
                ClearCheckState(node.Items);
            }
        }

        public List<TreeViewItem> GetCheckedItems()
        {
            List<TreeViewItem> list = GetCheckedItems(TreeView1.Items);
            return list;
        }

        public List<TreeViewItem> GetCheckedItems(ItemCollection nodes)
        {
            List<TreeViewItem> result = new List<TreeViewItem>();
            foreach (TreeViewItem node in nodes)
            {
                CheckBox cb = node.Header as CheckBox;
                if (cb != null)
                {
                    if(cb.IsChecked == true)
                    {
                        result.Add(node);
                    }
                }
                var subResult = GetCheckedItems(node.Items);
                result.AddRange(subResult);
            }
            return result;
        }

        private void ShowLeafNodesEx<TD, TF>(TD item, ItemsControl control) where TD : ITreeNodeEx<TD, TF> where TF : IId
        {
            if (item.LeafNodes != null)
            {
                foreach (var leaf in item.LeafNodes)
                {
                    TreeViewItem subNode = new TreeViewItem();
                    subNode.Header = leaf.ToString();
                    subNode.Tag = leaf;
                    subNode.Foreground = Brushes.Blue;
                    subNode.ContextMenu = DevMenu;

                    control.Items.Add(subNode);

                    DevNodeDict[leaf.Id] = subNode;
                }
            }
        }

        public void LoadData<T>(T root, bool onlyBuilding = false,bool showCheckBox=false) where T : ITreeNode<T>
        {
            AreaNodeDict.Clear();
            if (root == null)
            {
                ShowTree<T>(TreeView1, null, showCheckBox);
            }
            else
            {
                if (onlyBuilding)
                {
                    ShowTree(TreeView1, root.GetAllChildren((int)AreaTypes.大楼), showCheckBox);

                }
                else
                {
                    ShowTree(TreeView1, root.Children, showCheckBox);
                }
            }
        }

        private void ShowTree<T>(ItemsControl control, List<T> list,bool showCheckBox=false) where T : ITreeNode<T>
        {
            control.Items.Clear();

            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                TreeViewItem node = new TreeViewItem();


                if (showCheckBox)
                {
                    CheckBox cb = new CheckBox();
                    cb.Content = item.Name;
                    node.Header = cb;
                }
                else
                {
                    node.Header = item.Name;
                }

                node.Tag = item;
                node.ContextMenu = AreaMenu;
                //node.IsCheck = true;
                control.Items.Add(node);
                AreaNodeDict[item.Id] = node;
                ShowTree(node, item.Children,showCheckBox);
            }
        }

        public Dictionary<int, TreeViewItem> AreaNodeDict = new Dictionary<int, TreeViewItem>();

        public Dictionary<int, TreeViewItem> DevNodeDict = new Dictionary<int, TreeViewItem>();

        public TreeViewItem GetAreaNode(int id)
        {
            if (AreaNodeDict.ContainsKey(id))
            {
                return AreaNodeDict[id];
            }
            return null;
        }

        public TreeViewItem GetDevNode(int id)
        {
            if (DevNodeDict.ContainsKey(id))
            {
                return DevNodeDict[id];
            }
            return null;
        }

        public TreeViewItem RemoveDevNode(int id)
        {
            if (DevNodeDict.ContainsKey(id))
            {
                TreeViewItem devNode= DevNodeDict[id];
                (devNode.Parent as TreeViewItem).Items.Remove(devNode);
            }
            return null;
        }

        public void SelectFirst()
        {
            if (TreeView1.Items.Count > 0)
            {
                (TreeView1.Items[0] as TreeViewItem).IsSelected = true;
            }
        }

        public void ExpandLevel(int level)
        {
            TreeView1.ExpandLevel(level);
        }

        private void TreeView1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedObjectChanged != null)
            {
                SelectedObjectChanged(SelectedObject);
            }
        }

        public event Action<object> SelectedObjectChanged;

        public TreeView Tree
        {
            get
            {
                return TreeView1;
            }
        }

        public object SelectedObject
        {
            get
            {
                TreeViewItem node = TreeView1.SelectedItem as TreeViewItem;
                if (node != null)
                {
                    return node.Tag;
                }
                return TreeView1.SelectedItem;
            }
        }

        public TreeViewItem SelectedNode
        {
            get
            {
                TreeViewItem node = TreeView1.SelectedItem as TreeViewItem;
                return node;
            }
        }

        public void RemoveCurrentNode()
        {
            TreeViewItem node = TreeView1.SelectedItem as TreeViewItem;
            if (node == null) return;
            if (node.Parent == null) return;
            (node.Parent as TreeViewItem).Items.Remove(node);
        }

        public bool SelectNode(Type nodeType,int id,int subId)
        {
            var r= FindNode(id, nodeType);
            if (r != null)
            {
                SetSelectedState(r);
                ExpandParent(r);

                r.IsExpanded = true;

                TreeViewItem selectedNode = null;
                foreach (TreeViewItem node in r.Items)
                {
                    IEntity idObj = node.Tag as IEntity;
                    if (idObj.Id == subId)
                    {
                        node.IsSelected = true;
                        node.Foreground = Brushes.Red;

                        selectedNode = node;
                    }
                }

                TreeView1.ScrollTo(r);

                TreeView1.ScrollTo(selectedNode);
            }
            return r != null;
        }

        public void ExpandParent(TreeViewItem node)
        {
            if (node.Parent is TreeViewItem)
            {
                (node.Parent as TreeViewItem).IsExpanded = true;
                ExpandParent(node.Parent as TreeViewItem);
            }
        }

        public ContextMenu AreaMenu;
        public ContextMenu DevMenu;

        public TreeViewItem FindNode(int id, Type nodeType)
        {
            return FindNode(id, nodeType,TreeView1.Items);
        }

        public TreeViewItem FindNode(int id, Type nodeType, ItemCollection nodes)
        {
            foreach (TreeViewItem node in nodes)
            {
                IEntity idObj = node.Tag as IEntity;
                if (nodeType == null)
                {
                    if (idObj.Id == id)
                    {
                        //SetSelectedState(node);
                        return node;
                    }
                }
                else
                {
                    if (idObj.Id == id && idObj.GetType() == nodeType)
                    {
                        //SetSelectedState(node);
                        return node;
                    }
                }


                var r = FindNode(id, nodeType, node.Items);
                if (r != null) return r;
            }
            return null;
        }

        private void SetSelectedState(TreeViewItem node)
        {
            node.IsSelected = true;
            node.Foreground = Brushes.Red;
        }

        public void RefreshCurrentNode<TD, TF>(TD entity) where TD : ITreeNodeEx<TD, TF> where TF : IId
        {
            TreeViewItem item = TreeView1.SelectedItem as TreeViewItem;
            var tag = (TD)item.Tag;
            ShowTreeEx<TD,TF>(item, tag);
        }

        public void SelectNodeById(IId iId)
        {
            if (iId == null) return;
            int nodeType = 0;
            var r = FindNode(iId.Id, iId.GetType());
            if (r != null)
            {
                //TreeView1.SelectedItem = r;
                r.IsSelected = true;
                ExpandParent(r);
            }
        }
    }
}

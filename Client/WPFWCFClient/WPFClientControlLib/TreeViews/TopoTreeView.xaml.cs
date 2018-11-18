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
        public void LoadDataEx<TD,TF>(TD root, bool onlyBuilding = false) where TD : ITreeNodeEx<TD,TF>
        {
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
        }

        private void ShowTreeEx<TD, TF>(ItemsControl control, List<TD> list) where TD : ITreeNodeEx<TD, TF>
        {
            control.Items.Clear();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                TreeViewItem node = new TreeViewItem();
                node.Header = item.Name;
                node.Tag = item;
                control.Items.Add(node);

                ShowTreeEx<TD, TF>(node, item.Children);

                if (item.LeafNodes != null)
                {
                    foreach (var leaf in item.LeafNodes)
                    {
                        TreeViewItem subNode = new TreeViewItem();
                        subNode.Header = leaf.ToString();
                        subNode.Tag = leaf;
                        subNode.Foreground = Brushes.Blue;
                        node.Items.Add(subNode);
                    }
                }
            }
        }


        public void LoadData<T>(T root, bool onlyBuilding = false) where T : ITreeNode<T>
        {
            if (root == null)
            {
                ShowTree<T>(TreeView1, null);
            }
            else
            {
                if (onlyBuilding)
                {
                    ShowTree(TreeView1, root.GetAllChildren((int)AreaTypes.大楼));

                }
                else
                {
                    ShowTree(TreeView1, root.Children);
                }
            }
        }

        private void ShowTree<T>(ItemsControl control, List<T> list) where T : ITreeNode<T>
        {
            control.Items.Clear();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                TreeViewItem node = new TreeViewItem();
                node.Header = item.Name;
                node.Tag = item;
                control.Items.Add(node);
                ShowTree(node, item.Children);
            }
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
    }
}

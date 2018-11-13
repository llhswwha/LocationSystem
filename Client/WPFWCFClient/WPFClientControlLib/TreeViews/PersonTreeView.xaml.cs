using Location.IModel;
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
using TModel.Location.Nodes;
using WPFClientControlLib.Extensions;
using TreeNodeEntity = TModel.Location.Nodes.AreaNode;
namespace WPFClientControlLib
{
    /// <summary>
    /// PersonTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class PersonTreeView : UserControl
    {
        public PersonTreeView()
        {
            InitializeComponent();
        }

        public void LoadData(TreeNodeEntity root)
        {
            if (root == null)
            {
                ShowTree(TreeView1, null);
            }
            else
            {
                ShowTree(TreeView1, root.Children);
            }
        }

        public void ShowTree(ItemsControl control, List<TreeNodeEntity> list)
        {
            control.Items.Clear();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                TreeViewItem node = new TreeViewItem();
                if (item.TotalPersonCount > 0)
                { node.Header = string.Format("{0} ({1})", item.Name, item.TotalPersonCount); }
                else
                {
                    node.Header = string.Format("{0}", item.Name);
                }
                node.Tag = item;
                control.Items.Add(node);
                ShowTree(node, item.Children);

                if (item.Persons != null)
                    foreach (var leaf in item.Persons)
                    {
                        TreeViewItem subNode = new TreeViewItem();
                        subNode.Header = leaf.Name;
                        subNode.Tag = leaf;
                        subNode.Foreground = Brushes.Blue;
                        node.Items.Add(subNode);
                    }
            }
        }

        public void ExpandLevel(int level)
        {
            TreeView1.ExpandLevel(level);
        }
    }
}

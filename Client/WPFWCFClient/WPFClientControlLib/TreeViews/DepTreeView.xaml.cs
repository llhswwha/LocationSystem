using Location.TModel.Location.Person;
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
using WPFClientControlLib.Extensions;

namespace WPFClientControlLib
{
    /// <summary>
    /// DepTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class DepTreeView : UserControl
    {
        public DepTreeView()
        {
            InitializeComponent();
        }

        public void LoadData(Department root)
        {
            //TreeView1.ItemsSource = root.Children;
            if (root == null) return;
            ShowTree(TreeView1,root.Children);

        }

        public void ShowTree(ItemsControl control, List<Department> list)
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

                if(item.LeafNodes!=null)
                    foreach (var leaf in item.LeafNodes)
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

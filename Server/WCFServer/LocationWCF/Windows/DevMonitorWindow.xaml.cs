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
using System.Windows.Shapes;
using TModel.Location.AreaAndDev;

namespace LocationServer.Windows
{
    /// <summary>
    /// DevMonitorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DevMonitorWindow : Window
    {
        public DevMonitorWindow()
        {
            InitializeComponent();
        }

        public void Show(Dev_Monitor devMonitor)
        {
            CreateTree(devMonitor);
            this.Show();
        }

        private void CreateTree(Dev_Monitor devMonitor)
        {
            if (devMonitor == null) return;
            TreeViewItem root = GetTreeNode(devMonitor);
            TreeView1.Items.Add(root);
            CreateChildrenTreeNode(root, devMonitor);
        }

        private void CreateChildrenTreeNode(TreeViewItem parent, Dev_Monitor devMonitor)
        {
            if(devMonitor.ChildrenList!=null)
                foreach (Dev_Monitor monitor in devMonitor.ChildrenList)
                {
                    TreeViewItem subNode = GetTreeNode(monitor);
                    parent.Items.Add(subNode);

                    CreateChildrenTreeNode(subNode, monitor);
                }
        }

        private TreeViewItem GetTreeNode(Dev_Monitor devMonitor)
        {
            if (devMonitor == null) return null;
            TreeViewItem node = new TreeViewItem();
            node.Header = devMonitor.Name;
            node.Tag = devMonitor;
            return node;
        }

        private void TreeView1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem node = TreeView1.SelectedItem as TreeViewItem;
            if (node == null) return;
            Dev_Monitor monitor=node.Tag as Dev_Monitor;
            if (monitor == null) return;
            DataGrid1.ItemsSource = monitor.MonitorNodeList;
        }
    }
}

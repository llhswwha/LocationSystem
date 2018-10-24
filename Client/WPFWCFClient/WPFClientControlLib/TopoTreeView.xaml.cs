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

        public void LoadData(PhysicalTopology root)
        {
            TreeView1.ItemsSource = root.Children;
        }

        public void LoadData(Area root)
        {
            TreeView1.ItemsSource = root.Children;
        }

        private void TreeView1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }

        public TreeView Tree
        {
            get
            {
                return TreeView1;
            }
        }

        public object SelectedItem
        {
            get { return TreeView1.SelectedItem; }
        }
    }
}

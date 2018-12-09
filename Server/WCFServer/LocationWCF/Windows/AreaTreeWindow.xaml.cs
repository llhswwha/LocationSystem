using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;
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

namespace LocationServer.Windows
{
    /// <summary>
    /// AreaTreeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AreaTreeWindow : Window
    {
        public PhysicalTopology SelectedArea { get
            {
                return TopoTreeView1.SelectedObject as PhysicalTopology;
            }
             }

        public AreaTreeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new AreaService();
            var tree = service.GetTree();
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.ExpandLevel(2);
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

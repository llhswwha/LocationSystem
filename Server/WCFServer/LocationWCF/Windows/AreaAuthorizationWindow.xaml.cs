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
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;

namespace LocationServer.Windows
{
    /// <summary>
    /// AreaAuthorizationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AreaAuthorizationWindow : Window
    {
        public AreaAuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new AreaService();
            var tree=service.GetTree();

            TopoTreeView1.LoadData(tree);
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.SelectedObjectChanged += TopoTreeView1_SelectedObjectChanged;
        }

        private void TopoTreeView1_SelectedObjectChanged(object obj)
        {
            var entity = obj as PhysicalTopology;
            if (entity == null) return;

        }
    }
}

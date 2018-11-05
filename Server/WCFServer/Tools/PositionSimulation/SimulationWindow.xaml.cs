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
using BLL;
using DbModel.Location.AreaAndDev;
using LocationServer;
using LocationServices.Locations;
using LocationServices.Locations.Services;

namespace PositionSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        private Bll bll;

        public SimulationWindow()
        {
            InitializeComponent();
            bll = AppContext.GetLocationBll();
        }

        private void SimulationWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            AreaCanvas1.Init();
            LoadAreaTree();
            LoadDepTree();
            
        }

        private void LoadDepTree()
        {
            var service = new LocationService();
            var tree = service.GetDepartmentTree();
            DepTreeView1.LoadData(tree);
        }

        private void LoadAreaTree()
        {
            var tree = bll.GetAreaTree();
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
            TopoTreeView1.SelectFirst();
        }

        private Area area;

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            area = TopoTreeView1.SelectedObject as Area;
            if (area == null) return;
            //AreaCanvas1.ShowDev = true;
            AreaCanvas1.ShowArea(area);


            var service = new PersonService();
            var persons=service.GetListByArea(area.Id+"");

            AreaCanvas1.ShowPersons(persons);
        }
    }
}

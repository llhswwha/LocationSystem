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
using BLL;
using BLL.Initializers;
using DbModel.Location.Authorizations;
using DbModel.Location.Work;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations.Services;

namespace LocationServer.Windows
{
    /// <summary>
    /// AreaAuthorizationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AreaAuthorizationWindow : Window
    {

        private CardRole _role;

        private AreaAuthorizationRecordService aarService;
        private AreaAuthorizationService aaService;
        private AreaService areaService = new AreaService();

        public AreaAuthorizationWindow()
        {
            InitializeComponent();
        }

        public AreaAuthorizationWindow(CardRole role)
        {
            InitializeComponent();
            _role = role;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();
            areaService = new AreaService(bll);
            aarService = new AreaAuthorizationRecordService(bll);
            aaService=new AreaAuthorizationService(bll);

            if (_role != null)
            {
                var list1 = aarService.GetListByRole(_role.Id + "");
                var aaIds = new List<int>();
                foreach (var item in list1)
                {
                    aaIds.Add(item.AuthorizationId);
                }
                DataGrid2.ItemsSource = list1;
                var list2 = aaService.GetList(aaIds);
                DataGrid1.ItemsSource = list2;

                var areaIds = new List<int>();
                foreach (var item in list2)
                {
                    areaIds.Add(item.AreaId);
                }

                var tree = areaService.GetTree();
                //var nodes=tree.GetAllChildren(null);
                //foreach (var item in nodes)
                //{
                //    if (areaIds.Contains(item.Id))
                //    {
                //        item.IsChecked = true;
                //    }
                //}

                TopoTreeView1.LoadData(tree);

                foreach (var item in areaIds)
                {
                    var node = TopoTreeView1.GetAreaNode(item);
                    if(node!=null)
                        node.Foreground = Brushes.Blue;
                }

                TopoTreeView1.ExpandLevel(2);
                //TopoTreeView1.SelectedObjectChanged += TopoTreeView1_SelectedObjectChanged;
                TopoTreeView1.SelectFirst();
            }
            else
            {
                var tree = areaService.GetTree();
                TopoTreeView1.LoadData(tree);
                TopoTreeView1.ExpandLevel(2);
                TopoTreeView1.SelectedObjectChanged += TopoTreeView1_SelectedObjectChanged;
                TopoTreeView1.SelectFirst();
            }
        }

        //void LoadAreaTree()
        //{
        //    var tree = areaService.GetTree();
        //    TopoTreeView1.LoadData(tree);
        //    TopoTreeView1.ExpandLevel(2);
        //    TopoTreeView1.SelectedObjectChanged += TopoTreeView1_SelectedObjectChanged;
        //    TopoTreeView1.SelectFirst();
        //}

        private void TopoTreeView1_SelectedObjectChanged(object obj)
        {
            var area = obj as PhysicalTopology;
            if (area == null) return;
            DataGrid1.ItemsSource = aaService.GetListByArea(area.Id + "");
            DataGrid2.ItemsSource = aarService.GetListByArea(area.Id + "");
        }

        private void MenuAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new AreaAuthorizationRecordInfoWindow();
            win.Show();
        }

        private void MenuClear_OnClick(object sender, RoutedEventArgs e)
        {
            var bll = AppContext.GetLocationBll();
            DbInitializer initializer=new DbInitializer(bll);
            initializer.ClearAuthorization();
        }

        private void MenuInit_OnClick(object sender, RoutedEventArgs e)
        {
            var bll = AppContext.GetLocationBll();
            DbInitializer initializer = new DbInitializer(bll);
            initializer.InitAuthorization();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuAddRecord_OnClick(object sender, RoutedEventArgs e)
        {
            AreaAuthorization aa = DataGrid1.SelectedItem as AreaAuthorization;
            if (aa == null) return;
            var win = new CardRoleWindow();
            win.ShowOkButton();
            if (win.ShowDialog() == true)
            {
                var roles = win.GetSelectedRoles();
                foreach (var role in roles)
                {
                    AreaAuthorizationRecord aar = new AreaAuthorizationRecord(aa, role);
                    if (aarService.Post(aar) == null)
                    {
                        MessageBox.Show("分配权限失败");
                        break;
                    }
                }
            }
        }
    }
}

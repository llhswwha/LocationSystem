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
using DbModel.Location.Authorizations;
using BLL;
using Location.IModel;
using LocationServices.Locations;

namespace LocationServer.Windows
{
    /// <summary>
    /// CardRoleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CardRoleWindow : Window
    {
        public CardRoleWindow()
        {
            InitializeComponent();
        }

        public CardRoleWindow(int roleId)
        {
            InitializeComponent();
            this.roleId = roleId;
        }

        private int roleId = -1;

        public List<CardRole> _roles;
        Bll bll;
        AreaService areaService;
        AreaAuthorizationRecordService aarService;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bll = AppContext.GetLocationBll();
            areaService = new AreaService(bll);
            aarService = new AreaAuthorizationRecordService(bll);

            ITagRoleService service = new TagRoleService();
            _roles = service.GetList();

            var tree = areaService.GetTree();
            TopoTreeView1.LoadData(tree, false, true);
            TopoTreeView1.ExpandLevel(2);


            DataGrid1.ItemsSource = _roles;

            CardRole selectedRole = _roles.Find(i => i.Id == roleId);
            if (selectedRole != null)
            {
                selectedRole.IsChecked = true;
                DataGrid1.SelectedItem = selectedRole;
                //CheckRoleAreas(selectedRole.Id);
            }




        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var role = DataGrid1.SelectedItem as CardRole;
            if (role == null) return;
            var win = new AreaAuthorizationWindow(role);
            win.Show();
        }

        public CardRole Role
        {
            get { return DataGrid1.SelectedItem as CardRole; }
        }

        //public List<CardRole> GetSelectedRoles()
        //{
        //    List<CardRole> roles=new List<CardRole>();
        //    foreach (var role in _roles)
        //    {
        //        if (role.IsChecked)
        //        {
        //            roles.Add(role);
        //        }
        //    }
        //    return roles;
        //} 

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            //this.Close();
            DialogResult = true;
        }

        public void ShowOkButton()
        {
            ToolBar1.Visibility=Visibility.Visible;
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            CardRole role = DataGrid1.SelectedItem as CardRole;
            if (role == null) return;
            var nodes=TopoTreeView1.GetCheckedItems();
            List<int> ids = new List<int>();
            foreach (var node in nodes)
            {
                var tag = node.Tag;
                var id = (tag as IId).Id;
                ids.Add(id);
            }
            var result = new LocationService().SetCardRoleAccessAreas(role.Id, ids);
            if (result)
            {
                MessageBox.Show("修改成功");
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardRole role = DataGrid1.SelectedItem as CardRole;
            if (role == null) return;
            CheckRoleAreas(role.Id);

            //foreach (var item in _roles)
            //{
            //    item.IsChecked = false;
            //}

            //role.IsChecked = true;

            //DataGrid1.ItemsSource = _roles;
            //DataGrid1.SelectedItem = role;
        }

        private void CheckRoleAreas(int roleId)
        {
            var areas = new LocationService().GetCardRoleAccessAreas(roleId);
            TopoTreeView1.SetCheckByIds(areas);
        }
    }
}

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

        public IList<CardRole> _roles; 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ITagRoleService service = new TagRoleService();
            _roles = service.GetList();
            DataGrid1.ItemsSource = _roles;
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

        public List<CardRole> GetSelectedRoles()
        {
            List<CardRole> roles=new List<CardRole>();
            foreach (var role in _roles)
            {
                if (role.IsChecked)
                {
                    roles.Add(role);
                }
            }
            return roles;
        } 

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            //this.Close();
            DialogResult = true;
        }

        public void ShowOkButton()
        {
            ToolBar1.Visibility=Visibility.Visible;
        }
    }
}

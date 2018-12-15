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
    /// Interaction logic for TagWindow.xaml
    /// </summary>
    public partial class TagWindow : Window
    {
        public TagWindow()
        {
            InitializeComponent();
        }

        private TagService service;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            service = new TagService();
            LoadData();
        }

        private void LoadData()
        {
            DataGrid1.ItemsSource = service.GetList(true);
        }

        private void MenuSetRole_OnClick(object sender, RoutedEventArgs e)
        {
            var tag = DataGrid1.SelectedItem as Tag;
            if (tag == null) return;
            var win = new CardRoleWindow();
            win.ShowOkButton();
            if (win.ShowDialog()==true)
            {
                if (service.SetRole(tag.Id + "", win.Role.Id + "") == null)
                {
                    MessageBox.Show("设置失败");
                    return;
                }
                LoadData();
            }
        }

        private void MenuSetPerson_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuAuthorization_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

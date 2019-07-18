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
using Location.TModel.Location.Person;
using LocationServices.Locations.Services;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        public PersonWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        PersonService service = new PersonService();
        private void LoadData()
        {
            
            DataGrid1.ItemsSource = service.GetList(true,true);
        }

        public Personnel SelectedItem
        {
            get
            {
                return DataGrid1.SelectedItem as Personnel;
            }
        }

        private void MenuBindTag_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null) return;
            var win = new TagWindow();
            win.ShowOkButton();
            if (win.ShowDialog() == true)
            {
                if (win.SelectedItem == null) return;
                service.BindWithTag(SelectedItem.Id, win.SelectedItem.Id);
                LoadData();
            }
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void ShowOkButton()
        {
            ToolBar1.Visibility = Visibility.Visible;
        }
    }
}

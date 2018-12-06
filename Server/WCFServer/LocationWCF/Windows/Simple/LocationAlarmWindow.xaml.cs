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
using LocationServices.Locations.Services;

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// Interaction logic for LocationAlarmWindow.xaml
    /// </summary>
    public partial class LocationAlarmWindow : Window
    {
        public LocationAlarmWindow()
        {
            InitializeComponent();
        }

        LocationAlarmService service = new LocationAlarmService();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            LoadData();
        }

        private void LoadData()
        {
            DataGrid1.ItemsSource = service.GetList();
        }

        private void MenuClear_OnClick(object sender, RoutedEventArgs e)
        {
            service.Clear();
        }
    }
}

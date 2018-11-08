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

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for RealPosWindow.xaml
    /// </summary>
    public partial class RealPosWindow : Window
    {
        public RealPosWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var service = new PosService();
            DataGrid1.ItemsSource = service.GetList();
        }
    }
}

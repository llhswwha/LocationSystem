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

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for ArchorListWindow.xaml
    /// </summary>
    public partial class BusArchorListWindow : Window
    {
        public BusArchorListWindow()
        {
            InitializeComponent();
        }

        private void ArchorListWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = new Bll(false,false,false,false).bus_anchors.ToList();
        }
    }
}

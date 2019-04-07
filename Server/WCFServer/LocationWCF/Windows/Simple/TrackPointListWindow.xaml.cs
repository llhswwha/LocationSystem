using BLL;
using DbModel.Location.AreaAndDev;
using IModel.Enums;
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

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// TrackPointListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TrackPointListWindow : Window
    {
        public TrackPointListWindow()
        {
            InitializeComponent();
        }

        Bll bll = new Bll();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = bll.DevInfos.Where(i => i.Local_TypeCode == TypeCodes.TrackPoint);
        }

        private void MenuClear_Click(object sender, RoutedEventArgs e)
        {
            //bll.TrackPoints.Clear();
        }

        private void MenuExport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

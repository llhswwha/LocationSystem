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
using Location.TModel.Location.Alarm;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for DeviceAlarmListBox.xaml
    /// </summary>
    public partial class DeviceAlarmListBox : UserControl
    {
        public DeviceAlarmListBox()
        {
            InitializeComponent();
        }

        public void LoadData(DeviceAlarm[] list)
        {
            DataGrid1.ItemsSource = null;
            DataGrid1.ItemsSource = list;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

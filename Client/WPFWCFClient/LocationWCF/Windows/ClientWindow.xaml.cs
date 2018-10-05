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

namespace LocationWCFClient.Windows
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var client = AppContext.Instance.Client.InnerClient;
            var treeRoot1= client.GetPhysicalTopologyTree();
            var treeRoot2 = client.GetDepartmentTree();
            ResourceTreeView1.LoadData(treeRoot1, treeRoot2);

            var devList = client.GetDevInfos(null);
            DeviceListBox1.LoadData(devList);

            AppContext.Instance.CallbackClient.LocAlarmsReceved += CallbackClient_LocAlarmsReceved;
        }

        private void CallbackClient_LocAlarmsReceved(Location.TModel.Location.Alarm.LocationAlarm[] obj)
        {
            MessageBox.Show("告警推送");
        }
    }
}

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
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using LocationClient.WebApi;
using SignalRClientLib;

namespace LocationWCFClient.Windows
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private string SignalRServerURI = SignalRAppContext.ServerUrl;

        private AlarmHub alarmHub;

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

            var personList = client.GetPersonList();
            PersonListBox1.LoadData(personList);

            var tagList = client.GetTags();
            TagListBox1.LoadData(tagList);

            var archorList = client.GetArchors();
            AchorListBox1.LoadData(archorList);

            AppContext.Instance.CallbackClient.LocAlarmsReceved += CallbackClient_LocAlarmsReceved;

            DeviceAlarm[] devAlarms= client.GetDeviceAlarms(new AlarmSearchArg());
            ShowDeviceAlarms(devAlarms);

            LocationAlarm[] locAlarms = client.GetLocationAlarms(new AlarmSearchArg());
            ShowLocationAlarms(locAlarms);

            InitAlarmHub();
        }

        protected List<LocationAlarm> LocationAlarms = new List<LocationAlarm>();

        private void ShowLocationAlarms(LocationAlarm[] locAlarms)
        {
            LocationAlarms.AddRange(locAlarms);
            DataGridLocationAlarms.ItemsSource = null;
            DataGridLocationAlarms.ItemsSource = LocationAlarms;
        }

        private void ShowDeviceAlarms(DeviceAlarm[] devAlarms)
        {
            DeviceAlarms.AddRange(devAlarms);
            DeviceAlarmListBox1.LoadData(DeviceAlarms.ToArray());
        }

        protected List<DeviceAlarm> DeviceAlarms = new List<DeviceAlarm>();

        private async void InitAlarmHub()
        {
            alarmHub = new AlarmHub(SignalRServerURI);
            alarmHub.GetDeviceAlarms += AlarmHub_GetDeviceAlarms;
            alarmHub.GetLocationAlarms += AlarmHub_GetLocationAlarms;
            await alarmHub.Start();
        }

        private void AlarmHub_GetLocationAlarms(LocationAlarm[] obj)
        {
            this.Dispatcher.Invoke(() =>
                    ShowLocationAlarms(obj)
                );
        }

        private void AlarmHub_GetDeviceAlarms(DeviceAlarm[] obj)
        {
            this.Dispatcher.Invoke(() =>
                    ShowDeviceAlarms(obj)
                );
        }

        private void CallbackClient_LocAlarmsReceved(Location.TModel.Location.Alarm.LocationAlarm[] obj)
        {
            MessageBox.Show("告警推送");
        }

        private void MenuSignalR_Click(object sender, RoutedEventArgs e)
        {
            var window = new EchoHubWindow();
            window.Show();
        }

        private void MenuChatHubMenu_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChatHubWindow();
            window.Show();
        }

        private void MenuAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AlarmWindow();
            window.Show();
        }

        private void MenuWebApi_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new WebApiWindow();
            window.SetUrl(AppContext.Instance.GetWebApiUrl());
            window.Show();
        }

        private void BtnModifyArchor_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

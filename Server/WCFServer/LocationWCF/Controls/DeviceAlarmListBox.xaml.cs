using BLL;
using Location.BLL.Tool;
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using LocationServer.Tools;
using LocationServices.Locations;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using TModel.Tools;

namespace LocationServer.Controls
{
    /// <summary>
    /// DeviceAlarmListBox.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceAlarmListBox : UserControl
    {
        public DeviceAlarmListBox()
        {
            InitializeComponent();
        }

        private void BtnLoadDevAlarms_Click(object sender, RoutedEventArgs e)
        {
            LoadDeviceAlarms();
        }

        private List<DeviceAlarm> deviceAlarms;

        public void LoadDeviceAlarms(Action completed=null)
        {
            Worker.Run(() =>
            {
                Log.Info(LogTags.EventTest,"加载设备告警数据");
                var service = new LocationService();
                AlarmSearchArg arg = new AlarmSearchArg();
                arg.IsAll = true;
                deviceAlarms = service.GetDeviceAlarms(arg).devAlarmList;
            }, () =>
            {
                Log.Info(LogTags.EventTest, "加载设备告警数据-完成");
                DataGridDeviceAlarms.ItemsSource = deviceAlarms;
                if (completed != null)
                {
                    completed();
                }
            });
        }

        private void BtnRandomSelect100_OnClick(object sender, RoutedEventArgs e)
        {
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            int count = 100;
            Random r = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < count; i++)
            {
                int index = r.Next(deviceAlarms.Count);
                DeviceAlarm alarm = deviceAlarms[i];
                alarms.Add(alarm);
            }
            DataGridDeviceAlarms.ItemsSource = alarms;
        }

        BackgroundWorker sendDevAlarmsWorker;

        private void BtnFilterRepeatDev_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, DeviceAlarm> alarmDic = new Dictionary<int, DeviceAlarm>();
            foreach (var item in deviceAlarms)
            {
                if (alarmDic.ContainsKey(item.DevId))
                {

                }
                else
                {
                    alarmDic.Add(item.DevId, item);
                }
            }
            DataGridDeviceAlarms.ItemsSource = alarmDic.Values.ToList();
        }

        private void BtnSendDevAlarms_Click(object sender, RoutedEventArgs e)
        {
            if (BtnSendDevAlarms.Content.ToString() == "逐个发送告警")
            {
                BtnSendDevAlarms.Content = "停止发送告警";
                int interval = TxtSendDevAlarmInterval.Text.ToInt();
                int onceCount = TxtOnceSendCount.Text.ToInt();

                var currentDevAlarms = DataGridDeviceAlarms.ItemsSource as List<DeviceAlarm>;
                sendDevAlarmsWorker = Worker.Run(() =>
                {
                    List<DeviceAlarm> sendAlarms = new List<DeviceAlarm>();
                    for (int i = 0; i < currentDevAlarms.Count; i++)
                    {
                        var devAlarm = currentDevAlarms[i];
                        sendAlarms.Add(devAlarm);
                        if ((i + 1) % onceCount == 0)
                        {
                            Log.Info(LogTags.EventTest, string.Format("发送告警:{0}({1}/{2})", devAlarm, i + 1, currentDevAlarms.Count));
                            AlarmHub.SendDeviceAlarms(devAlarm);
                            Thread.Sleep(interval);
                        }
                    }
                }, null);
            }
            else
            {
                BtnSendDevAlarms.Content = "逐个发送告警";

                sendDevAlarmsWorker.CancelAsync();
            }
        }

        private void BtnSearchDevAlarms_Click(object sender, RoutedEventArgs e)
        {
            var key = TbSeachText.Text;
            if (string.IsNullOrEmpty(key))
            {
                DataGridDeviceAlarms.ItemsSource = deviceAlarms;
            }
            else
            {
                var type = CbSearchType.SelectedIndex;
                if (type == 0)
                {
                    DataGridDeviceAlarms.ItemsSource = deviceAlarms.FindAll(i => i.DevId == key.ToInt());
                }
            }
            
        }

        private void MenuSendDeviceAlarm_Click(object sender, RoutedEventArgs e)
        {
            var alarm = DataGridDeviceAlarms.SelectedItem as DeviceAlarm;
            AlarmHub.SendDeviceAlarms(alarm);
        }
    }
}

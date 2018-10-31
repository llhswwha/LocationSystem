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
using DbModel.Tools;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations;
using SignalRService.Hubs;
using TModel.Location.Data;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for EventSendTestWindow.xaml
    /// </summary>
    public partial class EventSendTestWindow : Window
    {
        public EventSendTestWindow()
        {
            InitializeComponent();
        }

        private void EventSendTestWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadDevList();
        }

        private void BtnPushAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            //LocationCallbackService.NotifyServiceStop();
            var service = new LocationService();
            var alarms = service.GetLocationAlarms(2);
            AlarmHub.SendLocationAlarms(alarms.ToArray());
        }
        private void BtnRemoveAlarm_Click(object sender, RoutedEventArgs e)
        {
            var service = new LocationService();
            var alarms = service.GetLocationAlarms(2, false);
            AlarmHub.SendLocationAlarms(alarms.ToArray());
        }
        private void MenuFireAlarm_Click(object sender, RoutedEventArgs e)
        {
            //var service = new LocationService();
            //var alarms = service.GetDeviceAlarms(2);
            //AlarmHub.SendDeviceAlarms(alarms);
        }

        private void MenuRecoverAlarm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeviceListBox1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSendMessage_OnClick(object sender, RoutedEventArgs e)
        {
            string msg = TbMessage.Text;
            ChatHub.SendToAll(msg);
        }

        private void LoadDevList()
        {
            LocationService service = new LocationService();
            var devlist = service.GetAllDevInfos();
            DeviceListBox1.LoadData(devlist);

            DeviceListBox1.AddMenu("告警", (se, arg) =>
            {
                //MessageBox.Show("告警" + DeviceListBox1.CurrentDev.Name);
                //todo:告警事件推送
                var dev = DeviceListBox1.CurrentDev;
                DeviceAlarm alarm = new DeviceAlarm()
                {
                    Id = dev.Id,
                    Level = Abutment_DevAlarmLevel.低,
                    Title = "告警" + dev.Id,
                    Message = "设备告警1",
                    CreateTime = new DateTime(2018, 8, 28, 9, 5, 34)
                }.SetDev(dev);
                AlarmHub.SendDeviceAlarms(alarm);
            });
            DeviceListBox1.AddMenu("消警", (se, arg) =>
            {
                //MessageBox.Show("消警" + DeviceListBox1.CurrentDev.Name);
                var dev = DeviceListBox1.CurrentDev;
                DeviceAlarm alarm = new DeviceAlarm()
                {
                    Id = dev.Id,
                    Level = Abutment_DevAlarmLevel.无,
                    Title = "消警" + dev.Id,
                    Message = "设备消警1",
                    CreateTime = new DateTime(2018, 8, 28, 9, 5, 34)
                }.SetDev(dev);
                AlarmHub.SendDeviceAlarms(alarm);
            });
        }

        /// <summary>
        /// 设置门禁信息
        /// </summary>
        private void SetDoorAccessInfo()
        {
            LocationService service = new LocationService();
            var devlist = service.GetAllDevInfos();
            var doorAccessList = service.GetAllDoorAccessInfo();
            if (devlist == null || doorAccessList == null) return;
            BindingDevInfo(devlist.ToList(), doorAccessList.ToList());
            DoorAccessListBox1.LoadData(doorAccessList.ToArray());

            DoorAccessListBox1.AddMenu("开门", (se, arg) =>
            {
                var dev = DoorAccessListBox1.CurrentDev;
                DoorAccessState doorAccessState = new DoorAccessState()
                {
                    DoorId = dev.DoorId,
                    Abutment_CardId = dev.Id.ToString(),
                    Abutment_CardState = "开",
                    Dev = dev.DevInfo
                };
                DoorAccessHub.SendDoorAccessInfo(doorAccessState);
            });
            DoorAccessListBox1.AddMenu("关门", (se, arg) =>
            {
                var dev = DoorAccessListBox1.CurrentDev;
                DoorAccessState doorAccessState = new DoorAccessState()
                {
                    DoorId = dev.DoorId,
                    Abutment_CardId = dev.Id.ToString(),
                    Abutment_CardState = "关",
                    Dev = dev.DevInfo
                };
                DoorAccessHub.SendDoorAccessInfo(doorAccessState);
            });
        }

        /// <summary>
        /// 门禁设备，绑定设备信息（For: DevInfo.Path）
        /// </summary>
        /// <param name="devList"></param>
        /// <param name="doorAccessList"></param>
        private void BindingDevInfo(List<DevInfo> devList, List<Dev_DoorAccess> doorAccessList)
        {
            foreach (var door in doorAccessList)
            {
                DevInfo info = devList.Find(i => i.DevID == door.DevID);
                if (info != null) door.DevInfo = info;
            }
        }
    }
}

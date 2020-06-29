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
using DbModel.Tools;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Locations;
using SignalRService.Hubs;
using TModel.Location.Data;
using IModel.Enums;
using Location.TModel.FuncArgs;
using TModel.Tools;
using LocationServer.Tools;
using System.Threading;
using System.ComponentModel;
using Location.BLL.Tool;
using WPFClientControlLib;
using DbModel.LocationHistory.Alarm;
using Location.TModel.Tools;

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
            LoadDeviceAlarms();
            SetDoorAccessInfo();
            tbLogController.Init(TbLogs, LogTags.EventTest);
        }

        public void LoadDeviceAlarms()
        {
            DeviceAlarmListBox1.LoadDeviceAlarms(()=>
            {
                Worker.Run(() =>
                {
                    Log.Info(LogTags.EventTest, "加载设备告警数据（历史）");
                    return Bll.Instance().DevAlarmHistorys.ToList();
                }, alarmHistory =>
                {
                    Log.Info(LogTags.EventTest, "加载设备告警数据（历史）-完成");
                    DeviceAlarmsHistory.ItemsSource = alarmHistory;
                });
            });
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
            DeviceListBox1.LoadData(devlist.ToList());

            DeviceListBox1.AddMenu("告警", (se, arg) =>
            {
                //MessageBox.Show("告警" + DeviceListBox1.CurrentDev.Name);
                //todo:告警事件推送
                //var dev = DeviceListBox1.CurrentDev;
                //DeviceAlarm alarm = CreateDevAlarm(dev);
                //AlarmHub.SendDeviceAlarms(alarm);
                var dev = DeviceListBox1.CurrentDev;
                DeviceAlarm alarm = CreateDevAlarm(dev);
                Bll bll = Bll.NewBllNoRelation();
                var hisItem = RemoveToHistory(alarm);
                bll.DevAlarmHistorys.Add(hisItem);
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
                    Message = "设备消警",
                    CreateTime = new DateTime(2018, 8, 28, 9, 5, 34)
                }.SetDev(dev);
                AlarmHub.SendDeviceAlarms(alarm);
            });
        }

        public DevAlarmHistory RemoveToHistory(DeviceAlarm alarm)
        {
            DevAlarmHistory newItem = new DevAlarmHistory();
            newItem.Id = alarm.Id;
            newItem.Abutment_Id = alarm.Abutment_Id;
            newItem.Title = alarm.Title;
            newItem.Msg = alarm.Message;
            newItem.Level = alarm.Level;
            newItem.Code = alarm.Code;
            newItem.Src = alarm.Src;
            newItem.DevInfoId = alarm.DevId;
            newItem.Device_desc = alarm.Device_desc;
            newItem.AlarmTime = alarm.CreateTime;
            newItem.AlarmTimeStamp = alarm.AlarmTimeStamp;
            newItem.HistoryTime = DateTime.Now;
            newItem.HistoryTimeStamp = TimeConvert.ToStamp((DateTime)newItem.HistoryTime);
            return newItem;
        }
        private DeviceAlarm CreateDevAlarm(DevInfo dev)
        {
            return new DeviceAlarm()
            {
                Id = dev.Id,
                Level = Abutment_DevAlarmLevel.低,
                Title = "告警" + dev.Id,
                Message = GetAlarmMsg(dev),
                CreateTime = new DateTime(2018, 8, 28, 9, 5, 34)
            }.SetDev(dev);
        }

        /// <summary>
        /// 获取告警信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        private string GetAlarmMsg(DevInfo dev)
        {
            string msg = "设备告警";
            string typecode = dev.TypeCode.ToString();
            if (TypeCodeHelper.IsBorderAlarmDev(typecode))
            {
                msg = string.Format("边界告警 : {0} 检测到非法越界.",dev.Name);
            }else if(TypeCodeHelper.IsAlarmDev(typecode))
            {
                msg= string.Format("消防告警 : {0} 消防装置被触发.", dev.Name);
            }
            return msg;
        }
        /// <summary>
        /// 设置门禁信息
        /// </summary>
        private void SetDoorAccessInfo()
        {
            IList<DevInfo> devlist = new List<DevInfo>();
            IList<Dev_DoorAccess> doorAccessList=new List<Dev_DoorAccess>();
            Worker.Run(() =>
            {
                LocationService service = new LocationService();
                devlist = service.GetAllDevInfos();
                doorAccessList = service.GetAllDoorAccessInfo();
                
            }, () =>
            {
                //MessageBox.Show("完成");
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


        private void BtnOnShowNoDevInHistory_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSendDeviceAlarmOfHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        LogTextBoxController tbLogController = new LogTextBoxController();

        BackgroundWorker sendSimulateDevAlarmsWorker;
        private void BtnSendDevSimulateAlarms_Click(object sender, RoutedEventArgs e)
        {
            if (BtnSendDevSimulateAlarms.Content.ToString() == "逐个发送告警")
            {
                BtnSendDevSimulateAlarms.Content = "逐个发送告警";
                int interval = TxtSendDevAlarmInterval.Text.ToInt();
                int onceCount = TxtOnceSendCount.Text.ToInt();

                var devList = DeviceListBox1.DeviceList;
                sendSimulateDevAlarmsWorker = Worker.Run(() =>
                {
                    List<DeviceAlarm> sendAlarms = new List<DeviceAlarm>();
                    for (int i = 0; i < devList.Count; i++)
                    {
                        var dev = devList[i];
                        DeviceAlarm devAlarm = CreateDevAlarm(dev);//创建设备告警
                        sendAlarms.Add(devAlarm);
                        if ((i + 1) % onceCount == 0)
                        {
                            Log.Info(LogTags.EventTest, string.Format("发送告警:{0}({1}/{2})", devAlarm, i + 1, devList.Count));
                            AlarmHub.SendDeviceAlarms(devAlarm);
                            Thread.Sleep(interval);
                        }
                    }
                }, null,null,"",true);
            }
            else
            {
                BtnSendDevSimulateAlarms.Content = "逐个发送告警";
                sendSimulateDevAlarmsWorker.CancelAsync();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            tbLogController.Dispose();
        }
    }
}

using LocationWCFService.ServiceHelper;
using LocationWCFServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DbModel.Tools;
using LocationServices.LocationCallbacks;
using LocationServices.Locations;
using LocationWCFService;
using System.Web.Http.SelfHost;
using WebApiService;
using LocationServices.Tools;
using Microsoft.Owin.Hosting;
using SignalRService.Hubs;

using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using BLL;
using DbModel.Location.Work;
using DbModel.LocationHistory.Data;
using EngineClient;
using Location.BLL;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using LocationServer;
using LocationServer.Windows;
using LocationServices.Converters;
using LocationServices.Locations.Interfaces;
using TModel.Location.AreaAndDev;
using TModel.Location.Data;
using TModel.Tools;
using WebNSQLib;
using LocationServer.Models.EngineTool;
using System.Text;
using LocationServer.Tools;
using LocationServer.Windows.Simple;

namespace LocationWCFServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            LogEvent.InfoEvent += LogEvent_InfoEvent;
        }

        private void LogEvent_InfoEvent(string obj)
        {
            //Location.BLL.Tool.Log.Info(obj);
        }

        private PositionEngineLog Logs = new PositionEngineLog();

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //LocationTestBox1.Logs = Logs;
            InitData();

            if (AppContext.AutoStartServer)
            {
                ServerManagerBox1.ClickStart();
            }

            if (EngineClientSetting.AutoStart)
            {
                ShowEngineClientWindow();
            }

            //var hex = "10 01 C0 01 02 85 A4 F4 8C C0 3A";
            //var bytes = ByteHelper.HexToBytes(hex);
            //var str = "85A4";
            //byte[] bytes1 = Encoding.UTF8.GetBytes(str);
            //byte[] bytes2 = Encoding.UTF32.GetBytes(str);
            //byte[] bytes4 = Encoding.ASCII.GetBytes(str);
            //byte[] bytes5 = Encoding.UTF7.GetBytes(str);
            //byte[] bytes6 = Encoding.Default.GetBytes(str);

            var version = ConfigurationHelper.GetValue("ServerVersionCode");

            this.Title += "    -v" + version;
        }

        private void InitData()
        {
            //StartReceiveAlarm();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            ServerManagerBox1.StopServices();
            ServerManagerBox1.StopListenLog();
        }


        //private void BtnConnectEngine_Click(object sender, RoutedEventArgs e)
        //{
        //    if (BtnConnectEngine.Content.ToString() == "连接定位引擎")
        //    {
        //        StartConnectEngine();
        //        BtnConnectEngine.Content = "断开定位引擎";
        //    }
        //    else
        //    {
        //        StopConnectEngine();
        //        BtnConnectEngine.Content = "连接定位引擎";
        //    }
        //}

        //public void StartConnectEngine()
        //{
        //    LocationTestBox1.StartConnectEngine();
        //}

        //public void StopConnectEngine()
        //{
        //    LocationTestBox1.StopConnectEngine();
        //}

        private void BtnOpenU3D_OnClick(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Location\\Location.exe";
            if (File.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                MessageBox.Show("未找到文件:" + path);
            }
        }

        private void MenuLocationEngionTool_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new LocationEngineToolWindow();
            win.Show();
        }

        private void MenuExportArchorPosition_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new BusAnchorListWindow();
            win.Show();
        }

        private void MenuAreaCanvas_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new AreaCanvasWindow();
            win.Show();
        }

        private void MenuArchorSettingExport_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ArchorListExportWindow();
            win.Show();
        }

        private void MenuDbExport_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new DbBrowserWindow();
            win.Show();
        }

        private void MenuDbInit_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new DbConfigureWindow();
            win.Show();
        }

        private void MenuLocationHistoryTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new LocationHistoryWindow();
            win.Show();
        }

        private void MenuEventSendTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new EventSendTestWindow();
            win.Show();
        }

        private void MenuEngineClient_OnClick(object sender, RoutedEventArgs e)
        {
            ShowEngineClientWindow();
        }

        private void ShowEngineClientWindow()
        {
            var win = new EngineClientWindow();
            win.Show();
        }

        private void MenuOpen3D_OnClick(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Location\\Location.exe";
            if (File.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                MessageBox.Show("未找到文件:" + path);
            }
        }

        private void MenuCmd1_OnClick(object sender, RoutedEventArgs e)
        {
            LocationService client = new LocationService();
            AreaStatistics recv = client.GetAreaStatistics(1);
            int PersonNum = recv.PersonNum;
            int DevNum = recv.DevNum;
            int LocationAlarmNum = recv.LocationAlarmNum;
            int DevAlarmNum = recv.DevAlarmNum;
        }

        private void MenuCardRole_Click(object sender, RoutedEventArgs e)
        {
            var win = new CardRoleWindow();
            win.Show();
        }

        private void MenuAreaAuthorization_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new AreaAuthorizationWindow();
            win.Show();
        }

        private void MenuTag_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new TagWindow();
            win.Show();
        }

        private void MenuPerson_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new PersonWindow();
            win.Show();
        }

        private void MenuRealPos_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new RealPosWindow();
            win.Show();
        }

        private void MenuArchorScane_Click(object sender, RoutedEventArgs e)
        {

            Bll bll = Bll.NewBllNoRelation();
            var list3 = bll.Archors.ToList();
            var areas = bll.Areas.ToList();
            if (list3 != null && areas != null)
                foreach (var item in list3)
                {
                    item.Parent = areas.Find(i => i.Id == item.ParentId);
                }

            var win = new ArchorConfigureWindow(list3);
            win.Show();

            //var win = new ArchorConfigureWindow();
            //win.Show();
        }

        private void MenuUDPSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuArchorCheck_Click(object sender, RoutedEventArgs e)
        {
            var win = new AnchorCheckWindow();
            win.Show();
        }

        private void MenuLocationAlarms_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new LocationAlarmWindow();
            win.Show();
        }

        private void MenuArchorSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ArchorWindow();
            win.Show();
        }

        private void MenuTrackPointList_Click(object sender, RoutedEventArgs e)
        {
            //var wind = new TrackPointListWindow();
            //wind.Show();

            var win = new ArchorListExportWindow();
            //win.CalculateAll = false;
            win.IsTrackPoint = true;
            win.Show();
        }

        private void GetAllNodeKKSData_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new KKSMonitorDataWindow();
            window.Show();
            
        }

        private void MenuCache_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            var bound1 = bll.Bounds.Find(i => i.Id == 1449);

            var bound2 = bll.Bounds.Find(i => i.Id == 1449);

            var bound3 = bll.Bounds.Find(i => i.Id == 1449);

            int nn = 0;
        }

        private void MenuClearDevAlarms_Click(object sender, RoutedEventArgs e)
        {
            Bll db = Bll.NewBllNoRelation();
            db.DevAlarms.Clear();
        }

        private void MenuGenerateDevAlarms_Click(object sender, RoutedEventArgs e)
        {
            Bll db = Bll.NewBllNoRelation();
            var devs = db.DevInfos.ToList();
            if (devs == null || devs.Count == 0) return;
            Random r = new Random((int)DateTime.Now.Ticks);
            DateTime now = DateTime.Now;
            for (int j = 0; j < 10; j++)
            {
                List<DeviceAlarm> alarms = new List<DeviceAlarm>();
                for (int i = 0; i < 10; i++)
                {
                    int devIndex = r.Next(devs.Count);
                    int month = r.Next(12) + 1;
                    int day = r.Next(28) + 1;
                    int hour = r.Next(24);
                    int m = r.Next(60);
                    int s = r.Next(60);
                    int lv = r.Next(5);
                    alarms.Add(new DeviceAlarm() { Level = (Abutment_DevAlarmLevel)lv, Title = "告警"+i, Message = "设备告警"+i, CreateTime = new DateTime(now.Year, now.Month, now.Day, hour, m, s) }.SetDev(devs[devIndex].ToTModel()));
                }
                db.DevAlarms.AddRange(alarms.ToDbModel());
            }
            MessageBox.Show("初始化成功");
        }

        private void MenuClearHisAlarms_Click(object sender, RoutedEventArgs e)
        {
            Bll db = Bll.NewBllNoRelation();
            DateTime now = DateTime.Now;
            //DateTime todayStart = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
            DateTime todayStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            DateTime todayEnd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
            var alarms=db.DevAlarms.Where(i => i.AlarmTime.Ticks < todayStart.Ticks);
            bool r=db.DevAlarms.RemoveList(alarms);
            MessageBox.Show("清空成功");
        }

        private void MenuUpdatePersons_Click(object sender, RoutedEventArgs e)
        {
            UpdatePersonWindow wnd = new UpdatePersonWindow();
            wnd.Show();
        }

        private void MenuException_Click(object sender, RoutedEventArgs e)
        {
            string s = null;
            double a = 1;
            double b = 0;
            double c = a / b;
            string s2 = s.ToString();
        }

        private void SyncAllData_Click(object sender, RoutedEventArgs e)
        {
            var win = new SyncAllDataWindow();
            win.Show();
        }

        private void KKSMonitorManager_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new KKSMonitorDataWindow();
            window.Show();
        }

        private void MenuInspectionTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new InspectionChoiceWindows();
            win.Show();
        }
    }
}

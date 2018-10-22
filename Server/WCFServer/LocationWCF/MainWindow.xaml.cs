using LocationWCFService.ServiceHelper;
using LocationWCFServices;
using System;
using System.Collections.Generic;
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
using DbModel.LocationHistory.Data;
using Location.BLL;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using LocationServer;
using LocationServices.Converters;
using LocationServices.Locations.Interfaces;
using TModel.Location.Data;
using WebNSQLib;

namespace LocationWCFServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer LogTimer;
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            LogTimer = new DispatcherTimer();
            LogTimer.Interval = TimeSpan.FromMilliseconds(200);
            LogTimer.Tick += LogTimer_Tick;
            LogTimer.Start();

            LogEvent.InfoEvent += LogEvent_InfoEvent;
        }

        private void LogEvent_InfoEvent(string obj)
        {
            Log.Info(obj);
        }

        private PositionEngineLog Logs = new PositionEngineLog();

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            TbResult2.Text = Logs.LogLeft;
            TbResult3.Text = Logs.LogRight;
        }


        //public void WriteLine(TextBox tb, string txt)
        //{
        //    tb.Dispatcher.BeginInvoke(new Action<string>((t) =>
        //    {
        //        tb.Text = txt + "\n" + tb.Text;
        //    }), txt);
        //}

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            //LoadDevList();

            //StartReceiveAlarm();

            //SetDoorAccessInfo();

            LocationEngineSettingBox1.LoadData();
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

        private void StartReceiveAlarm()
        {
            RealAlarm ra = new RealAlarm();
            ra.MessageHandler.DevAlarmReceived += Mh_DevAlarmReceived;
            if (alarmReceiveThread == null)
            {
                alarmReceiveThread = new Thread(ra.ReceiveRealAlarmInfo);
                alarmReceiveThread.Start();
            }
        }

        private void StopReceiveAlarm()
        {
            if (alarmReceiveThread != null)
            {
                alarmReceiveThread.Abort();
                alarmReceiveThread = null;
            }
        }

        private Thread alarmReceiveThread;

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

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            StopServices();
        }


        private void StopServices()
        {
            WriteLog("停止服务");
            StopConnectEngine();
            if (LocationService.u3dositionSP != null)
            {
                LocationService.u3dositionSP.Stop();
                LocationService.u3dositionSP = null;
            }

            if (httpHost != null)
            {
                httpHost.CloseAsync();
                httpHost = null;
            }
            if (SignalR != null)
            {
                SignalR.Dispose();
                SignalR = null;
            }

            if (wcfApiHost != null)
            {
                wcfApiHost.Close();
                wcfApiHost = null;
            }

            StopReceiveAlarm();
        }

        private void BtnStartService_Click(object sender, RoutedEventArgs e)
        {
            if (BtnStartService.Content.ToString() == "启动服务")
            {
                string host = TbHost.Text;
                string port = TbPort.Text;
                StartService(host, port);
                BtnStartService.Content = "停止服务";
            }
            else
            {
                StopServices();
                BtnStartService.Content = "启动服务";
            }
        }

        private void StartService(string host,string port)
        {
            try
            {
                WriteLog("启动服务");
                StartLocationService(host, port);
                StartLocationServiceApi(host, port);
                StartLocationAlarmService();
                StartWebApiService(host, port);
                StartSignalRService(host, port);

                //LocationService.ShowLog_Action += ShowTest;
                U3DPositionSP.ShowLog_Action += ShowTest;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        private IDisposable SignalR;

        private void StartSignalRService(string host, string port)
        {
            //端口和主服务器(8733)一致的情况下，2D和3D无法连接SignalR服务器
            port = "8735";
            string ServerURI = string.Format("http://{0}:{1}/", host,port);
            try
            {
                SignalR = WebApp.Start(ServerURI);
                WriteLog("SiganlR: " + ServerURI + "realtime");
            }
            catch (Exception ex)
            {
                //WriteToConsole("A server is already running at " + ServerURI);
                //this.Dispatcher.Invoke(() => ButtonStart.IsEnabled = true);
                //return;
                WriteLog(ex.ToString());
            }
        }

        private void WriteLog(string txt)
        {
            Log.Info(txt);
            TbResult1.AppendText(txt+"\n");
        }

        HttpSelfHostServer httpHost;

        private void StartWebApiService(string host, string port)
        {
            string path = string.Format("http://{0}:{1}/",host, port);
            var config = new HttpSelfHostConfiguration(path);
            WebApiConfiguration.Configure(config);
            httpHost = new HttpSelfHostServer(config);
            httpHost.OpenAsync().Wait();

            WriteLog("WebApiService: " + path+"api");
        }

        private ServiceHost locationServiceHost;

        private void StartLocationService(string host, string port)
        {
            //1.配置方式启动服务
            //locationServiceHost = new ServiceHost(typeof(LocationService));
            //locationServiceHost.SetProxyDataContractResolver();
            //locationServiceHost.Open();

            //2.编程方式启动服务
            string url = string.Format("http://{0}:{1}/LocationService", host, port);
            Uri baseAddres = new Uri(url);
            locationServiceHost = new ServiceHost(typeof (LocationService), baseAddres);
            BasicHttpBinding httpBinding = new BasicHttpBinding();
            locationServiceHost.AddServiceEndpoint(typeof (ILocationService), httpBinding, baseAddres);

            Binding binding = MetadataExchangeBindings.CreateMexHttpBinding();
            locationServiceHost.AddServiceEndpoint(typeof (IMetadataExchange), binding, "MEX");
            //开放数据交付终结点，客户端才能添加/更新服务引用。

            locationServiceHost.Open();

            WriteLog("LocationService: " + locationServiceHost.BaseAddresses[0]);
        }

        private WebServiceHost wcfApiHost;

        private void StartLocationServiceApi(string host, string port)
        {
            string path = string.Format("http://{0}:{1}/LocationService/api", host, port);
            //LocationService demoServices = new LocationService();
            wcfApiHost = new WebServiceHost(typeof(LocationService));
            WebHttpBinding httpBinding = new WebHttpBinding();
            wcfApiHost.AddServiceEndpoint(typeof(ITestService), httpBinding, new Uri(path+"/test"));//不能是相同的地址，不同的地址的话可以有多个。
            //wcfApiHost.AddServiceEndpoint(typeof(IDevService), httpBinding, new Uri(path));
            wcfApiHost.AddServiceEndpoint(typeof(IPhysicalTopologyService), httpBinding, new Uri(path));
            wcfApiHost.Open();
            WriteLog("LocationService: " + path);
        }

        private ServiceHost locationAlarmServiceHost;
        private void StartLocationAlarmService()
        {
            locationAlarmServiceHost = new ServiceHost(typeof(LocationCallbackService));
            locationAlarmServiceHost.SetProxyDataContractResolver();

            locationAlarmServiceHost.Open();

            WriteLog("LocationAlarmService: " + locationAlarmServiceHost.BaseAddresses[0]);
        }

        public void ShowTest(string str)
        {
            //textBox_U3DTEST.Text = str;
            //textBox_U3DTEST.AppendText( str);
        }


        private void BtnConnectEngine_Click(object sender, RoutedEventArgs e)
        {
            if (BtnConnectEngine.Content.ToString() == "连接定位引擎")
            {
                StartConnectEngine();
                BtnConnectEngine.Content = "断开定位引擎";
            }
            else
            {
                StopConnectEngine();
                BtnConnectEngine.Content = "连接定位引擎";
            }
        }

        public void StartConnectEngine()
        {
            LocationTestBox1.StartConnectEngine();
        }

        public void StopConnectEngine()
        {
            LocationTestBox1.StopConnectEngine();
        }

        private void BtnOpenSimulator_OnClick(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Simulator\\Simulator.exe";
            if (File.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                MessageBox.Show("未找到文件:" + path);
            }
        }

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

        private void BtnPushAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            //LocationCallbackService.NotifyServiceStop();
            var service=new LocationService();
            var alarms = service.GetLocationAlarms(2);
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
        private void Mh_DevAlarmReceived(DbModel.Location.Alarm.DevAlarm obj)
        {
            AlarmHub.SendDeviceAlarms(obj.ToTModel());
        }

        private void BtnSendMessage_OnClick(object sender, RoutedEventArgs e)
        {
            string msg = TbMessage.Text;
            ChatHub.SendToAll(msg);
        }

        private void BtnCreateHistoryPos_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();

            Position pos = PositionMocker.GetRandomPosition("223");
            pos.PersonnelID = 112;
            bll.Positions.Add(pos);

            DataGridHistoryPosList.ItemsSource = bll.Positions.ToList();
        }

        private void BtnGetHistoryList_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();
            DataGridHistoryPosList.ItemsSource = bll.Positions.ToList();
        }
    }
}

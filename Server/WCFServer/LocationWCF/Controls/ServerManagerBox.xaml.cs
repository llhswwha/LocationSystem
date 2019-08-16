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
using System.Windows.Controls;
using BLL;
using DbModel.LocationHistory.Data;
using EngineClient;
using Location.BLL;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.AreaAndDev;
using LocationServer;
using LocationServer.Windows;
using LocationServices.Converters;
using LocationServices.Locations.Interfaces;
using TModel.Location.Data;
using TModel.Tools;
using WebNSQLib;
using LocationServer.Tools;
using System.Net;
using WebApiCommunication.ExtremeVision;
using Newtonsoft.Json;
using Location.BLL.Tool;
using System.Text;
using WebApiLib.Clients;
using NsqSharp.Utils;
using BLL.Blls.Location;
using BLL.Blls;
using DbModel.Location.Alarm;
using DbModel.Others;
using NVSPlayer;
using DbModel.Location.AreaAndDev;
using System.Collections.Concurrent;

namespace LocationServer.Controls
{
    /// <summary>
    /// Interaction logic for ServerManagerBox.xaml
    /// </summary>
    public partial class ServerManagerBox : UserControl
    {
        public ServerManagerBox()
        {
            InitializeComponent();
            Location.BLL.Tool.Log.NewLogEvent += ListenToLog;
            GoFunc.InfoEvent += GoFunc_InfoEvent;

            TbPort.Text = ConfigurationHelper.GetValue("Port");
            TbHost.Text = ConfigurationHelper.GetValue("Host");
        }

        private void GoFunc_InfoEvent(string obj)
        {
            Log.Info(obj);
        }

        public void StopListenLog()
        {
            Location.BLL.Tool.Log.NewLogEvent -= ListenToLog;
            GoFunc.InfoEvent -= GoFunc_InfoEvent;
        }

        public void StopServices()
        {
            try
            {
                WriteLog("停止服务");
                //StopConnectEngine();
                if (LocationService.u3dositionSP != null)
                {
                    LocationService.u3dositionSP.Stop();
                    LocationService.u3dositionSP = null;
                    WriteLog("停止LocationService.u3dositionSP");
                }
                if (locationServiceHost != null)
                {
                    locationServiceHost.Close();
                    locationServiceHost = null;
                    WriteLog("停止locationServiceHost");
                }
                if (httpHost != null)
                {
                    httpHost.CloseAsync();
                    httpHost = null;
                    WriteLog("停止httpHost");
                }
                if (SignalR != null)
                {
                    SignalR.Dispose();
                    SignalR = null;
                    WriteLog("停止SignalR");
                }

                if (wcfApiHost != null)
                {
                    wcfApiHost.Close();
                    wcfApiHost = null;
                    WriteLog("停止wcfApiHost");
                }
                
                StopReceiveAlarm();
                StopLocationAlarmService();
                StopGetInspectionTrack();
                StopExtremeVisionListener();
                StopGetHistoryPositon();

                WriteLog("SisDataSaveClient.Stop");
                SisDataSaveClient.Stop();

                WriteLog("NVSManage.Stop");
                NVSManage.Stop();
                if (nginxCmdProcess != null)
                {
                    nginxCmdProcess.CloseMainWindow();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        //private void Start


        private void StartReceiveAlarm()
        {
            bool enableAlarmRecieve = ConfigurationHelper.GetBoolValue("EnableAlarmRecieve");
            if (!enableAlarmRecieve) return;
            RealAlarm ra = new RealAlarm(Mh_DevAlarmReceived);
            if (alarmReceiveThread == null)
            {
                alarmReceiveThread = new Thread(ra.ReceiveRealAlarmInfo);
                alarmReceiveThread.IsBackground = true;
                alarmReceiveThread.Start();
            }

            //RecvBaseCommunication Rbc = new RecvBaseCommunication();
            //Rbc.RvoPa.DevAlarmReceived += Mh_DevAlarmReceived;
            //Rbc.Raca.DevAlarmReceived += Mh_DevAlarmReceived;
            //Rbc.Rfa.DevAlarmReceived += Mh_DevAlarmReceived;
            //if (alarmReceiveThread == null)
            //{
            //    alarmReceiveThread = new Thread(Rbc.StartConnectTopic);
            //    alarmReceiveThread.IsBackground = true;
            //    alarmReceiveThread.Start();
            //}
        }

        private void Mh_DevAlarmReceived(DbModel.Location.Alarm.DevAlarm obj)
        {
            AlarmHub.SendDeviceAlarms(obj.ToTModel());
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

        private void BtnStartService_Click(object sender, RoutedEventArgs e)
        {
            ClickStart();
        }

        public void ClickStart()
        {
            if (BtnStartService.Content.ToString() == "启动服务")
            {
                string host = TbHost.Text;
                string port = TbPort.Text;

                //StartService(host, port);
                //BtnStartService.Content = "停止服务";

                if (logTimer == null)
                {
                    logTimer = new DispatcherTimer();
                    logTimer.Interval = TimeSpan.FromMilliseconds(300);
                    logTimer.Tick += LogTimer_Tick;
                }
                logTimer.Start();

                Worker.Run(() =>
                {
                    StartService(host, port);
                }, () =>
                {
                    StartPlayBackManage();//不能放到线程里面运行

                    BtnStartService.Content = "停止服务";
                });
            }
            else
            {
                StopServices();
                BtnStartService.Content = "启动服务";
            }
        }

        public void StopLogTimer()
        {
            if (logTimer != null)
            {
                logTimer.Stop();
            }
        }

        private void CheckCardRole()
        {
            var bll = Bll.NewBllNoRelation();
            var cards = bll.LocationCards.ToList();
            var roles = bll.Roles.ToList();
            //var noRoleCards = cards.Where(i => i.CardRoleId == null);
        }

        private void StartService(string host, string port)
        {
            try
            {
                AppContext.CurrentHost = host;
                AppContext.CurrentPort = port;

                WriteLog("启动服务");

                //WCF服务 项目：Location.Service
                StartLocationService(host, port);

                //基于WCF接口同时兼容的WebApi服务 http://{0}:8733/LocationService/api WebServiceHost 项目：Location.Service
                StartLocationServiceApi(host, port);

                ////定位告警回调服务（没用）, 基于 LocationCallbackService，在unity中不支持，无法使用。 项目：Location.Service 端口8734
                //StartLocationAlarmService();

                //设备告警对接服务 基于NSQ消息队列获取第三方告警信息 项目：WebNSQLib
                StartReceiveAlarm();

                //WebApi服务 主要是和这个 //http://{0}:{1}/ HttpSelfHostServer
                StartWebApiService(host, port);
                //用来代替StartLocationAlarmService发送信息给unity，推送消息给客户端。项目：SignalRService
                StartSignalRService(host, port);

                //移动巡检信息获取客户端 轮询获取 WebApi 项目：WebApiClients
                StartGetInspectionTrack();

                //上海宝信项目对接视频行为告警 基于HttpListener 端口8736
                StartExtremeVisionListener();//端口要不同

                Worker.Run(() =>
                {
                    try
                    {
                        CheckCardRole();//检查人员角色，发现有些定位卡没有绑定卡角色


                        Bll bll = Bll.Instance();
                        var count = bll.Areas.DbSet.Count(); //用于做数据迁移用，查询一下
                        if (count == 0)
                        {
                            Log.Error(LogTags.Server, bll.Areas.ErrorMessage);
                        }


                        LocationService.RefreshDeviceAlarmBuffer();//实现加载全部设备告警到内存中
                    }
                    catch (Exception e)
                    {
                        Log.Info(e);
                        Log.Error(LogTags.Server, "数据迁移出错！！：" + e.Message);
                    }
                }, null);

                StartGetHistoryPositon();//将定位历史数据保存到缓存中


               
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public void StartPlayBackManage()
        {
            var EnabelNVS = ConfigurationHelper.GetBoolValue("EnabelNVS");
            if (EnabelNVS)
            {
                NVSManage.RTMP_Host = ConfigurationHelper.GetValue("RTMP_Host");

                NVSManage.NVRIP = ConfigurationHelper.GetValue("NVRIP");
                NVSManage.NVRPort = ConfigurationHelper.GetValue("NVRPort");
                NVSManage.NVRUser = ConfigurationHelper.GetValue("NVRUser");
                NVSManage.NVRPass = ConfigurationHelper.GetValue("NVRPass");
                NVSManage.Init();//启动天地伟业Playback界面

                string nginx = AppDomain.CurrentDomain.BaseDirectory + "\\nginx-1.7.11.3-Gryphon\\restart-rtmp.bat";
                if (File.Exists(nginx))
                {
                    nginxCmdProcess=Process.Start(nginx);//启动nginx-rtmp
                }
                else
                {
                    WriteLog("找不到nginx启动文件:"+ nginx);
                }
            }
        }

        private Process nginxCmdProcess;

        CameraAlarmListener cameraAlarmListener;

        private void StartExtremeVisionListener()
        {
            bool enableVisionListener = ConfigurationHelper.GetBoolValue("EnableVisionListener");
            if (!enableVisionListener) return;
            string port = ConfigurationHelper.GetValue("ExtremeVisionListenerPort");
            string host = ConfigurationHelper.GetValue("ExtremeVisionListenerIP") ;
            int saveMode = ConfigurationHelper.GetIntValue("CameraAlarmPicSaveMode");
            if (cameraAlarmListener == null)
            {
                string url = string.Format("http://{0}:{1}/listener/ExtremeVision/callback/",host,port);
                cameraAlarmListener = new CameraAlarmListener(url, saveMode);
                bool r=cameraAlarmListener.Start();
                WriteLog("HttpListener: " + url+" ["+r+"]");
            }
        }

        private void StopExtremeVisionListener()
        {
            WriteLog("StopExtremeVisionListener");
            if (cameraAlarmListener != null)
            {
                cameraAlarmListener.Stop();
                cameraAlarmListener = null;
            }
        }


        public void ShowTest(string str)
        {
            //textBox_U3DTEST.Text = str;
            //textBox_U3DTEST.AppendText( str);
        }

        private IDisposable SignalR;

        private void StartSignalRService(string host, string port)
        {
            //端口和主服务器(8733)一致的情况下，2D和3D无法连接SignalR服务器
            port = "8735";
            //string ServerURI = string.Format("http://{0}:{1}/", host,port);
            string ServerURI = string.Format("http://{0}:{1}/", "*", port);
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

        private string serverLogs = "";

        private bool serverLogsChanged = false;

        private string clientLogs = "";

        private ConcurrentBag<LogInfo> clientLogInfos = new ConcurrentBag<LogInfo>();

        private bool clientLogsChanged = false;

        private void WriteLog(string txt)
        {
            //string log = string.Format("[{0}][{1}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), txt);
            Location.BLL.Tool.Log.Info(LogTags.Server,txt);
        }

        private int MaxLength = 100000;
        private int MaxLength2 = 50000;

        private DispatcherTimer logTimer;

        private void ListenToLog(LogInfo info)
        {
            try
            {
                string tag = info.Tag;
                if (serverLogs.Length > MaxLength)
                {
                    serverLogs = serverLogs.Substring(0, MaxLength2);
                }

                if (clientLogs.Length > MaxLength)
                {
                    clientLogs = clientLogs.Substring(0, MaxLength2);
                }


                //string[] parts = log.Split('|');
                if (tag == LogTags.Server 
                    //|| tag == LogTags.ExtremeVision
                    )
                {
                    serverLogs = info.Log + "\n" + serverLogs;
                    serverLogsChanged = true;
                }
                else
                {
                    if (tag != LogTags.Engine && tag != LogTags.BaseData)
                    {
                        //clientLogs = log + "\n" + clientLogs;
                        clientLogInfos.Add(info);

                        clientLogsChanged = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.ToString());
            }
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            if (serverLogsChanged)
            {
                TbServerLog.Text = serverLogs;
                serverLogsChanged = false;
            }

            if (clientLogsChanged)
            {
                //TbClientLog.Text = clientLogs;

                var temp = new List<LogInfo>();
                lock (clientLogInfos)
                {
                    temp.AddRange(clientLogInfos);
                    clientLogInfos = new ConcurrentBag<LogInfo>();
                }
                while (temp.Count > 0)
                {
                    var log = temp.First();
                    temp.RemoveAt(0);
                    LogTabControl1.AddLog(log);
                }
                clientLogsChanged = false;
            }
        }

        HttpSelfHostServer httpHost;

        private void StartWebApiService(string host, string port)
        {
            string path = string.Format("http://{0}:{1}/", host, port);
            var config = new HttpSelfHostConfiguration(path);
            WebApiConfiguration.Configure(config);
            httpHost = new HttpSelfHostServer(config);
            httpHost.OpenAsync().Wait();

            WriteLog("WebApiService: " + path + "api");

            if(AppContext.DatacaseWebApiUrl== "simulate")
            {
                AppContext.DatacaseWebApiUrl = string.Format("{0}:{1}/datacase", host, port);
            }
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
            locationServiceHost = new ServiceHost(typeof(LocationService), baseAddres);
            BasicHttpBinding httpBinding = new BasicHttpBinding();
            //httpBinding.MaxReceivedMessageSize = 2147483647;
            //httpBinding.MaxBufferPoolSize = 2147483647;

            //Binding httpBinding = new WSHttpBinding();
            locationServiceHost.AddServiceEndpoint(typeof(ILocationService), httpBinding, baseAddres);
            Binding binding = MetadataExchangeBindings.CreateMexHttpBinding();
            locationServiceHost.AddServiceEndpoint(typeof(IMetadataExchange), binding, "MEX");
            //开放数据交付终结点，客户端才能添加/更新服务引用。
            ServiceThrottlingBehavior throttle = locationServiceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior();
                locationServiceHost.Description.Behaviors.Add(throttle);
            }
            throttle.MaxConcurrentCalls = ConfigurationHelper.GetIntValue("MaxConcurrentCalls");
            throttle.MaxConcurrentSessions = ConfigurationHelper.GetIntValue("MaxConcurrentSessions");
            throttle.MaxConcurrentInstances = ConfigurationHelper.GetIntValue("MaxConcurrentInstances");

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
            wcfApiHost.AddServiceEndpoint(typeof(ITestService), httpBinding, new Uri(path + "/test"));//不能是相同的地址，不同的地址的话可以有多个。
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

        private void StopLocationAlarmService()
        {
            if (locationAlarmServiceHost!=null)
            {
                locationAlarmServiceHost.Close();
                WriteLog("StopLocationAlarmService: " + locationAlarmServiceHost.BaseAddresses[0]);
            }
        }


        InspectionTrackClient trackClient;//移动巡检

        private void StopGetInspectionTrack()
        {
            WriteLog("StopGetInspectionTrack");
            if (trackClient != null)
            {
                trackClient.Stop();
                trackClient = null;
            }
        }

        private void StartGetInspectionTrack()
        {
            bool EnableInspectionTrack= ConfigurationHelper.GetBoolValue("EnableInspectionTrack");

            if (EnableInspectionTrack && trackClient == null)
            {
                string strIp = AppContext.DatacaseWebApiUrl;
                trackClient = new InspectionTrackClient(strIp);
                trackClient.ListGot += (TrackList) =>
                {
                    InspectionTrackHub.SendInspectionTracks(TrackList.ToTModel());//发送给客户端
                };
                trackClient.Start();

                WriteLog("StartGetInspectionTrack:"+ strIp);
            }
        }

        //private void GetInspectionTrack()
        //{
        //    string strIp = AppContext.DatacaseWebApiUrl;
        //    InspectionTrackClient client = new InspectionTrackClient(strIp);
        //    client.ListGot += (list) =>
        //    {
        //        SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(list.ToWcfModelList().ToArray());
        //    };
        //    client.Start();
        //}

        private Thread HPThread;
        private void StartGetHistoryPositon()
        {
            bool EnableHistoryBuffer = ConfigurationHelper.GetBoolValue("EnableHistoryBuffer");
            if (EnableHistoryBuffer)
            {
                WriteLog("启动定位历史数据缓存");

                if (HPThread == null)
                {
                    var Phs = new LocationServices.Locations.Services.PosHistoryService();
                    HPThread = new Thread(Phs.GetHistoryPositonThread);
                    HPThread.IsBackground = true;
                    HPThread.Start();
                }
            }
        }
  
        private void StopGetHistoryPositon()
        {
            WriteLog("StopGetHistoryPositon");
            if (HPThread != null)
            {
                HPThread.Abort();
                HPThread = null;
            }
        }

        private void BtnClearServerLogs_OnClick(object sender, RoutedEventArgs e)
        {
            serverLogs = "";
            serverLogsChanged = true;
        }

        private void BtnClearClientLogs_OnClick(object sender, RoutedEventArgs e)
        {
            clientLogs = "";
            clientLogsChanged = true;
        }
    }
}

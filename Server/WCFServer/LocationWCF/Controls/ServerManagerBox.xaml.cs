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
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        //private void Start


        private void StartReceiveAlarm()
        {
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
                StartService(host, port);
                BtnStartService.Content = "停止服务";
            }
            else
            {
                StopServices();
                BtnStartService.Content = "启动服务";
            }
        }

        private void StartService(string host, string port)
        {
            try
            {
                WriteLog("启动服务");

                StartLocationService(host, port);
                StartLocationServiceApi(host, port);
                StartReceiveAlarm();
                StartLocationAlarmService();
                StartWebApiService(host, port);
                StartSignalRService(host, port);
                StartGetInspectionTrack();

                string port2 = ConfigurationHelper.GetValue("ExtremeVisionListenerPort");
                StartExtremeVisionListener(host, port2);//端口要不同
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        MyHttpListener httpListener;

        private void StartExtremeVisionListener(string host, string port)
        {
            if (httpListener == null)
            {
                string url = string.Format("http://{0}:{1}/listener/ExtremeVision/callback/",host,port);
                httpListener = new MyHttpListener(url);
                httpListener.OnReceived += (json) =>
                {
                    Log.Info(LogTags.ExtremeVision,string.Format("收到消息({0})", url));
                    Log.Info(LogTags.ExtremeVision, json);
                    DateTime now = DateTime.Now;
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\" + now.ToString("yyyy_mm_dd_HH_MM_ss_fff") + ".json";
                    File.WriteAllText(path, json);
                    CameraAlarmInfo info = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
                    //todo：发送告警
                    CameraAlarmHub.SendInfo(info);
                    return info.ToString();
                };
                httpListener.Start();
                WriteLog("ExtremeVisionListener: " + url);
            }
        }

        private void StopExtremeVisionListener()
        {
            if (httpListener != null)
            {
                httpListener.Stop();
                httpListener = null;
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

        string serverLogs = "";

        string clientLogs = "";

        private void WriteLog(string txt)
        {
            //string log = string.Format("[{0}][{1}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), txt);
            Location.BLL.Tool.Log.Info(LogTags.Server,txt);
        }

        private int MaxLength = 100000;
        private int MaxLength2 = 50000;

        private void ListenToLog(string tag,string log)
        {
            if (serverLogs.Length > MaxLength)
            {
                serverLogs = serverLogs.Substring(0,MaxLength2);
            }

            if (clientLogs.Length > MaxLength)
            {
                clientLogs = clientLogs.Substring(0, MaxLength2);
            }

            //string[] parts = log.Split('|');
            if (tag == LogTags.Server || tag == LogTags.ExtremeVision)
            {
                serverLogs = log + "\n" + serverLogs;
                TbServerLog.Dispatcher.Invoke(() =>
                {
                    TbServerLog.Text = serverLogs;
                });
            }
            else
            {
                if(tag != LogTags.Engine && tag != LogTags.BaseData)
                {
                    clientLogs = log + "\n" + clientLogs;
                    TbClientLog.Dispatcher.Invoke(() =>
                    {
                        TbClientLog.Text = clientLogs;
                    });
                }
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

            WriteLog("StartLocationAlarmService: " + locationAlarmServiceHost.BaseAddresses[0]);
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
                trackClient.ListGot += (list) =>
                {
                    SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(list.ToWcfModelList().ToArray());
                };
                trackClient.Start();

                WriteLog("StartGetInspectionTrack");
            }
        }

        private void GetInspectionTrack()
        {
            string strIp = AppContext.DatacaseWebApiUrl;
            InspectionTrackClient client = new InspectionTrackClient(strIp);
            client.ListGot += (list) =>
            {
                SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(list.ToWcfModelList().ToArray());
            };
            client.Start();
        }

    }
}

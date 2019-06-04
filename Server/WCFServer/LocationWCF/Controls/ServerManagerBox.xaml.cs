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
        }

        public void StopListenLog()
        {
            Location.BLL.Tool.Log.NewLogEvent -= ListenToLog;
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
            }
            catch (Exception ex)
            {
                ListenToLog(ex.ToString());
            }
        }


        private void StartReceiveAlarm()
        {
            RealAlarm ra = new RealAlarm();
            ra.MessageHandler.DevAlarmReceived += Mh_DevAlarmReceived;
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
            }
            catch (Exception ex)
            {
                ListenToLog(ex.ToString());
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

        string logText = "";

        private void WriteLog(string txt)
        {
            //string log = string.Format("[{0}][{1}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), txt);
            Location.BLL.Tool.Log.Info(txt);
        }

        private void ListenToLog(string log)
        {

            if (logText.Length > 20000)
            {
                logText = "";
            }
            logText = log + "\n" + logText;
            TbResult1.Dispatcher.Invoke(() =>
            {
                TbResult1.Text = logText;
            });
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


        

        private Thread GetInspectionTrackThread;

        private void StopGetInspectionTrack()
        {
            if (GetInspectionTrackThread != null)
            {
                GetInspectionTrackThread.Abort();
                GetInspectionTrackThread = null;
            }
        }

        private void StartGetInspectionTrack()
        {
            if (GetInspectionTrackThread == null)
            {
                GetInspectionTrackThread = new Thread(GetInspectionTrack);
                GetInspectionTrackThread.IsBackground = true;
                GetInspectionTrackThread.Start();
            }

            WriteLog("StartGetInspectionTrack");
        }

        private int nEightHourSecond = 28800;

        private void GetInspectionTrack()
        {
             string strIp = ConfigurationManager.AppSettings["DatacaseWebApiUrl"];
            //string datacaseUrl = string.Format("http://{0}:{1}/", strIp);
  
            var client = new WebApiLib.Clients.BaseDataClient(strIp, "api");
            bool bFirst = true;
            int nDay = -1;
            Bll bll = new Bll();

            while (true)
            {
                DateTime dtEnd = DateTime.Now;
                DateTime dtBegin = dtEnd.AddDays(nDay);
                if (bFirst)
                {
                    dtBegin = dtEnd.AddDays(-3);
                    bFirst = false;
                }

                if (DealInspectionTrack(client, dtBegin, dtEnd, true) == false)
                {
                    WriteLog("获取巡检轨迹失败！！ break!!");
                    break;
                }

                List<DbModel.Location.Work.InspectionTrack> send = bll.InspectionTracks.ToList();
                //List<InspectionTrackHistory> send2 = bll.InspectionTrackHistorys.ToList();
                if (send == null || send.Count() == 0)
                {
                    Thread.Sleep(5 * 60 * 1000);//等待5分钟
                    continue;
                }
                SignalRService.Hubs.InspectionTrackHub.SendInspectionTracks(send.ToWcfModelList().ToArray());
                //DateTime dt2 = DateTime.Now;
                Thread.Sleep(5 * 60 * 1000);//等待5分钟
            }
        }

        private bool DealInspectionTrack(WebApiLib.Clients.BaseDataClient client, DateTime dtBegin, DateTime dtEnd, bool bFlag)
        {
            List<DbModel.Location.Work.InspectionTrack> All = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.Location.Work.InspectionTrack> Add = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.Location.Work.InspectionTrack> Edit = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.Location.Work.InspectionTrack> Delete = new List<DbModel.Location.Work.InspectionTrack>();
            List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>();

            long lBegin = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtBegin) / 1000;
            long lEnd = Location.TModel.Tools.TimeConvert.DateTimeToTimeStamp(dtEnd) / 1000;
            
            var recv = client.Getinspectionlist(lBegin, lEnd, true);
            if (recv == null)
            {
                return false;
            }

            Bll bll = new Bll(false, false, true, false);//第三参数要设置为true
            List<DbModel.Location.Work.InspectionTrack> itList = bll.InspectionTracks.ToList();
            if (itList == null)
            {
                itList = new List<DbModel.Location.Work.InspectionTrack>();
            }

            List<DbModel.LocationHistory.Work.InspectionTrackHistory> itHList = bll.InspectionTrackHistorys.ToList();
            if (itHList == null)
            {
                itHList = new List<DbModel.LocationHistory.Work.InspectionTrackHistory>(0);
            }

            foreach (CommunicationClass.SihuiThermalPowerPlant.Models.patrols item in recv)
            {
                DbModel.Location.Work.InspectionTrack now = itList.Find(p => p.Abutment_Id == item.id);
                DbModel.LocationHistory.Work.InspectionTrackHistory history = itHList.Find(p => p.Abutment_Id == item.id);

                if (item.state == "新建" || item.state == "已下达" || item.state == "执行中")
                {
                    if (now == null)
                    {
                        now = new DbModel.Location.Work.InspectionTrack();

                        now.Abutment_Id = item.id;
                        now.Code = item.code;
                        now.Name = item.name;
                        now.CreateTime = (item.createTime + nEightHourSecond) * 1000;
                        now.dtCreateTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(now.CreateTime);
                        now.State = item.state;
                        now.StartTime = (item.startTime + nEightHourSecond) * 1000;
                        now.dtStartTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(now.StartTime);
                        now.EndTime = (item.endTime + nEightHourSecond) * 1000;
                        now.dtEndTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(now.EndTime);
                        Add.Add(now);
                    }
                    else
                    {
                        now.State = item.state;
                        Edit.Add(now);
                    }
                }
                else
                {
                    if (now != null)
                    {
                        Delete.Add(now);
                    }

                    if (history == null)
                    {
                        history = new DbModel.LocationHistory.Work.InspectionTrackHistory();

                        history.Abutment_Id = item.id;
                        history.Code = item.code;
                        history.Name = item.name;
                        history.CreateTime = (item.createTime + nEightHourSecond) * 1000;
                        history.dtCreateTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(history.CreateTime);
                        history.State = item.state;
                        history.StartTime = (item.startTime + nEightHourSecond) * 1000;
                        history.dtStartTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(history.StartTime);
                        history.EndTime = (item.endTime + nEightHourSecond) * 1000;
                        history.dtEndTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime(history.EndTime);

                        HAdd.Add(history);
                    }
                }
            }

            bll.InspectionTracks.AddRange(Add);
            bll.InspectionTracks.EditRange(Edit);
            bll.InspectionTracks.RemoveList(Delete);
            bll.InspectionTrackHistorys.AddRange(HAdd);

            All.AddRange(Add);
            All.AddRange(Edit);
            DealPatrolPoint(bll, All, Delete, HAdd, client);

            return true;
        }


        private void DealPatrolPoint(Bll bll, List<DbModel.Location.Work.InspectionTrack> All, List<DbModel.Location.Work.InspectionTrack> Delete, List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd, WebApiLib.Clients.BaseDataClient client)
        {
            List<DbModel.Location.Work.PatrolPoint> ppList = bll.PatrolPoints.ToList();
            List<DbModel.LocationHistory.Work.PatrolPointHistory> ppHList = bll.PatrolPointHistorys.ToList();
            List<DbModel.Location.AreaAndDev.DevInfo> devList = bll.DevInfos.ToList();
            if (ppList == null)
            {
                ppList = new List<DbModel.Location.Work.PatrolPoint>();
            }

            if (ppHList == null)
            {
                ppHList = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();
            }

            if (devList == null)
            {
                devList = new List<DbModel.Location.AreaAndDev.DevInfo>();
            }

            List<DbModel.Location.Work.PatrolPoint> PAll = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PAdd = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PEdit = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.Location.Work.PatrolPoint> PDelete = new List<DbModel.Location.Work.PatrolPoint>();
            List<DbModel.LocationHistory.Work.PatrolPointHistory> PHAdd = new List<DbModel.LocationHistory.Work.PatrolPointHistory>();

            foreach (DbModel.Location.Work.InspectionTrack item in All)
            {
                int Id = item.Id;
                int patrolId = (int)item.Abutment_Id;
                CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = client.Getcheckpoints(patrolId);
                if (recv == null || recv.route.Count() <= 0)
                {
                    continue;
                }

                foreach (CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints item2 in recv.route)
                {
                    DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p => p.KKS == item2.kksCode);
                    DbModel.Location.Work.PatrolPoint now = ppList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (now == null)
                    {
                        now = new DbModel.Location.Work.PatrolPoint();

                        now.ParentId = Id;
                        now.StaffCode = item2.staffCode;
                        now.StaffName = item2.staffName;
                        now.KksCode = item2.kksCode;
                        now.DevName = item2.deviceName;
                        now.DeviceCode = item2.deviceCode;
                        now.DeviceId = item2.deviceId;
                        if (dev1 != null)
                        {
                            now.DevId = dev1.Id;
                        }

                        PAdd.Add(now);
                    }
                    else
                    {
                        now.ParentId = Id;
                        now.StaffCode = item2.staffCode;
                        now.StaffName = item2.staffName;
                        now.KksCode = item2.kksCode;
                        now.DevName = item2.deviceName;
                        now.DeviceCode = item2.deviceCode;
                        now.DeviceId = item2.deviceId;
                        if (dev1 != null)
                        {
                            now.DevId = dev1.Id;
                        }

                        PEdit.Add(now);
                    }
                }
            }

            foreach (DbModel.Location.Work.InspectionTrack item in Delete)
            {
                int Id = item.Id;
                List<DbModel.Location.Work.PatrolPoint> lstDelete = ppList.FindAll(p => p.ParentId == Id).ToList();
                if (lstDelete != null && lstDelete.Count() > 0)
                {
                    PDelete.AddRange(lstDelete);
                }
            }

            foreach (DbModel.LocationHistory.Work.InspectionTrackHistory item in HAdd)
            {
                int Id = item.Id;
                int patrolId = (int)item.Abutment_Id;
                CommunicationClass.SihuiThermalPowerPlant.Models.patrols recv = client.Getcheckpoints(patrolId);
                if (recv == null || recv.route.Count() <= 0)
                {
                    continue;
                }

                foreach (CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints item2 in recv.route)
                {
                    DbModel.Location.AreaAndDev.DevInfo dev1 = devList.Find(p => p.KKS == item2.kksCode);
                    DbModel.LocationHistory.Work.PatrolPointHistory history = ppHList.Find(p => p.DeviceId == item2.deviceId && p.ParentId == Id);
                    if (history == null)
                    {
                        history = new DbModel.LocationHistory.Work.PatrolPointHistory();

                        history.ParentId = Id;
                        history.StaffCode = item2.staffCode;
                        history.StaffName = item2.staffName;
                        history.KksCode = item2.kksCode;
                        history.DevName = item2.deviceName;
                        history.DeviceCode = item2.deviceCode;
                        history.DeviceId = item2.deviceId;
                        if (dev1 != null)
                        {
                            history.DevId = dev1.Id;
                        }

                        PHAdd.Add(history);
                    }
                }
            }

            bll.PatrolPoints.AddRange(PAdd);
            bll.PatrolPoints.EditRange(PEdit);
            bll.PatrolPoints.RemoveList(PDelete);
            bll.PatrolPointHistorys.AddRange(PHAdd);

            PAll.AddRange(PAdd);
            PAll.AddRange(PEdit);
            DealPatrolPointItem(bll, All, HAdd, PAll, PDelete, PHAdd, client);

            return;
        }

        private void DealPatrolPointItem(Bll bll, List<DbModel.Location.Work.InspectionTrack> All, List<DbModel.LocationHistory.Work.InspectionTrackHistory> HAdd, List<DbModel.Location.Work.PatrolPoint> PAll, List<DbModel.Location.Work.PatrolPoint> PDelete, List<DbModel.LocationHistory.Work.PatrolPointHistory> PHAdd, WebApiLib.Clients.BaseDataClient client)
        {
            try
            {

                List<DbModel.Location.Work.PatrolPointItem> ppiList = bll.PatrolPointItems.ToList();
                List<DbModel.LocationHistory.Work.PatrolPointItemHistory> ppiHList = bll.PatrolPointItemHistorys.ToList();
                if (ppiList == null)
                {
                    ppiList = new List<DbModel.Location.Work.PatrolPointItem>();
                }

                if (ppiHList == null)
                {
                    ppiHList = new List<DbModel.LocationHistory.Work.PatrolPointItemHistory>();
                }

                List<DbModel.Location.Work.PatrolPointItem> PIAll = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.Location.Work.PatrolPointItem> PIAdd = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.Location.Work.PatrolPointItem> PIEdit = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.Location.Work.PatrolPointItem> PIDelete = new List<DbModel.Location.Work.PatrolPointItem>();
                List<DbModel.LocationHistory.Work.PatrolPointItemHistory> PIHAdd = new List<DbModel.LocationHistory.Work.PatrolPointItemHistory>();


                foreach (DbModel.Location.Work.PatrolPoint item in PAll)
                {
                    int Id = item.Id;
                    string deviceId = item.DeviceId;
                    int ParentId = item.ParentId;
                    DbModel.Location.Work.InspectionTrack it = All.Find(p => p.Id == ParentId);
                    if (it == null)
                    {
                        continue;
                    }

                    int patrolId = (int)it.Abutment_Id;
                    CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints recv = client.Getcheckresults(patrolId, deviceId);
                    if (recv == null || recv.checks.Count() <= 0)
                    {
                        continue;
                    }

                    foreach (CommunicationClass.SihuiThermalPowerPlant.Models.results item2 in recv.checks)
                    {
                        DbModel.Location.Work.PatrolPointItem now = ppiList.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                        if (now == null)
                        {
                            now = new DbModel.Location.Work.PatrolPointItem();
                            now.ParentId = Id;
                            now.KksCode = item2.kksCode;
                            now.CheckItem = item2.checkItem;
                            now.StaffCode = item2.staffCode;
                            now.CheckTime = null;
                            now.dtCheckTime = null;
                            if (item2.checkTime != null)
                            {
                                now.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                now.dtCheckTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime((long)now.CheckTime);
                            }
                            now.CheckId = item2.checkId;
                            now.CheckResult = item2.checkResult;
                            PIAdd.Add(now);
                        }
                        else
                        {
                            if (item2.checkTime != null)
                            {
                                now.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                now.dtCheckTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime((long)now.CheckTime);
                            }

                            now.CheckResult = item2.checkResult;
                            PIEdit.Add(now);
                        }
                    }
                }

                foreach (DbModel.Location.Work.PatrolPoint item in PDelete)
                {
                    int Id = item.Id;
                    List<DbModel.Location.Work.PatrolPointItem> lstDelete = ppiList.FindAll(p => p.ParentId == Id).ToList();
                    if (lstDelete != null && lstDelete.Count() > 0)
                    {
                        PIDelete.AddRange(lstDelete);
                    }
                }

                foreach (DbModel.LocationHistory.Work.PatrolPointHistory item in PHAdd)
                {
                    int Id = item.Id;
                    string deviceId = item.DeviceId;
                    int ParentId = item.ParentId;
                    DbModel.LocationHistory.Work.InspectionTrackHistory ith = HAdd.Find(p => p.Id == ParentId);
                    if (ith == null)
                    {
                        continue;
                    }

                    int patrolId = (int)ith.Abutment_Id;
                    CommunicationClass.SihuiThermalPowerPlant.Models.checkpoints recv = client.Getcheckresults(patrolId, deviceId);
                    if (recv == null || recv.checks.Count() <= 0)
                    {
                        continue;
                    }

                    foreach (CommunicationClass.SihuiThermalPowerPlant.Models.results item2 in recv.checks)
                    {
                        DbModel.LocationHistory.Work.PatrolPointItemHistory history = bll.PatrolPointItemHistorys.Find(p => p.CheckId == item2.checkId && p.ParentId == Id);

                        if (history == null)
                        {
                            history = new DbModel.LocationHistory.Work.PatrolPointItemHistory();
                            history.ParentId = Id;
                            history.KksCode = item2.kksCode;
                            history.CheckItem = item2.checkItem;
                            history.StaffCode = item2.staffCode;
                            history.CheckTime = null;
                            history.dtCheckTime = null;
                            if (item2.checkTime != null)
                            {
                                history.CheckTime = (item2.checkTime + nEightHourSecond) * 1000;
                                history.dtCheckTime = Location.TModel.Tools.TimeConvert.TimeStampToDateTime((long)history.CheckTime);
                            }
                            history.CheckId = item2.checkId;
                            history.CheckResult = item2.checkResult;
                            PIHAdd.Add(history);
                        }
                    }
                }

                bll.PatrolPointItems.AddRange(PIAdd);
                bll.PatrolPointItems.EditRange(PIEdit);
                bll.PatrolPointItems.RemoveList(PIDelete);
                bll.PatrolPointItemHistorys.AddRange(PIHAdd);
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
            }
            return;

        }

    }
}

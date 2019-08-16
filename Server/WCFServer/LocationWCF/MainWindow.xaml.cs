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
using TModel.Location.Data;
using TModel.Tools;
using WebNSQLib;
using LocationServer.Models.EngineTool;
using System.Text;
using LocationServer.Tools;
using LocationServer.Windows.Simple;
using NVSPlayer;
using LocationServer.Plugins.NVSPlayer.SDK;
using DbModel.Location.Alarm;
using WebApiCommunication.ExtremeVision;
using Newtonsoft.Json;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Base.Tools;

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
            this.Closing += MainWindow_Closing;
            LogEvent.InfoEvent += LogEvent_InfoEvent1;
        }

        private void LogEvent_InfoEvent1(LogEvent.LogEventInfo obj)
        {
            Location.BLL.Tool.Log.Info(obj);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否退出服务端程序", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                isCloseDaemonProcess= QuestionCloseDaemonProcess();

                var EnabelNVS = ConfigurationHelper.GetBoolValue("EnabelNVS");
                if (EnabelNVS || nginxCmdProcess!=null)
                {
                    if (nginxCmdProcess != null)
                    {
                        nginxCmdProcess.CloseMainWindow();
                    }

                    string nginx = AppDomain.CurrentDomain.BaseDirectory + "\\nginx-1.7.11.3-Gryphon\\stopq.bat";
                    if (File.Exists(nginx))
                    {
                        Process.Start(nginx); //关闭nginx-rtmp
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private PositionEngineLog Logs = new PositionEngineLog();

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)//调试模式
            {
                this.Topmost = false;//防止挡住代码
            }

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

            version = ConfigurationHelper.GetValue("ServerVersionCode");

            this.Title = "服务端    -v" + version;

            var isStartDaemon = ConfigurationHelper.GetBoolValue("StartDaemon");
            if (isStartDaemon)
            {
                StartDaemon(false);
            }

            bool isAutoRun = RegeditRW.ReadIsAutoRun();

            var registerDaemonAutoRun = ConfigurationHelper.GetBoolValue("RegisterDaemonAutoRun");
            if (registerDaemonAutoRun)
            {
                if (isAutoRun == false)
                {
                    RegeditRW.SetIsAutoRun(true);
                    string path2 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    Log.Info(LogTags.Server, "注册开机自启动:" + path2);
                }
                else
                {
                    Log.Info(LogTags.Server, "开机自启动:" + isAutoRun);
                }
            }
            else
            {
                Log.Info(LogTags.Server, "开机自启动:" + isAutoRun);
            }

            timeTimer = new DispatcherTimer();
            timeTimer.Interval = TimeSpan.FromMilliseconds(500);
            //timer2.Interval = TimeSpan.FromSeconds(1);
            timeTimer.Tick += TimeTimer_Tick;
            timeTimer.Start();
        }

        private string version;

        private DispatcherTimer timeTimer;

        private DateTime startTime = DateTime.Now;

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            var time = (now - startTime);
            this.Title = string.Format("{0}    -v{1} [{2}][{3:dd\\.hh\\:mm\\:ss}]", "服务端", version, now.ToString("HH:mm:ss"), time);
        }

        private void InitData()
        {
            //StartReceiveAlarm();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Bll.StopThread();//关闭静态线程

            ServerManagerBox1.StopServices();
            ServerManagerBox1.StopLogTimer();
            ServerManagerBox1.StopListenLog();

            CloseDaemonProcess();
            if(Application.Current!=null)
                Application.Current.Shutdown();
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
            win.Owner = this; win.Show();
        }

        private void MenuExportArchorPosition_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new BusAnchorListWindow();
            win.Owner = this; win.Show();
        }

        private void MenuAreaCanvas_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new AreaCanvasWindow();
            win.Owner = this; win.Show();
        }

        private void MenuArchorSettingExport_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ArchorListExportWindow();
            win.Owner = this; win.Show();
        }

        private void MenuDbExport_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new DbBrowserWindow();
            win.Owner = this; win.Show();
        }

        private void MenuDbInit_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new DbConfigureWindow();
            win.Owner = this; win.Show();
        }

        private void MenuLocationHistoryTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new LocationHistoryWindow();
            win.Owner = this; win.Show();
        }

        private void MenuEventSendTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new EventSendTestWindow();
            win.Owner = this; win.Show();
        }

        private void MenuEngineClient_OnClick(object sender, RoutedEventArgs e)
        {
            ShowEngineClientWindow();
        }

        private void ShowEngineClientWindow()
        {
            Log.Info(LogTags.Server, "打开定位引擎客户端窗口");
            var win = new EngineClientWindow();
            //win.Owner = this;
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
            var recv = client.GetAreaStatistics(1);
            int PersonNum = recv.PersonNum;
            int DevNum = recv.DevNum;
            int LocationAlarmNum = recv.LocationAlarmNum;
            int DevAlarmNum = recv.DevAlarmNum;
        }

        private void MenuCardRole_Click(object sender, RoutedEventArgs e)
        {
            var win = new CardRoleWindow();
            win.Owner = this; win.Show();
        }

        private void MenuAreaAuthorization_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new AreaAuthorizationWindow();
            win.Owner = this; win.Show();
        }

        private void MenuTag_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new TagWindow();
            win.Owner = this; win.Show();
        }

        private void MenuPerson_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new PersonWindow();
            win.Owner = this; win.Show();
        }

        private void MenuRealPos_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new RealPosWindow();
            win.Owner = this; win.Show();
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
            win.Owner = this; win.Show();

            //var win = new ArchorConfigureWindow();
            //win.Owner = this; win.Show();
        }

        private void MenuUDPSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuArchorCheck_Click(object sender, RoutedEventArgs e)
        {
            var win = new AnchorCheckWindow();
            win.Owner = this; win.Show();
        }

        private void MenuLocationAlarms_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new LocationAlarmWindow();
            win.Owner = this; win.Show();
        }

        private void MenuArchorSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ArchorWindow();
            win.Owner = this; win.Show();
        }

        private void MenuTrackPointList_Click(object sender, RoutedEventArgs e)
        {
            //var wind = new TrackPointListWindow();
            //wind.Show();

            var win = new ArchorListExportWindow();
            //win.CalculateAll = false;
            win.IsTrackPoint = true;
            win.Owner = this; win.Show();
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
            win.Owner = this; win.Show();
        }

        private void KKSMonitorManager_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new KKSMonitorDataWindow();
            window.Show();
        }

        private void MenuInspectionTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new InspectionChoiceWindows();
            win.Owner = this; win.Show();
        }

        private void MenuNVSPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            //因为dll是64位的，程序生成也要是64位的，幸亏没有引用其他32位的dll

            //NVSPlayerForm form = new NVSPlayerForm();
            //form.Show();

            //NVSSDK.NetClient_Startup_V4(0, 0, 0);

            //NVRPlayerClientWindow window = new NVRPlayerClientWindow();
            //window.Show()

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
                //WriteLog("找不到nginx启动文件:" + nginx);
            }
        }

        private Process nginxCmdProcess;

        private void MenuSaveCameraAlarmPicture_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                bool r = true;
                while (r)
                {
                    try
                    {
                        Log.Info("开始");

                        LocationService s = new LocationService();
                        //var list=s.GetAllCameraAlarms(true);
                        var bll = Bll.NewBllNoRelation();
                        int count = bll.CameraAlarmJsons.DbSet.Count();
                        Log.Info("count:" + count);
                        List<CameraAlarmJson> list2 = bll.CameraAlarmJsons.ToList();
                        Log.Info("获取到列表");
                        if (list2 != null)
                        {
                            Log.Info("成功");
                            for (int i1 = 0; i1 < list2.Count; i1++)
                            {
                                Log.Info(string.Format("进度:{0}/{1}", i1, list2.Count));
                                CameraAlarmJson camera = list2[i1];
                                SavePicture(camera, bll);
                            }
                        }
                        else
                        {
                            Log.Info("失败");
                            Log.Info("太多了取不出来，一个一个取");
                            for (int i = 0; i < count; i++)
                            {
                                Log.Info(string.Format("进度:{0}/{1}", i, count));
                                CameraAlarmJson camera = bll.CameraAlarmJsons.Find(i + 1);
                                if (camera == null)
                                {
                                    Log.Info("找不到id:" + (i + 1));
                                    continue;
                                }

                                SavePicture(camera, bll);
                            }
                        }
                        Log.Info("完成");
                        r = false;//真的完成
                    }
                    catch (Exception exception)
                    {
                        //出异常
                    }
                }
            }, () => { MessageBox.Show("完成"); });
        }

        public void GetImage(string base64)
        {
            base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(base64);
            //string imagebase64 = base64.Substring(base64.IndexOf(",") + 1);

            MemoryStream memStream = new MemoryStream(bytes);
            System.Drawing.Image mImage = System.Drawing.Image.FromStream(memStream);
            string path = AppDomain.CurrentDomain.BaseDirectory + "1.jpg";
            mImage.Save(path);

            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();
            //bi.StreamSource = new MemoryStream(bytes);
            //bi.EndInit();
            //Image1.Source = bi;
        }

        private void MenuSaveCameraAlarmPicture2_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                try
                {
                    Log.Info("开始");

                    var bll = Bll.NewBllNoRelation();
                    int count = bll.Pictures.DbSet.Count();
                    Log.Info("pic count:" + count);

                    LocationService s = new LocationService();
                    var list = s.GetAllCameraAlarms(false);

                    Log.Info("alarm count:" + list.Count);

                    List<string> picNameList = new List<string>();
                    foreach (var item in list)
                    {
                        if (!picNameList.Contains(item.pic_name))
                        {
                            picNameList.Add(item.pic_name);
                        }
                    }

                    Log.Info("pic count 2:" + picNameList.Count);

                    string dirPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Image\\CameraAlarms\\";

                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

                    if (dirInfo.Exists == false)
                    {
                        dirInfo.Create();
                    }

                    //return;
                    for (int i1 = 0; i1 < picNameList.Count; i1++)
                    {
                        Log.Info(string.Format("进度:{0}/{1}", i1, picNameList.Count));
                        //CameraAlarmInfo camera = picNameList[i1];
                        //SavePicture(camera, bll);
                        Picture pic = s.GetCameraAlarmPicture(picNameList[i1]);
                        if (pic == null) continue;//已经提取出来的
                       
                        //Log.Info(string.Format("进度:{0}/{1},[{2}]", i1, list.Count, r!=null));
                        string filePath = dirPath + pic.Name;
                        string base64 = Encoding.UTF8.GetString(pic.Info);
                        base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
                        byte[] bytes = Convert.FromBase64String(base64);
                        System.IO.File.WriteAllBytes(filePath, bytes);

                        var r = bll.Pictures.DeleteById(pic.Id);
                    }
                    Log.Info("完成");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }, () => { MessageBox.Show("完成"); });
        }

        private void SavePicture(CameraAlarmJson camera, Bll bll)
        {
            byte[] byte1 = camera.Json;
            string json = Encoding.UTF8.GetString(byte1);
            CameraAlarmInfo info = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
            info.id = camera.Id; //增加了id,这样能够获取到详情

            string pic = info.pic_data;
            if (!string.IsNullOrEmpty(pic))
            {
                info.pic_data = ""; //图片分开存
                string json2 = JsonConvert.SerializeObject(info); //新的没有图片的json
                camera.Json = Encoding.UTF8.GetBytes(json2);
                bll.CameraAlarmJsons.Edit(camera);
                var picName = info.pic_name;
                var picture = bll.Pictures.Find(i => i.Name == picName);
                if (picture == null)
                {
                    picture = new Picture();
                    picture.Name = info.pic_name;
                    picture.Info = Encoding.UTF8.GetBytes(pic);
                    bll.Pictures.Add(picture); //保存图片
                }
                else
                {
                    picture.Name = info.pic_name;
                    picture.Info = Encoding.UTF8.GetBytes(pic);
                    bll.Pictures.Edit(picture); //保存图片
                }
            }
            else
            {
                Log.Info("没有图片");

                DateTime now = GetDataTime(info.time_stamp);

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\CameraAlarms\\" + now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".json";
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                File.WriteAllText(path, json);//yyyy_mm_dd_HH_MM_ss_fff=>yyyy_MM_dd_HH_mm_ss_fff
            }
        }

        public DateTime GetDataTime(long time_stamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = ((long)time_stamp * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            var toNowNew = toNow.Add(TimeSpan.FromHours(8));
            DateTime AlarmTime = dtStart.Add(toNowNew);
            return AlarmTime;
        }

        private void MenuSetting_OnClick(object sender, RoutedEventArgs e)
        {
            SettingWindow win = new SettingWindow();
            win.Owner = this; win.Show();
        }

        private void MenuStartDaemon_OnClick(object sender, RoutedEventArgs e)
        {
            StartDaemon(true);
        }


        private bool isCloseDaemonProcess = false;

        private bool QuestionCloseDaemonProcess()
        {
            var processes = GetDaemonProcessList();
            if (processes.Count > 0)
            {
                if (MessageBox.Show("是否关闭守护进程？\n不关闭则会重新启动服务端。", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //foreach (Process process in processes)
                    //{
                    //    try
                    //    {
                    //        process.CloseMainWindow(); //关闭所有其他已经启动的守护进程
                    //    }
                    //    catch (Exception exception) //拒绝访问
                    //    {
                    //    }
                    //}
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void CloseDaemonProcess()
        {
            if (isCloseDaemonProcess)
            {
                var processes = GetDaemonProcessList();
                if (processes.Count > 0)
                {
                    foreach (Process process in processes)
                    {
                        try
                        {
                            process.CloseMainWindow(); //关闭所有其他已经启动的守护进程
                        }
                        catch (Exception exception) //拒绝访问
                        {
                        }
                    }
                }
            }
        }

        private List<Process> GetDaemonProcessList()
        {
            string processName = ConfigurationHelper.GetValue("DaemonProcess");
            Process[] processes = Process.GetProcessesByName(processName);
            List<Process> list = processes.Where(i => i.HasExited == false).ToList();
            return list;
        }

        private void StartDaemon(bool closeDaemon)
        {
            //var processName = "LocationDaemon";
            bool OnlyOneDaemon = ConfigurationHelper.GetBoolValue("OnlyOneDaemon");
            //string processName = ConfigurationHelper.GetValue("DaemonProcess");
            var process = GetDaemonProcessList();
                //string processName = "LocationDaemon";
            if (process.Count > 0)
            {
                if (closeDaemon == false)
                {
                    return;//程序启动时
                }
                else//手动启动收获进程
                {
                    if (OnlyOneDaemon)
                    {
                        foreach (Process process1 in process)
                        {
                            try
                            {
                                process1.CloseMainWindow(); //关闭所有其他已经启动的守护进程
                            }
                            catch (Exception e) //拒绝访问
                            {

                            }

                        }
                    }
                }

            }

            //string path = AppDomain.CurrentDomain.BaseDirectory + "LocationDaemon.exe";
            string path = ConfigurationHelper.GetValue("DaemonPath");

            FileInfo file = new FileInfo(path);
            if (File.Exists(file.FullName))
            {
                Process.Start(file.FullName);
            }
            else
            {
                FileInfo file2 = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + path);
                if (File.Exists(file2.FullName))
                {
                    Process.Start(file2.FullName);
                }
                else
                {
                    Log.Info(LogTags.Server, "找不到文件:" + file.FullName);
                    Log.Info(LogTags.Server, "找不到文件:" + file2.FullName);
                }
            }
        }

        private void MenuOpenDir_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory);
        }

        private void MenuRefreshHisPosBuffer_OnClick(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                var Phs = new LocationServices.Locations.Services.PosHistoryService();
                Phs.GetAllData(LogTags.HisPosBuffer, false);
            }, () => { });
            
        }
    }
}

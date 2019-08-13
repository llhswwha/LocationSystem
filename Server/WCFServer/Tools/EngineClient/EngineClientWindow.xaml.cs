using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
using System.Windows.Threading;
using DbModel.Tools;
using Location.BLL.Tool;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFServer;
using PositionSimulation;
using SignalRService.Hubs;
using TModel.Tools;
using WPFClientControlLib;
using System.IO;
using DbModel.LocationHistory.Data;
using BLL;

namespace EngineClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EngineClientWindow : Window
    {

        //public DispatcherTimer LogTimer;

        //private PositionEngineLog Logs = new PositionEngineLog();

        //private PositionEngineClient engineClient;

        //LogTextBoxController logTbController = new LogTextBoxController();

        public EngineClientWindow()
        {
            InitializeComponent();
            //Location.BLL.Tool.Log.NewLogEvent += Log_NewLogEvent;
            Closed += EngineClientWindow_Closed;
            //LogTimer = new DispatcherTimer();
            //LogTimer.Interval = TimeSpan.FromMilliseconds(200);
            //LogTimer.Tick += LogTimer_Tick;
            //LogTimer.Start();
        }

       

        //private void Log_NewLogEvent(string arg1, string arg2)
        //{
        //    //Location.BLL.Tool.Log.NewLogEvent -= ListenToLog;
        //    if (arg1 == LogTags.Engine)
        //    {
        //        Logs.WriteLogLeft(arg2);
        //    }
        //}

        private void EngineClientWindow_Closed(object sender, EventArgs e)
        {
            //Stop();
            EngineClientBox1.Stop();
        }

        //private void LogTimer_Tick(object sender, EventArgs e)
        //{
        //    TbResult2.Text = Logs.LogLeft;
        //    TbResult3.Text = Logs.LogRight;
        //}


        //private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        //{
        //    StartConnect();
        //}

        //private void StartConnect()
        //{
        //    if (BtnStart.Content.ToString() == "启动")
        //    {
        //        Start();
        //        BtnStart.Content = "停止";
        //    }
        //    else
        //    {
        //        Stop();
        //        BtnStart.Content = "启动";
        //    }
        //}

        //private void Start()
        //{
        //    if (engineClient == null)
        //    {
        //        EngineLogin login = new EngineLogin();
        //        login.LocalIp = TbLocalIp.Text;
        //        login.LocalPort = TbLocalPort.Text.ToInt();
        //        login.EngineIp = TbEngineIp.Text;
        //        login.EnginePort = TbEnginePort.Text.ToInt();
        //        //if (login.Valid() == false)
        //        //{
        //        //    MessageBox.Show("本地Ip和对端Ip必须是同一个Ip段的");
        //        //    return;
        //        //}

        //        engineClient = PositionEngineClient.Instance();
        //        engineClient.Logs = Logs;
        //        engineClient.IsWriteToDb = (bool)CbWriteToDb.IsChecked;
        //        engineClient.StartConnectEngine(login);
        //        engineClient.NewAlarmsFired += EngineClient_NewAlarmsFired;

        //        Log.Info(LogTags.Server, string.Format("开始定位引擎对接 local={0}:{1},engine={2}:{3}", login.LocalIp, login.LocalPort, login.EngineIp, login.EnginePort));
        //    }
        //}

        public void TestJson(string json)
        {
            Position pos = new Position();
            pos.Parse(json, LocationContext.OffsetX, LocationContext.OffsetY);
        }

        //private void EngineClient_NewAlarmsFired(List<DbModel.Location.Alarm.LocationAlarm> obj)
        //{
        //    AlarmHub.SendLocationAlarms(obj.ToTModel().ToArray());
        //}

        //private void Stop()
        //{
        //    if (engineClient != null)
        //    {
        //        engineClient.Stop();
        //        engineClient = null;
        //    }
           
        //}

        private void MenuHistory_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new HistoryPositionWindow();
            win.Show();
        }

        private void MenuReal_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new RealPositionWindow();
            win.Show();
        }

        private void EngineClientWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //TbLocalIp.ItemsSource = IpHelper.GetLocalIpList();
            //TbLocalIp.SelectedIndex = 0;

            //var list = new List<string>() { "127.0.0.1", "192.168.10.155", "172.16.100.25" };
            //TbEngineIp.ItemsSource = list;
            //TbEngineIp.SelectedIndex = 0;

            //TbEnginePort.ItemsSource = new string[] { "3456","3455"};
            //TbEnginePort.SelectedIndex = 0;

            //if(EngineClientSetting.AutoStart)
            //{
            //    if (!list.Contains(EngineClientSetting.EngineIp))
            //    {
            //        list.Add(EngineClientSetting.EngineIp);
            //        TbEngineIp.ItemsSource = list;
            //    }

            //    TbLocalIp.SelectedItem = EngineClientSetting.LocalIp;
            //    TbEngineIp.SelectedItem = EngineClientSetting.EngineIp;
            //    StartConnect();
            //}

            EngineClientBox1.Init();
        }

        private void MenuOpenSimulation3D_OnClick(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Simulator\\Simulator.exe";
            if (System.IO.File.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                MessageBox.Show("未找到文件:" + path);
            }
        }

        private void MenuOpenSimulation2D_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new SimulationWindow();
            win.Show();
        }

        private void MenuSimulateJson_Click(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\PositionJson\\json.txt";
            string json = File.ReadAllText(path);
            TestJson(json);
        }
    }
}

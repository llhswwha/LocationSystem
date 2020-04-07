using DbModel.Tools;
using Location.BLL.Tool;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFServer;
using SignalRService.Hubs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TModel.Tools;
using WPFClientControlLib;

namespace EngineClient
{
    /// <summary>
    /// EngineClientBox.xaml 的交互逻辑
    /// </summary>
    public partial class EngineClientBox : UserControl
    {
        public DispatcherTimer LogTimer;

        private PositionEngineLog Logs = new PositionEngineLog();

        private PositionEngineClient engineClient;

        LogTextBoxController logTbController = new LogTextBoxController();

        public EngineClientBox()
        {
            InitializeComponent();
            string faintRange = LocationServer.AppContext.FaintRange;
            int faintEffectiveTime = LocationServer.AppContext.FaintEffectiveTime;
            int faintTimeInterval = LocationServer.AppContext.FaintTimeInterval;

            FaintScope.Text = faintRange;
            FaintTime.Text = faintEffectiveTime.ToString();
            FaintIntervalTime.Text = faintTimeInterval.ToString();

        }

        public void Init()
        {
            Location.BLL.Tool.Log.NewLogEvent += Log_NewLogEvent;
            //Closed += EngineClientWindow_Closed;
            LogTimer = new DispatcherTimer();
            LogTimer.Interval = TimeSpan.FromMilliseconds(200);
            LogTimer.Tick += LogTimer_Tick;
            LogTimer.Start();

            TbLocalIp.ItemsSource = IpHelper.GetLocalIpList();
            TbLocalIp.SelectedIndex = 0;

            var list = new List<string>() { "127.0.0.1", "192.168.10.155", "172.16.100.25" };
            TbEngineIp.ItemsSource = list;
            TbEngineIp.SelectedIndex = 0;

            TbEnginePort.ItemsSource = new string[] { "3456", "3455" };
            TbEnginePort.SelectedIndex = 0;

            if (EngineClientSetting.AutoStart)
            {
                if (!list.Contains(EngineClientSetting.EngineIp))
                {
                    list.Add(EngineClientSetting.EngineIp);
                    TbEngineIp.ItemsSource = list;
                }

                TbLocalIp.SelectedItem = EngineClientSetting.LocalIp;
                TbEngineIp.SelectedItem = EngineClientSetting.EngineIp;
                StartConnect();
            }
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            TbResult2.Text = Logs.LogLeft;
            TbResult3.Text = Logs.LogRight;
        }

        private void Log_NewLogEvent(LogInfo info)
        {
            //Location.BLL.Tool.Log.NewLogEvent -= ListenToLog;
            if (info.Tag == LogTags.Engine)
            {
                Logs.WriteLogLeft(info.Log);
            }
        }

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            StartConnect();
        }

        public void StartConnect()
        {
            if (BtnStart.Content.ToString() == "启动")
            {
                Start();
                BtnStart.Content = "停止";
            }
            else
            {
                Stop();
                BtnStart.Content = "启动";
            }
        }

        public void Stop()
        {
            StopFaintAlarm();

            if (engineClient != null)
            {
                engineClient.NewAlarmsFired -= EngineClient_NewAlarmsFired;//不取消的话，事件会重复绑定
                engineClient.Stop();
                engineClient = null;
            }

        }

        public void Start()
        {
            if (engineClient == null)
            {
                EngineLogin login = new EngineLogin();
                login.LocalIp = TbLocalIp.Text;
                login.LocalPort = TbLocalPort.Text.ToInt();
                login.LocalPort2 = TbLocalAreaAlarmPort.Text.ToInt();
                login.EngineIp = TbEngineIp.Text;
                login.EnginePort = TbEnginePort.Text.ToInt();
                login.EnginePort2 = TbEngineAreaAlarmPort.Text.ToInt();
                //if (login.Valid() == false)
                //{
                //    MessageBox.Show("本地Ip和对端Ip必须是同一个Ip段的");
                //    return;
                //}

                engineClient = PositionEngineClient.Instance();
                engineClient.Logs = Logs;
                engineClient.IsWriteToDb = (bool)CbWriteToDb.IsChecked;
                engineClient.StartConnectEngine(login);
                engineClient.NewAlarmsFired += EngineClient_NewAlarmsFired;

                int nFaintFlag = LocationServer.AppContext.FaintFlag;
                if (nFaintFlag == 1)
                {
                    StartFaintAlarm();
                }
                
                Log.Info(LogTags.Server, string.Format("开始定位引擎对接 local={0}:{1}:{2},engine={3}:{4}:{5}", login.LocalIp, login.LocalPort, login.EnginePort2, login.EngineIp, login.EnginePort, login.EnginePort2));
            }
        }

        private void EngineClient_NewAlarmsFired(List<DbModel.Location.Alarm.LocationAlarm> obj)
        {
            Log.Info("LocationAlarm", "AlarmHub.SendLocationAlarms:" + obj.Count);
            AlarmHub.SendLocationAlarms(obj.ToTModel().ToArray());
        }

        private void FaintBtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            StartFaintBtn();
        }

        public void StartFaintBtn()
        {
            if (FaintBtnStart.Content.ToString() == "启动晕倒告警")
            {
                StartFaintAlarm();
            }
            else
            {
                StopFaintAlarm();
            }
        }

        private void StartFaintAlarm()
        {
            if (FaintBtnStart.Content.ToString() == "关闭晕倒告警")
            {
                return;
            }

            if (engineClient != null)
            {
                FaintAlarmLogin fal = new FaintAlarmLogin();
                fal.FaintScope = FaintScope.Text.ToString();
                fal.FaintTime = FaintTime.Text.ToInt();
                fal.FaintIntervalTime = FaintIntervalTime.Text.ToInt();
                engineClient.StartFaintAlarm(fal);
                FaintBtnStart.Content = "关闭晕倒告警";
            }
            else
            {
                MessageBox.Show("PositionEngineClient类对象还未创建");
            }

            return;
        }

        public void StopFaintAlarm()
        {
            if (FaintBtnStart.Content.ToString() == "启动晕倒告警")
            {
                return;
            }

            if (engineClient != null)
            {
                engineClient.StopFaintAlarm();
            }

            FaintBtnStart.Content = "启动晕倒告警";
        }

        private void MenuTest_OnClick(object sender, RoutedEventArgs e)
        {
            engineClient.TestInsertPostions();
        }

        private void MenuErrorPoint_OnClick(object sender, RoutedEventArgs e)
        {
            var disList = BLL.Bll.posDistanceList;
        }
    }
}

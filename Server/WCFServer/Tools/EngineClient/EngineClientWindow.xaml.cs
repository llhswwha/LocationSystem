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
using LocationServices.Tools;
using LocationWCFServer;
using PositionSimulation;
using TModel.Tools;

namespace EngineClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EngineClientWindow : Window
    {

        public DispatcherTimer LogTimer;

        private PositionEngineLog Logs = new PositionEngineLog();

        private PositionEngineClient engineClient;


        public EngineClientWindow()
        {
            InitializeComponent();
            Closed += EngineClientWindow_Closed;
            LogTimer = new DispatcherTimer();
            LogTimer.Interval = TimeSpan.FromMilliseconds(200);
            LogTimer.Tick += LogTimer_Tick;
            LogTimer.Start();
        }

        private void EngineClientWindow_Closed(object sender, EventArgs e)
        {
            Stop();
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            TbResult2.Text = Logs.LogLeft;
            TbResult3.Text = Logs.LogRight;
        }


        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
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

        private void Start()
        {
            if (engineClient == null)
            {
                EngineLogin login = new EngineLogin();
                login.LocalIp = TbLocalIp.Text;
                login.LocalPort = TbLocalPort.Text.ToInt();
                login.EngineIp = TbEngineIp.Text;
                login.EnginePort = TbEnginePort.Text.ToInt();
                if (login.Valid() == false)
                {
                    MessageBox.Show("本地Ip和对端Ip必须是同一个Ip段的");
                    return;
                }

                engineClient = new PositionEngineClient();
                engineClient.Logs = Logs;
                engineClient.IsWriteToDb = (bool)CbWriteToDb.IsChecked;
                engineClient.StartConnectEngine(login);
            }
        }

        private void Stop()
        {
            if (engineClient != null)
            {
                engineClient.Stop();
                engineClient = null;
            }
           
        }

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
            TbLocalIp.ItemsSource = IpHelper.GetLocalList();
            TbLocalIp.SelectedIndex = 0;

            TbEngineIp.ItemsSource = new string[] {"127.0.0.1", "192.168.10.155"};
            TbEngineIp.SelectedIndex = 0;

            TbEnginePort.ItemsSource = new string[] {"3455", "3456"};
            TbEnginePort.SelectedIndex = 0;
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
    }
}

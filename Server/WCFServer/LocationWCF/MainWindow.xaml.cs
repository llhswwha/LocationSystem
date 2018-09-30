using LocationWCFService.ServiceHelper;
using LocationWCFServices;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DbModel.Tools;
using log4net.Util;
using LocationServices.LocationCallbacks;
using LocationServices.Locations;
using LocationWCFService;

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

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            TbResult2.Text = LogLeft;
            TbResult3.Text = LogRight;
        }


        public string LogLeft = "";
        public string LogRight = "";


        //public void WriteLine(TextBox tb, string txt)
        //{
        //    tb.Dispatcher.BeginInvoke(new Action<string>((t) =>
        //    {
        //        tb.Text = txt + "\n" + tb.Text;
        //    }), txt);
        //}


        public void WriteLogLeft(string txt)
        {
            LogLeft = txt + "\n" + LogLeft;
            if (LogLeft.Length > 1000)
            {
                LogLeft = txt;
            }
        }

        public void WriteLogRight(string txt)
        {
            LogRight = txt + "\n" + LogRight;
            if (LogRight.Length > 1000)
            {
                LogRight = txt;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (insertThread != null)
            {
                insertThread.Abort();
                insertThread = null;
            }
            if (engineDa != null)
            {
                engineDa.Stop();
            }

            if (LocationService.u3dositionSP != null)
            {
                LocationService.u3dositionSP.Stop();
            }
        }



        private void BtnStartService_Click(object sender, RoutedEventArgs e)
        {
            StartService();
        }

        private void StartService()
        {
            try
            {
                Log.Info("启动服务");

                StartLocationService();
                StartLocationAlarmService();

                TbResult1.AppendText("启动服务");
                //LocationService.ShowLog_Action += ShowTest;
                U3DPositionSP.ShowLog_Action += ShowTest;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                TbResult1.AppendText(ex.ToString());
            }
        }

        private ServiceHost locationServiceHost;
        private void StartLocationService()
        {
            locationServiceHost = new ServiceHost(typeof(LocationService));
            locationServiceHost.SetProxyDataContractResolver();
            locationServiceHost.Open();

            //Uri tcpBaseAddress = new Uri("net.tcp://localhost:7001/LocationService");
            //host = new ServiceHost(typeof(LocationWCFServices.LocationService),tcpBaseAddress);
            //ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
            //metadataBehavior.HttpGetEnabled = true;
            //metadataBehavior.HttpGetUrl=new Uri("http://localhost:7001/LocationService");
            //host.Description.Behaviors.Add(metadataBehavior);
            //host.Open();
        }

        private ServiceHost locationAlarmServiceHost;
        private void StartLocationAlarmService()
        {
            locationAlarmServiceHost = new ServiceHost(typeof(LocationCallbackService));
            locationAlarmServiceHost.SetProxyDataContractResolver();

            locationAlarmServiceHost.Open();
            
        }

        public void ShowTest(string str)
        {
            //textBox_U3DTEST.Text = str;
            //textBox_U3DTEST.AppendText( str);
        }


        private void BtnConnectEngine_Click(object sender, RoutedEventArgs e)
        {
            StartConnectEngine();
        }


        private void BtnTestInsertData_OnClick(object sender, RoutedEventArgs e)
        {
            TestInsertData();
        }

        private void BtnGeneratePosition_OnClick(object sender, RoutedEventArgs e)
        {
            GeneratePosition();
        }

        private void BtnTestInsertData2_OnClick(object sender, RoutedEventArgs e)
        {
            //TestInsertData2();
            TestInsertData2Async(); //异步方式
        }


        private void BtnNewDb_OnClick(object sender, RoutedEventArgs e)
        {
            StopConnectEngine();

            Thread.Sleep(1000);

            //if (positionBll != null)
            //{
            //    positionBll.Dispose();
            //}
            //positionBll = new LocationBll();
            WriteLogLeft("重新生成");
        }

        private void BtnStopConnectEngine_OnClick(object sender, RoutedEventArgs e)
        {
            StopConnectEngine();
        }

        private void BtnTestInsertData3_OnClick(object sender, RoutedEventArgs e)
        {
            StartTestInsertPositions();
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
            LocationCallbackService.NotifyServiceStop();
        }
        
    }
}

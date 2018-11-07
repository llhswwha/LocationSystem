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
            Log.Info(obj);
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
        }

        private void InitData()
        {
            //StartReceiveAlarm();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            ServerManagerBox1.StopServices();
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
            var win = new ArchorListWindow();
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
            var win = new LocationHistoryTestWindow();
            win.Show();
        }

        private void MenuEventSendTest_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new EventSendTestWindow();
            win.Show();
        }

        private void MenuEngineClient_OnClick(object sender, RoutedEventArgs e)
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
    }
}

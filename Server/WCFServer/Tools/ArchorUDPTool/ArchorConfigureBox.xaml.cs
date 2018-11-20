using ArchorUDPTool.Commands;
using ArchorUDPTool.Models;
using Coldairarrow.Util.Sockets;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using LocationServer.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using ArchorUDPTool;
using TModel.Tools;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using LocationServer.Tools;
using static ArchorUDPTool.ArchorManager;
using LocationServer.Tools;

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for ArchorConfigureBox.xaml
    /// </summary>
    public partial class ArchorConfigureBox : UserControl
    {
        public ArchorConfigureBox()
        {
            InitializeComponent();

            //C0 A8 05 01 91 9C 9D BE //192.168.5.1

            IPAddress ip = IPAddress.Parse("192.168.5.1");
            IPAddress ip2 = ip.MapToIPv6();
        }

        public ArchorManager archorManager { get; set; }


        private void MenuSetting_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorUDPSettingWindow();
            win.Show();
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            var cmd = TbCommand.Text;
            archorManager.SendCmd(cmd);
        }

        private void MenuRestart_OnClick(object sender, RoutedEventArgs e)
        {
            int port = TbPort.Text.ToInt();
            archorManager.ResetAll(port);
        }

        private void MenuSetServerIP1_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp1);//192.168.5.1
        }

        private void MenuSetServerIp2_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp2);//192.168.10.155
        }

        private void MenuSetServerIp3_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp3251);//192.168.3.251
        }

        private void MenuSetServerIp4_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp4251);//192.168.4.251
        }

        private void MenuSetServerIp5_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp5251);//192.168.5.251
        }

        private void MenuSetServerIp6_OnClick(object sender, RoutedEventArgs e)
        {
            int port = TbPort.Text.ToInt();
            archorManager.SetServerIp251(port);
        }

        private void MenuTest_Click(object sender, RoutedEventArgs e)
        {
            var win = new CodeTestWindow();
            win.Show();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            FileInfo fi = new FileInfo(path);
            archorManager.SaveArchorList(path);
            Process.Start(fi.Directory.FullName);
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            FileInfo fi = new FileInfo(path);
            archorManager.LoadArchorList(path);
        }

        private void MenuSetServerIp7_OnClick(object sender, RoutedEventArgs e)
        {
            int port = TbPort.Text.ToInt();
            archorManager.SetServerIp253(port);
        }


        private void SetArchorList(ArchorManager archorManager, UDPArchorList list)
        {
            this.Dispatcher.Invoke(() => {
                if (archorManager == null)
                {
                    TbConsole.Text = "";
                    DataGrid3.ItemsSource = null;
                    LbCount.Content = "";
                    LbStatistics.Content = "";
                }
                else
                {
                    //IsDirty = true;
                    LbTime.Content = archorManager.GetTimeSpan();
                    TbConsole.Text = archorManager.Log;
                    DataGrid3.ItemsSource = list;
                    LbCount.Content = list.GetConnectedCount();
                    LbStatistics.Content = archorManager.GetStatistics();
                    LbServerIpList.Content = list.ServerList.GetText();

                    LbListenCount.Content = archorManager.valueList.Count;
                    LbListenStatistics.Content = archorManager.valueList.GetStatistics();
                }
            });
        }

        //private void AddArchor(UDPArchor archor)
        //{
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        {
        //            if (archorList == null)
        //            {
        //                archorList = new UDPArchorList();
        //            }
        //            if (archorList.AddOrUpdate(archor) == 1)
        //            {
        //                //DataGrid3.ItemsSource = archorList;
        //                DataGrid3.Items.Add(archor);
        //                LbCount.Content = archorList.Count;
        //                LbStatistics.Content = archorList.GetStatistics();
        //            }
        //            //else
        //            //{
        //            //    var index=DataGrid3.Items.IndexOf(archor);
        //            //}

        //            LbTime.Content = archorManager.GetTimeSpan();

        //            TbConsole.Text = archorManager.Log;
                    
        //        }
        //    });
        //}

        UDPArchorList archorList;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (archorManager == null)
            {
                archorManager = new ArchorManager();
                archorManager.ArchorListChanged += (list) =>
                {
                    SetArchorList(archorManager, list);
                };
                archorManager.PercentChanged += (p) =>
                {
                    ProgressBarEx1.Visibility = Visibility.Visible;
                    ProgressBarEx1.Value = p;
                };
                archorManager.LogChanged += (log) =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        TbConsole.Text = log;
                    });
                };
                //archorManager.NewArchorAdded += AddArchor;
            }

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(200);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private bool IsDirty = false;

        private void Timer_Tick(object sender, EventArgs e)
        {
            //if (IsDirty)
            //{
            //    LbTime.Content = archorManager.GetTimeSpan();
            //    TbConsole.Text = archorManager.Log;
            //    DataGrid3.ItemsSource = archorManager.archorList;
            //    LbCount.Content = archorManager.archorList.Count;
            //    LbStatistics.Content = archorManager.GetStatistics();
            //    IsDirty = false;
            //}
        }

        public DispatcherTimer Timer;

        private ArchorManager.ScanArg GetScanArg(params string[] cmds)
        {
            ArchorManager.ScanArg arg = new ArchorManager.ScanArg();
            arg.ipsText = TbRemote.Text;
            arg.port = TbPort.Text;
            arg.cmds = cmds;
            arg.OneIPS = (bool)CbOneIPS.IsChecked;
            arg.ScanList = (bool)CbList.IsChecked;
            arg.Ping = (bool)CbPing.IsChecked;
            return arg;
        }


        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            SetArchorList(null, null);

            List<string> cmds = UDPCommands.GetAll();
            archorManager.ScanArchors(GetScanArg(cmds.ToArray()));
        }



        private void BtnSearchId_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg(UDPCommands.GetId));
        }

        private void BtnSearchIp_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg(UDPCommands.GetIp));
        }

        private void BtnSearchPort_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetServerPort));
        }

        private void BtnSearchServerIP_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetServerIp));
        }

        private void BtnSearchType_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetType));
        }

        private void BtnSearchMask_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetMask));
        }

        private void BtnSearchGateway_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetGateway));
        }

        private void BtnSearchDHCP_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetDHCP));
        }

        private void BtnSearchSoftverson_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetSoftVersion));
        }

        private void BtnSearchHardverson_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetHardVersion));
        }

        private void BtnSearchPower_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetPower));
        }


        private void BtnSearchMAC_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg(UDPCommands.GetMAC));
        }

        private void BtnStopTime_Click(object sender, RoutedEventArgs e)
        {
            archorManager.StopTime();
        }

        private void MenuRefreshOne_Click(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            archorManager.ScanArchor(archor);
        }

        private void MenuRefeshMuti_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<UDPArchor>();
            foreach (var item in DataGrid3.SelectedItems)
            {
                var archor = item as UDPArchor;
                if (archor == null) continue;
                list.Add(archor);
            }
            archorManager.ScanArchor(list.ToArray());
        }

        private void MenuRefreshMultiChecked_Click(object sender, RoutedEventArgs e)
        {
            var list = GetCheckedArchors();
            archorManager.ScanArchor(list.ToArray());
        }

        private List<UDPArchor> GetCheckedArchors()
        {
            var list = new List<UDPArchor>();
            foreach (var item in DataGrid3.Items)
            {
                var archor = item as UDPArchor;
                if (archor == null) continue;
                if (archor.IsChecked)
                    list.Add(archor);
            }
            return list;
        }

        private void MenuPingArchor_Click(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            var pingWnd = new PingWindow(archor.GetIp());
            pingWnd.Show();
        }

        private void BtnClearBuffer_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ClearBuffer();
            DataGrid3.ItemsSource = null;
        }

        private void MenuPing_Click(object sender, RoutedEventArgs e)
        {
            var pingWnd = new PingWindow();
            pingWnd.Show();
        }

        private void MenuLoadList_Click(object sender, RoutedEventArgs e)
        {
            var list = ArchorHelper.LoadArchoDevInfo();
            archorManager.LoadList(list);
        }

        private void BtnSearchList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuListen_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorUDPListener(archorManager);
            win.Show();
        }

        private void MenuSetArchor_Click(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            var wnd = new UDPArchorInfoWindow(archorManager,archor);
            wnd.Show();
        }

        private void MenuRestartOne_OnClick(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            archorManager.Reset(archor);
        }

        private void BtnStartListen_Click(object sender, RoutedEventArgs e)
        {
            if (BtnStartListen.Content.ToString() == "开始监听")
            {
                archorManager.StartListen();
                BtnStartListen.Content = "停止监听";
            }
            else
            {
                BtnStartListen.Content = "开始监听";
                archorManager.StopListen();
            }
        }

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            
        }

        private void MenuCancel_Click(object sender, RoutedEventArgs e)
        {
            archorManager.Cancel();
            ProgressBarEx1.Stop();
        }

        private void CheckColumn_Checked(object sender, RoutedEventArgs e)
        {
            archorManager.CheckAll();
        }

        private void CheckColumn_Unchecked(object sender, RoutedEventArgs e)
        {
            archorManager.UnCheckAll();
        }

        private void MenuRestartChecked_Click(object sender, RoutedEventArgs e)
        {
            var list = GetCheckedArchors();
            archorManager.Reset(list.ToArray());
        }
    }
}

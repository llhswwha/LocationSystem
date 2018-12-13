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

        internal void Close()
        {
            archorManager.Close();
        }

        public ArchorManager archorManager { get; set; }
        public List<Archor> DbArchorList { get; internal set; }

        private void MenuSetting_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorUDPSettingWindow();
            win.Show();
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            var cmd = TbCommand.Text;
            var port = TbPort.Text.ToInt();
            archorManager.SendCmd(cmd,port);
        }

        private void MenuRestart_OnClick(object sender, RoutedEventArgs e)
        {
            int port = TbPort.Text.ToInt();
            if (CbList.IsChecked == true)
            {
                archorManager.ResetAll( port, DataGrid3.subArchorList);
            }
            else
            {
                archorManager.ResetAll(port);
            }
        }

        private void MenuSetServerIp6_OnClick(object sender, RoutedEventArgs e)
        {
            //archorManager.SetServerIp251();
        }

        private void MenuTest_Click(object sender, RoutedEventArgs e)
        {
            var win = new CodeTestWindow();
            win.Show();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            FileInfo fi1 = new FileInfo(path1);
            archorManager.SaveArchorList(path1);

            string path2 = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList"+DateTime.Now.ToString("(yyMMddHHmmss)")+".xml";
            FileInfo fi2 = new FileInfo(path2);
            archorManager.SaveArchorList(path2);
            Process.Start(fi1.Directory.FullName);
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            FileInfo fi = new FileInfo(path);
            archorManager.LoadArchorList(path);
            CbList.IsChecked = true;
        }

        private void MenuSetServerIp7_OnClick(object sender, RoutedEventArgs e)
        {
            //archorManager.SetServerIp253();
        }


        private void SetArchorList(ArchorManager archorManager, UDPArchorList list,UDPArchor item1=null,int id=-1)
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

                    if (DbArchorList != null)
                    {
                        foreach (var item in list)
                        {
                            var ar = DbArchorList.Find(i => i.Ip == item.GetClientIP());
                            if (ar != null)
                            {
                                item.RealArea = ar.Parent.Name;
                                if (item.GetClientIP() != ar.Ip)
                                {
                                    item.DbInfo = "IP:" + ar.Ip;
                                }
                                else
                                {
                                    item.DbInfo = "有";
                                }
                            }
                        }
                    }

                    DataGrid3.ItemsSource = list;
                    if (id > 0)
                    {
                        var item2 = list[id];
                        if (item2 != item1)
                        {

                        }
                    }
                    
                    LbCount.Content = list.GetConnectedCount();
                    LbStatistics.Content = archorManager.GetStatistics();
                    LbServerIpList.Content = list.ServerList.GetText();

                    LbListenCount.Content = archorManager.valueList.Count;
                    LbListenStatistics.Content = archorManager.valueList.GetStatistics();
                }
            });
        }

        UDPArchorList archorList;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (archorManager == null)
            {
                archorManager = new ArchorManager();
                archorManager.ArchorListChanged += (list,item) =>
                {
                    if (item != null)
                    {
                        var id = list.IndexOf(item);
                        SetArchorList(archorManager, list,item,id);
                    }
                    else
                    {
                        SetArchorList(archorManager, list);
                    }
                    
                   
                };
                archorManager.PercentChanged += (p) =>
                {
                    ProgressBarEx1.Visibility = Visibility.Visible;
                    ProgressBarEx1.Value = p;
                };
                //archorManager.NewArchorAdded += AddArchor;

                DataGrid3.archorManager = archorManager;
                archorManager.arg = GetScanArg();
            }

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(200);
            Timer.Tick += Timer_Tick;
            Timer.Start();

            CbServerIpList.ItemsSource = SetCommands.Cmds;
            CbServerIpList.DisplayMemberPath = "Name";
            CbServerIpList.SelectedIndex = 0;

            CbLocalIps.ItemsSource = IpHelper.GetLocalList();
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
            //arg.Ping = (bool)CbPing.IsChecked;
            return arg;
        }


        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            SetArchorList(null, null);

            List<string> cmds = UDPCommands.GetAll();
            archorManager.ScanArchors(GetScanArg(cmds.ToArray()),DataGrid3.subArchorList);
        }



        private void BtnSearchId_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg(UDPCommands.GetId), DataGrid3.subArchorList);
        }

        private void BtnSearchIp_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg(UDPCommands.GetIp), DataGrid3.subArchorList);
        }

        private void BtnSearchPort_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetServerPort), DataGrid3.subArchorList);
        }

        private void BtnSearchServerIP_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetServerIp), DataGrid3.subArchorList);
        }

        private void BtnSearchType_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetType), DataGrid3.subArchorList);
        }

        private void BtnSearchMask_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetMask), DataGrid3.subArchorList);
        }

        private void BtnSearchGateway_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetGateway), DataGrid3.subArchorList);
        }

        private void BtnSearchDHCP_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetDHCP), DataGrid3.subArchorList);
        }

        private void BtnSearchSoftverson_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetSoftVersion), DataGrid3.subArchorList);
        }

        private void BtnSearchHardverson_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetHardVersion), DataGrid3.subArchorList);
        }

        private void BtnSearchPower_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg( UDPCommands.GetPower), DataGrid3.subArchorList);
        }


        private void BtnSearchMAC_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(GetScanArg(UDPCommands.GetMAC), DataGrid3.subArchorList);
        }

        private void BtnStopTime_Click(object sender, RoutedEventArgs e)
        {
            archorManager.StopTime();
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
            CbList.IsChecked = true;
        }

        private void BtnSearchList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuListen_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorUDPListener(archorManager);
            win.Show();
        }



        private void BtnStartListen_Click(object sender, RoutedEventArgs e)
        {
            if (BtnStartListen.Content.ToString() == "")
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

        private void MenuRestartChecked_Click(object sender, RoutedEventArgs e)
        {
            var list = DataGrid3.GetCheckedArchors();
            archorManager.Reset(list.ToArray());
        }

        private void BtnSetServerIp_Click(object sender, RoutedEventArgs e)
        {
            var cmd = CbServerIpList.SelectedItem as SetCommand;
            if (cmd == null) return;
            var port = TbPort.Text.ToInt();

            if (CbList.IsChecked == true)
            {
                archorManager.SendCmd(cmd.Cmd, port, DataGrid3.subArchorList);
            }
            else
            {
                archorManager.SendCmd(cmd.Cmd, port);
            }
        }

        private void CbLocalIps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            archorManager.LocalIp = CbLocalIps.SelectedItem as IPAddress;
        }

        private void BtnStartPing_Click(object sender, RoutedEventArgs e)
        {
            var arg = GetScanArg(UDPCommands.GetId);
            arg.Ping = true;
            arg.PingLength = TbPingLength.Text.ToInt();
            arg.PingWaitTime = TbPingWaitTime.Text.ToInt();
            arg.PingCount = TbPingCount.Text.ToInt();
            archorManager.ScanArchors(arg, DataGrid3.subArchorList);
        }
    }
}

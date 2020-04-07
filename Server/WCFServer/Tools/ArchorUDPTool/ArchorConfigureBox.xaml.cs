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
using BLL;
using EngineClient;
using Location.BLL.Tool;

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

        private List<ArchorInfo> _archors = null;

        public List<ArchorInfo> DbArchorList
        {
            get { return _archors; }
            set
            {
                if (value == null)
                {
                    _archors = null;
                }
                else
                {
                    _archors = value.FindAll(i => !string.IsNullOrEmpty(i.Ip));
                    var ipNull = value.FindAll(i => string.IsNullOrEmpty(i.Ip));
                    if (ipNull != null && ipNull.Count > 0)
                    {
                        Log.Error("DbArchorList 存在IP为空的基站:" + ipNull.Count + "," + ipNull[0].Code);
                    }

                    if (_archors != null)
                    {
                        DbArchorListDict = _archors.ToDictionary(i => i.Ip);
                    }
                }
            }
        }


        public Dictionary<string, ArchorInfo> DbArchorListDict { get; internal set; }

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
            SaveResult();
        }

        private void SaveResult()
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            FileInfo fi1 = new FileInfo(path1);
            archorManager.SaveArchorList(path1);

            string path2 = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList" + DateTime.Now.ToString("(yyMMddHHmmss)") + ".xml";
            FileInfo fi2 = new FileInfo(path2);
            archorManager.SaveArchorList(path2);
            Process.Start(fi2.Directory.FullName);
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

                    if (DbArchorListDict != null)
                    {
                        //list.ClearInfo();
                        foreach (var item in list)
                        {
                            item.DbInfo = "";
                            item.RealArea = "";
                            var clientIP = item.GetClientIP();
                            //var ar = DbArchorList.Find(i => i.Ip == clientIP);
                            if (DbArchorListDict.ContainsKey(clientIP))
                            {
                                var ar = DbArchorListDict[clientIP];
                                if (ar != null)
                                {
                                    item.RealArea = ar.Parent.Name;
                                    if (item.GetClientIP() != ar.Ip)
                                    {
                                        item.DbInfo = "IP:" + ar.Ip;
                                    }
                                    else
                                    {
                                        string code = ar.Code.Trim();
                                        if (!string.IsNullOrEmpty(code))
                                        {
                                            item.DbInfo = "有:" + code;
                                        }
                                        else
                                        {
                                            item.DbInfo = "有:" + ar.Ip;
                                        }
                                    }
                                }
                            }
                            
                        }
                    }

                    if (id > 0)
                    {
                        var item2 = list[id];
                        if (item2 != item1)
                        {

                        }

                        int id2 = list.IndexOf(item1);
                    }

                    DataGrid3.ItemsSource = list;

                    if (id > 0)
                    {
                        var item2 = list[id];
                        if (item2 != item1)
                        {

                        }
                        int id2 = list.IndexOf(item1);
                    }

                    LbCount.Content = list.GetConnectedCount();
                    LbStatistics.Content = archorManager.GetStatistics();
                    LbServerIpList.Content = list.ServerList.GetText();

                    LbListenCount.Content = archorManager.valueList.Count;
                    LbListenStatistics.Content = archorManager.valueList.GetStatistics();
                }
            });
        }

        public DispatcherTimer updateGridTimer;

        public UDPArchorList UDPArchorList;

        public List<UDPArchor> progressList = new List<UDPArchor>();

        public int count = 0;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitArchorManager();

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(200);
            Timer.Tick += Timer_Tick;
            Timer.Start();

            CbServerIpList.ItemsSource = SetCommands.Cmds;
            CbServerIpList.DisplayMemberPath = "Name";
            CbServerIpList.SelectedIndex = 0;

            var list= IpHelper.GetLocalList();
            var ip = EngineClientSetting.LocalIp;
            var ip1 = list.Find(i => i.ToString() == ip);
            CbLocalIps.ItemsSource = list;
            CbLocalIps.SelectedItem = ip1;

            LoadList();
        }

        private void InitArchorManager()
        {
            if (archorManager == null)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += UpdateGridTimer_Tick;
                timer.Start();
                updateGridTimer = timer;

                archorManager = new ArchorManager();
                archorManager.ArchorListChanged += (list, item) =>
                {
                    UDPArchorList = list;

                    //if (item != null)
                    //{
                    //    var id = list.IndexOf(item);
                    //    SetArchorList(archorManager, list, item, id);
                    //}
                    //else
                    //{
                    //    SetArchorList(archorManager, list);
                    //}

                    if (item != null)
                    {
                        progressList.Add(item);
                        count++;
                    }
                    IsDirty = true;
                };
                archorManager.LogChanged += ArchorManager_LogChanged;
                archorManager.PercentChanged += (p) =>
                {
                    ProgressBarEx1.Value = p;

                    //if (p == 100)
                    //{
                    //    SetArchorList(archorManager, UDPArchorList);
                    //}
                    if (p == 100)
                    {
                        ThreadPool.QueueUserWorkItem(a =>
                        {
                            Thread.Sleep(3000);
                            //SaveResult();
                            SetArchorList(archorManager, UDPArchorList);
                            archorManager.SaveArchorListResult();
                            //scanCount++;
                            //if (scanCount > 1)//第一次启动扫描结果可能是不准的
                            //{
                            //    Thread.Sleep(2000);
                            //    SendAlarm();
                            //}

                            //IsBusySendAlarm = false;
                        });
                    }

                };
                //archorManager.NewArchorAdded += AddArchor;

                DataGrid3.archorManager = archorManager;
                archorManager.arg = GetScanArg();
            }
        }

        private void UpdateGridTimer_Tick(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                IsDirty = false;
                SetArchorList(archorManager, UDPArchorList);

                //var c1 = progressList.Count;
                //ProgressBarEx1.Value = (int)(count * 100.0 / UDPArchorList.Count);
            }
        }

        private void ArchorManager_LogChanged(string obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                TbConsole.Text = archorManager.Log;
            }
            );
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
            Clear();archorManager.ScanArchors(GetScanArg(cmds.ToArray()),DataGrid3.subArchorList);
        }



        private void BtnSearchId_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg(UDPCommands.GetId), DataGrid3.subArchorList);
        }

        private void Clear()
        {
            count = 0;
            progressList = new List<UDPArchor>();
        }

        private void BtnSearchIp_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg(UDPCommands.GetIp), DataGrid3.subArchorList);
        }

        private void BtnSearchPort_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetServerPort), DataGrid3.subArchorList);
        }

        private void BtnSearchServerIP_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetServerIp), DataGrid3.subArchorList);
        }

        private void BtnSearchType_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetType), DataGrid3.subArchorList);
        }

        private void BtnSearchMask_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetMask), DataGrid3.subArchorList);
        }

        private void BtnSearchGateway_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetGateway), DataGrid3.subArchorList);
        }

        private void BtnSearchDHCP_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetDHCP), DataGrid3.subArchorList);
        }

        private void BtnSearchSoftverson_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetSoftVersion), DataGrid3.subArchorList);
        }

        private void BtnSearchHardverson_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetHardVersion), DataGrid3.subArchorList);
        }

        private void BtnSearchPower_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg( UDPCommands.GetPower), DataGrid3.subArchorList);
        }


        private void BtnSearchMAC_Click(object sender, RoutedEventArgs e)
        {
            Clear();archorManager.ScanArchors(GetScanArg(UDPCommands.GetMAC), DataGrid3.subArchorList);
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
            LoadList();
        }

        private void LoadList()
        {
            var list = ArchorHelper.LoadArchoDevInfo();

            Bll bll = Bll.NewBllNoRelation();
            var anchors = bll.Archors.ToList();

            ArchorDevList list2 = new ArchorDevList();
            list2.ArchorList = new List<ArchorDev>();
            foreach (var item in list.ArchorList)
            {
                var a = anchors.Find(i => i.Code == item.ArchorID);
                if (a != null)
                {
                    list2.ArchorList.Add(item);
                }
            }

            archorManager.LoadList(list2);
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
            updateGridTimer.Stop();
            //updateGridTimer = null;
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
            Clear();archorManager.ScanArchors(arg, DataGrid3.subArchorList);
        }

        private void MenuLoadAnchors_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

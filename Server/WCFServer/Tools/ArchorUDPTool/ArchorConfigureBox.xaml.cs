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
            archorManager.SendCmd(UDPCommands.Restart);
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
            archorManager.SetServerIp251();
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

        private void MenuSetServerIp7_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SetServerIp253();
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
                    LbCount.Content = list.Count;
                    LbStatistics.Content = archorManager.GetStatistics();
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


        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            SetArchorList(null, null);

            List<string> cmds = UDPCommands.GetAll();
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, cmds.ToArray());
        }

        private void BtnSearchId_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetId);
        }

        private void BtnSearchIp_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetIp);
        }

        private void BtnSearchPort_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetPort);
        }

        private void BtnSearchServerIP_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetServerIp);
        }

        private void BtnSearchType_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetArchorType);
        }

        private void BtnSearchMask_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetMask);
        }

        private void BtnSearchGateway_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetGateway);
        }

        private void BtnSearchDHCP_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetDHCP);
        }

        private void BtnSearchSoftverson_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetSoftVersion);
        }

        private void BtnSearchHardverson_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetHardVersion);
        }

        private void BtnSearchPower_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchors(TbRemote.Text, TbPort.Text, UDPCommands.GetPower);
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
            foreach (var item in DataGrid3.SelectedItems)
            {
                var archor = item as UDPArchor;
                if (archor == null) continue;
                archorManager.ScanArchor(archor);
            }
        }

        private void BtnClearBuffer_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ClearBuffer();
        }
    }
}

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
        }


        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            if (archorManager == null)
            {
                archorManager=new ArchorManager();
                archorManager.ArchorListChanged += () =>
                {
                    TbConsole.Text = archorManager.Log;
                    DataGrid3.ItemsSource = archorManager.archorList;
                    
                };
            }
            archorManager.SearchArchor(TbRemote.Text, TbPort.Text);
            MessageBox.Show("完成");
        }

        private ArchorManager archorManager;


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
            archorManager.SendCmd(UDPCommands.ServerIp3);//192.168.3.251
        }

        private void MenuSetServerIp4_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp4);//192.168.4.251
        }

        private void MenuSetServerIp5_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SendCmd(UDPCommands.ServerIp5);//192.168.5.251
        }

        private void MenuSetServerIp6_OnClick(object sender, RoutedEventArgs e)
        {
            archorManager.SetServerIp();
        }
    }
}

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

        Dictionary<string, LightUDP> udps = new Dictionary<string, LightUDP>();

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {

            resultList = new CommandResultManager();

            //var ipStart = TbStartIp.Text;
            var ipsText = TbRemote.Text;
            var port = TbPort.Text;
            var ips = ipsText.Split(';');
            foreach(var ip in ips)
            {
                var localIp = IpHelper.GetLocalIp(ip);
                if (localIp != null)
                {
                    LightUDP udp = null;
                    var id = localIp.ToString();
                    if (udps.ContainsKey(id))
                    {
                        udp = udps[id];
                    }
                    else
                    {
                        udp = new LightUDP(localIp, 1111);
                        udp.DGramRecieved += Udp_DGramRecieved;
                        udps[id] = udp;
                    }
                    //foreach (var item in UDPCommands.GetAll())
                    //{
                    //    udp.SendHex(item.Trim(), new IPEndPoint(IPAddress.Parse(ipEnd), port.ToInt()));
                    //}
                    int sleepTime = 200;
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port.ToInt());
                    udp.SendHex(UDPCommands.GetServerIp, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetId, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetIp, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetPort, ipEndPoint);
                    Thread.Sleep(sleepTime);
                }
                else
                {
                    //MessageBox.Show("当前电脑不存在IP段:" + ips);
                }
                Thread.Sleep(1000);
            }
            

            MessageBox.Show("完成");
            
        }

        CommandResultManager resultList;

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            resultList.Add(dgram.iep, dgram.data);
            string hex = ByteHelper.byteToHexStr(dgram.data);
            //string str = Encoding.UTF7.GetString(dgram.data);
            string txt = string.Format("[{0}]:{1}\n",dgram.iep, hex);
            TbConsole.Text =txt +TbConsole.Text;

            var archorList = new List<UDPArchor>();
            foreach (var item in resultList.Groups)
            {
                var archor = new UDPArchor();
                archor.ServerIp = item.ServerIp;
                archor.ServerPort = item.ServerPort;
                archor.Id = item.ArchorId;
                archor.Ip = item.ArchorIp;
                archorList.Add(archor);
            }
            DataGrid3.ItemsSource = archorList;
        }

        private void MenuSetting_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorUDPSettingWindow();
            win.Show();
        }
    }
}

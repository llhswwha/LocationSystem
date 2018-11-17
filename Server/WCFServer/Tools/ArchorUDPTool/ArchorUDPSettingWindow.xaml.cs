using ArchorUDPTool.Commands;
using Coldairarrow.Util.Sockets;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocationServer.Windows
{
    /// <summary>
    /// ArchorUDPSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ArchorUDPSettingWindow : Window
    {
        public ArchorUDPSettingWindow()
        {
            InitializeComponent();
        }

        LightUDP udp1;

        private void BtnGet_Click(object sender, RoutedEventArgs e)
        {
            var ip = TbIP.Text;

            if (udp1 == null)
            {
                var ipLocal = IpHelper.GetLocalIp(ip);
                udp1 = new LightUDP(ipLocal, 1111);
                udp1.DGramRecieved += Udp_DGramRecieved;
            }
            var tmp = UDPCommands.GetServerIp;
            udp1.SendHex(tmp, new IPEndPoint(IPAddress.Parse(ip), 4646));
        }

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            var r = new UDPCommandResult();
            r.Parse(dgram.data);
            var value = r.GetValue();
            string hex = ByteHelper.byteToHexStr(dgram.data);
            //string str = Encoding.UTF7.GetString(dgram.data);
            string txt = string.Format("[{0}]:{1}\n", dgram.iep, hex);
            TbConsole.Text = txt + TbConsole.Text;
        }

        LightUDP udp2;

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            var ip = TbIP.Text;
            
            if (udp2 == null)
            {
                var ipLocal = IpHelper.GetLocalIp(ip);
                udp2 = new LightUDP(ipLocal, 1112);
                udp2.DGramRecieved += Udp_DGramRecieved;
            }
            
            var tmp = TbValue.Text;//10 01 C0 03 00 A4 FC C2 79
            udp2.SendHex(tmp, new IPEndPoint(IPAddress.Parse(ip), 4646));
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            var hex = TbHex.Text;
            var bytes = ByteHelper.HexToBytes(hex);
            string txt = "";
            foreach (var item in bytes)
            {
                txt += item + " ";
            }
            TbResult.Text = txt;
        }

        private void BtnChange2_Click(object sender, RoutedEventArgs e)
        {
            var str = TbString.Text;
            var bytes = Encoding.UTF8.GetBytes(str);
            string txt = "";
            foreach (var item in bytes)
            {
                txt += item + " ";
            }
            TbResult2.Text = txt;
        }
    }
}

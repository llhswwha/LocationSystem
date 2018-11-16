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

        LightUDP udp;

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            //var ipStart = TbStartIp.Text;
            var ipEnd = TbEndIp.Text;
            var port = TbPort.Text;
            
            var ip = IpHelper.GetLocalIp(ipEnd);
            if (ip != null)
            {
                if (udp == null)
                {
                    udp = new LightUDP(ip, 1111);
                    udp.DGramRecieved += Udp_DGramRecieved;
                }
                
                string hexCmds = @"10011001001b4395cb
100110130063b7e518
1001100e009cdb8904
10011005007f2f50cf
10011003002975f749
1001100a00f8b74c00";
                
                string[] cmdList = hexCmds.Split(new char[] { '\n','\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in cmdList)
                {
                    udp.SendHex(item.Trim(), new System.Net.IPEndPoint(IPAddress.Parse(ipEnd), port.ToInt()));
                }
            }
            else
            {
                MessageBox.Show("当前电脑不存在IP段:" + ipEnd);
            }

            MessageBox.Show("完成");
            
        }

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            string hex = ByteHelper.byteToHexStr(dgram.data);
            string str = Encoding.UTF7.GetString(dgram.data);
            string txt = string.Format("[{0}]:{1}({2})\n",dgram.iep, hex, str);
            TbConsole.Text =txt +TbConsole.Text;
        }
    }
}

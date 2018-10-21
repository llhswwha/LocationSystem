using BUDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfReceivers_UDP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        LightUDP ludp2 = null;
        int _num = 0;
        private void ClinetBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ClinetBTN.Content.ToString() == "开始监听")
            {
                StartListen();
                ClinetBTN.Content = "停止监听";
            }
            else
            {
                StopListen();
                ClinetBTN.Content = "开始监听";
            }
        }

        private void StopListen()
        {
            if (ludp2 != null)
            {
                ludp2.Close();
                ludp2 = null;
            }
        }

        private void StartListen()
        {
            string ipStr = IPtb.Text;
            IPAddress ip;
            if (!string.IsNullOrEmpty(ipStr) && !string.IsNullOrEmpty(Porttb.Text))
            {
                try
                {
                    int portStr = Convert.ToInt32(Porttb.Text);
                    if (IPAddress.TryParse(ipStr, out ip))
                    {
                        ludp2 = new LightUDP(ip, portStr);  //建立UDP  监听端口
                        ludp2.AutoEcho = true;
                        ludp2.DGramRecieved += new DGramRecievedHandle(ludp2_DGramRecieved);
                        // SendMessage("发送") ; //暂时不需要发送功能；
                    }
                    else
                    {
                        MessageBox.Show("IP格式有误");
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }
        }

        private  void SendMessage(string Info)
        {
            byte[] data = Encoding.Default.GetBytes(Info);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 488;
            ludp2.Send(data, new IPEndPoint(ip, port));
        }

        string ShowInfo = "";


        void ludp2_DGramRecieved(object sender, BUDPGram dgram)
        {
            string msg = dgram.iep.Address.ToString() + " " + dgram.iep.Port.ToString() + " " + Encoding.Default.GetString(dgram.data);
            msg = GetLogText(msg);
            ShowInfo = ShowInfo+ msg + System.Environment.NewLine;

            ReceiverTB.Text = ShowInfo;  //监听收到数据 可获得数据的来源和信息
        }

        private string GetLogText(string msg)
        {
            return DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string msg = TbMessage.Text;
            if (string.IsNullOrEmpty(msg)) return;
            if (ludp2 != null)
            {
                var bytes = Encoding.UTF8.GetBytes(msg);
                ludp2.SendAll(bytes);
            }
        }
    }
}

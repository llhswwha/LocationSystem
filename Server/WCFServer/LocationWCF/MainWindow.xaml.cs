using Location.BLL;
using Location.Model;
using LocationWCFServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
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
using Web.Sockets.Core;

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
        }


        ServiceHost host;
        private void BtnStartService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                host = new ServiceHost(typeof(LocationWCFServices.LocationService));
                host.SetProxyDataContractResolver();
                host.Open();

                //Uri tcpBaseAddress = new Uri("net.tcp://localhost:7001/LocationService");
                //host = new ServiceHost(typeof(LocationWCFServices.LocationService),tcpBaseAddress);
                //ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
                //metadataBehavior.HttpGetEnabled = true;
                //metadataBehavior.HttpGetUrl=new Uri("http://localhost:7001/LocationService");
                //host.Description.Behaviors.Add(metadataBehavior);
                //host.Open();
                TbResult.AppendText("启动服务");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                TbResult.AppendText(ex.ToString());
            }
        }


        UDPClientSocket client;
        private void BtnConnectEngine_Click(object sender, RoutedEventArgs e)
        {
            
            if (client == null)
            {
                IPAddress ip2 = IPAddress.Parse("192.168.10.100");
                IPEndPoint ipEndPort2 = new System.Net.IPEndPoint(ip2, 3456);

                client = new UDPClientSocket(ipEndPort2);

                IPAddress ip1 = IPAddress.Parse("192.168.10.100");
                IPEndPoint ipEndPort1 = new System.Net.IPEndPoint(ip1, 1000);
                client.Listen(ipEndPort1);

                client.MessageReceived += Client_MessageReceived;
                client.Start();
            }
           
            client.SendMsg("1");
        }

        PositionBll positionBll = new PositionBll();

        private void Client_MessageReceived(byte[] arg1, object arg2)
        {
            string msg = Encoding.UTF8.GetString(arg1);

            Position pos = new Position();
            if (pos.Parse(msg))
            {
                positionBll.Create(pos);
            }
            Console.WriteLine(msg);

            TbResult.Dispatcher.Invoke(new Action<string>(c =>
            {
                TbResult.AppendText(c+"\n");
            }), msg);
        }
    }
}

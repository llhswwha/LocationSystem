using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using Location.WCFServiceReferences.LocationServices;

namespace LocationWCFClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private LocationServiceClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGetMaps_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                MessageBox.Show("先连接");
                return;
            }
            try
            {
                Map[] maps = client.GetMaps();
                DataGrid1.ItemsSource = maps;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void BtnGetAreas_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                MessageBox.Show("先连接");
                return;
            }

            try
            {
                DataGrid1.ItemsSource = client.GetAreas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void BtnHello_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                MessageBox.Show("先连接");
                return;
            }
            try
            {
                TbResult.Text = client.Hello();
                User user = client.GetUser();
                TbResult.Text += user.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnConnect_OnClick(object sender, RoutedEventArgs e)
        {
            SetAddress();
        }

        private void SetAddress()
        {
            try
            {
                //client = new LocationServiceClient();

                string hostName = TbHost.Text;
                string port = TbPort.Text;
                System.ServiceModel.Channels.Binding wsBinding = new BasicHttpBinding();
                string url =
                    string.Format("http://{0}:{1}/Design_Time_Addresses/LocationWCFServices/LocationService/",
                        hostName, port);

                LbAddress.Text = url;

                EndpointAddress endpointAddress = new EndpointAddress(url);

                if (client != null)
                {
                    if (client.State == CommunicationState.Opened)
                    {
                        client.Close();
                    }
                }

                client = new LocationServiceClient(wsBinding, endpointAddress);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetAddress();
        }
    }
}

using Microsoft.AspNet.SignalR.Client;
using SignalRClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace LocationWCFClient.Windows
{
    /// <summary>
    /// SignalRWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EchoHubWindow : Window
    {

        //HubConnection Connection;
        //IHubProxy HubProxy;

        private string ServerURI = SignalRAppContext.ServerUrl;

        public EchoHubWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ConnectAsync();
        }
        EchoHub client;
        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        private async void ConnectAsync()
        {
            client = new EchoHub(ServerURI);
            client.Connection.Closed += Connection_Closed;
            client.Message += message =>
            {
                this.Dispatcher.Invoke(() =>
                    RichTextBoxConsole.AppendText(String.Format("{0}\r", message))
                );
            };
            bool r = await client.Start();
            if (r == false)
            {
                StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                return;
            }
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }

        /// <summary>
        /// If the server is stopped, the connection will time out after 30 seconds (default), and the 
        /// Closed event will fire.
        /// </summary>
        void Connection_Closed()
        {
            //Hide chat UI; show login UI
            var dispatcher = Application.Current.Dispatcher;
            //dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
            //dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
            //dispatcher.Invoke(() => StatusText.Content = "You have been disconnected.");
            //dispatcher.Invoke(() => SignInPanel.Visibility = Visibility.Visible);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            if (client != null)
            {
                client.Stop();
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                string msg = TbText.Text;
                client.Broadcast(msg);
                TbText.Text = "";
                TbText.Focus();
            }
            
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (BtnConnect.Content.ToString() == "Connect")
            {
                ServerURI = TbUrl.Text;
                ConnectAsync();
                BtnConnect.Content = "Disconnect";
            }
            else
            {
                Disconnect();
                BtnConnect.Content = "Connect";
            }
        }
    }
}

using SignalRClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// ChatHubWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChatHubWindow : Window
    {
        string ServerURI = "http://localhost:3333/signalr";

        public ChatHubWindow()
        {
            InitializeComponent();
        }

        private void BtnChangeNick_Click(object sender, RoutedEventArgs e)
        {

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

        ChatHub client;

        private async void ConnectAsync()
        {
            client = new ChatHub(ServerURI);
            //client.Connection.Closed += Connection_Closed;
            client.Message += message =>
            {
                this.Dispatcher.Invoke(() =>
                    RichTextBoxConsole.AppendText(String.Format("{0}\r", message))
                );
            };
            client.Welcome += (name, userList) =>
            {
                LbName.Content = name;
                LbUserList.ItemsSource = userList;
                LbUserList.DisplayMemberPath = "Name";
            };
            bool r = await client.Start();
            if (r == false)
            {
                StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                return;
            }
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.Send(TbText.Text);
            }
        }

        private void Disconnect()
        {
            if (client != null)
            {
                client.Stop();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }
    }
}

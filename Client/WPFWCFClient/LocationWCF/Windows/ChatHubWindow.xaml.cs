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
        private string ServerURI = SignalRAppContext.ServerUrl;

        public ChatHubWindow()
        {
            InitializeComponent();
        }

        private void BtnChangeNick_Click(object sender, RoutedEventArgs e)
        {
            string newName = TbNewName.Text;
            client.ChangeNickname(newName);
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
            client.Message += Client_Message;
            client.Welcome += Client_Welcome;
            client.NewUserNotification += Client_NewUserNotification;
            client.NicknameChangedNotification += Client_NicknameChangedNotification;
            client.UserDisconnectedNotification += Client_UserDisconnectedNotification;
            bool r = await client.Start();
            if (r == false)
            {
                StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                return;
            }
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }

        private void Client_UserDisconnectedNotification(UserData user)
        {
            this.Dispatcher.Invoke(() =>
            {
                UserData u = Users.Find(i => i.Id == user.Id);
                if (u != null)
                {
                    Users.Remove(u);

                    ShowUserList();

                    RichTextBoxConsole.AppendText(user.Name + " left the chat.\r");
                }
            });
        }

        private void ShowUserList()
        {
            LbUserList.ItemsSource = null;
            LbUserList.ItemsSource = Users;
            LbUserList.DisplayMemberPath = "Name";
        }

        private void Client_NicknameChangedNotification(UserData user, string oldName)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (user.Id == client.Connection.ConnectionId)
                {
                    LbName.Content = user.Name;
                }

                UserData u = Users.Find(i => i.Id == user.Id);
                if (u != null)
                {
                    u.Name = user.Name;
                    ShowUserList();
                    RichTextBoxConsole.AppendText(oldName + " is now " + user.Name + ".\r");
                }
            });
        }

        private void Client_NewUserNotification(UserData user)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (Users.Find(i => i.Id == user.Id) == null)
                {
                    Users.Add(user);

                    ShowUserList();

                    RichTextBoxConsole.AppendText("New user arrived: " + user.Name + ". Welcome!\r");
                }
            });
        }

        public List<UserData> Users=new List<UserData>();

        private void Client_Welcome(string name, UserData[] userList)
        {
            this.Dispatcher.Invoke(() =>
            {
                LbName.Content = name;

                Users = new List<UserData>();
                Users.AddRange(userList);

                ShowUserList();
            });
        }

        private void Client_Message(string message)
        {
            this.Dispatcher.Invoke(() =>
                   RichTextBoxConsole.AppendText(String.Format("{0}\r", message))
               );
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

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
using WCFClientLib;
using WCFServiceForWPF.LocationServices;

namespace LocationWCFClient.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            string port = TbPort.Text;
            string ip = TbIp.Text;
            string user = TbUser.Text;
            string pass = TbPass.Text;
            if (AppContext.Instance.Login(ip, port, user, pass))
            {
                ClientWindow clientWindow = new ClientWindow();
                clientWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("登录失败");
            }
        }
    }
}

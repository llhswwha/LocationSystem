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
using LocationClient.Windows;
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
            int type = CbHostType.SelectedIndex;
            WCFClientHostType hostType = (WCFClientHostType)type;
            string port = CbPort.Text;
            string ip = TbIp.Text;
            string user = TbUser.Text;
            string pass = TbPass.Text;

            if (AppContext.Instance.Login(ip, port, hostType, user, pass))
            {
                var clientWindow = new ClientWindow();
                clientWindow.Show();

                //var clientWindow = new NVRClientWindow();
                //clientWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("登录失败");
            }
        }

        private void CbHostType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int type = CbHostType.SelectedIndex;
            if(CbPort!=null)
                CbPort.SelectedIndex = type;
        }
    }
}

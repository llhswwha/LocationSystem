using ArchorUDPTool.Models;
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

namespace ArchorUDPTool
{
    /// <summary>
    /// UDPArchorInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UDPArchorInfoWindow : Window
    {
        public UDPArchorInfoWindow()
        {
            InitializeComponent();
        }

        UDPArchor archor;

        public UDPArchorInfoWindow(ArchorManager archorManager,UDPArchor archor)
        {
            InitializeComponent();
            this.archor = archor;
            this.archorManager = archorManager;
            this.archorManager.ArchorUpdated += ArchorManager_ArchorUpdated;
        }

        private void ArchorManager_ArchorUpdated(UDPArchor obj)
        {
            ShowValue(obj);
        }

        private ArchorManager archorManager;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowValue(null);
        }

        private void ShowValue(UDPArchor ar)
        {
            if (ar != null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    AibId.Value = ar.Id;
                    AibIp.Value = ar.Ip;
                    AibType.Value = ar.Type;
                    AibServerIP.Value = ar.ServerIp;
                    AibServerPort.Value = ar.ServerPort;
                    AibMask.Value = ar.Mask;
                    AibGateway.Value = ar.Gateway;
                    AibDHCP.Value = ar.DHCP;
                    AibSoftVersion.Value = ar.SoftVersion;
                    AibHardVersion.Value = ar.HardVersion;
                    AibPower.Value = ar.Power;
                    AibMAC.Value = ar.MAC;
                });
            }
        }

        private void Aib_GetEvent(UDPArchorInfoBox arg1, string arg2)
        {
            archorManager.GetArchorInfo(archor, arg1.GetKey());
        }

        private void Aib_SetEvent(UDPArchorInfoBox arg1, string arg2)
        {
            archorManager.SetArchorInfo(archor, arg1.GetKey());
        }

        private void MenuGetAll_Click(object sender, RoutedEventArgs e)
        {
            archorManager.ScanArchor(archor);
        }
    }
}

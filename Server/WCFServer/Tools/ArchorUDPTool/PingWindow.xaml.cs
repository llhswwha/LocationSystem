using ArchorUDPTool.Tools;
using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
using TModel.Tools;

namespace ArchorUDPTool
{
    /// <summary>
    /// PingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PingWindow : Window
    {
        public PingWindow()
        {
            InitializeComponent();
        }

        public PingWindow(string ip)
        {
            InitializeComponent();
            TbIp.Text = ip;
            StartPing();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartPing();
        }

        private void StartPing()
        {
            var ip = TbIp.Text;
            int count = TbCount.Text.ToInt();
            PingEx pingSender = new PingEx();
            pingSender.ProgressChanged += (p, pr) =>
            {
                TbConsole1.Text += pr.Line + "\n";
            };
            pingSender.SetData(TbSize.Text.ToInt());
            pingSender.Ping(ip, count);
        }
        PingEx pingSender;
        private void BtnStartRange_Click(object sender, RoutedEventArgs e)
        {
            var ip = TbIp.Text;
            int count = TbCount.Text.ToInt();
            pingSender = new PingEx();
            pingSender.ProgressChanged += (p, pr) =>
            {
                if (p > 0)
                {
                    ProgressBarEx1.Value = p;
                    TbConsole1.Text = pr.Line + "\n"+ TbConsole1.Text;
                    
                }
                else
                {
                    if (!string.IsNullOrEmpty(pr.ResultText))
                    {
                        TbConsole2.Text = pr.ResultText + "\n"+ TbConsole2.Text;
                    }
                }

            };
            pingSender.SetData(TbSize.Text.ToInt());
            pingSender.PingRange(ip, count);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbIp.ItemsSource = IpHelper.GetLocalList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (pingSender != null)
            {
                pingSender.Cancel();
            }
        }
    }
}

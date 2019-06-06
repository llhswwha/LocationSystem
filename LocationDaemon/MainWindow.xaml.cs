using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace LocationDaemon
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

        private DispatcherTimer timer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int interval = ConfigurationHelper.GetIntValue("Ip"); ;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(interval);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string processName = ConfigurationHelper.GetValue("processName");
            string path = ConfigurationHelper.GetValue("path");
            var ps = Process.GetProcessesByName(processName);
            if (ps.Length > 0)
            {

            }
            else
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("找不到文件:" + path);
                }
                Process.Start(path);
                TxtLog.Text = string.Format("[{0}]启动程序:{1}\n{2}", DateTime.Now.ToLongTimeString(), path, TxtLog.Text);
                Thread.Sleep(1000);
            }
        }
    }
}

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

        private DispatcherTimer timer2;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int interval = ConfigurationHelper.GetIntValue("interval"); ;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(interval);
            timer.Tick += Timer_Tick;
            timer.Start();//守护进程

            bool enableDeleteLog= ConfigurationHelper.GetBoolValue("enableDeleteLog");
            if (enableDeleteLog)
            {


                int interval2 = ConfigurationHelper.GetIntValue("logDeleteCheckHour"); ;
                timer2 = new DispatcherTimer();
                timer2.Interval = TimeSpan.FromHours(interval2);
                //timer2.Interval = TimeSpan.FromSeconds(1);
                timer2.Tick += Timer2_Tick;
                timer2.Start();
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            var logDir= ConfigurationHelper.GetValue("logDir");
            if (!Directory.Exists(logDir))
            {
                MessageBox.Show("找不到目录:" + logDir + "\n请重新设置并启动");
                timer2.Stop();
                return;
            }
            int keepDay= ConfigurationHelper.GetIntValue("logKeepDay");
            FileInfo[] logFiles = new DirectoryInfo(logDir).GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo file in logFiles)
            {
                try
                {
                    TimeSpan time = DateTime.Now - file.CreationTime;
                    if (time.TotalDays > keepDay)
                    {
                        file.Delete();
                        TxtLog.Text = string.Format("[{0}]删除日志:{1}\n{2}", DateTime.Now, file.Name, TxtLog.Text);
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

        }

        private List<int> errorProcess = new List<int>();

        private void Timer_Tick(object sender, EventArgs e)
        {
            string processName = ConfigurationHelper.GetValue("processName");
            string path = ConfigurationHelper.GetValue("path");
            var ps1 = Process.GetProcessesByName(processName).ToList() ;
            List<Process> ps = new List<Process>();
            for (int i = 0; i < ps1.Count; i++)
            {
                if (!errorProcess.Contains(ps1[i].Id))
                {
                    ps.Add(ps1[i]);
                }
            }
            
            if (ps.Count > 0)
            {
                if(ps.Count > 1)//出错崩溃，后台还在
                {
                    foreach (Process item in ps)
                    {
                        try
                        {
                            item.Kill();
                            errorProcess.Add(item.Id);//关闭不了，记录下来
                        }
                        catch (Exception ex)
                        {
                            errorProcess.Add(item.Id);//关闭不了，记录下来
                        }
                    }
                }
                var faultProcess = "WerFault";//XX 已停止工作界面
                var ps2 = Process.GetProcessesByName(faultProcess);
                if (ps2.Length > 0)
                {
                    foreach (Process item in ps2)
                    {
                        item.CloseMainWindow();
                    }
                }
            }
            else
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("找不到文件:" + path+"\n请重新设置并启动");
                    timer.Stop();
                    return;
                }
                Process.Start(path);
                TxtLog.Text = string.Format("[{0}]启动程序:{1}\n{2}", DateTime.Now.ToLongTimeString(), path, TxtLog.Text);
                Thread.Sleep(1000);
            }
        }
    }
}

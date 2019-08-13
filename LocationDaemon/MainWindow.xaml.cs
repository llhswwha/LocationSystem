using Base.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private DispatcherTimer daemonTimer;

        private DispatcherTimer deleteLogTimer;

        private DispatcherTimer timeTimer;


        private string targetProcessName;
        private string targetProcessPath;

        private string logDir;
        private int keepDay;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int interval = ConfigurationHelper.GetIntValue("interval");
                ;
                daemonTimer = new DispatcherTimer();
                daemonTimer.Interval = TimeSpan.FromSeconds(interval);
                daemonTimer.Tick += DaemonTimerTick;
                daemonTimer.Start(); //守护进程

                targetProcessName = ConfigurationHelper.GetValue("processName");
                targetProcessPath = ConfigurationHelper.GetValue("path");

                FileInfo file = new FileInfo(targetProcessPath);
                WriteLog("目标程序:" + file.FullName);
                WriteLog("进程名称:" + targetProcessName);
                WriteLog("----------------");
                bool enableDeleteLog = ConfigurationHelper.GetBoolValue("enableDeleteLog");
                if (enableDeleteLog)
                {
                    logDir = ConfigurationHelper.GetValue("logDir");
                    keepDay = ConfigurationHelper.GetIntValue("logKeepDay");
                    DirectoryInfo dir = new DirectoryInfo(logDir);

                    WriteLog("日志文件夹:" + dir.FullName);
                    WriteLog("保留时间:" + keepDay + "天");
                    WriteLog("----------------");

                    int interval2 = ConfigurationHelper.GetIntValue("logDeleteCheckHour");
                    ;
                    deleteLogTimer = new DispatcherTimer();
                    deleteLogTimer.Interval = TimeSpan.FromHours(interval2);
                    //timer2.Interval = TimeSpan.FromSeconds(1);
                    deleteLogTimer.Tick += DeleteLogTimerTick;
                    deleteLogTimer.Start();
                }

                bool EnableRestart = ConfigurationHelper.GetBoolValue("EnableRestart");
                if (EnableRestart)
                {
                    restartTimer = new DispatcherTimer();
                    restartTimer.Interval = TimeSpan.FromSeconds(1);
                    //timer2.Interval = TimeSpan.FromSeconds(1);
                    restartTimer.Tick += RestartTimer_Tick;
                    restartTimer.Start();



                    RestartMode = ConfigurationHelper.GetIntValue("RestartMode");
                    RestartInterval = ConfigurationHelper.GetIntValue("RestartInterval");
                    string temp = ConfigurationHelper.GetValue("RestartTime");

                    if (RestartMode == 1)
                    {
                        WriteLog("重启时间（每天）:" + temp);
                    }
                    else if (RestartMode == 2)
                    {
                        WriteLog("重启时间（间隔）:" + RestartInterval + "分钟");
                    }

                    string[] parts = temp.Split(':');
                    if (parts.Length == 2)
                    {
                        DateTime now = DateTime.Now;
                        RestartTime = new DateTime(now.Year, now.Month, now.Day, parts[0].ToInt(), parts[1].ToInt(), 0);
                    }
                    //WriteLog("RestartTime:" + RestartTime);

                    WriteLog("----------------");
                }

                timeTimer = new DispatcherTimer();
                timeTimer.Interval = TimeSpan.FromMilliseconds(500);
                timeTimer.Tick += TimeTimer_Tick;
                timeTimer.Start();

                bool EchoRestart = ConfigurationHelper.GetBoolValue("EchoRestart");
                if (EchoRestart)
                {
                    int EchoInterval = ConfigurationHelper.GetIntValue("EchoInterval");
                    EchoUrl = ConfigurationHelper.GetValue("EchoUrl");
                    EchoLog = ConfigurationHelper.GetBoolValue("EchoLog");

                    WriteLog("心跳包地址:" + EchoUrl);
                    WriteLog("心跳包间隔:" + EchoInterval+"s");

                    echorTimer = new DispatcherTimer();
                    echorTimer.Interval = TimeSpan.FromSeconds(EchoInterval);
                    echorTimer.Tick += EchorTimer_Tick;
                    echorTimer.Start();
                }
            }
            catch (Exception exception)
            {
                WriteLog("出错:" + exception);
            }
        }

        private void EchorTimer_Tick(object sender, EventArgs e)
        {
            string t = GetString(EchoUrl);
            if (string.IsNullOrEmpty(t))
            {
                if (RestartProcess() == false)
                {
                    echorTimer.Stop();
                }
            }
            else
            {
                if(EchoLog)
                    WriteLog("获取心跳包返回:" + t);
            }
        }

        public string GetString(string uri, string accept = "")
        {
            try
            {
                //LastUrl = uri;
                if (uri == null) return null;
                //Log.Info(LogTags.BaseData, "uri:" + uri);
                var client = new HttpClient();
                if (!string.IsNullOrEmpty(accept))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                }
                var resMsg = client.GetAsync(uri).Result;
                var result = resMsg.Content.ReadAsStringAsync().Result;
                //LastJson = result;
                return result;
            }
            catch (Exception ex)
            {
                //Log.Error(LogTags.Server, string.Format("WebApiHelper.GetString:uri={0},error={1}",uri,ex.Message));
                //return null;
                WriteLog("获取心跳包出错:" + ex.Message);
                return "";
            }
        }

        private string EchoUrl;

        private bool EchoLog;

        private DispatcherTimer echorTimer;

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            this.Title = string.Format("{0} [{1}][{2:dd\\.hh\\:mm\\:ss}]", "守护进程", now.ToString("HH:mm:ss"), (now - startTime));
        }

        private DispatcherTimer restartTimer;

        private int RestartMode = 0;
        private DateTime RestartTime;
        private int RestartInterval = 0;

        private DateTime lastRestartTime=DateTime.Now;

        private DateTime startTime = DateTime.Now;

        private bool RestartProcess()
        {
            var ps1 = Process.GetProcessesByName(targetProcessName).ToList();
            foreach (Process p in ps1)
            {
                if (p.HasExited)
                {

                }
                else
                {
                    p.CloseMainWindow(); //关闭
                }
            }

            return StartProcess();
        }

        private void RestartTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;
                if (RestartMode == 2)
                {
                    if ((now - lastRestartTime).TotalMinutes > RestartInterval)
                    {
                        if (RestartProcess() == false)
                        {
                            restartTimer.Stop();
                        }
                    }
                }
                else if (RestartMode == 1)
                {
                    if (now.Hour == RestartTime.Hour 
                        && now.Minute == RestartTime.Minute
                        && (now - lastRestartTime).TotalMinutes > 1)
                    {
                        if (StartProcess() == false)
                        {
                            restartTimer.Stop();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("出错:" + ex);
            }

        }

        private void DeleteLogTimerTick(object sender, EventArgs e)
        {


            if (!Directory.Exists(logDir))
            {
                MessageBox.Show("找不到目录:" + logDir + "\n请重新设置并启动");
                deleteLogTimer.Stop();
                return;
            }
            
            FileInfo[] logFiles = new DirectoryInfo(logDir).GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo file in logFiles)
            {
                try
                {
                    TimeSpan time = DateTime.Now - file.CreationTime;
                    if (time.TotalDays > keepDay)
                    {
                        file.Delete();
                        WriteLog("删除日志:" + file.Name);
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

        }

        private List<int> errorProcess = new List<int>();

        private void DaemonTimerTick(object sender, EventArgs e)
        {
            var ps1 = Process.GetProcessesByName(targetProcessName).ToList() ;
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
                            //errorProcess.Add(item.Id);//关闭不了，记录下来
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
                if (StartProcess() == false)
                {
                    daemonTimer.Stop();
                }
            }
        }

        private bool StartProcess()
        {
            if (!File.Exists(targetProcessPath))
            {
                MessageBox.Show("找不到文件:" + targetProcessPath + "\n请重新设置并启动");
                
                return false;
            }

            Process process = Process.Start(targetProcessPath); //核心，启动目标程序

            FileInfo file = new FileInfo(targetProcessPath);
            WriteLog("启动程序:" + file.FullName);
            lastRestartTime = DateTime.Now;

            Thread.Sleep(1000);
            return true;
        }

        private void WriteLog(string log)
        {
            TxtLog.Text = string.Format("[{0}]{1}\n{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log,
                TxtLog.Text);
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void MenuSetting_OnClick(object sender, RoutedEventArgs e)
        {
            SettingWindow win=new SettingWindow();
            win.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
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
                string v= ConfigurationHelper.GetValue("version");
                titleName += " v" + v;


                var memory = SystemHelper.GetPhisicalMemory();
                WriteLog("系统内存:" + memory+"GB");

                int interval = ConfigurationHelper.GetIntValue("interval");
                
                daemonTimer = new DispatcherTimer();
                daemonTimer.Interval = TimeSpan.FromSeconds(interval);
                daemonTimer.Tick += DaemonTimerTick;
                daemonTimer.Start(); //守护进程

                targetProcessName = ConfigurationHelper.GetValue("processName");
                targetProcessPath = ConfigurationHelper.GetValue("path");
                WriteLog("processName:" + targetProcessName);
                WriteLog("path:" + targetProcessPath);

                if (!targetProcessPath.Contains("\\")&& !targetProcessPath.Contains("/"))//相对路径，只写了启动程序名的
                {
                    targetProcessPath = AppDomain.CurrentDomain.BaseDirectory + targetProcessPath;
                }

                FileInfo file = new FileInfo(targetProcessPath);
                WriteLog("目标程序:" + file.FullName);
                bool exists = File.Exists(file.FullName);
                WriteLog("程序是否存在:" + exists);
                if (exists)
                {
                    System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(file.FullName);
                    if (fv != null)
                    {
                        WriteLog("程序文件版本:" + fv.FileVersion);
                    }
                }
                else
                {
                    WriteLog("目标程序不存在！！！");
                }

                WriteLog("进程名称:" + targetProcessName);
                var ps = GetProcesses(targetProcessName);
                WriteLog("进程数量:" + ps.Count);
                if (ps.Count == 1)
                {
                    GetMemerySize(ps[0]);
                }

                MaxMemorySize= ConfigurationHelper.GetDoubleValue("MaxMemorySize");
                WriteLog("内存限制:" + MaxMemorySize+"MB");

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

                //CloseWerFault();

                if (exists==false)
                {
                    MessageBox.Show("目标程序不存在！！！");
                }
            }
            catch (Exception exception)
            {
                WriteLog("出错:" + exception);
            }
        }

        private double MaxMemorySize;

        private bool isEchorBusy = false;

        private void EchorTimer_Tick(object sender, EventArgs e)
        {
            if (isEchorBusy) return;
            if (EchoLog)
            {
                WriteLog2("发送心跳包，等待返回...");
            }

            Worker.Run(() =>
            {
                isEchorBusy = true;
                string t = GetString(EchoUrl);
                return t;
            }, (t) =>
            {
                isEchorBusy = false;
                if (string.IsNullOrEmpty(t))
                {
                    WriteLog2("无心跳包返回,重启程序！");
                    bool r = RestartProcess("EchorTimer");//重启
                    if (r == false)//重启失败
                    {
                        echorTimer.Stop();
                    }
                    else//重启成功
                    {
                        
                    }
                }
                else
                {
                    if (EchoLog)
                        WriteLog2("获取心跳包返回:" + t);
                }
            });
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
            RefreshTitle();
        }

        private string titleName = "守护进程";

        private void RefreshTitle()
        {
            DateTime now = DateTime.Now;

            this.Title = string.Format("{0} [{1}][{2:dd\\.hh\\:mm\\:ss}]{3}", titleName, now.ToString("HH:mm:ss"), (now - startTime), MemorySize);
        }

        private DispatcherTimer restartTimer;

        private int RestartMode = 0;
        private DateTime RestartTime;
        private int RestartInterval = 0;

        private DateTime lastRestartTime=DateTime.Now;

        private DateTime startTime = DateTime.Now;

        private bool RestartProcess(string tag)
        {
            var ps1 = GetProcesses(targetProcessName);
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

            return StartProcess(tag);
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
                        if (RestartProcess("RestartMode == 2") == false)
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
                        if (StartProcess("RestartMode == 1") == false)
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

        public List<Process> GetProcesses(string processName)
        {
            //WriteLog("Debugger.IsAttached:" + Debugger.IsAttached);
            var ps1 = Process.GetProcessesByName(processName).Where(i=>i.HasExited==false).ToList();
            if (ps1.Count == 0)
            {
                if (Debugger.IsAttached)//vs调试模式程序
                {
                    ps1 = Process.GetProcessesByName(processName + ".vshost").Where(i => i.HasExited == false).ToList();
                }
              
                //var ps2=Process.GetProcesses().ToList();
                //var ps3 = ps2.FindAll(i => i.ProcessName.ToLower().StartsWith("l"));
                //foreach (var ps in ps3)
                //{
                //    if (ps.ProcessName.Contains(processName))
                //    {
                //        ps1.Add(ps);
                //    }
                //}
            }
            return ps1;
        }

        private void DaemonTimerTick(object sender, EventArgs e)
        {
            //WriteLog("targetProcessName:" + targetProcessName);
            var ps1 = GetProcesses(targetProcessName);
            //WriteLog("ps count1:"+ps1.Count);
            List<Process> ps = new List<Process>();
            for (int i = 0; i < ps1.Count; i++)
            {
                if (!errorProcess.Contains(ps1[i].Id))
                {
                    ps.Add(ps1[i]);
                }
            }
            //WriteLog("ps count2:" + ps.Count);
            if (ps.Count > 0)
            {
                if(ps.Count > 1)//出错崩溃，后台还在
                {
                    //WriteLog("ps count3:" + ps.Count);
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
                else //存在一个目标进程
                {
                    var p = ps[0];

                    bool r=CloseWerFault();
                    if (r)
                    {
                        RestartProcess("WerFault");
                    }
                    else
                    {
                        var size=GetMemerySize(p);
                        if (MaxMemorySize > 0 && size > MaxMemorySize)
                        {
                            RestartProcess("MaxMemorySize");
                            GetMemerySize(currentProcess);
                        }
                    }
                }
            }
            else
            {
                if (StartProcess("DaemonTimer") == false)
                {
                    daemonTimer.Stop();
                }
            }
        }

        private float GetMemerySize(Process p)
        {
            float size = 0;
            try
            {
                //WriteLog("WorkingSet64:" + (p.WorkingSet64/(1024*1024))) ;
                //WriteLog("PrivateMemorySize:" + (p.PrivateMemorySize64 / (1024 * 1024)));
                //WriteLog("PagedMemorySize:" + (p.PagedMemorySize64 / (1024 * 1024)));
                PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", p.ProcessName);   //第二个参数就是得到自有工作集
                size = (pf1.NextValue() / (1024 * 1024));
                //WriteLog("Performance PagedMemorySize:" + size);//这个才是和任务管理器里面的内存值相同的，但是比较耗费性能
            }
            catch (Exception e)
            {
                
            }

            MemorySize = String.Format("[{0:F1}MB]", size);
            return size;
        }

        private bool getMemoryBusy = false;

        private string MemorySize = "";

        private bool CloseWerFault()
        {
            bool r = false;
            var faultProcess = "WerFault";//XX 已停止工作界面
            var ps2 = GetProcesses(faultProcess);
            //WriteLog("ps count4:" + ps.Count);
            if (ps2.Count > 0)
            {
                try
                {
                    foreach (Process item in ps2)
                    {
                        //if (item.MainWindowTitle == targetProcessName) //不一样
                        {
                            item.CloseMainWindow();
                            r = true;
                        }
                        
                    }
                }
                catch (Exception exception)
                {

                }
            }
            else//且正常运行
            {

            }

            return r;
        }

        private bool StartProcess(string tag)
        {
            if (!File.Exists(targetProcessPath))
            {
                MessageBox.Show("找不到文件:" + targetProcessPath + "\n请重新设置并启动");
                
                return false;
            }

            currentProcess = Process.Start(targetProcessPath); //核心，启动目标程序

            FileInfo file = new FileInfo(targetProcessPath);
            WriteLog(tag+"|启动程序:" + file.FullName);
            lastRestartTime = DateTime.Now;

            Thread.Sleep(1000);
            return true;
        }

        private Process currentProcess;

        private string log1 = "";

        private string log2 = "";

        private int maxLogLength = 10000;

        private void WriteLog(string log)
        {
            log1 = string.Format("[{0}]{1}\n{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log, log1);
            if (log1.Length > maxLogLength)
            {
                log1 = log1.Substring(0, maxLogLength / 2);
            }
            TxtLog.Text = log1;
        }

        private void WriteLog2(string log)
        {
            log2 = string.Format("[{0}]{1}\n{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log, log2);
            if (log2.Length > maxLogLength)
            {
                log2 = log2.Substring(0, maxLogLength / 2);
            }

            TxtLog2.Text = log2;
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

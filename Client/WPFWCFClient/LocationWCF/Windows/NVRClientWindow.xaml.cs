using LocationWCFClient;
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
using System.Windows.Threading;
using TModel.Tools;
using WCFServiceForWPF.LocationServices;

namespace LocationClient.Windows
{
    /// <summary>
    /// NVRClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NVRClientWindow : Window
    {
        public NVRClientWindow()
        {
            InitializeComponent();
            client = AppContext.Instance.Client.InnerClient;
        }

        private DispatcherTimer timer;

        LocationServiceClient client;
        private DownloadInfo info;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            info = new DownloadInfo();
            info.CId = Convert.ToInt32(TbCid.Text);
            info.StartTime = TbStart.Text;
            info.EndTime = TbEnd.Text;
            info.Channel = TbChannel.Text;
            info.Ip = TbIp.Text;

            var r = client.StartGetNVSVideo(info);
            if (r != null)
            {
                WriteLog(r.Result+"|"+r.Message);

                if (r.Result)
                {
                    if (string.IsNullOrEmpty(r.Url))
                    {
                        WriteLog("启动计时器");
                        if (timer == null)
                        {
                            timer = new DispatcherTimer();
                            timer.Interval = TimeSpan.FromMilliseconds(100);
                            timer.Tick += Timer_Tick;
                        }
                        timer.Start();
                    }
                    else
                    {
                        MessageBox.Show("已经下载过:" + r.Url);
                    }
                }
                else
                {
                    MessageBox.Show("下载错误:" + r.Message);
                }
            }
            else
            {
                MessageBox.Show("结果为空");
            }
        }

        private string _log = "";

        private void WriteLog(string log)
        {
            string txt = string.Format("[{0}]{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log);
            _log = txt + "\n" + _log;
            TbResult.Text = _log;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var progress = client.GetNVSProgress(info);
            if (progress != null)
            {
                WriteLog("p:"+progress.Progress);
                ProgressBar1.Value = progress.Progress;
                if (progress.IsFinished)
                {
                    timer.Stop();
                    WriteLog("下载完成:" + progress.Url);
                    MessageBox.Show("下载完成:"+ progress.Url);
                    
                }
            }
            else
            {

                timer.Stop();
                MessageBox.Show("进度为空");
            }

        }
    }
}

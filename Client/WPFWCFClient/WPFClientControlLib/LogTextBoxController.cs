using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPFClientControlLib
{
    public class LogTextBoxController:IDisposable
    {
        public TextBox TbLogs;

        public List<string> Tags = new List<string>();

        public LogTextBoxController(TextBox tb,params string[] tags):this()
        {
            Init(tb, tags);
        }

        public LogTextBoxController()
        {

        }

        public void Init(TextBox tb, params string[] tags)
        {
            this.TbLogs = tb;
            this.Tags = tags.ToList();
            Location.BLL.Tool.Log.NewLogEvent += AddLog;

            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isDirty)
            {
                TbLogs.Text = logs;
                isDirty = false;

                if (LogChanged != null)
                {
                    LogChanged(last, count);
                }
            }
            
        }

        DispatcherTimer timer;

        private int MaxLength = int.MaxValue;
        private int MaxLength2 = int.MaxValue/2;

        private string logs = "";
        private bool isDirty = false;

        public void SetMaxLength(int length)
        {
            MaxLength = length;
            MaxLength2 = length / 2;
        }

        private int count = 0;
        LogInfo last;
        public void AddLog(LogInfo info)
        {
            if (logs.Length > MaxLength)
            {
                logs = logs.Substring(0, MaxLength2);
            }

            //string[] parts = log.Split('|');
            if (Tags.Contains(info.Tag))
            {
                last = info;
                count++;
                logs = info.Log + "\n" + logs;
                isDirty = true;
                //TbLogs.Dispatcher.Invoke(() =>
                //{
                //    TbLogs.Text = logs;
                //});
                //Tags.Add()
            }

        }

        public event Action<LogInfo,int> LogChanged;

        public void Dispose()
        {
            if (timer!=null)
            {
                timer.Stop();
            }
            
            Location.BLL.Tool.Log.NewLogEvent -= AddLog;
        }
    }
}

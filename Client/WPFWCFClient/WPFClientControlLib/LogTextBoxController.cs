using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

            Location.BLL.Tool.Log.NewLogEvent += Log_NewLogEvent;
        }

        private int MaxLength = int.MaxValue;
        private int MaxLength2 = int.MaxValue/2;

        private string logs = "";

        public void SetMaxLength(int length)
        {
            MaxLength = length;
            MaxLength2 = length / 2;
        }

        private void Log_NewLogEvent(string tag, string log)
        {
            if (logs.Length > MaxLength)
            {
                logs = logs.Substring(0, MaxLength2);
            }

            //string[] parts = log.Split('|');
            if (Tags.Contains(tag))
            {
                logs = log + "\n" + logs;
                TbLogs.Dispatcher.Invoke(() =>
                {
                    TbLogs.Text = logs;
                });
            }
        }

        public void Dispose()
        {
            Location.BLL.Tool.Log.NewLogEvent -= Log_NewLogEvent;
        }
    }
}

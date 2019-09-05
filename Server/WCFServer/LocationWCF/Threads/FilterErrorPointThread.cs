using Base.Common.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TModel.Tools;

namespace LocationServer.Threads
{
    /// <summary>
    /// 删除错误点进程
    /// 手动删除以前的全部错误点后，又不添加新的错误点的话，可以不用这个，保险起见，每天4:00检查一遍
    /// </summary>
    public class FilterErrorPointThread: FunctionThread
    {
        public DateTime lastDoTime;

        public DateTime doTime;

        public FilterErrorPointThread(string time):base("FilterErrorPointThread")
        {
            string[] parts = time.Split(':');
            if (parts.Length == 2)
            {
                DateTime now = DateTime.Now;
                doTime = new DateTime(now.Year, now.Month, now.Day, parts[0].ToInt(), parts[1].ToInt(), 0);
            }
        }

        public override void DoFunction()
        {
            while (true)
            {

                DateTime now = DateTime.Now;
                if (now.Hour == doTime.Hour
                        && now.Minute == doTime.Minute
                        && (now - lastDoTime).TotalMinutes > 1)
                {
                    //DoFunction();

                }
                Thread.Sleep(1000);
            }
        }

        public void DailyDoFunction()
        {
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TModel.Tools;

namespace Base.Common.Threads
{
    public abstract class DailyTimerThread : IntervalTimerThread
    {

        public DateTime lastDoTime;

        public DateTime doTime;

        public DailyTimerThread(string time,TimeSpan interval, TimeSpan? delay) : base(interval, delay)
        {
            string[] parts = time.Split(':');
            if (parts.Length == 2)
            {
                DateTime now = DateTime.Now;
                doTime = new DateTime(now.Year, now.Month, now.Day, parts[0].ToInt(), parts[1].ToInt(), 0);
            }
        }

        public override bool TickFunction()
        {
            DateTime now = DateTime.Now;
            if (now.Hour == doTime.Hour
                    && now.Minute == doTime.Minute
                    && (now - lastDoTime).TotalMinutes > 1)
            {
                DailyFunction();//一天一次
            }
            return true;
        }

        public abstract void DailyFunction();
    }
}

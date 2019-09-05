using Base.Common.Threads;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Base.Common.Threads
{
    public abstract class IntervalTimerThread:FunctionThread
    {
        public TimeSpan Interval { get; set; }
        public TimeSpan? Delay { get; set; }
        public IntervalTimerThread(TimeSpan interval, TimeSpan? delay)
        {
            Interval = interval;
            Delay = delay;
        }

        public override void DoFunction()
        {
            Log.Info(Name, "Start");
            if (Delay != null)
            {
                Thread.Sleep((TimeSpan)Delay);
            }
           
            while (true)
            {
                try
                {
                    if (TickFunction() == false)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Name, ex.ToString());
                }
                finally
                {
                    Thread.Sleep(Interval);
                }
            }
        }

        public abstract bool TickFunction();
    }
}

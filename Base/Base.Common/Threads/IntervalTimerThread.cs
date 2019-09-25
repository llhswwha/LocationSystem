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

        protected abstract void DoBeforeWhile();

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
                    if (IsStop) break;
                    Thread.Sleep(Interval);
                }
                catch (Exception ex)
                {
                    Log.Error(Name, ex.ToString());
                    if (IsStop) break;
                    Thread.Sleep(Interval);
                }
                //不能用Finally，会导致无法Abort退出的
                //finally
                //{
                //    Thread.Sleep(Interval);
                //}
            }
        }

        public void StartTimer()
        {

        }

        public bool IsStop = false;

        public override void Abort()
        {
            IsStop = true;
            base.Abort();
        }

        public abstract bool TickFunction();
    }
}

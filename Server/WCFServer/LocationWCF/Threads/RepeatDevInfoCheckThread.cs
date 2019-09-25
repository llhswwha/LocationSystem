using Base.Common.Threads;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    public class RepeatDevInfoCheckThread:IntervalTimerThread
    {
        public RepeatDevInfoCheckThread(int seconds)
            :base(
                 TimeSpan.FromSeconds(seconds)
                 , TimeSpan.FromSeconds(1))
        {
            
        }

        public override bool TickFunction()
        {
            using (Bll bll = new Bll())
            {
                AreaTreeInitializer initializer = new AreaTreeInitializer(bll);
                initializer.ClearRepeatDev(Name);
            }
            //throw new NotImplementedException();
            return true;
        }

        protected override void DoBeforeWhile()
        {
            TickFunction();
        }
    }
}

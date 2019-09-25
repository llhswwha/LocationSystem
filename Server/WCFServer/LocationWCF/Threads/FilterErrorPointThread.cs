using Base.Common.Threads;
using BLL;
using LocationServices.Locations.Services;
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
    public class FilterErrorPointThread : DailyTimerThread
    {
        public FilterErrorPointThread(string time) 
            : base(time, TimeSpan.FromSeconds(10), null)
        {
            
        }

        public override void DailyFunction()
        {
            Bll bll = Bll.NewBllNoRelation();
            DateTime now = DateTime.Now;
            DateTime start = now.AddHours(-25);
            var list=bll.Positions.GetInfoListOfDate(start, now);
            var count=bll.Positions.RemoveErrorPoints(list);
            bll.Dispose();
        }

        protected override void DoBeforeWhile()
        {
            //DailyFunction();
        }
    }
}

using Base.Common.Threads;
using BLL;
using Location.BLL.Tool;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    public class AlarmRemoveThread : IntervalTimerThread
    {
        private int DevAlarmKeepDays = 0;

        public AlarmRemoveThread(int days)
            :base(
                 TimeSpan.FromHours(10)//10小时检查一次
                 //TimeSpan.FromDays(1)//一天检查一次
                 , TimeSpan.FromSeconds(5))
        {
            DevAlarmKeepDays = days;
        }

        public override bool DailyFunction()
        {
            Log.Info(Name, "检查设备告警");
            //清除某一个时间之前的所有告警
            Bll db = Bll.NewBllNoRelation();
            DateTime nowTime = DateTime.Now;
            DateTime starttime = DateTime.Now.AddDays(-DevAlarmKeepDays);
            var starttimeStamp = TimeConvert.ToStamp(starttime);
            var query = db.DevAlarms.DbSet.Where(i => i.AlarmTimeStamp < starttimeStamp);
            var count = query.Count();
            if (count > 0)
            {
                query.DeleteFromQuery();//这样删除效率高
                Log.Info(Name, "清除历史设备告警,数量:" + count);
            }
            return true;
        }
    }
}

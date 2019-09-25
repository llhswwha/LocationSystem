using System.Collections.Generic;
using DAL;
using DbModel.Location.Alarm;

namespace BLL.Blls.Location
{
    public class LocationAlarmBll : BaseBll<LocationAlarm, LocationDb>
    {
        public LocationAlarmBll():base()
        {

        }
        public LocationAlarmBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationAlarms;
        }

        public override List<LocationAlarm> ToList(bool isTracking = false)
        {
            var list= base.ToList(isTracking);
            foreach (var item in list)
            {
                if(item.Content== null && item.AlarmType == DbModel.Tools.LocationAlarmType.低电告警)
                {
                    item.Content = "低电告警";
                }
            }
            //var list2 = list.FindAll(i => i.Content != null);
            return list;
        }
    }
}

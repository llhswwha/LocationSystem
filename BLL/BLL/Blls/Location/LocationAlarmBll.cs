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
    }
}

using DAL;
using DbModel.Location.Alarm;

namespace BLL.Blls.Location
{
    public class DevAlarmBll : BaseBll<DevAlarm, LocationDb>
    {
        public DevAlarmBll():base()
        {

        }
        public DevAlarmBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevAlarms;
        }
    }
}

using DAL;
using DbModel.LocationHistory.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class LocationAlarmHistoryBll : BaseBll<LocationAlarmHistory, LocationHistoryDb>
    {
        public LocationAlarmHistoryBll() : base()
        {

        }
        public LocationAlarmHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationAlarmHistorys;
        }
    }
}

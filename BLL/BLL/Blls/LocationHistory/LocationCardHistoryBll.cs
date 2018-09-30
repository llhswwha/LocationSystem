using DAL;
using DbModel.LocationHistory.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class LocationCardHistoryBll : BaseBll<LocationCardHistory, LocationHistoryDb>
    {
        public LocationCardHistoryBll() : base()
        {

        }
        public LocationCardHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationCardHistorys;
        }
    }
}

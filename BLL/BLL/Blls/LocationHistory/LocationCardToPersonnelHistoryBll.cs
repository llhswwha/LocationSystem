using DAL;
using DbModel.LocationHistory.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class LocationCardToPersonnelHistoryBll : BaseBll<LocationCardToPersonnelHistory, LocationHistoryDb>
    {
        public LocationCardToPersonnelHistoryBll() : base()
        {

        }
        public LocationCardToPersonnelHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationCardToPersonnelHistorys;
        }
    }
}

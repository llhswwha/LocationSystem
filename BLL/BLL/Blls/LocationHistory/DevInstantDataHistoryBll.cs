using DAL;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class DevInstantDataHistoryBll : BaseBll<DevInstantDataHistory, LocationHistoryDb>
    {
        public DevInstantDataHistoryBll() : base()
        {

        }
        public DevInstantDataHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevInstantDataHistorys;
        }
    }
}

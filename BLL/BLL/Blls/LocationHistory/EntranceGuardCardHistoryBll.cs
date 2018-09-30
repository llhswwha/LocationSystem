using DAL;
using DbModel.LocationHistory.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class EntranceGuardCardHistoryBll : BaseBll<EntranceGuardCardHistory, LocationHistoryDb>
    {
        public EntranceGuardCardHistoryBll() : base()
        {

        }
        public EntranceGuardCardHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.EntranceGuardCardHistorys;
        }
    }
}

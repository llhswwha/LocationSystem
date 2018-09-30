using DAL;
using DbModel.LocationHistory.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class EntranceGuardCardToPersonnelHistoryBll : BaseBll<EntranceGuardCardToPersonnelHistory, LocationHistoryDb>
    {
        public EntranceGuardCardToPersonnelHistoryBll() : base()
        {

        }
        public EntranceGuardCardToPersonnelHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.EntranceGuardCardToPersonnelHistorys;
        }
    }
}

using DAL;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class PatrolPointHistoryBll : BaseBll<PatrolPointHistory, LocationHistoryDb>
    {
        public PatrolPointHistoryBll() : base()
        {

        }
        public PatrolPointHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PatrolPointHistorys;
        }
    }
}

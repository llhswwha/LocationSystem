using DAL;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class WorkTicketHistoryBll : BaseBll<WorkTicketHistory, LocationHistoryDb>
    {
        public WorkTicketHistoryBll() : base()
        {

        }
        public WorkTicketHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.WorkTicketHistorys;
        }
    }
}

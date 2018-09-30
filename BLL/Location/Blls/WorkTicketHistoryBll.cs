using Location.DAL;
using Location.Model.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class WorkTicketHistoryBll : BaseBll<WorkTicketHistory, LocationHistoryDb>
    {
        public WorkTicketHistoryBll():base()
        {

        }
        public WorkTicketHistoryBll(LocationHistoryDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.WorkTicketHistorys;
        }
    }
}

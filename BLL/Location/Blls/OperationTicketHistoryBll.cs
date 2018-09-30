using Location.DAL;
using Location.Model.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class OperationTicketHistoryBll : BaseBll<OperationTicketHistory, LocationHistoryDb>
    {
        public OperationTicketHistoryBll():base()
        {

        }
        public OperationTicketHistoryBll(LocationHistoryDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.OperationTicketHistorys;
        }
    }
}

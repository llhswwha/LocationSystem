using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model.Work;

namespace Location.BLL.Blls
{
    public class OperationTicketBll : BaseBll<OperationTicket, LocationDb>
    {
        public OperationTicketBll():base()
        {

        }
        public OperationTicketBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.OperationTickets;
        }
    }
}

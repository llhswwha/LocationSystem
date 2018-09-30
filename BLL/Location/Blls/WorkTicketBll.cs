using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model.Work;

namespace Location.BLL.Blls
{
    public class WorkTicketBll : BaseBll<WorkTicket, LocationDb>
    {
        public WorkTicketBll():base()
        {

        }
        public WorkTicketBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.WorkTickets;
        }
    }
}

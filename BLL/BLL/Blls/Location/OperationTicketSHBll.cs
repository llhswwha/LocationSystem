using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class OperationTicketSHBll : BaseBll<OperationTicketSH, LocationDb>
    {
        public OperationTicketSHBll() : base()
        {

        }
        public OperationTicketSHBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.OpeartionTicketSHs;
        }

        public List<OperationTicketSH> GetListByCondition(string value, DateTime startTime, DateTime endTime)
        {
            List<OperationTicketSH> list = new List<OperationTicketSH>();
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(value))
            {
                sqlwhere += "  and (ticketCode like '%"+value+"%' or ticketName like '%"+value+"%')";
            }
            string sqlstr = string.Format(@"select * from operationticketshes where  StartTime>'{0}' and EndTime<'{1}' "+sqlwhere, startTime, endTime);
            DbRawSqlQuery<OperationTicketSH> result = Db.Database.SqlQuery<OperationTicketSH>(sqlstr);
            list = result.ToList();
            return list;
        }
    }
}

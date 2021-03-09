using DAL;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;

namespace BLL.Blls.LocationHistory
{
    public class WorkTicketHistorySHBll : BaseBll<WorkTicketHistorySH, LocationHistoryDb>
    {
        public WorkTicketHistorySHBll():base()
        { }
        public WorkTicketHistorySHBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.WorkTicketHistorySHes;
        }


        public PageInfo<WorkTicketHistorySH> GetPageByCondition(string value, DateTime startTime, DateTime endTime, int pageIndex, int pageSize)
        {

            string sqlwhere = "";
            if (!string.IsNullOrEmpty(value))
            {
                sqlwhere += "  and (ticketCode like '%" + value + "%' or ticketName like '%" + value + "%')";
            }
            if (startTime != null && endTime != null)
            {
                sqlwhere += string.Format(" and  StartTime>'{0}' and EndTime<'{1}'", startTime, endTime);
            }
            string sqlCount = string.Format(@"select count(id) as count from worktickethistoryshes where 1=1 " + sqlwhere);
            string strsql = string.Format(@"select id,Abutment_Id,fromorder,ticketcode,type,state,createtime,StartTime,endtime,null as  detail,rawid,ticketname  from worktickethistoryshes  where 1=1  {0} ", sqlwhere);
            PageInfo<WorkTicketHistorySH> page = GetPageList(sqlCount, strsql, pageIndex, pageSize);
            return page;
        }


        public List<WorkTicketHistorySH> GetListByTime(int count, string field, string sort)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(field))
            {
                sqlwhere += " order by " + field + " " + sort;
            }
            if (count > 0)
            {
                sqlwhere += " limit " + count;
            }
            string strsql = string.Format(@"select id,Abutment_Id,fromorder,ticketcode,type,state,createtime,StartTime,endtime,null as  detail,rawid,ticketname  from worktickethistoryshes  where 1=1 " + sqlwhere);
            List<WorkTicketHistorySH> list = GetListBySql(strsql);
            return list;
        }
    }
}

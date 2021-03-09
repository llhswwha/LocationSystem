using DAL;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using TModel.Tools;

namespace BLL.Blls.LocationHistory
{
    public class OperationTicketHistorySHBll : BaseBll<OperationTicketHistorySH, LocationHistoryDb>
    {
        public OperationTicketHistorySHBll():base()
        {

        }
        public OperationTicketHistorySHBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.OperationTicketHistorySHs;
        }

        public List<OperationTicketHistorySH> GetListByCondition(string value, DateTime startTime, DateTime endTime)
        {
            List<OperationTicketHistorySH> list = new List<OperationTicketHistorySH>();
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(value))
            {
                sqlwhere += "  and (ticketCode like '%" + value + "%' or ticketName like '%" + value + "%')";
            }
            string sqlstr = string.Format(@"select * from operationtickethistoryshes where  StartTime>'{0}' and EndTime<'{1}' " + sqlwhere, startTime, endTime);
            DbRawSqlQuery<OperationTicketHistorySH> result = Db.Database.SqlQuery<OperationTicketHistorySH>(sqlstr);
            list = result.ToList();
            return list;
        }


        public PageInfo<OperationTicketHistorySH> GetPageByCondition(string value, DateTime startTime, DateTime endTime, int pageIndex, int pageSize)
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
            string sqlCount = string.Format(@"select count(id) as count from operationtickethistoryshes where 1=1 "+sqlwhere);
            string strsql = string.Format(@"select id,Abutment_Id,fromorder,ticketcode,type,state,createtime,StartTime,endtime,null as  detail,rawid,ticketname  from operationtickethistoryshes  where 1=1  {0} ", sqlwhere);
            PageInfo<OperationTicketHistorySH> page = GetPageList(sqlCount,strsql,pageIndex,pageSize);
            return page;
        }


        public List<OperationTicketHistorySH> GetListByTime(int count,string field,string sort)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(field))
            {
                sqlwhere += " order by "+field+" "+sort;
            }
            if (count > 0)
            {
                sqlwhere += " limit "+count;
            }
            string strsql = string.Format(@"select id,Abutment_Id,fromorder,ticketcode,type,state,createtime,StartTime,endtime,null as  detail,rawid,ticketname  from operationtickethistoryshes  where 1=1 "+sqlwhere);
            List<OperationTicketHistorySH> list = GetListBySql(strsql);
            return list;
        }

    }
}

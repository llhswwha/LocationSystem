using DAL;
using DbModel.LocationHistory.Door;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Tools;
using System.Data.Entity.Infrastructure;
using TModel.Tools;

namespace BLL.Blls.LocationHistory
{
    public class DoorClickBll : BaseBll<DoorClick, LocationHistoryDb>
    {
        public DoorClickBll() : base()
        {

        }
        public DoorClickBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DoorClicks;
        }

        public List<DoorClick> FindByCondition(DateTime startTime, DateTime endTime, string eventType, string[] personIds, string[] doorIndexCodes)
        {

            string startTzz = startTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string endTzz = endTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            List<DoorClick> list = new List<DoorClick>();
            string personSelect = "";
            if (personIds != null && personIds.Length > 0)
            {
                foreach (string personId in personIds)
                {
                    personSelect += "'" + personId + "',";
                }
                personSelect = personSelect.Substring(0, personSelect.Length - 1);
            }
            string doors = "";
            if (doorIndexCodes != null && doorIndexCodes.Length > 0)
            {
                foreach (string doorIndexCode in doorIndexCodes)
                {
                    doors += "'" + doorIndexCode + "',";
                }
                doors = doors.Substring(0, doors.Length - 1);
            }
            
            string sqlWhere = "";
            if (personSelect != "")
            {
                sqlWhere += " and personId in ("+personSelect+")";
            }
            if (doors != "")
            {
                sqlWhere += " and doorIndexCode in ("+doors+")";
            }

            if (eventType != "")
            {
                sqlWhere += " and eventType = '" + eventType + "'";
            }
            //string strSql = string.Format(@"select * from  doorclicks where  eventTime>'{0}' and  eventTime<'{1}' and eventType='{2}'  " + sqlWhere,startTzz,endTzz,eventType); ;
            string strSql = string.Format(@"select * from  doorclicks where  eventTime>'{0}' and  eventTime<'{1}'" + sqlWhere, startTzz, endTzz); ;
            DbRawSqlQuery<DoorClick> result2 = Db.Database.SqlQuery<DoorClick>(strSql);
            list = result2.ToList();

            return list;
        }

        public PageInfo<DoorClick> GetPageByCondition(DateTime startTime, DateTime endTime, string eventType, string[] personIds, string[] doorIndexCodes,string personName, int pageNo, int pageSize)
        {
            string sqlwhere = "";
            if (startTime != null && endTime != null)
            {
                //sqlwhere += "and eventTime between  '"+ startTime + "' and  '"+ endTime + "'";
                sqlwhere += "and eventTime >=  '" + startTime + "' and  eventTime<='" + endTime + "'";
            }
            if (!string.IsNullOrEmpty(eventType))
            {
                sqlwhere += " and eventType="+eventType;
            }
            string personIdss = "";
            if (personIds != null && personIds.Length > 0)
            {
                for(int i=0; i<personIds.Length;i++)
                {
                    personIdss += "'" + personIds[i] + "',";
                }
                personIdss = personIdss.Substring(0,personIdss.Length-1);
            }
            if (personIdss != "")
            {
                sqlwhere += "  and personId in ("+personIdss+")";
            }
            string doorIndexCodess = "";
            if (doorIndexCodes != null && doorIndexCodes.Length > 0)
            {
              for(int i=0;i<doorIndexCodes.Length;i++)
                { 
                    doorIndexCodess += "'"+doorIndexCodes[i]+"',";
                }
                doorIndexCodess = doorIndexCodess.Substring(0, doorIndexCodess.Length - 1);
            }
            if (doorIndexCodess != "")
            {
                sqlwhere += " and  doorIndexCode in ("+doorIndexCodess+")";
            }
            if (pageNo == 0)
            {
                pageNo = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 1000000;
            }
            if (!string.IsNullOrEmpty(personName))
            {
                personName = personName.Replace("/", "").Replace("\"", "").Replace("|", "").Replace("*","");
                sqlwhere += " and  personName like '%"+personName+"%' ";
            }
            string sqlCount = string.Format(@"select count(id) from doorclicks where 1=1 " + sqlwhere);
            string strsql = string.Format(@" select * from doorclicks  where 1=1  {0} ", sqlwhere);
            PageInfo<DoorClick> page = GetPageList<DoorClick>(sqlCount, strsql,pageNo, pageSize);
            return page;
        }
    }
}

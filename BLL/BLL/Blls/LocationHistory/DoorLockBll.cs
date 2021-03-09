using DAL;
using DbModel.LocationHistory.Door;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Tools;
using System.Data.Entity.Infrastructure;

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

    }
}

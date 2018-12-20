using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using Location.TModel.LocationHistory.Data;
using Location.TModel.Tools;
using LocationServices.Converters;
using TModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface IPosHistoryService
    {
        IList<Position> GetHistoryList(string start, string end, string tag, string person,string area);
    }
    public class PosHistoryService: IPosHistoryService
    {
        private Bll db;

        private PositionBll dbSet;

        public PosHistoryService()
        {
            db = new Bll(false, true, false, true);
            dbSet = db.Positions;
        }

        public PosHistoryService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Positions;
        }

        public IList<Position> GetHistory()
        {
            return dbSet.ToList().ToWcfModelList();
        }

        public IList<Position> GetHistoryList(string start, string end, string tagCode, string personId,string areaId)
        {
            if (string.IsNullOrEmpty(start))
            {
                start = "1970-1-1";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = "2100-1-1";
            }

            IList<Position> result = new List<Position>();
            if (!string.IsNullOrEmpty(tagCode))
            {
                result = GetHistoryByTag(tagCode, start.ToDateTime(), end.ToDateTime());
            }
            else if (!string.IsNullOrEmpty(personId))
            {
                result = GetHistoryByPerson(personId.ToInt(), start.ToDateTime(), end.ToDateTime());
            }
            else if (!string.IsNullOrEmpty(areaId))
            {
                result = GetHistoryByArea(areaId.ToInt(), start.ToDateTime(), end.ToDateTime());
            }
            else
            {
                result = GetHistoryByTime(start.ToDateTime(), end.ToDateTime());
            }
            return result;
        }

        public List<Position> GetHistoryByTag(string tag)
        {
            var info = from u in dbSet.DbSet
                       where u.Code.Contains(tag)
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByTime(DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in dbSet.DbSet
                       where u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
                       select u;
            var tempList = info.ToList();

            //var list = dbSet.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByTag(string tag, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in dbSet.DbSet
                       where u.Code.Contains(tag) && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        ///  获取标签3D历史位置
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            var info = from u in db.U3DPositions.DbSet
                       where tagcode == u.Code && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <param name="personnelID"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPerson(int personnelID)
        {
            var info = from u in dbSet.DbSet
                       where personnelID == u.PersonnelID
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <param name="personnelID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPerson(int personnelID, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }

            var info = from u in dbSet.DbSet
                       where personnelID == u.PersonnelID && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        /// <summary>
        /// 获取历史位置信息根据PersonnelID和TopoNodeId建筑id
        /// </summary>
        /// <param name="personnelID"></param>
        /// <param name="topoNodeId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPersonAndArea(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            if (topoNodeIds == null || topoNodeIds.Count == 0)
            {
                return GetHistoryByPerson(personnelID, start, end);
            }

            return GetHistoryByArea(personnelID, topoNodeIds, start, end);
        }

        private List<Position> GetHistoryByArea(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in dbSet.DbSet
                where
                    personnelID == u.PersonnelID && u.IsInArea(topoNodeIds) && u.DateTimeStamp >= startStamp &&
                    u.DateTimeStamp <= endStamp
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByAreas(List<int> topoNodeIds, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in dbSet.DbSet
                       where
                           u.IsInArea(topoNodeIds) && u.DateTimeStamp >= startStamp &&
                           u.DateTimeStamp <= endStamp
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByAreas(List<int> topoNodeIds)
        {
            var info = from u in dbSet.DbSet
                       where
                           u.IsInArea(topoNodeIds) 
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByArea(int topoNodeIds)
        {
            var info = from u in dbSet.DbSet
                       where
                           u.IsInArea(topoNodeIds)
                       select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public List<Position> GetHistoryByArea(int topoNode, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(end);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in dbSet.DbSet
                where
                    topoNode == u.AreaId && u.DateTimeStamp >= startStamp &&
                    u.DateTimeStamp <= endStamp
                select u;
            var tempList = info.ToList();
            return tempList.ToWcfModelList();
        }

        public static long GetTimeStamp(DateTime time)
        {
            //DateTime zero = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long msStamp = (long)(time - zero).TotalMilliseconds;//毫秒
            //if (msStamp < 0)
            //{
            //    msStamp = 0;
            //}

            long msStamp2 = TimeConvert.DateTimeToTimeStamp(time);//秒
            return msStamp2;
        }
    }
}

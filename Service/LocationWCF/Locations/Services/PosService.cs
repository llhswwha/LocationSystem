using System.Collections.Generic;
using TEntity = Location.TModel.Location.Data.TagPosition;
using BLL;
using BLL.Blls.Location;
using LocationServices.Converters;
using DbModel.Tools;
using TModel.Tools;
using Location.TModel.Location.Data;
using System;
using Location.TModel.LocationHistory.Data;
using System.Linq;

namespace LocationServices.Locations.Services
{
    public interface IPosService : IEntityService<TEntity>
    {
        IList<Position> GetHistoryList(string start, string end, string tag, string person);
    }
    public class PosService : IPosService
    {
        private Bll db;

        private LocationCardPositionBll dbSet;

        public PosService()
        {
            db = new Bll(false, true, false, true);
            dbSet = db.LocationCardPositions;
        }

        public PosService(Bll bll)
        {
            this.db = bll;
            dbSet = db.LocationCardPositions;
        }

        public TEntity Delete(string id)
        {
            var item = dbSet.DeleteById(id.ToInt());
            return item.ToTModel();
        }

        public TEntity GetEntity(string id)
        {
            var item = dbSet.Find(id.ToInt());
            return item.ToTModel();
        }

        public IList<TEntity> GetList()
        {
            var devInfoList = dbSet.ToList().ToTModel();
            return devInfoList.ToWCFList();
        }

        public IList<TEntity> GetListByName(string name)
        {
            var devInfoList = dbSet.GetListByName(name).ToTModel();
            return devInfoList.ToWCFList();
        }

        public TEntity Post(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity Put(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public IList<Position> GetHistory()
        {
            return db.Positions.ToList().ToWcfModelList();
        }

        public IList<Position> GetHistoryList(string start, string end, string tag, string person)
        {
            return db.Positions.ToList().ToWcfModelList();
        }

        public List<Position> GetHistoryPositonsByTime(string tag, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(start);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in db.Positions.DbSet
                                        where tag == u.Code && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
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
            long endStamp = GetTimeStamp(start);
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
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Position> GetHistoryByPerson(int personnelID, DateTime start, DateTime end)
        {
            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(start);
            if (startStamp >= endStamp)
            {
                return null;
            }

            var info = from u in db.Positions.DbSet
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

            long startStamp = GetTimeStamp(start);
            long endStamp = GetTimeStamp(start);
            if (startStamp >= endStamp)
            {
                return null;
            }
            var info = from u in db.Positions.DbSet
                                            where personnelID == u.PersonnelID && topoNodeIds.Contains((int)u.TopoNodeId) && u.DateTimeStamp >= startStamp && u.DateTimeStamp <= endStamp
                                            select u;
                var tempList = info.ToList();
                return tempList.ToWcfModelList();
        }

        public static long GetTimeStamp(DateTime time)
        {
            DateTime zero = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long msStamp = (long)(time - zero).TotalMilliseconds;//毫秒
            if (msStamp < 0)
            {
                msStamp = 0;
            }
            return msStamp;
        }
    }
}

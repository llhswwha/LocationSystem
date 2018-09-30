using System;
using System.Collections.Generic;
using System.Linq;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using Location.BLL.Tool;

namespace BLL.ServiceHelpers
{
    /// <summary>
    /// LocationService的复杂的函数内容，使之在服务和网站中都能复用
    /// </summary>
    public static class LocationSP
    {
        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public static Area GetPhysicalTopologyTree()
        {
            try
            {
                using (Bll db = new Bll(false, false, false))
                {
                    Log.InfoStart("GetPhysicalTopologyTree");


                    //LocationBll dbEx = new LocationBll(false, true, false);
                    //List<Area> list = dbEx.PhysicalTopologys.ToList();
                    //Area root = list[0];
                    //return root;

                    List<Area> list = db.Areas.ToList();

                    //List<TransformM> transformMs = db.TransformMs.ToList();
                    //foreach (Area p in list)
                    //{
                    //    p.Transfrom = transformMs.Find((item) => item.Id == p.TransfromId);
                    //}

                    //List<NodeKKS> kksList=db.NodeKKSs.ToList();
                    //foreach (Area p in list)
                    //{
                    //    p.Nodekks = kksList.Find((item) => item.Id == p.NodekksId);
                    //}

                    List<Bound> bounds = db.Bounds.ToList();
                    List<Point> points = db.Points.ToList();
                    foreach (Point point in points)
                    {
                        Bound bound = bounds.Find(i => i.Id == point.BoundId);
                        if (bound != null)
                        {
                            bound.AddPoint(point);
                        }
                    }

                    foreach (Area p in list)
                    {
                        p.InitBound = bounds.Find((item) => item.Id == p.InitBoundId);
                        p.EditBound = bounds.Find((item) => item.Id == p.EditBoundId);
                    }

                    List<DevInfo> leafNodes = db.DevInfos.ToList();
                    List<Area> roots = TreeHelper.CreateTree(list, leafNodes);

                    Log.InfoEnd("GetPhysicalTopologyTree");

                    if (roots.Count > 0)
                    {
                        return roots[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetPhysicalTopologyTree", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取标签历史位置根据PersonnelID
        /// </summary>
        /// <param name="personnelID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end)
        {
            using (Bll db = new Bll(false, false, false))
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                long startTotalMilliseconds = (long)(start - startTime).TotalMilliseconds;
                long endTotalMilliseconds = (long)(end - startTime).TotalMilliseconds;
                if (startTotalMilliseconds >= endTotalMilliseconds)
                {
                    return null;
                }
                if (startTotalMilliseconds < 0)
                {
                    startTotalMilliseconds = 0;
                }
                if (endTotalMilliseconds < 0)
                {
                    endTotalMilliseconds = 0;
                }

                IQueryable<Position> info = from u in db.Positions.DbSet
                                            where personnelID == u.PersonnelID && u.DateTimeStamp >= startTotalMilliseconds && u.DateTimeStamp <= endTotalMilliseconds
                                            select u;
                //var info = db.Positions.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                List<Position> tempList = info.ToList();
                return tempList.ToWCFList();
            }
        }

        /// <summary>
        /// 获取历史位置信息根据PersonnelID和TopoNodeId建筑id
        /// </summary>
        /// <param name="personnelID"></param>
        /// <param name="topoNodeId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            using (Bll db = new Bll(false, false, false))
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                long startTotalMilliseconds = (long)(start - startTime).TotalMilliseconds;
                long endTotalMilliseconds = (long)(end - startTime).TotalMilliseconds;
                if (startTotalMilliseconds >= endTotalMilliseconds)
                {
                    return null;
                }
                if (startTotalMilliseconds < 0)
                {
                    startTotalMilliseconds = 0;
                }
                if (endTotalMilliseconds < 0)
                {
                    endTotalMilliseconds = 0;
                }

                IQueryable<Position> info = from u in db.Positions.DbSet
                                            where personnelID == u.PersonnelID && topoNodeIds.Contains((int)u.TopoNodeId) && u.DateTimeStamp >= startTotalMilliseconds && u.DateTimeStamp <= endTotalMilliseconds
                                            select u;
                //var info = db.Positions.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                List<Position> tempList = info.ToList();
                return tempList.ToWCFList();
            }
        }

        public static List<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            using (Bll db = new Bll(false, false, false))
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                long startTotalMilliseconds = (long) (start - startTime).TotalMilliseconds;
                long endTotalMilliseconds = (long) (end - startTime).TotalMilliseconds;
                if (startTotalMilliseconds >= endTotalMilliseconds)
                {
                    return null;
                }
                if (startTotalMilliseconds < 0)
                {
                    startTotalMilliseconds = 0;
                }
                if (endTotalMilliseconds < 0)
                {
                    endTotalMilliseconds = 0;
                }

                IQueryable<Position> info = from u in db.Positions.DbSet
                    where tagcode == u.Code && u.DateTimeStamp >= startTotalMilliseconds && u.DateTimeStamp <= endTotalMilliseconds
                    select u;
                //var info = db.Positions.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                List<Position> tempList = info.ToList();
                return tempList.ToWCFList();
            }
        }

        /// <summary>
        ///  获取标签3D历史位置
        /// </summary>
        /// <param name="tagcode"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            using (Bll db = new Bll(false, false, false))
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                long startTotalMilliseconds = (long)(start - startTime).TotalMilliseconds;
                long endTotalMilliseconds = (long)(end - startTime).TotalMilliseconds;
                if (startTotalMilliseconds >= endTotalMilliseconds)
                {
                    return null;
                }
                if (startTotalMilliseconds < 0)
                {
                    startTotalMilliseconds = 0;
                }
                if (endTotalMilliseconds < 0)
                {
                    endTotalMilliseconds = 0;
                }

                IQueryable<U3DPosition> info = from u in db.U3DPositions.DbSet
                                               where tagcode == u.Code && u.DateTimeStamp >= startTotalMilliseconds && u.DateTimeStamp <= endTotalMilliseconds
                                               select u;
                //var info = db.Positions.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                List<U3DPosition> tempList = info.ToList();
                return tempList.ToWCFList();
            } 
        }
    }
}

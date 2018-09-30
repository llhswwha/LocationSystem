using System;
using System.Collections.Generic;
using System.Linq;
using Location.BLL.Tool;
using Location.Model;
using Location.Model.Base;
using Location.Model.LocationTables;
using Location.Model.Tools;

namespace Location.BLL.ServiceHelpers
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
        public static PhysicalTopology GetPhysicalTopologyTree()
        {
            try
            {
                using (LocationBll db = new LocationBll(false, false, false))
                {
                    Log.InfoStart("GetPhysicalTopologyTree");


                    //LocationBll dbEx = new LocationBll(false, true, false);
                    //List<PhysicalTopology> list = dbEx.PhysicalTopologys.ToList();
                    //PhysicalTopology root = list[0];
                    //return root;

                    List<PhysicalTopology> list = db.PhysicalTopologys.ToList();

                    List<TransformM> transformMs = db.TransformMs.ToList();
                    foreach (PhysicalTopology p in list)
                    {
                        p.Transfrom = transformMs.Find((item) => item.Id == p.TransfromId);
                    }

                    List<NodeKKS> kksList=db.NodeKKSs.ToList();
                    foreach (PhysicalTopology p in list)
                    {
                        p.Nodekks = kksList.Find((item) => item.Id == p.NodekksId);
                    }

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

                    foreach (PhysicalTopology p in list)
                    {
                        p.InitBound = bounds.Find((item) => item.Id == p.InitBoundId);
                        p.EditBound = bounds.Find((item) => item.Id == p.EditBoundId);
                    }

                    List<DevInfo> leafNodes = db.DevInfos.ToList();
                    List<PhysicalTopology> roots = TreeHelper.CreateTree(list, leafNodes);

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
        public static IList<Position> GetHistoryPositonsByPersonnelID(int personnelID, DateTime start, DateTime end)
        {
            using (LocationBll db = new LocationBll(false, false, false))
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

                IQueryable<Position> info = from u in db.Position.DbSet
                                            where personnelID == u.PersonnelID && u.Time >= startTotalMilliseconds && u.Time <= endTotalMilliseconds
                                            select u;
                //var info = db.Position.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                IList<Position> tempList = info.ToList();
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
        public static IList<Position> GetHistoryPositonsByPidAndTopoNodeIds(int personnelID, List<int> topoNodeIds, DateTime start, DateTime end)
        {
            using (LocationBll db = new LocationBll(false, false, false))
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

                IQueryable<Position> info = from u in db.Position.DbSet
                                            where personnelID == u.PersonnelID && topoNodeIds.Contains((int)u.TopoNodeId) && u.Time >= startTotalMilliseconds && u.Time <= endTotalMilliseconds
                                            select u;
                //var info = db.Position.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                IList<Position> tempList = info.ToList();
                return tempList.ToWCFList();
            }
        }

        public static IList<Position> GetHistoryPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            using (LocationBll db = new LocationBll(false, false, false))
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

                IQueryable<Position> info = from u in db.Position.DbSet
                    where tagcode == u.Tag && u.Time >= startTotalMilliseconds && u.Time <= endTotalMilliseconds
                    select u;
                //var info = db.Position.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                IList<Position> tempList = info.ToList();
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
        public static IList<U3DPosition> GetHistoryU3DPositonsByTime(string tagcode, DateTime start, DateTime end)
        {
            using (LocationBll db = new LocationBll(false, false, false))
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
                                               where tagcode == u.Tag && u.Time >= startTotalMilliseconds && u.Time <= endTotalMilliseconds
                                               select u;
                //var info = db.Position.DbSet.Where(c => c.Time >=startTotalMilliseconds && c.Time <= endTotalMilliseconds).ToList();
                IList<U3DPosition> tempList = info.ToList();
                return tempList.ToWCFList();
            } 
        }
    }
}

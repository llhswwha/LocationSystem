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
        public static Area GetPhysicalTopologyTree(Bll db,bool isWithDev=true)
        {
            try
            {
                Log.InfoStart("GetPhysicalTopologyTree");
                List<Area> list = db.Areas.ToList();

                //List<Bound> bounds = db.Bounds.ToList();
                ////List<Point> points = db.Points.ToList();
                ////BindBoundWithPoint(points, bounds);
                //BindAreaWithBound(list, bounds);

                List<DevInfo> leafNodes = new List<DevInfo>();
                if (isWithDev)
                {
                    leafNodes = db.DevInfos.ToList();
                }
                
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
            catch (Exception ex)
            {
                Log.Error("GetPhysicalTopologyTree", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public static Area GetPhysicalTopologyTree()
        {
            using (Bll db = new Bll(false, false, false, false))
            {
                return GetPhysicalTopologyTree(db);
            }
        }

        private static void BindAreaWithBound(List<Area> list, List<Bound> bounds)
        {
            foreach (Area p in list)
            {
                p.InitBound = bounds.Find((item) => item.Id == p.InitBoundId);
                p.EditBound = bounds.Find((item) => item.Id == p.EditBoundId);
            }
        }

        private static void BindBoundWithPoint(List<Point> points, List<Bound> bounds)
        {
            foreach (Point point in points)
            {
                Bound bound = bounds.Find(i => i.Id == point.BoundId);
                if (bound != null)
                {
                    bound.AddPoint(point);
                }
            }
        }
    }
}

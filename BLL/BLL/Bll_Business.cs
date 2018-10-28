using BLL.Blls.Engine;
using BLL.Blls.Location;
using BLL.Blls.LocationHistory;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.Tool;

namespace BLL
{
    public partial class Bll : IDisposable
    {
        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public Area GetAreaTree(bool isWithDev = true)
        {
            try
            {
                Log.InfoStart("GetPhysicalTopologyTree");
                List<Area> list = Areas.ToList();

                List<Bound> bounds = Bounds.ToList();
                List<Point> points = Points.ToList();
                BindBoundWithPoint(points, bounds);
                BindAreaWithBound(list, bounds);

                List<DevInfo> leafNodes = new List<DevInfo>();
                if (isWithDev)
                {
                    leafNodes = DevInfos.ToList();
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

        public static Area GetArchorRoom(List<Area> rooms,Archor archor)
        {
            var inRooms = rooms.FindAll(j => j.InitBound != null && j.InitBound.Contains(archor.X, archor.Z));
            if (inRooms.Count > 0)
            {
                if (inRooms.Count == 1)
                {
                    return inRooms[0];
                }
                else
                {
                    Log.Warn("Archor有多个机房:" + archor.Name);
                    return inRooms[0];
                }
            }
            else
            {
                return null;
            }
        }
    }
}

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
        public Area GetAreaTree(bool isWithDev = true,int? id=null,bool containCAD=false)
        {
            try
            {
                Log.InfoStart("GetPhysicalTopologyTree");
                List<Area> list = Areas.ToList();
                List<Area> list2 = new List<Area>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (containCAD)
                    {
                        list2.Add(list[i]);
                    }
                    else
                    {
                        if (list[i].Type != AreaTypes.CAD)//过滤掉柱子等CAD形状
                        {
                            list2.Add(list[i]);
                        }
                    }
                }
                list = list2;
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
                    var result= roots[0].FindChild(id);
                    return result;
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

        /// <summary>
        /// 获取设备所在的机房
        /// </summary>
        /// <param name="floor">楼层</param>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static Area GetDevRoom(Area floor, DevInfo dev)
        {
            var rooms = GetFloorRooms(floor.Id);
            floor.Children = rooms;
            return GetDevRoom(rooms, dev);
        }

        public static List<Area> GetFloorRooms(int floorId)
        {
            Bll bll = new Bll();
            var rooms = bll.Areas.FindAll(j => j.ParentId == floorId);
            List<Bound> bounds = bll.Bounds.ToList();
            List<Point> points = bll.Points.ToList();
            BindBoundWithPoint(points, bounds);
            BindAreaWithBound(rooms, bounds);
            return rooms;
        }

        public static Area GetRoomByPos(int floorId,float x,float z)
        {
            var rooms = GetFloorRooms(floorId);
            return GetRoomByPos(rooms, x, z);
        }


        /// <summary>
        /// 获取设备所在的机房
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static Area GetDevRoom(List<Area> rooms, DevInfo dev)
        {
            return GetRoomByPos(rooms, dev.PosX, dev.PosZ);
        }

        /// <summary>
        /// 获取设备所在的机房
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static Area GetRoomByPos(List<Area> rooms, float x,float z)
        {
            var inRooms = rooms.FindAll(j => j.InitBound != null && j.InitBound.Contains(x, z));
            if (inRooms.Count > 0)
            {
                if (inRooms.Count == 1)
                {
                    return inRooms[0];
                }
                else
                {
                    Log.Warn("该位置有多个机房");
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

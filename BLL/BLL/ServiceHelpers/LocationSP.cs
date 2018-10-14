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
                using (Bll db = new Bll(false, false, false,false))
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
    }
}

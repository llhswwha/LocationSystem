using System.Collections.Generic;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.Model;
using Location.BLL.Tool;

namespace Location.BLL.ServiceHelpers
{
    public class PhysicalTopologySP
    {
        private Bll db;

        public PhysicalTopologySP()
        {
            db = new Bll(false, false, false);
        }

        public PhysicalTopologySP(Bll db)
        {
            this.db = db;
        }

        public static string tag = "PhysicalTopologySP";

        /// <summary>
        /// 获取园区下的监控范围
        /// </summary>
        /// <returns></returns>
        public List<Area> GetParkMonitorRange()
        {
            try
            {
                List<Area> list = db.Areas.ToList();
                list = CreateTransformMRelation(list);
                List<Area> roots = TreeHelper.CreateTree(list);
                List<Area> presults = FilterParkMonitorRange(roots);
                return presults;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetParkMonitorRange", "Exception:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 筛选园区下的监控范围
        /// </summary>
        /// isDeep,是否继续往子节点下面搜
        private List<Area> FilterParkMonitorRange(List<Area> listT)
        {
            try
            {
                List<Area> ps = new List<Area>();
                if (listT == null)
                {
                    return ps;
                }
                foreach (Area p in listT)
                {
                    if (p.Type == AreaTypes.区域 || p.Type == AreaTypes.分组 || p.Type == AreaTypes.大楼 || p.Type == AreaTypes.范围)
                    {
                        if (p.HaveTransform())
                        {
                            //presults.Add(p);
                            ps.Add(p);
                        }
                        //Area t = p.Children.Find((item) => item.Type == AreaTypes.范围);
                        //if (t != null)
                        //{
                        //    //presults.Add(p);
                        //    ps.Add(p);
                        //}
                        ps.AddRange(FilterParkMonitorRange(p.Children));
                    }
                    else
                    {
                        continue;
                    }
                }
                return ps;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "FilterParkMonitorRange", "Exception:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 获取楼层下的监控范围
        /// </summary>
        /// <returns></returns>
        public List<Area> GetFloorMonitorRange()
        {
            try
            {
                List<Area> list = db.Areas.ToList();
                list = CreateTransformMRelation(list);
                List<Area> roots = TreeHelper.CreateTree(list);
                List<Area> fresults = FilterFloorMonitorRange(roots);
                return fresults;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetFloorMonitorRange", "Exception:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 筛选楼层下的监控范围
        /// </summary>
        private List<Area> FilterFloorMonitorRange(List<Area> listT)
        {
            try
            {
                List<Area> ps = new List<Area>();
                foreach (Area p in listT)
                {
                    if (p.Type == AreaTypes.楼层 || p.Type == AreaTypes.分组 || p.Type == AreaTypes.机房 || p.Type == AreaTypes.范围)
                    {
                        if (p.HaveTransform())
                        {
                            ps.Add(p);

                        }
                        //Area t = p.Children.Find((item) => item.Type == AreaTypes.范围);
                        //if (t != null)
                        //{
                        //    ps.Add(p);
                        //}
                        ps.AddRange(FilterParkMonitorRange(p.Children));
                    }
                    else
                    {
                        continue;
                    }
                }
                return ps;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "FilterFloorMonitorRange", "Exception:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 根据PhysicalTopology的Id,获取该楼层以下的监控范围
        /// </summary>
        /// <returns></returns>
        public List<Area> GetFloorMonitorRange(int id)
        {
            try
            {
                List<Area> list = db.Areas.ToList();
                list = CreateTransformMRelation(list);
                List<Area> roots = TreeHelper.CreateTree(list);

                Area p = GetPhysicalTopology(id, roots);
                List<Area> ps = new List<Area>();
                ps.Add(p);
                List<Area> results = FilterFloorMonitorRange(ps);
                return results;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetFloorMonitorRange", "Exception:" + ex);
                return null;
            }
        }

        public Area GetPhysicalTopology(int id, List<Area> ps)
        {
            try
            {
                Area p = ps.Find((item) => item.Id == id);
                if (p != null) return p;

                foreach (Area pt in ps)
                {
                    p = GetPhysicalTopology(id, pt.Children);
                    if (p != null) return p;
                }

                return null;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetPhysicalTopology", "Exception:" + ex);
                return null;
            }
        }

        /// <summary>
        /// 创建Transform关联
        /// </summary>
        /// <returns></returns>
        public List<Area> CreateTransformMRelation(List<Area> listT)
        {
            //List<TransformM> transformMs = db.TransformMs.ToList();
            //foreach (Area p in listT)
            //{
            //    TransformM t = transformMs.Find((item) => item.Id == p.TransfromId);
            //    if (t != null)
            //    {
            //        p.Transfrom = t;
            //    }
            //}
            return listT;
        }
    }
}

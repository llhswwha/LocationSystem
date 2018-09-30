using System.Collections.Generic;
using Location.Model;
using Location.Model.Tools;

namespace Location.BLL.ServiceHelpers
{
    public class PhysicalTopologySP
    {
        private LocationBll db;

        public PhysicalTopologySP()
        {
            db = new LocationBll(false, false, false);
        }

        public PhysicalTopologySP(LocationBll db)
        {
            this.db = db;
        }

        /// <summary>
        /// 获取园区下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetParkMonitorRange()
        {
            List<PhysicalTopology> list = db.PhysicalTopologys.ToList();
            list = CreateTransformMRelation(list);
            List<PhysicalTopology> roots = TreeHelper.CreateTree(list);
            List<PhysicalTopology> presults = FilterParkMonitorRange(roots);
            return presults;
        }

        /// <summary>
        /// 筛选园区下的监控范围
        /// </summary>
        /// isDeep,是否继续往子节点下面搜
        private List<PhysicalTopology> FilterParkMonitorRange(List<PhysicalTopology> listT)
        {
            List<PhysicalTopology> ps = new List<PhysicalTopology>();
            if (listT == null)
            {
                return ps;
            }
            foreach (PhysicalTopology p in listT)
            {
                if (p.Type == Types.区域 || p.Type == Types.分组|| p.Type == Types.大楼 || p.Type == Types.范围)
                {
                    if (p.Transfrom != null)
                    {
                        //presults.Add(p);
                        ps.Add(p);
                    }
                    //PhysicalTopology t = p.Children.Find((item) => item.Type == Types.范围);
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

        /// <summary>
        /// 获取楼层下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRange()
        {
            List<PhysicalTopology> list = db.PhysicalTopologys.ToList();
            list = CreateTransformMRelation(list);
            List<PhysicalTopology> roots = TreeHelper.CreateTree(list);
            List<PhysicalTopology> fresults = FilterFloorMonitorRange(roots);
            return fresults;
        }

        /// <summary>
        /// 筛选楼层下的监控范围
        /// </summary>
        private List<PhysicalTopology> FilterFloorMonitorRange(List<PhysicalTopology> listT)
        {
            List<PhysicalTopology> ps = new List<PhysicalTopology>();
            foreach (PhysicalTopology p in listT)
            {
                if (p.Type == Types.楼层 || p.Type == Types.分组 || p.Type == Types.机房 || p.Type == Types.范围)
                {
                    if (p.Transfrom != null)
                    {
                        ps.Add(p);

                    }
                    //PhysicalTopology t = p.Children.Find((item) => item.Type == Types.范围);
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

        /// <summary>
        /// 根据PhysicalTopology的Id,获取该楼层以下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRange(int id)
        {
            List<PhysicalTopology> list = db.PhysicalTopologys.ToList();
            list = CreateTransformMRelation(list);
            List<PhysicalTopology> roots = TreeHelper.CreateTree(list);

            PhysicalTopology p = GetPhysicalTopology(id, roots);
            List<PhysicalTopology> ps = new List<PhysicalTopology>();
            ps.Add(p);
            List<PhysicalTopology> results= FilterFloorMonitorRange(ps);
            return results;
        }

        public PhysicalTopology GetPhysicalTopology(int id, List<PhysicalTopology> ps)
        {
            PhysicalTopology p = ps.Find((item) => item.Id == id);
            if (p != null) return p;

            foreach (PhysicalTopology pt in ps)
            {
                p = GetPhysicalTopology(id, pt.Children);
                if (p != null) return p;
            }

            return null;
        }

        /// <summary>
        /// 创建Transform关联
        /// </summary>
        /// <returns></returns>
        public List<PhysicalTopology> CreateTransformMRelation(List<PhysicalTopology> listT)
        {
            List<TransformM> transformMs = db.TransformMs.ToList();
            foreach (PhysicalTopology p in listT)
            {
                TransformM t = transformMs.Find((item) => item.Id == p.TransfromId);
                if (t != null)
                {
                    p.Transfrom = t;
                }
            }
            return listT;
        }
    }
}

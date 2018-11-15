using System;
using System.Collections.Generic;
using Location.BLL.ServiceHelpers;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using LocationServices.Locations.Services;
using TModel.Location.Nodes;
using TModel.Location.AreaAndDev;
using TModel.Location.Person;
using System.Linq;

namespace LocationServices.Locations
{
    //管理相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetPhysicalTopologyList()
        {
            return new AreaService(db).GetList();
        }

        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            return new AreaService(db).GetEntity(id, getChildren);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            return new AreaService(db).GetListByName(name);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string pid)
        {
            return new AreaService(db).GetListByPid(pid);
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public PhysicalTopology GetPhysicalTopologyTree(int view)
        {
            return new AreaService(db).GetTree(view);
        }

        public AreaNode GetPhysicalTopologyTreeNode(int view)
        {
            return new AreaService(db).GetBasicTree(view);
        }

        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            return new AreaService(db).GetTree(id);
        }

        public PhysicalTopology AddPhysicalTopology(string pid,PhysicalTopology item)
        {
            return new AreaService(db).Post(pid, item);
        }

        public PhysicalTopology AddPhysicalTopology(PhysicalTopology item)
        {
            return new AreaService(db).Post(item);
        }

        public PhysicalTopology EditPhysicalTopology(PhysicalTopology item)
        {
            return new AreaService(db).Put(item);
        }

        public PhysicalTopology RemovePhysicalTopology(string id)
        {
            return new AreaService(db).Delete(id);
        }

        public IList<PhysicalTopology> RemovePhysicalTopologyChildren(string pid)
        {
            return new AreaService(db).DeleteListByPid(pid);
        }

        /// <summary>
        /// 获取园区下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetParkMonitorRange()
        {
            PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
            var results = physicalTopologySp.GetParkMonitorRange();
            return results.ToWcfModelList();
        }

        /// <summary>
        /// 获取楼层下的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRange()
        {
            PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
            var results = physicalTopologySp.GetFloorMonitorRange();
            return results.ToWcfModelList();
        }

        /// <summary>
        /// 根据PhysicalTopology的Id获取楼层以下级别的监控范围
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetFloorMonitorRangeById(int id)
        {
            PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
            var results = physicalTopologySp.GetFloorMonitorRange(id);
            return results.ToWcfModelList();
        }

        /// <summary>
        /// 根据节点添加监控范围
        /// </summary>
        public bool EditMonitorRange(PhysicalTopology pt)
        {
            //db.TransformMs.Edit(pt.Transfrom);
            return db.Areas.Edit(pt.ToDbModel());
        }

        /// <summary>
        /// 根据节点添加子监控范围
        /// </summary>
        public bool AddMonitorRange(PhysicalTopology pt)
        {
            return db.Areas.Add(pt.ToDbModel());
        }
        
        public AreaStatistics GetAreaStatistics(int id)
        {
            List<int?> lst = new List<int?>();
            List<int?> lstRecv;

            var areaList = db.Areas.ToList();
            var a1 = areaList.Find(p => p.Id == id);
            if (a1 == null)
            {
                return new AreaStatistics();
            } 

            lst.Add(a1.Id);
            lstRecv = GetAreaStatisticsInner(a1.Id, areaList);
            if (lstRecv != null || lstRecv.Count > 0)
            {
                lst.AddRange(lstRecv);
            }

            AreaService asr = new AreaService();
            AreaStatistics ast = asr.GetAreaStatisticsCount(lst);

            return ast;
        }

        private List<int?> GetAreaStatisticsInner(int id, List<DbModel.Location.AreaAndDev.Area> areaList)
        {
            List<int?> lst = new List<int?>();
            List<DbModel.Location.AreaAndDev.Area> alist2 = areaList.FindAll(p => p.ParentId == id);
            List<int?> lstRecv;
            if (alist2 == null || alist2.Count <= 0)
            {
                return lst;
            }

            foreach (DbModel.Location.AreaAndDev.Area item in alist2)
            {
                lst.Add(item.Id);
                lstRecv = GetAreaStatisticsInner(item.Id, areaList);
                if (lstRecv != null || lstRecv.Count > 0)
                {
                    lst.AddRange(lstRecv);
                }
            }

            return lst;
        }

        public List<NearbyPerson> GetNearbyPerson_Currency(int id, float fDis)
        {
            PersonService ps = new PersonService();
            List<NearbyPerson> lst = ps.GetNearbyPerson_Currency(id, fDis);
            if (lst == null)
            {
                lst = new List<NearbyPerson>();
            }
            
            return lst;
        }

        public List<NearbyPerson> GetNearbyPerson_Alarm(int id)
        {
            PersonService ps = new PersonService();
            List<NearbyPerson> lst = ps.GetNearbyPerson_Alarm(id);
            if (lst == null)
            {
                lst = new List<NearbyPerson>();
            }

            return lst;
        }
    }
}

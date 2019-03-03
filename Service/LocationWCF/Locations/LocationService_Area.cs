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
using DbModel.Location.AreaAndDev;
using BLL;
using System.Diagnostics;

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
            ShowLog(">>>>> GetPhysicalTopologyList");
            return new AreaService(db).GetList();
        }

        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            ShowLog(">>>>> GetPhysicalTopology id"+id);
            return new AreaService(db).GetEntity(id, getChildren);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            ShowLog(">>>>> GetPhysicalTopologyListByName name" + name);
            return new AreaService(db).GetListByName(name);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string pid)
        {
            ShowLog(">>>>> GetPhysicalTopologyListByPid pid" + pid);
            return new AreaService(db).GetListByPid(pid);
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public PhysicalTopology GetPhysicalTopologyTree(int view)
        {
            ShowLog(">>>>> GetPhysicalTopologyTree view=" + view);
            BLL.Bll dbpt = new BLL.Bll(false, false, false, false);
            return new AreaService(dbpt).GetTree(view);
            //return null;
            //return new PhysicalTopology() { Id = 1, Name = "root" };
        }

        public AreaNode GetPhysicalTopologyTreeNode(int view)
        {
            ShowLog(">>>>> GetPhysicalTopologyTreeNode view=" + view);
            var result= new AreaService(db).GetBasicTree(view);
            ShowLog("<<<<< GetPhysicalTopologyTreeNode view=" + view);
            return result;
            //return null;
            //return new AreaNode() { Id = 1, Name = "root" };
        }

        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            ShowLog(">>>>> GetPhysicalTopologyTreeById id=" + id);
            return new AreaService(db).GetTree(id);
        }

        public PhysicalTopology AddPhysicalTopology(string pid, PhysicalTopology item)
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
        /// 根据节点修改监控范围
        /// </summary>
        public bool EditMonitorRange(PhysicalTopology pt)
        {

            var initializer = new AreaTreeInitializer(db);
            Area area = db.Areas.Find((i) => i.Id == pt.Id);
            if (area != null)
            {
                pt.InitBound.SetInitBound(pt.Transfrom);
                area.SetTransform(pt.Transfrom.ToDbModel());
                DbModel.Location.AreaAndDev.Bound InitBoundT = pt.InitBound.ToDbModel();
                db.Bounds.Edit(InitBoundT);
                area.SetBound(InitBoundT);
                var points = area.InitBound.Points;
                //foreach (DbModel.Location.AreaAndDev.Point p in points)
                //{
                //    DbModel.Location.AreaAndDev.Point pointT = db.Points.Find((i) => i.BoundId == InitBoundT.Id && i.Index == p.Index);
                //    if (pointT != null)
                //    {
                //        db.Points.Edit(pointT);
                //    }
                //    else
                //    {
                //        db.Points.Add(pointT);
                //    }
                //}
                db.Points.EditRange(points);
                return db.Areas.Edit(area);
            }
            else
            {
                return false;
            }
            //return db.Areas.Edit(pt.ToDbModel());
        }

        //    private void SetInitBound(Area topo, DbModel.Location.AreaAndDev.Point[] points, float thicknessT, bool isRelative = true,
        //float bottomHeightT = 0, bool isOnNormalArea = true, bool isOnAlarmArea = false, bool isOnLocationArea = false)
        //    {
        //        DbModel.Location.AreaAndDev.Bound initBound = new DbModel.Location.AreaAndDev.Bound(points, bottomHeightT, thicknessT, isRelative);
        //        DbModel.Location.AreaAndDev.Bound editBound = new DbModel.Location.AreaAndDev.Bound(points, bottomHeightT, thicknessT, isRelative);
        //        TransformM transfrom = new TransformM(initBound);
        //        db.Bounds.Add(initBound);
        //        db.Bounds.Add(editBound);
        //        transfrom.IsCreateAreaByData = isOnNormalArea;
        //        transfrom.IsOnAlarmArea = isOnAlarmArea;
        //        transfrom.IsOnLocationArea = isOnLocationArea;
        //        //TransformMs.Add(transfrom);

        //        topo.SetTransform(transfrom);
        //        topo.InitBound = initBound;
        //        topo.EditBound = editBound;
        //        Areas.Edit(topo);
        //    }

        ///// <summary>
        ///// 根据节点修改监控范围
        ///// </summary>
        //public bool EditMonitorRangeTransformM(int physicalTopologyId, TransformM tranM)
        //{
        //    //db.TransformMs.Edit(pt.Transfrom);
        //    Area area = db.Areas.Find((i) => i.Id == physicalTopologyId);
        //    if (area != null)
        //    {
        //        area.SetTransform(tranM.ToDbModel());
        //        return db.Areas.Edit(area);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    //return db.Areas.Edit(pt.ToDbModel());
        //}

        /// <summary>
        /// 根据节点添加子监控范围
        /// </summary>
        public bool AddMonitorRange(PhysicalTopology pt)
        {
            return db.Areas.Add(pt.ToDbModel());
        }

        /// <summary>
        /// 根据id，获取区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Area GetAreaById(int id)
        {
            return db.Areas.Find(i => i.Id == id);
        }

        public AreaStatistics GetAreaStatistics(int id)
        {
            ShowLog(">>>>> GetAreaStatistics id=" + id);
            Stopwatch watch = new Stopwatch();
            watch.Start();
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
            watch.Stop();
            TimeSpan time = watch.Elapsed;
            ShowLog("time:" + time);
            ShowLog("<<<<<< GetAreaStatistics id=" + id);
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

        public List<NearbyPerson> GetNearbyPerson_Alarm(int id, float fDis)
        {
            PersonService ps = new PersonService();
            List<NearbyPerson> lst = ps.GetNearbyPerson_Alarm(id, fDis);
            if (lst == null)
            {
                lst = new List<NearbyPerson>();
            }

            return lst;
        }
    }
}

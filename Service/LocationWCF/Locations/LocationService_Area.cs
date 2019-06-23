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
using DbModel.Location.Alarm;
using DbModel.LocationHistory.Alarm;
using Location.BLL.Tool;

namespace LocationServices.Locations
{
    //管理相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public IList<PhysicalTopology> GetPhysicalTopologyList(int view)
        {
            ShowLogEx(">>>>> GetPhysicalTopologyList");
            BLL.Bll dbpt = Bll.NewBllNoRelation();
            return new AreaService(dbpt).GetList(view);
        }

        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            ShowLogEx(">>>>> GetPhysicalTopology id" + id);
            return new AreaService(db).GetEntity(id, getChildren);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            ShowLogEx(">>>>> GetPhysicalTopologyListByName name" + name);
            return new AreaService(db).GetListByName(name);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string pid)
        {
            ShowLogEx(">>>>> GetPhysicalTopologyListByPid pid" + pid);
            return new AreaService(db).GetListByPid(pid);
        }

        public AreaPoints GetPoints(int areaId)
        {
            return new AreaService(db).GetPoints(areaId + "");
        }

        public List<AreaPoints> GetPointsByPid(int pid)
        {
            return new AreaService(db).GetPointsByPid(pid + "");
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public PhysicalTopology GetPhysicalTopologyTree(int view)
        {
            ShowLogEx(">>>>> GetPhysicalTopologyTree view=" + view);
            BLL.Bll dbpt = Bll.NewBllNoRelation();
            return new AreaService(dbpt).GetTree(view);
            //return null;
            //return new PhysicalTopology() { Id = 1, Name = "root" };
        }

        public AreaNode GetPhysicalTopologyTreeNode(int view)
        {
            ShowLogEx(">>>>> GetPhysicalTopologyTreeNode view=" + view);
            var result = new AreaService(db).GetBasicTree(view);
            ShowLogEx("<<<<< GetPhysicalTopologyTreeNode view=" + view);
            return result;
            //return null;
            //return new AreaNode() { Id = 1, Name = "root" };
        }

        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            ShowLogEx(">>>>> GetPhysicalTopologyTreeById id=" + id);
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
            try
            {
                Area area = db.Areas.Find((i) => i.Id == pt.Id);
                if (area != null && pt.InitBound != null)
                {
                    pt.InitBound.SetInitBound(pt.Transfrom);
                    area.SetTransform(pt.Transfrom.ToDbModel());
                    db.Areas.Edit(area);

                    DbModel.Location.AreaAndDev.Bound InitBoundT = pt.InitBound.ToDbModel();
                    DbModel.Location.AreaAndDev.Bound InitBound = db.Bounds.Find(p => p.Id == InitBoundT.Id);
                    InitBound.SetInitBound(InitBoundT);
                    db.Bounds.Edit(InitBound);

                    List<DbModel.Location.AreaAndDev.Point> lst = db.Points.FindAll(p => p.BoundId == InitBound.Id);
                    foreach (var item in InitBoundT.Points)
                    {
                        DbModel.Location.AreaAndDev.Point pi = lst.Find(p => p.Index == item.Index);
                        pi.SetPoint(item.X, item.Y, item.Z);
                    }

                    db.Points.EditRange(lst);

                    TagRelationBuffer.Instance().PuUpdateData();
                    BLL.Buffers.AuthorizationBuffer.Instance(db).PubUpdateData();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
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
        public PhysicalTopology AddMonitorRange(PhysicalTopology pt)
        {
            //return db.Areas.Add(pt.ToDbModel());
            var areaT = pt.ToDbModel();

            pt.InitBound = new Location.TModel.Location.AreaAndDev.Bound();
            pt.InitBound.IsRelative = pt.IsRelative;
            pt.InitBound.SetInitBound(pt.Transfrom);
            areaT.SetTransform(pt.Transfrom.ToDbModel());
            DbModel.Location.AreaAndDev.Bound InitBoundT = pt.InitBound.ToDbModel();
            db.Bounds.Add(InitBoundT);
            areaT.SetBound(InitBoundT);
            var points = areaT.InitBound.Points;
            //db.Points.AddRange(points);

            var result = db.Areas.Add(areaT);

            if (result)
            {
                TagRelationBuffer.Instance().PuUpdateData();
                BLL.Buffers.AuthorizationBuffer.Instance(db).PubUpdateData();

                return areaT.ToTModel();
            }
            return null;
        }

        /// <summary>
        /// 删除区域范围
        /// </summary>
        public bool DeleteMonitorRange(PhysicalTopology pt)
        {
            try
            {
                if (pt == null) return false;
                int AreaId = pt.Id;
                Area aa = db.Areas.Find(p => p.Id == AreaId);
                if (aa != null)
                {
                    db.Areas.Remove(aa);
                }

                if (pt.InitBound != null)
                {
                    int BoundId = pt.InitBound.Id;

                    db.Bounds.DeleteById(BoundId);

                    List<DbModel.Location.AreaAndDev.Point> lst = db.Points.FindAll(p => p.BoundId == BoundId);
                    if (lst != null)
                    {
                        db.Points.RemoveList(lst);
                    }
                }

                TagRelationBuffer.Instance().PuUpdateData();
                BLL.Buffers.AuthorizationBuffer.Instance(db).PubUpdateData();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据id，获取区域信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Area GetAreaById(int id)
        {
            //return db.Areas.Find(i => i.Id == id);
            AreaService asr = new AreaService();
            return asr.GetAreaById(id);
        }

        //AreaService asr = new AreaService();

        public AreaStatistics GetAreaStatistics(int id)
        {
            AreaService asr = new AreaService(db);
            return asr.GetAreaStatistics(id);
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

        /// <summary>
        /// 获取首页图片名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetHomePageNameList()
        {
            try
            {
                Bll bll = Bll.NewBllNoRelation();
                List<string> lst = bll.HomePagePictures.DbSet.Select(p => p.Name).ToList();
                //if (lst == null || lst.Count == 0)
                //{
                //    lst = new List<string>();
                //}

                if (lst.Count == 0)
                {
                    lst = null;
                }

                return lst;
            }
            catch (Exception ex)
            {
                Log.Error("LocationService.GetHomePageNameList",ex);
                return null;
            }
            
        }

        /// <summary>
        /// 根据图片名称获取首页图片信息
        /// </summary>
        /// <param name="strPictureName"></param>
        /// <returns></returns>
        public byte[] GetHomePageByName(string strPictureName)
        {
            Bll bll = Bll.NewBllNoRelation();
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\HomePages\\" + strPictureName;
            byte[] byteArray = LocationServices.Tools.ImageHelper.LoadImageFile(strPath);

            return byteArray;
        }
    }
}

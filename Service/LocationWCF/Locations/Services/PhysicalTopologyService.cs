using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Location.AreaAndDev;
using TModel.Location.Nodes;
using LocationServices.Converters;
using BLL;
using Location.BLL.Tool;
using DbModel.Location.AreaAndDev;
using Location.BLL.ServiceHelpers;
using DbModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface IPhysicalTopologyService
    {
        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>

        IList<PhysicalTopology> GetPhysicalTopologyList(int view);

        PhysicalTopology GetPhysicalTopology(string id, bool getChildren);

        IList<PhysicalTopology> GetPhysicalTopologyListByName(string name);

        IList<PhysicalTopology> GetPhysicalTopologyListByPid(string id);

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        PhysicalTopology GetPhysicalTopologyTree(int view);

        AreaNode GetPhysicalTopologyTreeNode(int view);

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        PhysicalTopology GetPhysicalTopologyTreeById(string id);

        PhysicalTopology AddPhysicalTopology(PhysicalTopology item);

        PhysicalTopology EditPhysicalTopology(PhysicalTopology item);

        PhysicalTopology RemovePhysicalTopology(string id);

        IList<PhysicalTopology> GetParkMonitorRange();

        IList<PhysicalTopology> GetFloorMonitorRange();

        IList<PhysicalTopology> GetFloorMonitorRangeById(int id);

        bool EditMonitorRange(PhysicalTopology pt);

        PhysicalTopology AddMonitorRange(PhysicalTopology pt);

        bool DeleteMonitorRange(PhysicalTopology pt);

        IList<PhysicalTopology> GetSwitchAreas();
    }

    public class PhysicalTopologyService : IPhysicalTopologyService
    {
        public static string tag = "PhysicalTopologyService";

        private Bll db;
        public PhysicalTopologyService()
        {
            db = Bll.NewBllNoRelation();
        }

        public PhysicalTopology AddMonitorRange(PhysicalTopology pt)
        {
            try
            {
                if (pt == null) return null;
                var transform = pt.Transfrom;

                //var parent = db.Areas.Find(pt.ParentId);//父区域
                //var parentBound = db.Bounds.Find(parent.InitBoundId);//父区域的范围
                //if (parent.Type == AreaTypes.楼层 && pt.IsRelative)//加上偏移量
                //{
                //    transform.X += parentBound.MinX;
                //    transform.Z += parentBound.MinY;
                //}

                //return db.Areas.Add(pt.ToDbModel());
                var areaNew = pt.ToDbModel();
                pt.InitBound = NewBound(pt);
                var newTransform = transform.ToDbModel();
                areaNew.SetTransform(newTransform);
                var newBound = pt.InitBound.ToDbModel();
                db.Bounds.Add(newBound);
                areaNew.SetBound(newBound);
                var points = areaNew.InitBound.Points;
                //db.Points.AddRange(points);

                var addR = db.Areas.Add(areaNew);

                if (addR)
                {
                    TagRelationBuffer.Instance().PuUpdateData();
                    BLL.Buffers.AuthorizationBuffer.Instance(db).PubUpdateData();

                    var result = areaNew.ToTModel();

                    //if (parent.Type == AreaTypes.楼层 && pt.IsRelative)//减去偏移量
                    //{
                    //    result.Transfrom.X -= parentBound.MinX;
                    //    result.Transfrom.Z -= parentBound.MinY;
                    //}
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Error(tag, "AddMonitorRange", e.ToString());
                return null;
            }
        }
        private Location.TModel.Location.AreaAndDev.Bound NewBound(PhysicalTopology pt)
        {
            try
            {
                var bound = new Location.TModel.Location.AreaAndDev.Bound();
                bound.IsRelative = pt.IsRelative;
                bound.SetInitBound(pt.Transfrom);
                return bound;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "NewBound", ex.ToString());
                return null;
            }
        }



        public PhysicalTopology AddPhysicalTopology(PhysicalTopology item)
        {
            return new AreaService(db).Post(item);
        }

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
                Log.Error(tag, "DeleteMonitorRange", ex.ToString());
                return false;
            }

            return true;
        }

        public bool EditMonitorRange(PhysicalTopology pt)
        {
            try
            {
                Area area = db.Areas.Find((i) => i.Id == pt.Id);
                if (area != null && pt.InitBound != null)
                {
                    area.Name = pt.Name;//2019_07_18_cww:添加名称，区域名称可以修改的
                    var transform = pt.Transfrom;

                    //var parent = db.Areas.Find(area.ParentId);//父区域
                    //var parentBound = db.Bounds.Find(parent.InitBoundId);//父区域的范围
                    //if (parent.Type == AreaTypes.楼层 && pt.IsRelative)//加上偏移量
                    //{
                    //    transform.X += parentBound.MinX;
                    //    transform.Z += parentBound.MinY;
                    //}

                    pt.InitBound.SetInitBound(pt.Transfrom);
                    area.SetTransform(pt.Transfrom.ToDbModel());
                    db.Areas.Edit(area);

                    var InitBoundT = pt.InitBound.ToDbModel();
                    var InitBound = db.Bounds.Find(p => p.Id == InitBoundT.Id);
                    InitBound.SetInitBound(InitBoundT);
                    db.Bounds.Edit(InitBound);//修改

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
                Log.Error(tag, "EditMonitorRange", ex.ToString());
                return false;
            }

            return true;
        }

        public PhysicalTopology EditPhysicalTopology(PhysicalTopology item)
        {
            try
            {
                return new AreaService(db).Put(item);
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "EditPhysicalTopology", ex.ToString());
                return null;
            }
        }

        public IList<PhysicalTopology> GetFloorMonitorRange()
        {
            try
            {
                PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
                var results = physicalTopologySp.GetFloorMonitorRange();
                return results.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetFloorMonitorRange", ex.ToString());
                return null;
            }
        }

        public IList<PhysicalTopology> GetFloorMonitorRangeById(int id)
        {
            try
            {
                PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
                var results = physicalTopologySp.GetFloorMonitorRange(id);
                return results.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetFloorMonitorRangeById", ex.ToString());
                return null;
            }
        }

        public IList<PhysicalTopology> GetParkMonitorRange()
        {
            try
            {
                PhysicalTopologySP physicalTopologySp = new PhysicalTopologySP(db);
                var results = physicalTopologySp.GetParkMonitorRange();
                return results.ToWcfModelList();
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetParkMonitorRange", ex.ToString());
                return null;
            }
        }

        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            return new AreaService(db).GetEntity(id, getChildren);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyList(int view)
        {
            return new AreaService(db).GetList(view);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            return new AreaService(db).GetListByName(name);
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string id)
        {
            return new AreaService(db).GetListByPid(id);
        }

        public PhysicalTopology GetPhysicalTopologyTree(int view)
        {
            return new AreaService(db).GetTree(view);
        }

        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            return new AreaService(db).GetTree(id);
        }

        public AreaNode GetPhysicalTopologyTreeNode(int view)
        {
            var result = new AreaService(db).GetBasicTree(view);
            return result;
        }

        public IList<PhysicalTopology> GetSwitchAreas()
        {
            try
            {
                var switchAreas = db.bus_anchor_switch_area.ToList();

                var subSwitchAreas = switchAreas;

                int ShowFloor = 0;
                if (ShowFloor == 1)//1层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z == 0 || i.min_z == 150);
                }
                else if (ShowFloor == 2)//2层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z == 450 || i.min_z == 600);
                }
                else if (ShowFloor == 3)//3层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z == 880);
                }
                else if (ShowFloor == 4)//4层
                {
                    subSwitchAreas = switchAreas.FindAll(i => i.min_z > 880);
                }
                else
                {

                }

                List<PhysicalTopology> areas = new List<PhysicalTopology>();
                float scale = 100.0f;
                foreach (var item in subSwitchAreas)
                {
                    var switchArea = new PhysicalTopology();
                    //todo:这部分的具体数值其他项目时需要调整。
                    float x1 = item.start_x / scale + 2059;
                    float x2 = item.end_x / scale + 2059;
                    float y1 = item.start_y / scale + 1565;
                    float y2 = item.end_y / scale + 1565;
                    float z1 = item.min_z / scale;
                    float z2 = item.max_z / scale;
                    switchArea.InitBound = new Location.TModel.Location.AreaAndDev.Bound(x1, y1, x2, y2, z1, z2, false);
                    //switchArea.Parent = area;
                    switchArea.Name = item.area_id;
                    switchArea.Type = AreaTypes.SwitchArea;

                    //AddAreaRect(switchArea, null, scale);
                    switchArea.SetTransformM();

                    areas.Add(switchArea);
                }

                return areas;
            }
            catch (System.Exception ex)
            {
                Log.Error(tag, "GetSwitchAreas", ex.ToString());
                return null;
            }
        }

        public PhysicalTopology RemovePhysicalTopology(string id)
        {
            return new AreaService(db).Delete(id);
        }
    }
}

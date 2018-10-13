using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using BLL.ServiceHelpers;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.FuncArgs;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Obsolete;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.Person;
using Location.TModel.LocationHistory.Data;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFService;
using LocationWCFService.ServiceHelper;
using TModel.Tools;
using ConfigArg = Location.TModel.Location.AreaAndDev.ConfigArg;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using KKSCode = Location.TModel.Location.AreaAndDev.KKSCode;
using Post = Location.TModel.Location.AreaAndDev.Post;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;

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
            var list = db.Areas.ToList();
            return list.ToWcfModelList();
        }

        public PhysicalTopology GetPhysicalTopology(string id, bool getChildren)
        {
            var item = db.Areas.Find(id.ToInt());
            if(getChildren)
                GetChildren(item);
            return item.ToTModel();
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByName(string name)
        {
            var list = db.Areas.FindListByName(name);
            return list.ToWcfModelList();
        }

        public IList<PhysicalTopology> GetPhysicalTopologyListByPid(string pid)
        {
            var list = db.Areas.FindListByPid(pid.ToInt());
            return list.ToWcfModelList();
        }

        /// <summary>
        /// 获取物理逻辑拓扑
        /// </summary>
        /// <returns></returns>
        public PhysicalTopology GetPhysicalTopologyTree()
        {
            try
            {
                Area root0 = LocationSP.GetPhysicalTopologyTree();
                PhysicalTopology root = root0.ToTModel();
                //string xml = XmlSerializeHelper.GetXmlText(root, Encoding.UTF8);
                //PhysicalTopology obj = XmlSerializeHelper.LoadFromText<PhysicalTopology>(xml);
                return root;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public PhysicalTopology GetPhysicalTopologyTreeById(string id)
        {
            var item = db.Areas.Find(id.ToInt());
            GetChildrenTree(item);
            return item.ToTModel();
        }

        private List<Area> GetChildren(Area area)
        {
            if (area != null)
            {
                var list = db.Areas.FindListByPid(area.Id);
                area.Children = list;
                return list;
            }
            else
            {
                return new List<Area>();
            }
        }

        private void GetChildrenTree(Area area)
        {
            if (area == null) return;
            var list=GetChildren(area);
            if (list != null)
            {
                foreach (var item in list)
                {
                    GetChildrenTree(item);
                }
            }

        }

        public PhysicalTopology AddPhysicalTopology(string id,PhysicalTopology item)
        {
            item.ParentId = id.ToInt();
            var dbItem = item.ToDbModel();
            var result = db.Areas.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public PhysicalTopology AddPhysicalTopology(PhysicalTopology item)
        {
            var dbItem = item.ToDbModel();
            var result = db.Areas.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public PhysicalTopology EditPhysicalTopology(PhysicalTopology item)
        {
            var dbItem = item.ToDbModel();
            var result=db.Areas.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public PhysicalTopology RemovePhysicalTopology(string id)
        {
            var item = db.Areas.Find(id.ToInt());
            GetChildren(item);
            if (item.Children != null && item.Children.Count > 0)//不能删除有子物体的节点
            {
                //throw new Exception("Have Children !");
            }
            else
            {
                db.Areas.Remove(item);
            }
            return item.ToTModel();
        }

        public List<PhysicalTopology> RemovePhysicalTopologyChildren(string id)
        {
            var list2 = new List<Area>();
            var list = db.Areas.FindListByPid(id.ToInt());
            foreach (var item in list)
            {
                GetChildren(item);
                if (item.Children == null || item.Children.Count == 0)
                {
                    bool r=db.Areas.Remove(item);//只删除无子物体的节点
                    if (r)
                    {
                        list2.Add(item);
                    }
                }
            }
            return list2.ToWcfModelList();
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

    }
}

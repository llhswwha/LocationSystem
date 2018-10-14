using BLL;
using BLL.ServiceHelpers;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using TModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface IAreaService : ITreeEntityService<PhysicalTopology>
    {

    }
    public class AreaService : IAreaService
    {
        private Bll db;

        public AreaService()
        {
            db = new Bll(false, false, false, false);
        }

        public AreaService(Bll bll)
        {
            this.db = bll;
        }

        LocationService service = new LocationService();

        public IList<PhysicalTopology> GetList()
        {
            var list = db.Areas.ToList();
            return list.ToWcfModelList();
        }

        public PhysicalTopology GetTree()
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

        public PhysicalTopology GetTree(string id)
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

        private List<DbModel.Location.AreaAndDev.DevInfo> GetDevices(Area area)
        {
            if (area != null)
            {
                var list = db.DevInfos.FindListByPid(area.Id);
                area.LeafNodes = list;
                return list;
            }
            else
            {
                return new List<DbModel.Location.AreaAndDev.DevInfo>();
            }
        }

        private void GetChildrenTree(Area area)
        {
            if (area == null) return;
            var list = GetChildren(area);
            if (list != null)
            {
                foreach (var item in list)
                {
                    GetChildrenTree(item);
                }
            }

        }

        public IList<PhysicalTopology> GetListByName(string name)
        {
            var list = db.Areas.FindListByName(name);
            return list.ToWcfModelList();
        }

        public IList<PhysicalTopology> GetListByPid(string pid)
        {
            var list = db.Areas.FindListByPid(pid.ToInt());
            return list.ToWcfModelList();
        }

        public PhysicalTopology GetEntity(string id)
        {
            return GetEntity(id, true);
        }

        public PhysicalTopology GetEntity(string id, bool getChildren)
        {
            var item = db.Areas.Find(id.ToInt());
            if (getChildren)
            {
                GetChildren(item);
                GetDevices(item);
            }
            return item.ToTModel();
        }

        public PhysicalTopology GetParent(string id)
        {
            var item = db.Areas.Find(id.ToInt());
            if (item == null) return null;
            return GetEntity(item.ParentId + "");
        }

        public PhysicalTopology Post(PhysicalTopology item)
        {
            var dbItem = item.ToDbModel();
            var result = db.Areas.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public PhysicalTopology Post(string pid, PhysicalTopology item)
        {
            item.ParentId = pid.ToInt();
            var dbItem = item.ToDbModel();
            var result = db.Areas.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public PhysicalTopology Put(PhysicalTopology item)
        {
            var dbItem = item.ToDbModel();
            var result = db.Areas.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public PhysicalTopology Delete(string id)
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

        public List<PhysicalTopology> DeleteChildren(string id)
        {
            var list2 = new List<Area>();
            var list = db.Areas.FindListByPid(id.ToInt());
            foreach (var item in list)
            {
                GetChildren(item);
                if (item.Children == null || item.Children.Count == 0)
                {
                    bool r = db.Areas.Remove(item);//只删除无子物体的节点
                    if (r)
                    {
                        list2.Add(item);
                    }
                }
            }
            return list2.ToWcfModelList();
        }
    }
}

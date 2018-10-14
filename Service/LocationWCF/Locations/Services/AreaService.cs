using BLL;
using BLL.Blls.Location;
using BLL.ServiceHelpers;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using TModel.Tools;
using DbEntity = DbModel.Location.AreaAndDev.Area;
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace LocationServices.Locations.Services
{
    public interface IAreaService : ITreeEntityService<TEntity>
    {

    }
    public class AreaService : IAreaService
    {
        private Bll db;

        private AreaBll dbSet;

        public AreaService()
        {
            db = new Bll(false, false, false, false);
            dbSet = db.Areas;
        }

        public AreaService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Areas;
        }

        LocationService service = new LocationService();


        public IList<TEntity> GetList()
        {
            var list = dbSet.ToList();
            return list.ToWcfModelList();
        }

        public TEntity GetTree()
        {
            try
            {
                var root0 = LocationSP.GetPhysicalTopologyTree();
                var root = root0.ToTModel();
                return root;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public TEntity GetTree(string id)
        {
            var item = dbSet.Find(id.ToInt());
            GetChildrenTree(item);
            return item.ToTModel();
        }

        private List<DbEntity> GetChildren(DbEntity item)
        {
            if (item != null)
            {
                var list = dbSet.FindListByPid(item.Id);
                item.Children = list;
                return list;
            }
            else
            {
                return new List<DbEntity>();
            }
        }

        private List<DbModel.Location.AreaAndDev.DevInfo> GetLeafNodes(DbEntity area)
        {
            if (area != null)
            {
                var list = db.DevInfos.GetListByPid(area.Id);
                area.LeafNodes = list;
                return list;
            }
            else
            {
                return new List<DbModel.Location.AreaAndDev.DevInfo>();
            }
        }

        private void GetChildrenTree(DbEntity entity)
        {
            if (entity == null) return;
            var list = GetChildren(entity);
            if (list != null)
            {
                foreach (var item in list)
                {
                    GetChildrenTree(item);
                }
            }

        }

        public IList<TEntity> GetListByName(string name)
        {
            var list = dbSet.FindListByName(name);
            return list.ToWcfModelList();
        }

        public IList<TEntity> GetListByPid(string pid)
        {
            var list = dbSet.FindListByPid(pid.ToInt());
            return list.ToWcfModelList();
        }

        public TEntity GetEntity(string id)
        {
            return GetEntity(id, true);
        }

        public TEntity GetEntity(string id, bool getChildren)
        {
            var item = dbSet.Find(id.ToInt());
            if (getChildren)
            {
                GetChildren(item);
                GetLeafNodes(item);
            }
            return item.ToTModel();
        }

        public TEntity GetParent(string id)
        {
            var item = dbSet.Find(id.ToInt());
            if (item == null) return null;
            return GetEntity(item.ParentId + "");
        }

        public TEntity Post(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity Post(string pid, TEntity item)
        {
            item.ParentId = pid.ToInt();
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity Put(TEntity item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public TEntity Delete(string id)
        {
            var item = dbSet.Find(id.ToInt());
            GetChildren(item);
            if (item.Children != null && item.Children.Count > 0)//不能删除有子物体的节点
            {
                //throw new Exception("Have Children !");
            }
            else
            {
                dbSet.Remove(item);
            }
            return item.ToTModel();
        }

        public IList<TEntity> DeleteListByPid(string pid)
        {
            var list2 = new List<DbEntity>();
            var list = dbSet.FindListByPid(pid.ToInt());
            foreach (var item in list)
            {
                GetChildren(item);
                if (item.Children == null || item.Children.Count == 0)
                {
                    bool r = dbSet.Remove(item);//只删除无子物体的节点
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

using BLL;
using BLL.Blls.Location;
using BLL.ServiceHelpers;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel.Location.Person;
using DbModel.Tools;
using Location.IModel;
using TModel.Location.Nodes;
using TModel.Tools;
using DbEntity = DbModel.Location.AreaAndDev.Area;
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace LocationServices.Locations.Services
{
    public interface IAreaService : ITreeEntityService<TEntity>
    {
        IList<TEntity> GetListWithPerson();

        TEntity GetTreeWithDev();
        TEntity GetTreeWithPerson();

        /// <summary>
        /// 获取树节点
        /// </summary>
        /// <param name="view">0:基本数据;1:设备信息;2:人员信息;3:设备信息+人员信息</param>
        /// <returns></returns>
        TEntity GetTree(int view);

        /// <summary>
        /// 获取树节点基本数据
        /// </summary>
        /// <param name="view">0:基本数据;1:基本设备信息;2:基本人员信息;3:基本设备信息+基本人员信息</param>
        /// <returns></returns>
        AreaNode GetBasicTree(int view);
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

        public IList<TEntity> GetList()
        {
            var list1 = dbSet.ToList();
            return list1.ToWcfModelList();
        }

        public IList<TEntity> GetListWithPerson()
        {
            var pList = GetPersonAreaList();
            IList<TEntity> list = GetList();
            foreach (var item in pList)
            {
                var entity = list.First(i => i.Id == item.Area);
                if (entity != null)
                {
                    entity.AddPerson(item.Person.ToTModel());
                }
            }
            //todo:有必要的话做一个简化版的 只有区域和人员人员基本信息的列表
            return list;
        }

        private List<PersonArea> GetPersonAreaList()
        {
            var query = from p in db.Personnels.DbSet
                        join r in db.LocationCardToPersonnels.DbSet on p.ParentId equals r.PersonnelId
                        join tag in db.LocationCards.DbSet on r.LocationCardId equals tag.Id
                        join pos in db.LocationCardPositions.DbSet on tag.Code equals pos.Code
                        select new PersonArea { Person = p, Area = pos.AreaId };
            var pList = query.ToList();
            return pList;
        } 

        class PersonArea
        {
            public Personnel Person { get; set; }

            public int? Area { get; set; }
        }

        public TEntity GetTree()
        {
            var root0 = LocationSP.GetPhysicalTopologyTree(db, false);
            var root = root0.ToTModel();
            return root;
        }

        /// <summary>
        /// 获取树节点
        /// </summary>
        /// <param name="view">0:基本数据;1:设备信息;2:人员信息;3:设备信息+人员信息</param>
        /// <returns></returns>
        public TEntity GetTree(int view)
        {
            TEntity tree = null;
            if (view == 0)
            {
                tree = GetTree();
            }
            else if (view == 1)
            {
                tree = GetTreeWithDev();
            }
            else if (view == 2)
            {
                tree = GetTreeWithPerson();
            }
            else if (view == 3)
            {
                var leafNodes = db.DevInfos.ToList();
                tree = GetTreeWithPerson(leafNodes.ToTModel());
            }
            return tree;
        }

        /// <summary>
        /// 获取树节点基本数据
        /// </summary>
        /// <param name="view">0:基本数据;1:基本设备信息;2:基本人员信息;3:基本设备信息+基本人员信息</param>
        /// <returns></returns>
        public AreaNode GetBasicTree(int view)
        {
            var areaList = dbSet.ToList();
            var list = areaList.ToTModelS();

            List<DevNode> devs = null;
            if (view == 0)
            {

            }
            else if (view == 1)
            {
                devs = db.DevInfos.ToList().ToTModelS();
            }
            else if (view == 2)
            {
                var personList = GetPersonAreaList();
                foreach (var item in personList)
                {
                    var entity = list.First(i => i.Id == item.Area);
                    if (entity != null)
                    {
                        entity.AddPerson(item.Person.ToTModelS());
                    }
                }
            }
            else if (view == 3)
            {
                var personList = GetPersonAreaList();
                foreach (var item in personList)
                {
                    var entity = list.First(i => i.Id == item.Area);
                    if (entity != null)
                    {
                        entity.AddPerson(item.Person.ToTModelS());
                    }
                }
                devs = db.DevInfos.ToList().ToTModelS();
            }

            var roots = TreeHelper.CreateTree(list, devs);
            if (roots.Count > 0)
            {
                return roots[0];
            }
            else
            {
                return null;
            }
        }

        public TEntity GetTreeWithDev()
        {
            var root0 = LocationSP.GetPhysicalTopologyTree(db, true);
            var root = root0.ToTModel();
            return root;
        }

        public TEntity GetTreeWithPerson()
        {
            return GetTreeWithPerson(null);
        }

        private TEntity GetTreeWithPerson(List<Location.TModel.Location.AreaAndDev.DevInfo> devs)
        {
            List<TEntity> list = GetListWithPerson().ToList();
            List<TEntity> roots = TreeHelper.CreateTree<TEntity, Location.TModel.Location.AreaAndDev.DevInfo>(list, devs);
            if (roots.Count > 0)
            {
                return roots[0];
            }
            else
            {
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

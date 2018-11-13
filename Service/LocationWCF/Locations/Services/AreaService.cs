using BLL;
using BLL.Blls.Location;
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
using TModel.Location.AreaAndDev;
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
        /// <param name="view">0:基本数据;1:基本设备信息;2:基本人员信息;3:基本设备信息+基本人员信息;4:只显示设备的节点;5:只显示人员的节点;6:只显示人员或设备的节点</param>
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

        public AreaStatistics GetAreaStatisticsCount(List<int?> lst)
        {
          
            var query = from t1 in db.LocationCardPositions.DbSet
                        join t2 in db.Personnels.DbSet on t1.PersonId equals t2.Id
                        where lst.Contains(t1.AreaId)
                        select t2;

            var query2 = from t1 in db.DevInfos.DbSet
                         where lst.Contains(t1.ParentId)
                         select t1;

            var query3 = from t1 in db.LocationAlarms.DbSet
                         where lst.Contains(t1.AreadId)
                         select t1;

            var query4 = from t1 in db.DevInfos.DbSet
                         join t2 in db.DevAlarms.DbSet on t1.Id equals t2.DevInfoId
                         where lst.Contains(t1.ParentId)
                         select t2;
            
            var pList = query.ToList();
            var dvList = query2.ToList();
            var laList = query3.ToList();
            var daList = query4.ToList();

            var ass=new AreaStatistics();
            ass.PersonNum= pList.Count;
            ass.DevNum = dvList.Count;
            ass.LocationAlarmNum = laList.Count;
            ass.DevAlarmNum = daList.Count;
            return ass;
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
            var query = from r in db.LocationCardToPersonnels.DbSet
                        join p in db.Personnels.DbSet on r.PersonnelId equals p.Id
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
            var root0 = db.GetAreaTree(false);
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
            else if (view == 1 || view == 4)
            {
                devs = db.DevInfos.ToList().ToTModelS();
            }
            else if (view == 2 || view == 5)
            {
                BindPerson(list);
            }
            else if (view == 3 || view == 6)
            {
                BindPerson(list);
                devs = db.DevInfos.ToList().ToTModelS();
            }

            var roots = TreeHelper.CreateTree(list, devs);
            AreaNode root = null;
            if (roots.Count > 0)
            {
                root = roots[0];//根节点

                var park = root.Children[0];//四会热电厂

                //将电厂下的人员移动到其他区域中
                var otherArea = new AreaNode();
                otherArea.Id = 100000;
                otherArea.Name = "厂区内";
                if (park.Persons != null)
                {
                    foreach (var person in park.Persons)
                    {
                        otherArea.AddPerson(person);
                    }
                    park.Persons.Clear();
                }
                park.Children.Add(otherArea);
                root = park;//将电厂做为根节点
            }
            if (view == 4 || view == 5 || view == 6)
            {
                RemoveEmptyNodes(root);
            }

            SumNodeCount(root);

            //if (root.Children.Count == 0)
            //{
            //    root.Children = null;
            //}

            SetChildrenNull(root);

            return root;
        }

        private void SetChildrenNull(AreaNode node)
        {
            if (node.Children != null)
            {
                foreach (var subNode in node.Children)
                {
                    SetChildrenNull(subNode);
                }
                if (node.Children!=null && node.Children.Count == 0)
                {
                    node.Children = null;
                }
                if (node.Persons != null && node.Persons.Count == 0)
                {
                    node.Persons = null;
                }
                if (node.LeafNodes != null && node.LeafNodes.Count == 0)
                {
                    node.Children = null;
                }
            }
        }

        private void RemoveEmptyNodes(AreaNode node)
        {
            if (node == null) return;
            if (node.Children != null)
                for (int i = 0; i < node.Children.Count; i++)
                {
                    AreaNode subNode = node.Children[i];
                    if (subNode.IsSelftEmpty())
                    {
                        node.Children.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        RemoveEmptyNodes(subNode);
                        if (subNode.IsSelftEmpty())
                        {
                            node.Children.RemoveAt(i);
                            i--;
                        }
                    }
                }
        }

        /// <summary>
        /// 遍历并计算数量
        /// </summary>
        /// <param name="node"></param>
        private void SumNodeCount(AreaNode node)
        {
            if (node == null) return;
            if(node.Persons!=null)
                node.TotalPersonCount = node.Persons.Count;
            if (node.LeafNodes != null)
                node.TotalDevCount = node.LeafNodes.Count;

            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    AreaNode subNode = node.Children[i];
                    SumNodeCount(subNode);
                    node.TotalPersonCount += subNode.TotalPersonCount;
                    node.TotalDevCount += subNode.TotalDevCount;
                }
            }
        }

        private void BindPerson(List<AreaNode> list)
        {
            var personList = GetPersonAreaList();
            foreach (var item in personList)
            {
                if (item.Area == null) continue;
                var entity = list.First(i => i.Id == item.Area);
                if (entity != null)
                {
                    entity.AddPerson(item.Person.ToTModelS());
                }
            }
        }

        public TEntity GetTreeWithDev()
        {
            var root0 = db.GetAreaTree(true);
            var root = root0.ToTModel();
            return root;
        }

        public TEntity GetTreeWithPerson()
        {
            return GetTreeWithPerson(null);
        }

        private TEntity GetTreeWithPerson(List<Location.TModel.Location.AreaAndDev.DevInfo> devs)
        {
            var list = GetListWithPerson().ToList();
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

        public List<TEntity> GetListByPid(string pid)
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
            if (item.InitBound == null)
            {
                item.InitBound=db.Bounds.Find(item.InitBoundId);
                item.InitBound.Points = db.Points.FindAll(i => i.BoundId == item.InitBoundId);
            }
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

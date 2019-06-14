using BLL;
using BLL.Blls.Location;
using DbModel.Tools;
using Location.BLL.Tool;
using Location.TModel.Location.Person;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using TModel.Tools;
using DbEntity = DbModel.Location.Person.Department;
using TEntity = Location.TModel.Location.Person.Department;

namespace LocationServices.Locations.Services
{
    public interface IDepartmentService : ITreeEntityService<TEntity>
    {
        TEntity GetTree(List<Personnel> leafNodes);

        /// <summary>
        /// 获取树节点
        /// </summary>
        /// <param name="view">0:基本数据;1:人员信息</param>
        /// <returns></returns>
        TEntity GetTree(int view);
    }

    public class DepartmentService : IDepartmentService
    {
        private Bll db;

        private DepartmentBll dbSet;

        public DepartmentService()
        {
            db = new Bll(false, false, false, false);
            dbSet = db.Departments;
        }

        public DepartmentService(Bll bll)
        {
            this.db = bll;
            dbSet = db.Departments;
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

        public TEntity GetEntity(string id)
        {
            DbEntity depDep = dbSet.Find(id.ToInt());

            SetNull(depDep);

            if (depDep.Children != null)
            {
                foreach (var subDep in depDep.Children)//子部门
                {
                    subDep.Parent = null;//切除关联防止序列化出错
                    subDep.Children = null;//切除关联防止序列化出错

                    SetNull(subDep);
                }
            }
            TEntity dep = depDep.ToTModel();
            return dep;
        }

        private static void SetNull(DbEntity depDep)
        {
            if (depDep.Children != null && depDep.Children.Count == 0)
            {
                depDep.Children = null;
            }

            if (depDep.LeafNodes != null && depDep.LeafNodes.Count == 0)
            {
                depDep.LeafNodes = null;
            }
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

        private List<DbModel.Location.Person.Personnel> GetLeafNodes(DbEntity entity)
        {
            if (entity != null)
            {
                var list = db.Personnels.GetListByPid(entity.Id);
                entity.LeafNodes = list;
                return list;
            }
            else
            {
                return new List<DbModel.Location.Person.Personnel>();
            }
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

        public List<TEntity> GetList()
        {
            var list = dbSet.ToList();
            var list2 = list.ToTModel();
            return list2.ToWCFList();
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

        public TEntity GetParent(string id)
        {
            var item = dbSet.Find(id.ToInt());
            if (item == null) return null;
            return GetEntity(item.ParentId + "");
        }

        public TEntity GetTree()
        {
            return GetTree(new List<Personnel>());
        }

        PersonService personService;

        public TEntity GetTree(int view)
        {
            if (view == 0)
            {
                return GetTree(new List<Personnel>());
            }
            else if (view == 1)
            {
                List<Personnel> leafNodes = db.Personnels.ToList().ToTModel();
                return GetTree(leafNodes);
            }
            else if (view == 2)
            {
                
                if (personService == null)
                {
                    personService = new PersonService();
                }
                List<Personnel> leafNodes = personService.GetList(true);
                return GetTree(leafNodes);
            }
            else
            {
                return GetTree(new List<Personnel>());
            }
            
        }

        public TEntity GetTree(List<Personnel> leafNodes)
        {
            try
            {
                var list = dbSet.ToList().ToTModel();
                //var leafNodes = GetPersonList();
                var roots = TreeHelper.CreateTree(list, leafNodes);
                //foreach (var p in leafNodes)
                //{
                //    p.Parent = list.Find(i => i.Id == p.ParentId);
                //}

                if (roots.Count > 0)
                {
                    Department d = new Department();
                    if (d.Children == null)
                    {
                        d.Children = new List<Department>();
                    }
                    foreach (Department t in roots)
                    {
                        d.Children.Add(t);
                    }
                    return d;
                    //return roots[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
            }
        }

        public TEntity GetTree(string id)
        {
            var item = dbSet.Find(id.ToInt());
            GetChildrenTree(item);
            return item.ToTModel();
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
    }
}

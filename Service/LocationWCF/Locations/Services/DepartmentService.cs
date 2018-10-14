using BLL;
using BLL.Blls.Location;
using DbModel.Tools;
using Location.TModel.Location.Person;
using LocationServices.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;
using DbEntity = DbModel.Location.Person.Department;
using TEntity = Location.TModel.Location.Person.Department;

namespace LocationServices.Locations.Services
{
    public interface IDepartmentService : ITreeEntityService<Department>
    {

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

        public Department Delete(string id)
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

        public List<Department> DeleteChildren(string id)
        {
            var list2 = new List<DbEntity>();
            var list = dbSet.FindListByPid(id.ToInt());
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

        public Department GetEntity(string id)
        {
            return dbSet.Find(id.ToInt()).ToTModel();
        }

        public Department GetEntity(string id, bool getChildren)
        {
            var item = dbSet.Find(id.ToInt());
            if (getChildren)
            {
                GetChildren(item);
                GetLeafNodes(item);
            }
            return item.ToTModel();
        }

        private List<DbModel.Location.Person.Personnel> GetLeafNodes(DbModel.Location.Person.Department area)
        {
            if (area != null)
            {
                var list = db.Personnels.FindListByPid(area.Id);
                area.LeafNodes = list;
                return list;
            }
            else
            {
                return new List<DbModel.Location.Person.Personnel>();
            }
        }

        private List<DbModel.Location.Person.Department> GetChildren(DbModel.Location.Person.Department item)
        {
            if (item != null)
            {
                var list = dbSet.FindListByPid(item.Id);
                item.Children = list;
                return list;
            }
            else
            {
                return new List<DbModel.Location.Person.Department>();
            }
        }

        public IList<Department> GetList()
        {
            var list = dbSet.ToList();
            var list2 = list.ToTModel();
            return list2.ToWCFList();
        }

        public IList<Department> GetListByName(string name)
        {
            var list = dbSet.FindListByName(name);
            return list.ToWcfModelList();
        }

        public IList<Department> GetListByPid(string pid)
        {
            var list = dbSet.FindListByPid(pid.ToInt());
            return list.ToWcfModelList();
        }

        public Department GetParent(string id)
        {
            var item = dbSet.Find(id.ToInt());
            if (item == null) return null;
            return GetEntity(item.ParentId + "");
        }

        public Department GetTree()
        {
            try
            {
                var list = dbSet.ToList().ToTModel();
                //var leafNodes = GetPersonList();
                var roots = TreeHelper.CreateTree<Department, Personnel>(list, null);
                if (roots.Count > 0)
                {
                    return roots[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public Department GetTree(string id)
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

        public Department Post(Department item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public Department Post(string pid, Department item)
        {
            item.ParentId = pid.ToInt();
            var dbItem = item.ToDbModel();
            var result = dbSet.Add(dbItem);
            return result ? dbItem.ToTModel() : null;
        }

        public Department Put(Department item)
        {
            var dbItem = item.ToDbModel();
            var result = dbSet.Edit(dbItem);
            return result ? dbItem.ToTModel() : null;
        }
    }
}

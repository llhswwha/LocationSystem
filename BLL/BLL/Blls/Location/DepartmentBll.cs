using DAL;
using System.Linq;
using DbModel.Location.Person;
using System.Collections.Generic;

namespace BLL.Blls.Location
{
    public class DepartmentBll : BaseBll<Department, LocationDb>
    {
        public DepartmentBll():base()
        {

        }
        public DepartmentBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Departments;
        }

        public Department GetRoot()
        {
            return DbSet.FirstOrDefault();
        }

        public List<Department> FindListByPid(int pid)
        {
            return DbSet.Where(i => i.ParentId == pid).ToList();
        }

        public List<Department> FindListByName(string name)
        {
            var list = DbSet.Where(i => i.Name.Contains(name)).ToList();
            foreach (var item in list)
            {
                item.Children = null;//todo:Find一个不会获取Children，但是用Contains查找会有，为什么呢？要再看看EF的书。
            }
            return list;
        }
    }
}

using DAL;
using System.Linq;
using DbModel.Location.Person;

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
    }
}

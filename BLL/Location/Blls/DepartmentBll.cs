using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class DepartmentBll : BaseBll<Department,LocationDb>
    {
        public DepartmentBll():base()
        {

        }
        public DepartmentBll(LocationDb db):base(db)
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

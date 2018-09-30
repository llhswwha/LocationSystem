using DAL;
using DbModel.Location.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class RoleBll : BaseBll<Role, LocationDb>
    {
        public RoleBll():base()
        {

        }
        public RoleBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Roles;
        }
    }
}

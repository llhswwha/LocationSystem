using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class UserBll : BaseBll<User, LocationDb>
    {
        public UserBll():base()
        {

        }
        public UserBll(LocationDb db):base(db)
        {

        }
        protected override void InitDbSet()
        {
            DbSet = Db.Users;
        }
    }
}

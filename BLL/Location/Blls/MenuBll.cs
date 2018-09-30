using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class MenuBll: BaseBll<Menu, LocationDb>
    {
        public MenuBll():base()
        {

        }
        public MenuBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Menus;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.DAL;

namespace Location.BLL
{
    public class AreaBll:BaseBll<Area>
    {
        public AreaBll():base()
        {
            
        }

        public AreaBll(LocationDb db):base(db)
        {
            
        }

        protected override void InitDbSet()
        {
            DbSet = Db.Areas;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model.Base;
using Location.Model.LocationTables;

namespace Location.BLL.Blls
{
    public class BoundBll : BaseBll<Bound, LocationDb>
    {
        public BoundBll():base()
        {

        }
        public BoundBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Bounds;
        }
    }
}

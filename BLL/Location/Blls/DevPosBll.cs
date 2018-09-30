using Location.DAL;
using Location.Model.LocationTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class DevPosBll : BaseBll<DevPos, LocationDb>
    {
        public DevPosBll():base()
        {

        }
        public DevPosBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevPos;
        }
    }
}

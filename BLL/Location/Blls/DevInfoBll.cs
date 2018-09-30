using Location.DAL;
using Location.Model.LocationTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class DevInfoBll : BaseBll<DevInfo, LocationDb>
    {
        public DevInfoBll():base()
        {

        }
        public DevInfoBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevInfos;
        }
    }
}

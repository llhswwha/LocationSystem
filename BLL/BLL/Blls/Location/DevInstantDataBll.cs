using DAL;
using DbModel.Location.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class DevInstantDataBll : BaseBll<DevInstantData, LocationDb>
    {
        public DevInstantDataBll():base()
        {

        }
        public DevInstantDataBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevInstantDatas;
        }
    }
}

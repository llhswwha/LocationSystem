using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class DevModelBll : BaseBll<DevModel, LocationDb>
    {
        public DevModelBll():base()
        {

        }
        public DevModelBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevModels;
        }
    }
}

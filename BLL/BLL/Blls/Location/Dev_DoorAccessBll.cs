using System.Collections.Generic;
using System.Linq;
using DAL;
using DbModel.Location.AreaAndDev;

namespace BLL.Blls.Location
{
    public class Dev_DoorAccessBll : BaseBll<Dev_DoorAccess, LocationDb>
    {
        public Dev_DoorAccessBll() : base()
        {

        }
        public Dev_DoorAccessBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Dev_DoorAccess;
        }
    }
}

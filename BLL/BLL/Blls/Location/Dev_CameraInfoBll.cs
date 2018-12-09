using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class Dev_CameraInfoBll : BaseBll<Dev_CameraInfo, LocationDb>
    {
        public Dev_CameraInfoBll() : base()
        {

        }
        public Dev_CameraInfoBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Dev_CameraInfos;
        }
    }
}

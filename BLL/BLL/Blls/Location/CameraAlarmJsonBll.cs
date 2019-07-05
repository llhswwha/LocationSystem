using DAL;
using DbModel.Location.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class CameraAlarmJsonBll : BaseBll<CameraAlarmJson, LocationDb>
    {

        public CameraAlarmJsonBll() : base()
        {

        }

        public CameraAlarmJsonBll(LocationDb db) : base()
        {
        
}

        protected override void InitDbSet()
        {
            DbSet = Db.CameraAlarmJsonBll;
        }
    }
}

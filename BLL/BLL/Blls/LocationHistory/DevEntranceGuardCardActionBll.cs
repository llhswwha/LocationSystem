using DAL;
using DbModel.LocationHistory.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class DevEntranceGuardCardActionBll : BaseBll<DevEntranceGuardCardAction, LocationHistoryDb>
    {
        public DevEntranceGuardCardActionBll() : base()
        {

        }
        public DevEntranceGuardCardActionBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevEntranceGuardCardActions;
        }
    }
}

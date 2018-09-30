using DAL;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class U3DPositionBll : BaseBll<U3DPosition, LocationHistoryDb>
    {
        public U3DPositionBll() : base()
        {

        }
        public U3DPositionBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.U3DPositions;
        }
    }
}

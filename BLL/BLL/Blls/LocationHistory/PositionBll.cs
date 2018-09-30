using DAL;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class PositionBll : BaseBll<Position, LocationHistoryDb>
    {
        public PositionBll() : base()
        {

        }
        public PositionBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Positions;
        }
    }
}

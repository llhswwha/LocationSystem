using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Others;
using DAL;

namespace BLL.Blls.LocationHistory
{
    public class PointHistoryBll : BaseBll<PointHistory, LocationHistoryDb>
    {
        public PointHistoryBll() : base()
        {

        }
        public PointHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PointHistories;
        }
    }
}

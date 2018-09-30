using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class PositionBll : BaseBll<Position, LocationHistoryDb>
    {
        public PositionBll():base()
        {

        }
        public PositionBll(LocationHistoryDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Position;
        }
    }
}

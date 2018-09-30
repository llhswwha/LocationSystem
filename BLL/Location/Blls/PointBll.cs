using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model.Base;

namespace Location.BLL.Blls
{
    public class PointBll : BaseBll<Point, LocationDb>
    {
        public PointBll():base()
        {

        }
        public PointBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Points;
        }
    }
}

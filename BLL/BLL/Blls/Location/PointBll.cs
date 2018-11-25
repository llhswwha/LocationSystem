using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class PointBll : BaseBll<Point, LocationDb>
    {
        public PointBll():base()
        {

        }
        public PointBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Points;
        }
    }

    //public class ShapePointBll : BaseBll<Point, LocationDb>
    //{
    //    public ShapePointBll() : base()
    //    {

    //    }
    //    public ShapePointBll(LocationDb db) : base(db)
    //    {

    //    }

    //    //protected override void InitDbSet()
    //    //{
    //    //    DbSet = Db.ShapePoints;
    //    //}
    //}
}

using DAL;
using DbModel.Location.AreaAndDev;

namespace BLL.Blls.Location
{
    public class BoundBll : BaseBll<Bound, LocationDb>
    {
        public BoundBll():base()
        {

        }
        public BoundBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Bounds;
        }
    }

    //public class ShapeBll : BaseBll<Bound, LocationDb>
    //{
    //    public ShapeBll() : base()
    //    {

    //    }
    //    public ShapeBll(LocationDb db) : base(db)
    //    {

    //    }

    //    protected override void InitDbSet()
    //    {
    //        DbSet = Db.Shapes;
    //    }
    //}
}

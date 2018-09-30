using System.Linq;
using DAL;
using DbModel.Location.AreaAndDev;

namespace BLL.Blls.Location
{
    public class AreaBll : BaseBll<Area, LocationDb>
    {
        public AreaBll():base()
        {

        }
        public AreaBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Areas;
        }

        public Area GetRoot()
        {
            return DbSet.FirstOrDefault();
        }

        public Area FindByName(string name)
        {
            return DbSet.FirstOrDefault(i => i.Name == name);
        }
    }
}

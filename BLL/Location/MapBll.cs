using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Location.Model;
using Location.DAL;

namespace Location.BLL
{
    public class MapBll:BaseBll<Map>
    {
        public MapBll():base()
        {

        }

        public MapBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Map;
        }
    }
}

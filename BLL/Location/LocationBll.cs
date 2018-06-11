using Location.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class LocationBll
    {
        public LocationDb Db = new LocationDb();

        public AreaBll Area { get; set; }

        public MapBll Map { get; set; }

        public PositionBll Position { get; set; }

        public TagPositionBll TagPosition { get; set; }

        public LocationBll()
        {
            Area = new AreaBll(Db);
        }
    }
}

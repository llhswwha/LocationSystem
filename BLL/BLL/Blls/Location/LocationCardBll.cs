using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class LocationCardBll : BaseBll<LocationCard, LocationDb>
    {
        public LocationCardBll():base()
        {

        }
        public LocationCardBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationCards;
        }

        public List<LocationCard> GetListByName(string name)
        {
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }

        public List<LocationCard> GetListByRole(int roleId)
        {
            return DbSet.Where(i => i.CardRoleId==roleId).ToList();
        }
    }
}

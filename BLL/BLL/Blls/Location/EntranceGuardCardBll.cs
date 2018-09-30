using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class EntranceGuardCardBll : BaseBll<EntranceGuardCard, LocationDb>
    {
        public EntranceGuardCardBll():base()
        {

        }
        public EntranceGuardCardBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.EntranceGuardCards;
        }
    }
}

using DAL;
using DbModel.Location.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class EntranceGuardCardToPersonnelBll : BaseBll<EntranceGuardCardToPersonnel, LocationDb>
    {
        public EntranceGuardCardToPersonnelBll():base()
        {

        }
        public EntranceGuardCardToPersonnelBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.EntranceGuardCardToPersonnels;
        }
    }
}

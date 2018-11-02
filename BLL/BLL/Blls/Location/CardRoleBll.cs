using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DbModel.Location.Authorizations;

namespace BLL.Blls.Location
{
    public class CardRoleBll : BaseBll<CardRole, LocationDb>
    {
        public CardRoleBll():base()
        {

        }
        public CardRoleBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.CardRoles;
        }
    }
}

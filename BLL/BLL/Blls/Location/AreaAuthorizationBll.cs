using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class AreaAuthorizationBll : BaseBll<AreaAuthorization, LocationDb>
    {
        public AreaAuthorizationBll():base()
        {

        }
        public AreaAuthorizationBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.AreaAuthorizations;
        }
    }
}

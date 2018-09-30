using Location.DAL;
using Location.Model.topviewxp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.topviewxpBlls
{
    public class t_SetModelBll : BaseBll<t_SetModel, LocationDb>
    {
        public t_SetModelBll():base()
        {

        }
        public t_SetModelBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.t_SetModelS;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model.topviewxp;
using Location.DAL;

namespace Location.BLL.topviewxpBlls
{
    public class t_KKSCodeBll : BaseBll<t_KKSCode, topviewxpDb>
    {
        public t_KKSCodeBll():base()
        {

        }
        public t_KKSCodeBll(topviewxpDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.t_KKSCodes;
        }
    }
}

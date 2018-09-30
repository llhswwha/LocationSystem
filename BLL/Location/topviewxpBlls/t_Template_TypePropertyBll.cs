using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model.topviewxp;
using Location.DAL;

namespace Location.BLL.topviewxpBlls
{
    public class t_Template_TypePropertyBll:BaseBll<t_Template_TypeProperty, LocationDb>
    {
        public t_Template_TypePropertyBll():base()
        {

        }
        public t_Template_TypePropertyBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.t_Template_TypeProperties;
        }
    }
}

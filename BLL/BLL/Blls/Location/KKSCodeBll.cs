using DAL;
using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class KKSCodeBll : BaseBll<KKSCode, LocationDb>
    {
        public KKSCodeBll():base()
        {

        }
        public KKSCodeBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.KKSCodes;
        }
    }
}

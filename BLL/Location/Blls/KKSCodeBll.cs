using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.Model.Manage;
using Location.DAL;


namespace Location.BLL.Blls
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

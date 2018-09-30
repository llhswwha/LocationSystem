using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class MobileInspectionItemBll : BaseBll<MobileInspectionItem, LocationDb>
    {
        public MobileInspectionItemBll():base()
        {

        }
        public MobileInspectionItemBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.MobileInspectionItems;
        }
    }
}

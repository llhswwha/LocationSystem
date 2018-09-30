using Location.DAL;
using Location.Model.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class MobileInspectionContentBll : BaseBll<MobileInspectionContent, LocationDb>
    {
        public MobileInspectionContentBll():base()
        {

        }
        public MobileInspectionContentBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.MobileInspectionContents;
        }
    }
}

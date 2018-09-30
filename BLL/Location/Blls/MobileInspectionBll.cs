using Location.DAL;
using Location.Model.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class MobileInspectionBll : BaseBll<MobileInspection, LocationDb>
    {
        public MobileInspectionBll():base()
        {

        }
        public MobileInspectionBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.MobileInspections;
        }
    }
}

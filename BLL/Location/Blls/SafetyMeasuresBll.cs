using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model.Work;

namespace Location.BLL.Blls
{
    public class SafetyMeasuresBll : BaseBll<SafetyMeasures, LocationDb>
    {
        public SafetyMeasuresBll():base()
        {

        }
        public SafetyMeasuresBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.SafetyMeasuress;
        }
    }
}

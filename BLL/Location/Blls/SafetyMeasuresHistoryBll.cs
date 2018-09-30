using Location.DAL;
using Location.Model.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class SafetyMeasuresHistoryBll : BaseBll<SafetyMeasuresHistory, LocationHistoryDb>
    {
        public SafetyMeasuresHistoryBll():base()
        {

        }
        public SafetyMeasuresHistoryBll(LocationHistoryDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.SafetyMeasuresHistorys;
        }
    }
}

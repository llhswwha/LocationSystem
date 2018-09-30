using DAL;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class PersonnelMobileInspectionHistoryBll : BaseBll<PersonnelMobileInspectionHistory, LocationHistoryDb>
    {
        public PersonnelMobileInspectionHistoryBll() : base()
        {

        }
        public PersonnelMobileInspectionHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PersonnelMobileInspectionHistorys;
        }
    }
}

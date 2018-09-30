using DAL;
using DbModel.LocationHistory.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class PersonnelHistoryBll : BaseBll<PersonnelHistory, LocationHistoryDb>
    {
        public PersonnelHistoryBll() : base()
        {

        }
        public PersonnelHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PersonnelHistorys;
        }
    }
}

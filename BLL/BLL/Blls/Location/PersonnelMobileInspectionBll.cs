using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class PersonnelMobileInspectionBll : BaseBll<PersonnelMobileInspection, LocationDb>
    {
        public PersonnelMobileInspectionBll():base()
        {

        }
        public PersonnelMobileInspectionBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PersonnelMobileInspections;
        }
    }
}

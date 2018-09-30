using Location.DAL;
using Location.Model.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
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

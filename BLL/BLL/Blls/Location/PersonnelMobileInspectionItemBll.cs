using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class PersonnelMobileInspectionItemBll : BaseBll<PersonnelMobileInspectionItem, LocationDb>
    {
        public PersonnelMobileInspectionItemBll():base()
        {

        }
        public PersonnelMobileInspectionItemBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PersonnelMobileInspectionItems;
        }
    }
}

using DAL;
using DbModel.Location.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class PersonnelFirstInAreaBll : BaseBll<PersonnelFirstInArea, LocationDb>
    {
        public PersonnelFirstInAreaBll() : base()
        {

        }
        public PersonnelFirstInAreaBll(LocationDb db) : base(db)
        {

        }
        protected override void InitDbSet()
        {
            DbSet = Db.PersonnelFirstInAreas;
        }
    }
}

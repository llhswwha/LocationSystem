using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.Model.Manage;
using Location.DAL;

namespace Location.BLL.Blls
{
    public class PersonnelBll : BaseBll<Personnel, LocationDb>
    {
        public PersonnelBll():base()
        {

        }
        public PersonnelBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Personnels;
        }

        public List<Personnel> FindListByName(string name)
        {
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }
    }
}

using DAL;
using DbModel.Location.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
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

        public List<Personnel> GetListByName(string name)
        {
            return DbSet.Where(i => i.Name.Contains(name)).ToList();
        }

        public List<Personnel> GetListByPid(int pid)
        {
            return DbSet.Where(i => i.ParentId == pid).ToList();
        }

        public List<Personnel> DeleteListByPid(int pid)
        {
            var list = GetListByPid(pid);
            foreach (var item in list)
            {
                Remove(item, false);
            }
            bool r = Save();
            if (r)
                return list;
            else
                return null;
        }
    }
}

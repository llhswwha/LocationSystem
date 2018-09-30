using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.DAL;

namespace Location.BLL.Blls
{
    public class ArchorBll : BaseBll<Archor, LocationDb>
    {
        public ArchorBll():base()
        {

        }
        public ArchorBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Archors;
        }


        public Archor FindByCode(string code)
        {
            return DbSet.FirstOrDefault(i => i.Code == code);
        }

        public List<Archor> FindByCodes(List<string> codes)
        {
            return DbSet.Where(i => codes.Contains(i.Code)).ToList();
        }
    }
}

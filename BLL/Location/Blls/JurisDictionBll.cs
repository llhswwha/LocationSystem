using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.DAL;

namespace Location.BLL.Blls
{
    public class JurisDictionBll : BaseBll<JurisDiction, LocationDb>
    {
        public JurisDictionBll():base()
        {

        }
        public JurisDictionBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.JurisDictions;
        }
    }
}

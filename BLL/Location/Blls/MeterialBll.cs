using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.DAL;

namespace Location.BLL.Blls
{
    public class MeterialBll : BaseBll<Meterial, LocationDb>
    {
        public MeterialBll():base()
        {

        }
        public MeterialBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Meterials;
        }
    }
}

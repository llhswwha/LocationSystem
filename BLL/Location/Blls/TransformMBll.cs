using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class TransformMBll : BaseBll<TransformM, LocationDb>
    {
        public TransformMBll():base()
        {

        }
        public TransformMBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.TransformMs;
        }
    }
}

using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class OperationItemBll : BaseBll<OperationItem, LocationDb>
    {
        public OperationItemBll():base()
        {

        }
        public OperationItemBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.OperationItems;
        }
    }
}

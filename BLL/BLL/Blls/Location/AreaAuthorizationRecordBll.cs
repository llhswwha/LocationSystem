using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class AreaAuthorizationRecordBll : BaseBll<AreaAuthorizationRecord, LocationDb>
    {
        public AreaAuthorizationRecordBll():base()
        {

        }
        public AreaAuthorizationRecordBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.AreaAuthorizationRecords;
        }
    }
}

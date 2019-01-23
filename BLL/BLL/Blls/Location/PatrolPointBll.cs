using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class PatrolPointBll : BaseBll<PatrolPoint, LocationDb>
    {
        public PatrolPointBll():base()
        {

        }
        public PatrolPointBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PatrolPoints;
        }
    }
}

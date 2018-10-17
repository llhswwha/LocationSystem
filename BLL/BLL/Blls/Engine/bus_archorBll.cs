using DAL;
using DbModel.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Engine
{
    public class bus_anchorBll : BaseBll<bus_anchor, EngineDb>
    {
        public bus_anchorBll() : base()
        {

        }
        public bus_anchorBll(EngineDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.bus_anchors;
        }
    }
}

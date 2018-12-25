using DAL;
using DbModel.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Engine
{
    public class bus_anchor_switch_areaBll : BaseBll<bus_anchor_switch_area, EngineDb>
    {
        public bus_anchor_switch_areaBll() : base()
        {

        }
        public bus_anchor_switch_areaBll(EngineDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.bus_anchor_switch_areas;
        }
    }
}

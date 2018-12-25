using DAL;
using DbModel.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Engine
{
    public class bus_anchor_configBll : BaseBll<bus_anchor_config, EngineDb>
    {
        public bus_anchor_configBll() : base()
        {

        }
        public bus_anchor_configBll(EngineDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.bus_anchor_configs;
        }
    }
}

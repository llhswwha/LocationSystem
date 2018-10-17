using DAL;
using DbModel.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Engine
{
    public class bus_tagBll : BaseBll<bus_tag, EngineDb>
    {
        public bus_tagBll() : base()
        {

        }
        public bus_tagBll(EngineDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.bus_tags;
        }
    }
}

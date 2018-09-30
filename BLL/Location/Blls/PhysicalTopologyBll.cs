using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.DAL;

namespace Location.BLL.Blls
{
    public class PhysicalTopologyBll : BaseBll<PhysicalTopology, LocationDb>
    {
        public PhysicalTopologyBll():base()
        {

        }
        public PhysicalTopologyBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.PhysicalTopologys;
        }

        public PhysicalTopology GetRoot()
        {
            return DbSet.FirstOrDefault();
        }

        public PhysicalTopology FindByName(string name)
        {
            return DbSet.FirstOrDefault(i => i.Name == name);
        }
    }
}

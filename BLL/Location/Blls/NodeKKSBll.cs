using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;
using Location.Model.Manage;
using Location.DAL;

namespace Location.BLL.Blls
{
    public class NodeKKSBll : BaseBll<NodeKKS, LocationDb>
    {
        public NodeKKSBll():base()
        {

        }
        public NodeKKSBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.NodeKKSs;
        }
    }
}

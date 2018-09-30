using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.DAL;
using Location.Model;

namespace Location.BLL.Blls
{
    public class PostBll : BaseBll<Post, LocationDb>
    {
        public PostBll():base()
        {

        }
        public PostBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Posts;
        }
    }
}

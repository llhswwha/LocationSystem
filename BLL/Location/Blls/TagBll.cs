using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL.Blls
{
    public class TagBll : BaseBll<Tag, LocationDb>
    {
        public TagBll():base()
        {

        }
        public TagBll(LocationDb db):base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.Tags;
        }
    }
}

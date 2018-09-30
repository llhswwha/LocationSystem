using Location.DAL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class TagPositionBll : BaseBll<TagPosition,LocationDb>
    {
        public TagPositionBll():base()
        {

        }
        public TagPositionBll(LocationDb db):base(db)
        {

        }
        protected override void InitDbSet()
        {
            DbSet = Db.TagPosition;
        }

        public TagPosition FindByCode(string code)
        {
            return DbSet.Find(code);
        }
    }
}

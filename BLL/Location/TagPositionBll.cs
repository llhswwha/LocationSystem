using Location.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.BLL
{
    public class TagPositionBll : BaseBll<TagPosition>
    {
        protected override void InitDbSet()
        {
            DbSet = Db.TagPosition;
        }
    }
}

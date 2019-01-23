using DAL;
using DbModel.Location.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class InspectionTrackBll : BaseBll<InspectionTrack, LocationDb>
    {
        public InspectionTrackBll():base()
        {

        }
        public InspectionTrackBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.InspectionTracks;
        }
    }
}

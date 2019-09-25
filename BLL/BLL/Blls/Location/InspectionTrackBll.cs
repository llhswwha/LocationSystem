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

        public override List<InspectionTrack> ToList(bool isTracking = false)
        {
            var list = base.ToList(isTracking);
            if(list!= null)
            {
                list.Sort((a, b) =>
                {
                    //var r1 = b.CreateTime.CompareTo(a.CreateTime);
                    //if (r1 == 0)
                    //{
                    return b.StartTime.CompareTo(a.StartTime);
                    //}
                    //return r1;
                });
            }
            else
            {
                list = new List<InspectionTrack>();
            }
            return list;
        }
    }
}

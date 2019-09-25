using DAL;
using DbModel.LocationHistory.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.LocationHistory
{
    public class InspectionTrackHistoryBll : BaseBll<InspectionTrackHistory, LocationHistoryDb>
    {
        public InspectionTrackHistoryBll() : base()
        {

        }
        public InspectionTrackHistoryBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.InspectionTrackHistorys;
        }

        public override List<InspectionTrackHistory> ToList(bool isTracking = false)
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
                list = new List<InspectionTrackHistory>();
            }
           
            return list;
        }
    }
}

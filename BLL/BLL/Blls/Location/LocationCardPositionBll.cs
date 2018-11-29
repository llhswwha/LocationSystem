using DAL;
using DbModel.Location.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class LocationCardPositionBll : BaseBll<LocationCardPosition, LocationDb>
    {
        public LocationCardPositionBll():base()
        {

        }
        public LocationCardPositionBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationCardPositions;
        }

        public LocationCardPosition FindByCode(string code)
        {
            return DbSet.Find(code);
        }

        public List<LocationCardPosition> GetListByTagCode(string name)
        {
            return DbSet.Where(i => i.Code.Contains(name)).ToList();
        }

        //public List<LocationCardPosition> GetListByPerson(int person)
        //{
        //    return DbSet.Where(i => i.PersonId==person).ToList();
        //}

        public List<LocationCardPosition> GetListByArea(int area)
        {
            return DbSet.Where(i => i.AreaId==area).ToList();
        }

        public override List<LocationCardPosition> ToList(bool isTracking = false)
        {
            var list= base.ToList(isTracking);
            //设置实时位置的移动状态
            foreach (var tag1 in list)
            {
                TimeSpan time = DateTime.Now - tag1.DateTime;
                if (time.TotalSeconds > 30)//30s
                {
                    if (tag1.Flag == "0:0:0:0:1")
                    {
                        tag1.MoveState = 1;
                        if (time.TotalSeconds > 300)//5m 长时间不动，在三维中显示为告警
                        {
                            tag1.MoveState = 3;
                        }
                        //else
                        //{
                        //    tag1.MoveState = 2;
                        //}
                    }
                    else
                    {
                        if (time.TotalSeconds > 50)//5m 长时间不动，在三维中显示为告警
                        {
                            tag1.MoveState = 3;
                            //tag1.AreaState = 1;//这里因为是运动突然消失，时间超过300秒，可能是人已经离开，或卡失去联系
                        }
                        else
                        {
                            tag1.MoveState = 2;
                        }  
                    }
                }
            }
            return list;
        }
    }
}

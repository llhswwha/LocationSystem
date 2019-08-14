using DAL;
using DbModel;
using DbModel.Location.Data;
using LocationServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class LocationCardPositionBll : BaseBll<LocationCardPosition, LocationDb,string>
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
            return DbSet.Where(i => i.Id.Contains(name)).ToList();
        }

        //public List<LocationCardPosition> GetListByPerson(int person)
        //{
        //    return DbSet.Where(i => i.PersonId==person).ToList();
        //}

        public List<LocationCardPosition> GetListByArea(int area)
        {
            return DbSet.Where(i => i.IsInArea(area)).ToList();
        }

        public override List<LocationCardPosition> ToList(bool isTracking = false)
        {
            var list= base.ToList(isTracking);
            //设置实时位置的移动状态
            if(list!=null)
                foreach (var tag1 in list)
                {
                    SetPostionState(tag1);
                }
            return list;
        }

        /// <summary>
        /// 设置位置信息的状态
        /// </summary>
        /// <param name="tag1"></param>
        public static void SetPostionState(LocationCardPosition tag1)
        {
            if (tag1 == null) return;
            TimeSpan time = DateTime.Now - tag1.DateTime;

            double timeT = AppContext.PositionMoveStateWaitTime;

            double offlineTime = AppContext.PositionMoveStateOfflineTime;
            tag1.MoveState = 0;

            //0:运动 4s内有数据且不是待机状态
            //1.待机 有标志位0:0:0:0:1且4s-5分钟内不动
            //2.静止 4s-5分钟内不动
            //3.离线 5分钟以上不动
            if (time.TotalSeconds > timeT)//4s多没有位置
            {
                if (tag1.Flag == "0:0:0:0:1")
                {
                    if (time.TotalSeconds > offlineTime)//5m 长时间不动，在三维中显示为告警
                    {
                        tag1.MoveState = 3;
                    }
                    else
                    {
                        tag1.MoveState = 1;//待机状态
                    }
                }
                else
                {
                    if (time.TotalSeconds > offlineTime)//5m 长时间不动，在三维中显示为告警
                    {
                        tag1.MoveState = 3;
                        //tag1.AreaState = 1;//这里因为是运动突然消失，时间超过300秒，可能是人已经离开，或卡失去联系
                    }
                    else
                    {
                        //tag1.MoveState = 2;
                        tag1.MoveState = 1;//如果没做到实时插入，这里totalSeconds会大于60s
                    }
                }
            }
            else
            {
                if (tag1.Flag == "0:0:0:0:1")//4s内的待机状态
                {
                    tag1.MoveState = 1;
                }
            }

            if (tag1.Power >= AppSetting.LowPowerFlag)
            {
                tag1.PowerState = 0;
            }
            else
            {
                tag1.PowerState = 1;
            }
        }

        public override bool AddRange(IList<LocationCardPosition> list, int maxTryCount = 3)
        {
            return base.AddRange(list, maxTryCount);
        }

        public override bool AddRange(LocationDb Db, IEnumerable<LocationCardPosition> list, int maxTryCount = 3)
        {
            return base.AddRange(Db, list, maxTryCount);
        }

        public override bool AddRange(params LocationCardPosition[] list)
        {
            return base.AddRange(list);
        }

        public override bool Add(LocationCardPosition item, bool isSave = true)
        {
            return base.Add(item, isSave);
        }

        public override bool AddOrUpdate(LocationCardPosition item, bool isSave = true)
        {
            return base.AddOrUpdate(item, isSave);
        }
    }
}

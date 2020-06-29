using Base.Common.Threads;
using BLL;
using DbModel.Location.Alarm;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.Location.Work;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using Location.BLL.Tool;
using LocationServices.Converters;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    /// <summary>
    /// 签到告警产生及处理
    /// </summary>
    public class  SignInAlarmThread : IntervalTimerThread
    {
        List<LocationAlarm> dbAlarmList;  //数据库数据；
        private Bll db;
        //List<LocationAlarm> cacheList;
        public SignInAlarmThread() :
            base(TimeSpan.FromMinutes(1)
                 , TimeSpan.FromSeconds(5))
        {
            db = Bll.NewBllNoRelation();
        }


        public override bool TickFunction()
        {    
            try
            {
                //每天清除进入区域记录
                DateTime now = DateTime.Now;
                if (now.Hour == 23)
                {
                    bool result = DeleteRecord();
                }

                //产生告警
                List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
                newAlarmList.AddRange(GetAlarmList());
                dbAlarmList = db.LocationAlarms.ToList();
                foreach (var item in dbAlarmList)
                {
                    newAlarmList.RemoveAll(i => i.PersonnelId == item.PersonnelId && i.AreaId == item.AreaId && i.AlarmType == item.AlarmType);
                }
                if (newAlarmList != null && newAlarmList.Count > 0)
                {
                    //dbAlarmList.AddRange(newAlarmList);
                    List<LocationAlarm> boolList = new List<LocationAlarm>();
                    foreach (LocationAlarm alarm in newAlarmList)
                    {
                        bool result=db.LocationAlarms.Add(alarm);
                        if(result)
                        {
                            boolList.Add(alarm);
                        }
                    }
                    //db.LocationAlarms.AddRange(newAlarmList);

                    AlarmHub.SendLocationAlarms(boolList.ToTModel().ToArray());
                }
                
            }
            catch (Exception ex)
            {
                Log.Error("SignInAlarmThread:"+ex.ToString());
            }

            return true;
        }
        private List<LocationAlarm> GetAlarmList()
        {
            List<LocationAlarm> list = new List<LocationAlarm>();
            try
            {
                List<Personnel> personnelList = db.Personnels.ToList();
                List<Area> areaList = db.Areas.ToList();
                List<AreaAuthorizationRecord> aarList = db.AreaAuthorizationRecords.FindAll(i => i.SignIn == true);
                foreach (Personnel person in personnelList)
                {
                    string strsql = string.Format(@"select a.* from cardroles a  inner join locationcards  b  on a.id=b.CardRoleId  inner join  locationcardtopersonnels c on b.id=c.LocationCardId  where c.PersonnelId=" + person.Id);
                     CardRole role = db.CardRoles.GetDataBySql<CardRole>(strsql);
                     LocationCardToPersonnel card = db.LocationCardToPersonnels.Find(i=>i.PersonnelId==person.Id);
                    if (role == null||card==null)
                    {
                        continue;
                    }
                    foreach (Area area in areaList)
                    {
                        AreaAuthorizationRecord aar = aarList.Find(i => i.AreaId == area.Id && i.CardRoleId == role.Id);
                        if (aar != null)
                        {
                            DateTime nowTime = DateTime.Now;
                            DateTime nowEndTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, aar.EndTime.Hour, aar.EndTime.Minute, aar.EndTime.Second);
                            DateTime nowStartTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, aar.StartTime.Hour, aar.StartTime.Minute, aar.StartTime.Second);
                            if (nowTime > nowEndTime)//可以判断是否产生告警
                            {
                                Position position = new Position();
                                position.CardId = card.LocationCardId;
                                position.RoleId = role.Id;
                                position.PersonnelID = person.Id;
                                PersonnelFirstInArea personArea = db.PersonnelFirstInAreas.Find(i => i.areaId == area.Id && i.personId == person.Id && i.type == 1);
                                if (personArea == null)//表示人员person当天未到过该区域
                                {
                                    //未签到告警
                                    LocationAlarm alarm1 = new LocationAlarm(position, area.Id, aar, person.Name, LocationAlarmLevel.四级告警, LocationAlarmType.签到告警);
                                    list.Add(alarm1);
                                }
                                else
                                {
                                    DateTime dateTime = personArea.dateTime;
                                    if (dateTime > nowStartTime && dateTime < nowEndTime)//时间范围内进入过
                                    {
                                        //正常告警
                                        LocationAlarm alarm1 = new LocationAlarm(position, area.Id, aar, person.Name, LocationAlarmLevel.正常, LocationAlarmType.签到告警);
                                        list.Add(alarm1);
                                    }
                                    else
                                    {
                                        //未签到告警
                                        LocationAlarm alarm1 = new LocationAlarm(position, area.Id, aar, person.Name, LocationAlarmLevel.四级告警, LocationAlarmType.签到告警);
                                        list.Add(alarm1);
                                    }

                                }
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error("SignInAlarmThread.GetCacheList:" + ex.ToString());
            }
            return list;
        }

        private bool DeleteRecord()
        {
            try
            {
                string strsql = string.Format(@"select Id from personnelfirstinareas where Type=1");
                List<int> list = db.PersonnelFirstInAreas.GetListIntBySql(strsql);
                if (list != null && list.Count > 0)
                {
                    PersonnelFirstInArea person=db.PersonnelFirstInAreas.DeleteByKeys(list);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("SignInAlarmThread.DeleteRecord:" + ex.ToString());
                return false;
            }
        }


        protected override void DoBeforeWhile()
        {
            throw new NotImplementedException();
        }
    }
}

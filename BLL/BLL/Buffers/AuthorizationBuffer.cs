using DbModel.Location.Authorizations;
using DbModel.Location.Work;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Location.Alarm;
using DbModel.LocationHistory.Alarm;
using DbModel.Tools;

namespace BLL.Buffers
{
    public class AuthorizationBuffer : BaseBuffer
    {
        Bll _bll;

        List<AreaAuthorizationRecord> aarList;
        List<CardRole> roles;
        private List<LocationAlarm> realAlarms;
        private List<LocationAlarmHistory> hisAlarms = new List<LocationAlarmHistory>();
        public AuthorizationBuffer(Bll bll)
        {
            _bll = bll;
        }

        protected override void UpdateData()
        {
            aarList = _bll.AreaAuthorizationRecords.ToList();
            roles = _bll.CardRoles.ToList();
            realAlarms = _bll.LocationAlarms.ToList();//定位实时告警信息
            //bll.LocationAlarms.AddRange(NewAlarms);

            lock (hisAlarms)
            {
                if (hisAlarms.Count > 0)
                {
                    _bll.LocationAlarmHistorys.AddRange(hisAlarms);
                    hisAlarms.Clear();
                }
            }
        }

        public List<LocationAlarm> GetNewAlarms(List<Position> list1)
        {
            var alarms = GetAlarms(list1);
            var updateAlarms=new List<LocationAlarm>();//修改的告警
            var addedAlarms = new List<LocationAlarm>();//新增的告警
            var removeAlarms = new List<LocationAlarm>();//恢复的告警
            removeAlarms.AddRange(realAlarms);
            var noChangeAlarms = new List<LocationAlarm>();//没有变化的告警
            foreach (var alarm in alarms)
            {
                var realAlarm = realAlarms.Find(i => i.AuzId==alarm.AuzId && i.LocationCardId == alarm.LocationCardId);
                //某张卡基于某个规则产生的告警
                if (realAlarm == null)
                {
                    addedAlarms.Add(alarm);
                }
                else
                {
                    removeAlarms.Remove(realAlarm);
                    var id1 = realAlarm.GetAlarmId();
                    var id2 = alarm.GetAlarmId();
                    if (id1 == id2)
                    {
                        noChangeAlarms.Add(realAlarm);
                    }
                    else
                    {
                        realAlarm.Update(alarm);
                        updateAlarms.Add(realAlarm);
                    }
                }
            }
            var newAlarms = new List<LocationAlarm>();
            newAlarms.AddRange(addedAlarms);
            newAlarms.AddRange(updateAlarms);
            //newAlarms.AddRange(removeAlarms);
            //foreach (var alarm in removeAlarms)
            //{
            //    alarm.AlarmLevel = LocationAlarmLevel.正常;
            //}

            _bll.LocationAlarms.AddRange(addedAlarms);
            realAlarms.AddRange(addedAlarms);

            _bll.LocationAlarms.EditRange(updateAlarms);
            //_bll.LocationAlarms.EditRange(removeAlarms);
            //_bll.LocationAlarms.RemoveList(removeAlarms);

            foreach (var alarm in newAlarms)
            {
                hisAlarms.Add(alarm.RemoveToHistory());
            }
            return newAlarms;
        }

        List<LocationAlarm> alarms= new List<LocationAlarm>();
        Dictionary<Position, List<LocationAlarm>> posAlarms = new Dictionary<Position, List<LocationAlarm>>();
        List<Position> noAlarmPos = new List<Position>();

        private LocationAlarm AddAlarm(Position p, AreaAuthorizationRecord arr, string content, LocationAlarmLevel level)
        {
            LocationAlarm alarm = new LocationAlarm(p, arr, content, level);
            alarms.Add(alarm);
            return alarm;
        }
        public List<LocationAlarm> GetAlarms(List<Position> list1)
        {
            alarms = new List<LocationAlarm>();
            posAlarms = new Dictionary<Position, List<LocationAlarm>>();
            noAlarmPos = new List<Position>();

            LoadData();
            foreach (Position p in list1)
            {
                if (p == null) continue;
                List<LocationAlarm> posAlarm = new List<LocationAlarm>();
                posAlarms[p] = posAlarm;
                CardRole role = roles.Find(i => i.Id == p.RoleId);
                if (role != null)
                {
                    //var aarList2 = aarList.FindAll(i => i.CardRoleId == role.Id);
                    //var aarList3 = aarList2.FindAll(i => i.AreaId == p.AreaId);
                    
                    var aarList4 = aarList.FindAll(i => i.AreaId == p.AreaId && i.CardRoleId == role.Id);
                    if (aarList4.Count > 0)
                    {
                        foreach (var arr in aarList4)
                        {
                            if (arr.AccessType == AreaAccessType.Enter)
                            {
                                if (arr.IsTimeValid(p.DateTime)==false)
                                {
                                    posAlarm.Add(AddAlarm(p, arr, "未在有效时间范围内", LocationAlarmLevel.四级告警));
                                }
                                else
                                {
                                    posAlarm.Add(AddAlarm(p, arr, "正常", LocationAlarmLevel.正常));
                                    noAlarmPos.Add(p);
                                }
                            }
                            else if (arr.AccessType == AreaAccessType.EnterLeave)
                            {
                                if (arr.IsTimeValid(p.DateTime) == false)
                                {
                                    posAlarm.Add(AddAlarm(p, arr, "未在有效时间范围内", LocationAlarmLevel.四级告警));
                                }
                                else
                                {
                                    posAlarm.Add(AddAlarm(p, arr, "正常", LocationAlarmLevel.正常));
                                    noAlarmPos.Add(p);
                                }
                            }
                            else if (arr.AccessType == AreaAccessType.Leave)
                            {
                                posAlarm.Add(AddAlarm(p, arr, "进入无权限的区域，必须离开", LocationAlarmLevel.三级告警));
                            }
                            else if (arr.AccessType == AreaAccessType.None)
                            {
                                posAlarm.Add(AddAlarm(p, arr, "进入无权限的区域", LocationAlarmLevel.三级告警));
                            }
                            else
                            {
                                posAlarm.Add(AddAlarm(p, arr, "正常", LocationAlarmLevel.正常));
                                noAlarmPos.Add(p);
                            }
                        }
                    }
                    else
                    {
                        posAlarm.Add(AddAlarm(p, null, "标签所在区域未配置权限", LocationAlarmLevel.四级告警));
                    }
                }
                else
                {
                    posAlarm.Add(AddAlarm(p, null, "标签未配置区域权限", LocationAlarmLevel.四级告警));
                }
                //1.找出区域相关的所有权限
                //2.判断当前定位卡是否有权限进入该区域
                //  2.1找的卡所在的标签角色
                //  2.2判断该组是否是在权限内
                //  2.3不在则发出警告，进入非法区域
                //  2.4默认标签角色CardRole 1.超级管理员、巡检人员、管理人员、施工人员、参观人员
                //p.AreaId
            }
            return alarms;
        }
    }
}

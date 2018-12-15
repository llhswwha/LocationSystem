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
            UpdateInterval = 10;
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
            var newAlarms = GetAlarms(list1);//根据位置信息产生告警，包括正常的。
            //var updateAlarms=new List<LocationAlarm>();//修改的告警
            var addedAlarms = new List<LocationAlarm>();//要新增的告警
            var removeAlarms = new List<LocationAlarm>();//要删除的告警，移动到历史表中
            var noChangeAlarms = new List<LocationAlarm>();//没有变化的告警
            foreach (var newAlarm in newAlarms)
            {
                var newAlarmId = newAlarm.GetAlarmId();

                var tagAlarms = realAlarms.FindAll(i => i.LocationCardId == newAlarm.LocationCardId);
                //某张卡基于某个规则产生的告警，可能有多个
                if (tagAlarms == null|| tagAlarms.Count==0)
                {
                    addedAlarms.Add(newAlarm);//新增加一条告警
                }
                //else if (tagAlarms.Count == 1)
                //{

                //}
                else
                {
                    foreach (var tagAlarm in tagAlarms)
                    {
                        var alarmId = tagAlarm.GetAlarmId();
                        if (alarmId == newAlarmId)//相同则不变
                        {
                            noChangeAlarms.Add(tagAlarm);
                        }
                        else
                        {
                            //realAlarm.Update(alarm);
                            //updateAlarms.Add(realAlarm);
                            if (tagAlarm.AreadId != newAlarm.AreadId)//在相同区域，不同告警规则产生的告警不用删除
                            {
                                removeAlarms.Add(tagAlarm);//删除不同的告警
                            }
                            if (!addedAlarms.Contains(newAlarm))//可能重复添加
                            {
                                addedAlarms.Add(newAlarm);//
                            }
                        }
                    }
                }
            }
            //var newAlarms = new List<LocationAlarm>();
            //newAlarms.AddRange(addedAlarms);
            //newAlarms.AddRange(updateAlarms);
            //newAlarms.AddRange(removeAlarms);
            //foreach (var alarm in removeAlarms)
            //{
            //    alarm.AlarmLevel = LocationAlarmLevel.正常;
            //}

            _bll.LocationAlarms.AddRange(addedAlarms);
            _bll.LocationAlarms.RemoveList(removeAlarms);
            foreach (var alarm in removeAlarms)//删除掉的告警
            {
                hisAlarms.Add(alarm.RemoveToHistory());
                realAlarms.Remove(alarm);
            }
            foreach (var alarm in addedAlarms)//新增加的告警
            {
                hisAlarms.Add(alarm.RemoveToHistory());
                realAlarms.Add(alarm);
            }
            if (newAlarms.Count > 0)
            {
                Console.WriteLine("newAlarms.Count > 0");
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
                if (p.AreaId == null || p.AreaId == 0)
                {
                    AddAlarm(p, null, "不在园区有效区域内", LocationAlarmLevel.四级告警);
                    continue;
                }
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
                                posAlarm.Add(AddAlarm(p, arr, string.Format("进入无权限的区域'{0}'，必须离开",p.Area), LocationAlarmLevel.三级告警));
                            }
                            else if (arr.AccessType == AreaAccessType.None)
                            {
                                posAlarm.Add(AddAlarm(p, arr, string.Format("进入无权限的区域'{0}'", p.Area), LocationAlarmLevel.三级告警));
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
                        posAlarm.Add(AddAlarm(p, null, string.Format("标签角色'{0}'在区域'{1}'未配置权限", role, p.Area), LocationAlarmLevel.四级告警));
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

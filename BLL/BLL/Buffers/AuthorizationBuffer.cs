using DbModel.Location.Authorizations;
using DbModel.Location.Work;
using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Location.Alarm;

namespace BLL.Buffers
{
    public class AuthorizationBuffer : BaseBuffer
    {
        Bll _bll;

        List<AreaAuthorizationRecord> aarList;
        List<CardRole> roles;
        public AuthorizationBuffer(Bll bll)
        {
            _bll = bll;
        }

        protected override void UpdateData()
        {
            aarList = _bll.AreaAuthorizationRecords.ToList();
            roles = _bll.CardRoles.ToList();
            
        }

        public List<LocationAlarm> GetAlarms(List<Position> list1)
        {
            var alarms = new List<LocationAlarm>();
            LoadData();
            foreach (Position p in list1)
            {
                if (p == null) continue;
                CardRole role = roles.Find(i => i.Id == p.RoleId);
                if (role != null)
                {
                    //var aarList2 = aarList.FindAll(i => i.CardRoleId == role.Id);
                    //var aarList3 = aarList2.FindAll(i => i.AreaId == p.AreaId);
                    
                    var aarList4 = aarList.FindAll(i => i.AreaId == p.AreaId && i.CardRoleId == role.Id);
                    if (aarList4.Count > 0)
                    {
                        foreach (var arr in aarList)
                        {
                            if (arr.AccessType == AreaAccessType.Enter)
                            {
                                if (arr.IsTimeValid(p.DateTime)==false)
                                {
                                    LocationAlarm alarm = new LocationAlarm(p, "未在有效时间范围内");
                                    alarms.Add(alarm);
                                }
                            }
                            else if (arr.AccessType == AreaAccessType.EnterLeave)
                            {
                                if (arr.IsTimeValid(p.DateTime) == false)
                                {
                                    LocationAlarm alarm = new LocationAlarm(p, "未在有效时间范围内");
                                    alarms.Add(alarm);
                                }
                            }
                            else if (arr.AccessType == AreaAccessType.Leave)
                            {
                                LocationAlarm alarm = new LocationAlarm(p, "进入无权限的区域，必须离开");
                                alarms.Add(alarm);
                            }
                            else if (arr.AccessType == AreaAccessType.None)
                            {
                                LocationAlarm alarm = new LocationAlarm(p, "进入无权限的区域");
                                alarms.Add(alarm);
                            }
                        }
                    }
                    else
                    {
                        LocationAlarm alarm=new LocationAlarm(p,"不存在可进入的区域");
                        alarms.Add(alarm);
                    }
                }
                else
                {
                    LocationAlarm alarm = new LocationAlarm(p, "未配置区域权限");
                    alarms.Add(alarm);
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

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
using Location.BLL.Tool;

namespace BLL.Buffers
{
    public class AuthorizationBuffer : BaseBuffer
    {
        Bll _bll;

        public static AuthorizationBuffer Single = null;

        List<AreaAuthorizationRecord> aarList;
        List<CardRole> roles;
        private List<LocationAlarm> realAlarms;
        private List<LocationAlarmHistory> hisAlarms = new List<LocationAlarmHistory>();

        public static AuthorizationBuffer Instance(Bll bll)
        {
            if (Single == null)
            {
                Single = new AuthorizationBuffer(bll);
            }

            return Single;
        }

        private AuthorizationBuffer(Bll bll)
        {
            if (Single == null)
            {
                Single = this;
            }

            _bll = bll;
            //UpdateInterval = 10;
        }

        public void PubUpdateData()
        {
            UpdateData();
        }

        protected override void UpdateData()
        {
            //lock (realAlarms)
            {
                aarList = _bll.AreaAuthorizationRecords.ToList();
                roles = _bll.CardRoles.ToList();
                realAlarms = _bll.LocationAlarms.ToList();//定位实时告警信息
            }

            //bll.LocationAlarms.AddRange(NewAlarms);

            //SaveHisAlarms();
        }

        private void SaveHisAlarms()
        {
            lock (hisAlarms)
            {
                if (hisAlarms.Count > 0)
                {
                    //_bll.LocationAlarmHistorys.AddRange(hisAlarms);
                    foreach (var item in hisAlarms)
                    {
                        bool r=_bll.LocationAlarmHistorys.Add(item);
                    }
                    hisAlarms.Clear();
                }
            }
        }

        public List<LocationAlarm> GetNewAlarms(List<Position> list1, int nn)
        {
            var newAlarms = GetAlarms(list1);//根据位置信息产生告警，包括正常的。
            //var updateAlarms=new List<LocationAlarm>();//修改的告警

            //var ps1 = newAlarms.FindAll(i => i.LocationCardId==19);
            //var ps2 = realAlarms.FindAll(i => i.LocationCardId == 19);


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
                    //if (tagAlarms.Count > 1)
                    //{

                    //}
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
                            //if (tagAlarm.AreadId != newAlarm.AreadId||tagAlarm.AlarmLevel!=newAlarm.AlarmLevel)
                                //1.在相同区域，不同告警规则产生的告警不用删除
                                //2.正常->告警，告警->正常
                            {
                                if (!removeAlarms.Contains(tagAlarm))
                                    removeAlarms.Add(tagAlarm);//删除不同的告警
                            }
                            //else
                            //{

                            //}

                            if (!addedAlarms.Contains(newAlarm))//可能重复添加
                            {
                                addedAlarms.Add(newAlarm);//
                                if (addedAlarms.Count > 1)
                                {

                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            //if (addedAlarms.Count > 1)
            //{

            //}

            foreach (var alarm in removeAlarms)//删除掉的告警
            {
                //if(alarm.AlarmLevel!=LocationAlarmLevel.正常)
                hisAlarms.Add(alarm.RemoveToHistory());
                realAlarms.Remove(alarm);
            }
            foreach (var alarm in addedAlarms)//新增加的告警
            {
                //hisAlarms.Add(alarm.RemoveToHistory());
                realAlarms.Add(alarm);
            }
            if (newAlarms.Count > 0)
            {
                Log.Info("newAlarms.Count > 0");
            }

            bool result1=_bll.LocationAlarms.RemoveList(removeAlarms);
            if (result1 == false)
            {

            }
            _bll.LocationAlarms.AddRange(addedAlarms);

            var realAlarms2 = _bll.LocationAlarms.ToList();//定位实时告警信息
            if (realAlarms2.Count > 1)
            {

            }

            SaveHisAlarms();
            return addedAlarms;
        }

        List<LocationAlarm> alarms= new List<LocationAlarm>();
        Dictionary<Position, List<LocationAlarm>> posAlarms = new Dictionary<Position, List<LocationAlarm>>();
        //List<Position> noAlarmPos = new List<Position>();

        private LocationAlarm AddAlarm(Position p,int area, AreaAuthorizationRecord arr, string content, LocationAlarmLevel level)
        {
            LocationAlarm alarm = new LocationAlarm(p, area, arr, content, level);
            //alarms.Add(alarm);
            return alarm;
        }
        public List<LocationAlarm> GetAlarms(List<Position> list1)
        {
            alarms = new List<LocationAlarm>();
            posAlarms = new Dictionary<Position, List<LocationAlarm>>();
            

            LoadData();
            foreach (Position p in list1)
            {
                if (p == null) continue;
                if (p.IsAreaNull())
                {
                    AddAlarm(p,0, null, "不在园区有效区域内。", LocationAlarmLevel.四级告警);
                    continue;
                }

                List<LocationAlarm> posAlarmState = new List<LocationAlarm>();//告警状态
                List<LocationAlarm> posNormalState = new List<LocationAlarm>();//正常状态
                List<Position> noAlarmPos = new List<Position>();
                
                CardRole role = roles.Find(i => i.Id == p.RoleId);
                if (role != null)
                {
                    foreach (var area in p.Areas)
                    {
                        AreaAuthorizationRecord aar = aarList.Find(i => i.AreaId == area.Id && i.CardRoleId == role.Id);
                        if (aar != null)
                        {
                            LocationAlarm la = realAlarms.Find(j => j.LocationCardId == p.CardId && j.AreaId == area.Id);
                            DateTime dtBegin = p.DateTime;
                            DateTime dtEnd = p.DateTime;
                            if (la != null)
                            {
                                dtBegin = la.AlarmTime;
                            }

                            TimeSpan ts = dtEnd.Subtract(dtBegin).Duration();
                            int nTimeStamp = Convert.ToInt32(ts.TotalMinutes);

                            if (aar.AccessType == AreaAccessType.不能进入)
                            {
                                posAlarmState.Add(AddAlarm(p, area.Id, null, string.Format("标签角色'{0}'在区域'{1}'未配置权限。", role, area), LocationAlarmLevel.四级告警));
                            }
                            else
                            {
                                //if (aar.IsTimeValid(dtBegin, dtEnd, nTimeStamp) == false)
                                //{
                                //    posAlarmState.Add(AddAlarm(p, area.Id, aar, string.Format("可以进入区域'{0}',但是未在有效时间范围内。", area), LocationAlarmLevel.四级告警));
                                //}
                                //else
                                //{
                                //    posNormalState.Add(AddAlarm(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常));
                                //    noAlarmPos.Add(p);
                                //}
                            }
                        }
                        else
                        {
                            if (role.Id == 1)//超级管理员
                            {
                                posNormalState.Add(AddAlarm(p, area.Id, null, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常));
                                noAlarmPos.Add(p);
                            }
                            else
                            {
                                posAlarmState.Add(AddAlarm(p, area.Id, null, string.Format("标签角色'{0}'在区域'{1}'未配置权限。", role, area), LocationAlarmLevel.四级告警));
                            }
                        }
                    }
                }
                else
                {
                    posAlarmState.Add(AddAlarm(p, 0, null, "标签未配置区域权限。", LocationAlarmLevel.四级告警));
                }

                //1.找出区域相关的所有权限
                //2.判断当前定位卡是否有权限进入该区域
                //  2.1找的卡所在的标签角色
                //  2.2判断该组是否是在权限内
                //  2.3不在则发出警告，进入非法区域
                //  2.4默认标签角色CardRole 1.超级管理员、巡检人员、管理人员、施工人员、参观人员
                //p.AreaId

                if (posAlarmState.Count > 0&& posNormalState.Count > 0)//存在告警状态和正常状态，则去掉正常状态
                {
                    posNormalState.Clear();
                }

                List<LocationAlarm> posAlarm0 = new List<LocationAlarm>();
                foreach (var item in posAlarmState)
                {
                    alarms.Add(item);
                    posAlarm0.Add(item);
                }

                if (posNormalState.Count > 0)//在一个区域内的多个规则的正常告警（状态）合并成一个
                {
                    if (posNormalState.Count > 1)
                    {
                        for (int i = 1; i < posNormalState.Count; i++)
                        {
                            posNormalState[0].AllAuzId += ";" + posNormalState[i].AuzId;
                        }
                    }
                    alarms.Add(posNormalState[0]);
                    posAlarm0.Add(posNormalState[0]);
                }
                posAlarms[p] = posAlarm0;
            }
            if (alarms.Count > 1)
            {

            }
            return alarms;
        }


        /// <summary>
        /// 获取新告警
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        public List<LocationAlarm> GetNewAlarms(List<Position> list1)
        {
            List<LocationAlarm> newAlarmList = GenerateAlarm(list1);
            List<LocationAlarm> ReviseAlarmList = new List<LocationAlarm>();
            List<LocationAlarm> DeleteList = new List<LocationAlarm>();

            foreach (LocationAlarm item in realAlarms)
            {
                if (item.AlarmLevel == LocationAlarmLevel.正常)
                {
                    //当该卡片在数据库中是正常告警时，出现异常告警则上报，并将原本的正常记录转移到历史数据；出现正常告警则忽略

                    List<LocationAlarm> item2 = newAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel != LocationAlarmLevel.正常);
                    if (item2.Count() > 0)
                    {
                        DeleteList.Add(item);
                        hisAlarms.Add(item.RemoveToHistory());
                    }

                    int nCount = newAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常).Count();
                    if (nCount > 0)
                    {
                        newAlarmList.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常);
                    }
                }
                else
                {
                    LocationAlarm ReviseAlarm = item.Copy();
                    ReviseAlarm.AlarmLevel = LocationAlarmLevel.正常;

                    //当该卡片在数据库中在指定区域是异常告警时,出现正常告警或没有该区域的异常告警，则告警恢复；出现该区域的异常告警，则忽略
                    
                    LocationAlarm item3 = newAlarmList.Find(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常);
                    if (item3 != null)
                    {
                        //if (item3.PersonnelId == 246)
                        //{
                        //    Location.BLL.Tool.Log.InfoStart("告警恢复，告警Id" + Convert.ToInt32(item3.Id));
                        //}

                        ReviseAlarmList.Add(ReviseAlarm);
                        DeleteList.Add(item);
                        hisAlarms.Add(item.RemoveToHistory());
                    }
                    else
                    {
                        if (newAlarmList.Count() == 0)
                        {
                            continue;
                        }
                        
                        int nCount = newAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId).Count();
                        if (nCount >= 1)
                        {
                            newAlarmList.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId);
                        }
                        //if (nCount == 0)
                        //{
                        //    ReviseAlarmList.Add(ReviseAlarm);
                        //    DeleteList.Add(item);
                        //    hisAlarms.Add(item.RemoveToHistory());
                        //}
                        //else
                        //{
                        //    newAlarmList.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId);
                        //}
                    }




                    //int nCount = newAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId).Count();
                    //if (item3 != null || (item3 == null && nCount == 0))
                    //{
                    //    ReviseAlarmList.Add(ReviseAlarm);
                    //    DeleteList.Add(item);
                    //    hisAlarms.Add(item.RemoveToHistory());
                    //}
                    //else if(nCount > 0)
                    //{
                    //    LocationAlarm item4 = newAlarmList.Find(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId);
                    //    newAlarmList.Remove(item4);
                    //}

                    




                    //LocationAlarm item4 = newAlarmList.Find(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId && p.AlarmLevel != LocationAlarmLevel.正常);
                    //if (nCount == 0)
                    //{
                    //    ReviseAlarmList.Add(ReviseAlarm);
                    //    DeleteList.Add(item);
                    //    hisAlarms.Add(item.RemoveToHistory());
                    //}
                    //else
                    //{
                    //    if (item4 == null)
                    //    {
                    //        ReviseAlarmList.Add(ReviseAlarm);
                    //        DeleteList.Add(item);
                    //        hisAlarms.Add(item.RemoveToHistory());
                    //    }
                    //    else
                    //    {
                    //        newAlarmList.Remove(item4);
                    //    }
                    //}

                    //if (nCount > 0 && item4 == null)
                    //{
                    //    ReviseAlarmList.Add(ReviseAlarm);
                    //    DeleteList.Add(item);
                    //    hisAlarms.Add(item.RemoveToHistory());
                    //}
                    //else if(nCount > 0 && item4 != null)
                    //{
                    //    newAlarmList.Remove(item4);
                    //}
                }
            }

            if (newAlarmList.Count() > 0)
            {
                //向LocationAlarm表添加数据
                _bll.LocationAlarms.AddRange(newAlarmList);
                realAlarms.AddRange(newAlarmList);
            }

            if (DeleteList.Count() > 0)
            {
                //删除恢复正常的告警
                _bll.LocationAlarms.RemoveList(DeleteList);
                foreach (LocationAlarm item in DeleteList)
                {
                    realAlarms.Remove(item);
                }
            }

            //将恢复正常的告警插入历史表
            SaveHisAlarms();

            if (ReviseAlarmList.Count() > 0)
            {    
                //告警恢复，把id相同、告警类型相同的告警去掉           
                foreach (var alarmT in ReviseAlarmList)
                {
                    if (alarmT == null) continue;
                    LocationAlarm alarm = newAlarmList.Find(i => i != null && i.PersonnelId == alarmT.PersonnelId&&i.AlarmLevel == alarmT.AlarmLevel);
                    if(alarm!=null)
                    {
                        newAlarmList.Remove(alarm);
                    }
                }
                newAlarmList.AddRange(ReviseAlarmList);
            }


            if (newAlarmList.Count > 0)
            {
                int nn = 0;
            }

            return newAlarmList;
        }

        public static string tag = "AuthorizationBuffer";

        /// <summary>
        /// 产生告警
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        private List<LocationAlarm> GenerateAlarm(List<Position> list1)
        {
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
            if (list1 == null) {
                Log.Error(tag, "GenerateAlarm", "list1 == null");
                return newAlarmList;
            }
            

            LoadData();

            foreach (Position p in list1)
            {
                if (p== null || p.IsAreaNull() || p.PersonnelID == null || p.AreaId == null)
                {
                    continue;
                }

                CardRole role = roles.Find(i => i.Id == p.RoleId);
                if (role == null)
                {
                    int pAreaId = (int)p.AreaId;
                    //RemoveDuplicateAlarms(p, pAreaId, null, "标签未配置区域权限。", LocationAlarmLevel.四级告警, ref newAlarmList);
                    continue;
                }

                foreach (var area in p.Areas)
                {
                    AreaAuthorizationRecord aar = aarList.Find(i => i.AreaId == area.Id && i.CardRoleId == role.Id);
                    if (aar != null)
                    {
                        LocationAlarm la = realAlarms.Find(j => j.LocationCardId == p.CardId && j.AreaId == area.Id);
                        DateTime dtBegin = p.DateTime;
                        DateTime dtEnd = p.DateTime;
                        if (la != null)
                        {
                            dtBegin = la.AlarmTime;
                        }

                        TimeSpan ts = dtEnd.Subtract(dtBegin).Duration();
                        int nTimeStamp = Convert.ToInt32(ts.TotalMinutes);

                        if (aar.AccessType == AreaAccessType.不能进入)
                        {
                            RemoveDuplicateAlarms(p, area.Id, null, string.Format("标签角色'{0}'在区域'{1}'未配置权限。", role, area), LocationAlarmLevel.四级告警, ref newAlarmList);
                        }
                        else
                        {
                            //if (aar.IsTimeValid(dtBegin, dtEnd, nTimeStamp) == false)
                            //{
                            //    RemoveDuplicateAlarms(p, area.Id, aar, string.Format("可以进入区域'{0}',但是未在有效时间范围内。", area), LocationAlarmLevel.四级告警, ref newAlarmList);
                            //}
                            //else
                            {
                                RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList);
                            }
                        }
                    }
                    else
                    {
                        if (role.Id == 1)//超级管理员
                        {
                            RemoveDuplicateAlarms(p, area.Id, null, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList);
                        }
                        else
                        {
                            RemoveDuplicateAlarms(p, area.Id, null, string.Format("标签角色'{0}'在区域'{1}'未配置权限。", role, area), LocationAlarmLevel.四级告警, ref newAlarmList);
                        }
                    }
                }
                
            }

            return newAlarmList;
        }

        /// <summary>
        /// 功能：1、去除重复的正常告警，2、当卡片有正常告警和异常告警时，去除正常告警
        /// </summary>
        /// <param name="p"></param>
        /// <param name="area"></param>
        /// <param name="arr"></param>
        /// <param name="content"></param>
        /// <param name="level"></param>
        /// /// <param name="newAlarmList"></param>
        private void RemoveDuplicateAlarms(Position p, int area, AreaAuthorizationRecord arr, string content, LocationAlarmLevel level, ref List<LocationAlarm> newAlarmList)
        {
          
            if (level == LocationAlarmLevel.正常)
            {
                //如果缓存中 没有正常告警，添加正常告警；如果缓存中有正常告警，添加正常告警规则Id
                int nCount = newAlarmList.FindAll(i => i.LocationCardId == p.CardId && i.AlarmLevel != LocationAlarmLevel.正常).Count();
                LocationAlarm alarm = newAlarmList.Find(i => i.LocationCardId == p.CardId && i.AlarmLevel == LocationAlarmLevel.正常);
                if (nCount == 0 && alarm == null)
                {
                    alarm = AddAlarm(p, area, arr, content, level);
                    newAlarmList.Add(alarm);
                }
                else if(nCount == 0 && alarm != null)
                {
                    if (arr != null)
                    {
                        alarm.AllAuzId += ";" + arr.Id;
                    }
                    else
                    {
                        Log.Error("AuthorizationBuffer.RemoveDuplicateAlarms", "arr==null");
                    }
                }
            }
            else
            {
                //如果缓存中有正常告警，去除正常告警，再添加异常告警
                int nCount = newAlarmList.FindAll(i => i.LocationCardId == p.CardId && i.AlarmLevel == LocationAlarmLevel.正常).Count();
                if (nCount > 0)
                {
                    newAlarmList.RemoveAll(i => i.LocationCardId == p.CardId && i.AlarmLevel == LocationAlarmLevel.正常);
                }

                LocationAlarm alarm = AddAlarm(p, area, arr, content, level);
                newAlarmList.Add(alarm);
            }

            return;
        }
        
    }
}

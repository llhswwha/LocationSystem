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
using LocationServer;
using Location.TModel.Tools;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;

namespace BLL.Buffers
{
    public class AuthorizationBuffer : BaseBuffer
    {
        //Bll _bll;

        public static AuthorizationBuffer Single = null;

        List<AreaAuthorizationRecord> aarList;
        List<CardRole> roles;
        private List<LocationAlarm> realAlarms;
        private Dictionary<int,Personnel> personDic;
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

            //_bll = bll;
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
                using (var _bll = Bll.NewBllNoRelation())
                {
                    aarList = _bll.AreaAuthorizationRecords.ToList();
                    roles = _bll.CardRoles.ToList();
                    realAlarms = _bll.LocationAlarms.ToList();//定位实时告警信息
                    personDic = _bll.Personnels.ToDictionary();
                }
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
                        using (var _bll = Bll.NewBllNoRelation())
                        {
                            bool r = _bll.LocationAlarmHistorys.Add(item);
                        }
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

            using (var _bll = Bll.NewBllNoRelation())
            {
                bool result1 = _bll.LocationAlarms.RemoveList(removeAlarms);
                if (result1 == false)
                {

                }
                _bll.LocationAlarms.AddRange(addedAlarms);

                var realAlarms2 = _bll.LocationAlarms.ToList();//定位实时告警信息
                if (realAlarms2.Count > 1)
                {

                }
            }

            SaveHisAlarms();
            return addedAlarms;
        }

        List<LocationAlarm> alarms= new List<LocationAlarm>();
        Dictionary<Position, List<LocationAlarm>> posAlarms = new Dictionary<Position, List<LocationAlarm>>();
        //List<Position> noAlarmPos = new List<Position>();

        private LocationAlarm AddAlarm(Position p,int area, AreaAuthorizationRecord arr, string content, LocationAlarmLevel level, LocationAlarmType alarmType = LocationAlarmType.区域告警)
        {
            LocationAlarm alarm = new LocationAlarm(p, area, arr, content, level,alarmType);
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
                        string personDepartment = GetPersonDepartInfo(p.PersonnelID, p.Code);
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
                                //string.Format("标签角色'{0}'在区域'{1}'未配置权限。", role, area)
                                posAlarmState.Add(AddAlarm(p, area.Id, null, string.Format("人员：{0}，在区域'{1}'未配置权限。", personDepartment, area), LocationAlarmLevel.四级告警));
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
                                posAlarmState.Add(AddAlarm(p, area.Id, null, string.Format("人员：{0}，在区域'{1}'未配置权限。", personDepartment, area), LocationAlarmLevel.四级告警));
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

        private LocationAlarm CreatePowerAlarm(Position position)
        {
            LocationAlarm alarm = new LocationAlarm();
            alarm.AlarmType = LocationAlarmType.低电告警;
            alarm.AlarmLevel = LocationAlarmLevel.四级告警;
            alarm.PersonnelId = position.PersonnelID;
            alarm.LocationCardId = position.CardId;
            //alarm.Content = "低电告警:"+position.Code;
            alarm.Content = "低电告警:" + GetPersonDepartInfo(position.PersonnelID, position.Code);
            return alarm;
        }

        /// <summary>
        /// 产生低电告警
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="onAlarmGenerated"></param>
        private void GeneratePowerAlarm(List<Position> list1,Action<List<LocationAlarm>,List<LocationAlarm>>onAlarmGenerated)
        {
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
            List<LocationAlarm> normalAlarmList = new List<LocationAlarm>();
            if (list1 == null)
            {
                Log.Error(tag, "GenerateAreaAlarm", "list1 == null");
                if (onAlarmGenerated != null) onAlarmGenerated(newAlarmList,normalAlarmList);
            }
            else
            {
                foreach (Position position in list1)
                {
                    var alarm = CreatePowerAlarm(position);
                    if(position.PowerState==0)
                    {
                        alarm.AlarmLevel = LocationAlarmLevel.正常;
                        normalAlarmList.Add(alarm);
                    }
                    else
                    {                       
                        newAlarmList.Add(alarm);
                    }
                }
                if (onAlarmGenerated != null) onAlarmGenerated(newAlarmList,normalAlarmList);
            }                       
        }

        /// <summary>
        /// 产生SOS告警
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        private List<LocationAlarm> GenerateSOSAlarm(List<Position> list1)
        {
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
            if (list1 == null)
            {
                Log.Error(tag, "GenerateEventAlarm", "list1 == null");
                return newAlarmList;
            }

            foreach (Position position in list1)
            {
                if (position.EventType == 1)
                {
                    LocationAlarm alarm = new LocationAlarm();
                    alarm.AlarmType = LocationAlarmType.求救信号;
                    alarm.AlarmLevel = LocationAlarmLevel.一级告警;
                    alarm.PersonnelId = position.PersonnelID;
                    alarm.LocationCardId = position.CardId;
                    //alarm.Content = "求救信号:" + position.Code;
                    alarm.Content = "求救信号:" + GetPersonDepartInfo(position.PersonnelID, position.Code);
                    newAlarmList.Add(alarm);
                }
            }

            return newAlarmList;
        }

        /// <summary>
        /// 获取晕倒告警
        /// </summary>
        /// <param name="nFaintScope"></param>
        /// <param name="nFaintTime"></param>
        /// <returns></returns>

        public List<LocationAlarm> GetFaintAlarm(string nFaintScope, int nFaintTime)
        {
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
            List<LocationAlarm> eventAlarmList = new List<LocationAlarm>();
            List<LocationAlarm> addList = new List<LocationAlarm>();
            List<LocationAlarm> deleteList = new List<LocationAlarm>();
            List<LocationAlarm> reviseList = new List<LocationAlarm>();

            try
            {
                using (var _bll = Bll.NewBllNoRelation())
                {
                    if (realAlarms == null)
                    {
                        realAlarms = _bll.LocationAlarms.ToList();//定位实时告警信息
                    }

                    eventAlarmList = GenerateFaintAlarm(nFaintScope, nFaintTime);
                    foreach (LocationAlarm item in realAlarms)
                    {
                        if (item.AlarmType != LocationAlarmType.晕倒告警)
                        {
                            continue;
                        }

                        if (item.AlarmLevel == LocationAlarmLevel.正常)
                        {
                            //当该卡片在数据库中是正常告警时，出现异常告警则上报，并将原本的正常记录转移到历史数据；出现正常告警则忽略

                            List<LocationAlarm> item2 = eventAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel != LocationAlarmLevel.正常);
                            if (item2.Count() > 0)
                            {
                                deleteList.Add(item);
                                hisAlarms.Add(item.RemoveToHistory());
                            }

                            int nCount = eventAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常).Count();
                            if (nCount > 0)
                            {
                                eventAlarmList.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常);
                            }
                        }
                        else
                        {
                            LocationAlarm ReviseAlarm = item.Copy();
                            ReviseAlarm.AlarmLevel = LocationAlarmLevel.正常;

                            //当该卡片在数据库中在指定区域是异常告警时,出现正常告警或没有该区域的异常告警，则告警恢复；出现该区域的异常告警，则忽略
                            LocationAlarm item3 = eventAlarmList.Find(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常);
                            if (item3 != null)
                            {
                                reviseList.Add(ReviseAlarm);
                                deleteList.Add(item);
                                hisAlarms.Add(item.RemoveToHistory());
                                eventAlarmList.Remove(item3);//把当前正常的区域移除，把告警恢复的区域添加并发给客户端
                            }
                            else
                            {
                                if (eventAlarmList.Count() == 0)
                                {
                                    continue;
                                }

                                int nCount = eventAlarmList.FindAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId).Count();
                                if (nCount >= 1)
                                {
                                    eventAlarmList.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId);
                                }
                            }
                        }


                    }

                    if (eventAlarmList.Count() > 0)
                    {
                        newAlarmList.AddRange(eventAlarmList);
                    }

                    if (newAlarmList.Count() > 0)
                    {
                        //向LocationAlarm表添加数据
                        _bll.LocationAlarms.AddRange(newAlarmList);
                        realAlarms.AddRange(newAlarmList);
                    }

                    if (deleteList.Count() > 0)
                    {
                        //删除恢复正常的告警
                        _bll.LocationAlarms.RemoveList(deleteList);
                        foreach (LocationAlarm item in deleteList)
                        {
                            realAlarms.Remove(item);
                        }
                    }

                    //将恢复正常的告警插入历史表
                    SaveHisAlarms();
                    if (reviseList.Count > 0)
                    {
                        newAlarmList.AddRange(reviseList);//告警恢复的区域，也需要发给客户端
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("FaintAlarm", "AuthorizationBuffer.GetFaintAlarm:" + ex.ToString());
            }

            return newAlarmList;
        }

        /// <summary>
        /// 产生晕倒告警
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>

        public List<LocationAlarm> GenerateFaintAlarm(string nFaintScope, int nFaintTime)
        {
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
            using (var _bll = Bll.NewBllNoRelation())
            {
                float minFaintPointCount = nFaintTime * 60*0.5f;//1秒1个点，最少超过20*60*0.5=600个点，才进行晕倒计算(有时可能2s插入一次数据)
                nFaintTime = nFaintTime * 60 * 1000;
                List<LocationCard> cardList = _bll.LocationCards.ToList();
                

                try
                {
                    long endTime = TimeConvert.ToStamp(DateTime.Now);
                    long beginTime = endTime - nFaintTime;
                    List<Position> pList = _bll.Positions.FindAll(p => p.DateTimeStamp >= beginTime && p.DateTimeStamp <= endTime).ToList();
                    foreach (LocationCard card in cardList)
                    {
                        List<Position> pList2 = pList.FindAll(p => p.CardId == card.Id).ToList();
                        if (pList2 == null || pList2.Count() <= 0)
                        {
                            continue;
                        }
                        if(pList2.Count< minFaintPointCount)
                        {
                            continue;
                        }

                        LocationAlarm alarm = CalculationScope(pList2, nFaintScope);
                        if (alarm == null)
                        {
                            continue;
                        }

                        newAlarmList.Add(alarm);
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("FaintAlarm", "AuthorizationBuffer.GenerateFaintAlarm:" + ex.ToString());
                }
            }

            return newAlarmList;
        }

        private LocationAlarm CalculationScope(List<Position> pList, string nFaintScope)
        {
            Position first = pList[0];
            List<Position> ScopeList = new List<Position>();
            LocationAlarm alarm = null;

            BLL.Tools.PosDistanceHelper.FilterErrorPoints<Position>(pList);
            pList.Sort((a, b) =>
            {
                return a.X.CompareTo(b.X);
            });

            if (pList == null || pList.Count <= 0)
            {
                return alarm;
            }

            int nCount = pList.Count();
            float xMin = pList[0].X;
            float xmax = pList[nCount-1].X;
            float length = xmax - xMin;
            length = length / 2;
            if (length < 0)
            {
                length = 0 - length;
            }

            float fFaintScope = Convert.ToSingle(nFaintScope);
            float fFaintScope2 = fFaintScope + 1;

            if (fFaintScope > length)
            {
                alarm = new LocationAlarm();
                alarm.AlarmType = LocationAlarmType.晕倒告警;
                alarm.PersonnelId = first.PersonnelID;
                alarm.LocationCardId = first.CardId;
                alarm.Content = "晕倒告警:" + GetPersonDepartInfo(first.PersonnelID, first.Code);
                alarm.AlarmLevel = LocationAlarmLevel.一级告警;
            }
            else if (fFaintScope2 <= length)
            {
                alarm = new LocationAlarm();
                alarm.AlarmType = LocationAlarmType.晕倒告警;
                alarm.PersonnelId = first.PersonnelID;
                alarm.LocationCardId = first.CardId;
                alarm.Content = "晕倒告警:" + GetPersonDepartInfo(first.PersonnelID, first.Code);
                alarm.AlarmLevel = LocationAlarmLevel.正常;
            }


            return alarm;
        }

        /// <summary>
        /// 通过人员id,获取人员名称和部门（例如：张三 (检修部)）
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetPersonDepartInfo(int? personId,string code)
        {
            try
            {
                Personnel per = GetPersonInfoById(personId);
                string value = "";
                if (per != null)
                {
                    string pst = "";
                    if (per.Pst != null) pst = per.Pst.Replace("\n", "");//测试发现，部门后有\n
                    string department = string.IsNullOrEmpty(pst) ? "" : string.Format("({0})", pst);
                    value = string.Format("{0} {1}", per.Name, department);
                    value.TrimEnd();//如果部门为空，去除后面的空格
                }
                else
                {
                    value = code;
                }
                return value;
            }catch(Exception e)
            {
                return code;
            }          
        }

        /// <summary>
        /// 通过人员id，获取人员信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        private Personnel GetPersonInfoById(int? personId)
        {
            if (personId == null) return null;
            if(personDic==null)
            {
                LoadData();
            }
            if (personDic == null) return null;
            if(personDic.ContainsKey((int)personId))
            {
                Personnel per = personDic[(int)personId];
                return per;
            }
            else
            {
                return null;
            }            
        }
        /// <summary>
        /// 当人从告警区域，进入不确定区域（Area=null）后,消警
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="posListT"></param>
        /// <param name="areaAlarms"></param>
        /// <returns></returns>
        private bool IsAlarmReviseInUndefinedArea(LocationAlarm alarm,List<Position>posListT, List<LocationAlarm> areaAlarms)
        {
            if(areaAlarms==null||areaAlarms.Count==0)
            {
                Position p = posListT.Find(i=>i.CardId==alarm.LocationCardId);
                return p != null;
            }
            return false;
        }
        /// <summary>
        /// 获取新告警
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        public List<LocationAlarm> GetNewAlarms(List<Position> list1, List<LocationAlarm> UdpAlarm)
        {
            LoadData();

            List<LocationAlarm> ReviseAlarmList = new List<LocationAlarm>();
            List<LocationAlarm> DeleteList = new List<LocationAlarm>();
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();


            #region 低电告警
            GeneratePowerAlarm(list1,(powAlarms, normalList)=> 
            {
                foreach (LocationAlarm item in realAlarms)
                {
                    if (item.AlarmType == LocationAlarmType.低电告警)
                    {
                        var newAlarm = powAlarms.FindAll(i => i.LocationCardId == item.LocationCardId);
                        if (item.AlarmLevel!=LocationAlarmLevel.正常&&newAlarm != null && newAlarm.Count > 0)
                        {
                            foreach (var alarm in newAlarm)
                            {
                                powAlarms.Remove(alarm);//删除已经存在的告警
                            }
                        }
                        else
                        {
                            var normalAlarm = normalList.FindAll(i => i.LocationCardId == item.LocationCardId);
                            if (normalAlarm != null && normalAlarm.Count > 0)
                            {                                                                
                                if (item.AlarmLevel != LocationAlarmLevel.正常)//只处理原来是告警的情况
                                {
                                    LocationAlarm ReviseAlarm = item.Copy();
                                    DeleteList.Add(item);
                                    ReviseAlarm.AlarmLevel = LocationAlarmLevel.正常;
                                    ReviseAlarmList.Add(ReviseAlarm);//告警恢复
                                    // hisAlarms.Add(item.RemoveToHistory());//目前低电告警不存到历史数据中，只实时提示2019/11/20wk
                                }
                            }
                        }
                    }
                }
                if (powAlarms.Count() > 0)//新的低电告警
                {
                    newAlarmList.AddRange(powAlarms);
                }
            });            
            #endregion

            #region 区域告警
            List<LocationAlarm> areaAlarms = GenerateAreaAlarm(list1);//区域告警
            //newAlarmList.AddRange(areaAlarms);
            Log.Info("LocationAlarm", "newAlarmList:" + areaAlarms.Count);
            //return newAlarmList;

            foreach (LocationAlarm item in realAlarms)
            {
                if (item.AlarmType == LocationAlarmType.区域告警)
                {
                    if (item.AlarmLevel == LocationAlarmLevel.正常)
                    {
                        //当该卡片在数据库中是正常告警时，出现异常告警则上报，并将原本的正常记录转移到历史数据；出现正常告警则忽略

                        List<LocationAlarm> item2 = areaAlarms.FindAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel != LocationAlarmLevel.正常);
                        if (item2.Count() > 0)
                        {
                            DeleteList.Add(item);
                            hisAlarms.Add(item.RemoveToHistory());
                            UdpAlarm.AddRange(item2);
                        }

                        int nCount = areaAlarms.FindAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常).Count();
                        if (nCount > 0)
                        {
                            areaAlarms.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常);
                        }
                    }
                    else
                    {
                        LocationAlarm ReviseAlarm = item.Copy();
                        ReviseAlarm.AlarmLevel = LocationAlarmLevel.正常;

                        //当该卡片在数据库中在指定区域是异常告警时,出现正常告警或没有该区域的异常告警，则告警恢复；出现该区域的异常告警，则忽略
                        LocationAlarm item3 = areaAlarms.Find(p => p.LocationCardId == item.LocationCardId && p.AlarmLevel == LocationAlarmLevel.正常);
                        if (item3 != null || IsAlarmReviseInUndefinedArea(ReviseAlarm, list1, areaAlarms))
                        {
                            //人从告警区域，走到某个位置（不属于任何区域），则AreaAlarms中不会产生告警和消警。这种情况也要消警
                            ReviseAlarmList.Add(ReviseAlarm);
                            DeleteList.Add(item);
                            hisAlarms.Add(item.RemoveToHistory());
                            if (areaAlarms.Contains(item3)) areaAlarms.Remove(item3);//把当前正常的区域移除，把告警恢复的区域添加并发给客户端
                        }
                        else
                        {
                            if (areaAlarms.Count() == 0)
                            {
                                continue;
                            }

                            int nCount = areaAlarms.FindAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId).Count();
                            if (nCount >= 1)
                            {
                                areaAlarms.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AreaId == item.AreaId);
                                UdpAlarm.Add(item);
                            }
                        }
                    }
                }
                else if (item.AlarmType == LocationAlarmType.超时告警)
                {
                    areaAlarms.RemoveAll(p => p.LocationCardId == item.LocationCardId && p.AlarmType== LocationAlarmType.超时告警&&p.AreaId==item.AreaId);
                }
                
            }
            if (areaAlarms.Count() > 0)
            {               
                newAlarmList.AddRange(areaAlarms);
            }
            #endregion

            #region 求救信号
            List<LocationAlarm> eventAlarms = GenerateSOSAlarm(list1);//求救信号
            foreach (LocationAlarm item in eventAlarms)
            {
                var newAlarm = realAlarms.FindAll(i => i.LocationCardId == item.LocationCardId && i.AlarmType == LocationAlarmType.求救信号);
                if (newAlarm.Count == 0)
                {
                    newAlarmList.Add(item);
                }
            }

            #endregion

            using (var _bll = Bll.NewBllNoRelation())
            {
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
            }

            //将恢复正常的告警插入历史表
            SaveHisAlarms();
            if(ReviseAlarmList.Count>0)
            {
                newAlarmList.AddRange(ReviseAlarmList);//告警恢复的区域，也需要发给客户端
            }
            return newAlarmList;
        }

        public static string tag = "AuthorizationBuffer";
        public static string SuperUserName = "超级管理员";
        /// <summary>
        /// 产生告警
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        private List<LocationAlarm> GenerateAreaAlarm(List<Position> list1)
        {
            List<LocationAlarm> newAlarmList = new List<LocationAlarm>();
            if (list1 == null) {
                Log.Error(tag, "GenerateAreaAlarm", "list1 == null");
                return newAlarmList;
            }

            foreach (Position p in list1)
            {
                if (p== null || p.IsAreaNull() || p.PersonnelID == null || p.AreaId == null||p.IsDynamicAreaPos)
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
                //清除人员进入区域记录
                Bll db = Bll.NewBllNoRelation();
                List<PersonnelFirstInArea> paList = db.PersonnelFirstInAreas.Where(i => i.personId == p.PersonnelID&&i.type==0);
                List<Area> areaList = p.Areas.ToList();
                foreach (PersonnelFirstInArea personnelInArea in paList)
                {
                    Area area = areaList.Find(i => i.Id == personnelInArea.areaId);
                    if (area == null)
                    {
                        db.PersonnelFirstInAreas.DeleteById(personnelInArea.Id);  
                    }
                }
                
             
                                        
                string personDepartment = GetPersonDepartInfo(p.PersonnelID, p.Code);
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
                            RemoveDuplicateAlarms(p, area.Id, null, string.Format("人员：{0}，在区域'{1}'未配置权限。", personDepartment, area), LocationAlarmLevel.四级告警, ref newAlarmList);
                        }
                        else
                        {
                            DateTime startTime = new DateTime(p.DateTime.Year, p.DateTime.Month, p.DateTime.Day, aar.StartTime.Hour, aar.StartTime.Minute, aar.StartTime.Second);
                            DateTime endTime = new DateTime(p.DateTime.Year, p.DateTime.Month, p.DateTime.Day, aar.EndTime.Hour, aar.EndTime.Minute, aar.EndTime.Second);
                            if (aar.TimeType == TimeSettingType.无限制)
                            {
                                RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList);
                            }
                            else if (aar.TimeType == TimeSettingType.时间长度)
                            {
                                //获取
                                PersonnelFirstInArea getPA = db.PersonnelFirstInAreas.Find(i => i.personId == p.PersonnelID && i.areaId == area.Id && i.type == 0);
                                if (getPA == null) //不存在表示已经出了这个区域
                                {
                                    PersonnelFirstInArea person = new PersonnelFirstInArea();
                                    DateTime nowTime = DateTime.Now;
                                    person.personId = p.PersonnelID;
                                    person.areaId = area.Id;
                                    person.dateTime = nowTime;
                                    person.type = 0;
                                    bool result = db.PersonnelFirstInAreas.Add(person);
                                    Log.Info("记录人员：" + p.PersonnelName + "进入区域：" + area.Name + "权限：时间长度,结果：" + result);
                                    RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList,LocationAlarmType.超时告警);
                                }
                                else//告警
                                {
                                    if ((p.DateTime - getPA.dateTime).TotalMinutes > aar.TimeSpan)//超过时长
                                    {
                                        string timeOut = ((p.DateTime - getPA.dateTime).TotalMinutes - aar.TimeSpan).ToString();
                                        RemoveDuplicateAlarms(p, area.Id, aar, string.Format("人员：{0}，在区域:{1}超时停留{2}分", personDepartment, area, timeOut), LocationAlarmLevel.四级告警, ref newAlarmList,LocationAlarmType.超时告警);
                                    }
                                    else
                                    {
                                        RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList, LocationAlarmType.超时告警);
                                    }
                                }

                            }

                            else if (aar.TimeType == TimeSettingType.时间点范围)
                            {
                                if (p.DateTime > startTime && p.DateTime < endTime)
                                {
                                    RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList, LocationAlarmType.超时告警);
                                }
                                else
                                {
                                    RemoveDuplicateAlarms(p, area.Id, aar, string.Format("告警，人员：{0} 在区域:{1}未设置权限", area), LocationAlarmLevel.四级告警, ref newAlarmList, LocationAlarmType.超时告警);
                                }
                            }
                            else if (aar.TimeType == TimeSettingType.时间长度加范围)
                            {
                                if (p.DateTime > startTime && p.DateTime < endTime)  //判断在时间范围内后，再判断时间长度
                                {
                                    PersonnelFirstInArea getPA = db.PersonnelFirstInAreas.Find(i => i.personId == p.PersonnelID && i.areaId == area.Id && i.type == 0);
                                    if (getPA == null) //不存在表示已经出了这个区域
                                    {
                                        PersonnelFirstInArea person = new PersonnelFirstInArea();
                                        DateTime nowTime = DateTime.Now;
                                        person.personId = p.PersonnelID;
                                        person.areaId = area.Id;
                                        person.dateTime = nowTime;
                                        person.type = 0;
                                        bool result = db.PersonnelFirstInAreas.Add(person);
                                        Log.Info("记录人员：" + p.PersonnelName + "进入区域：" + area.Name + "权限：时间长度加范围,结果：" + result);
                                        RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList, LocationAlarmType.超时告警);
                                    }
                                    else//告警
                                    {
                                        if ((p.DateTime - getPA.dateTime).TotalMinutes > aar.TimeSpan)//超过时长
                                        {
                                            RemoveDuplicateAlarms(p, area.Id, aar, string.Format("人员：{0}，在区域:{1}未设置权限", personDepartment, area), LocationAlarmLevel.四级告警, ref newAlarmList, LocationAlarmType.超时告警);
                                        }
                                        else
                                        {
                                            RemoveDuplicateAlarms(p, area.Id, aar, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList, LocationAlarmType.超时告警);
                                        }
                                    }
                                }
                                else
                                {
                                    RemoveDuplicateAlarms(p, area.Id, aar, string.Format("告警，人员：{0} 在区域:{1}未设置权限", area), LocationAlarmLevel.四级告警, ref newAlarmList);
                                }
                            }
                         
                                                          


                            //if (aar.IsTimeValid(dtBegin, dtEnd, nTimeStamp) == false)
                            //{
                            //    RemoveDuplicateAlarms(p, area.Id, aar, string.Format("可以进入区域'{0}',但是未在有效时间范围内。", area), LocationAlarmLevel.四级告警, ref newAlarmList);
                            //}
                            //else
                            
                        }
                        if (aar.SignIn)//有签到限制(添加一条人员进入区域的记录)
                        {
                            PersonnelFirstInArea getPA = db.PersonnelFirstInAreas.Find(i => i.personId == p.PersonnelID && i.areaId == area.Id && i.type == 1);
                            if (getPA == null)
                            {
                                PersonnelFirstInArea person = new PersonnelFirstInArea();
                                DateTime nowTime = DateTime.Now;
                                person.personId = p.PersonnelID;
                                person.areaId = area.Id;
                                person.dateTime = nowTime;
                                person.type = 1;
                                bool result = db.PersonnelFirstInAreas.Add(person);
                                Log.Info("记录人员：" + p.PersonnelName + "进入区域：" + area.Name + "权限：签到,结果：" + result);
                            }
                        }
                        


                    }
                    else
                    {
                        if (role.Id == 1||role.Name== SuperUserName)//超级管理员
                        {
                            RemoveDuplicateAlarms(p, area.Id, null, string.Format("正常，所在区域:{0}", area), LocationAlarmLevel.正常, ref newAlarmList);
                        }
                        else
                        {
                            RemoveDuplicateAlarms(p, area.Id, null, string.Format("人员：{0}，在区域'{1}'未配置权限。", personDepartment, area), LocationAlarmLevel.四级告警, ref newAlarmList);
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
        private void RemoveDuplicateAlarms(Position p, int area, AreaAuthorizationRecord arr, string content, LocationAlarmLevel level, ref List<LocationAlarm> newAlarmList, LocationAlarmType alarmType = LocationAlarmType.区域告警)
        {
          
            if (level == LocationAlarmLevel.正常)
            {
                //如果缓存中 没有正常告警，添加正常告警；如果缓存中有正常告警，添加正常告警规则Id
                int nCount = newAlarmList.FindAll(i => i.LocationCardId == p.CardId && i.AlarmLevel != LocationAlarmLevel.正常).Count();
                LocationAlarm alarm = newAlarmList.Find(i => i.LocationCardId == p.CardId && i.AlarmLevel == LocationAlarmLevel.正常);
                if (nCount == 0 && alarm == null)
                {
                    alarm = AddAlarm(p, area, arr, content, level,alarmType);
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

                LocationAlarm alarm = AddAlarm(p, area, arr, content, level,alarmType);
                newAlarmList.Add(alarm);
            }

            return;
        }

        public List<LocationAlarm> DeleteSpecifiedLocationAlarm(List<int>LocationIdList)
        {
            //bool bReturn = true;
            List<LocationAlarm> reviseListT = new List<LocationAlarm>();//恢复的告警
            if (LocationIdList == null) return null;
            try
            {
                using (var _bll = Bll.NewBllNoRelation())
                {
                    List<LocationAlarm> removeList = new List<LocationAlarm>();
                    Dictionary<int,LocationAlarm>alarmDic = _bll.LocationAlarms.ToDictionary();                 
                    if (alarmDic == null) return null;
                    foreach(var item in LocationIdList)
                    {
                        LocationAlarm alarm = alarmDic.ContainsKey(item)?alarmDic[item]:null;
                        if (alarm == null) continue;                       
                        hisAlarms.Add(alarm.RemoveToHistory());
                        removeList.Add(alarm);
                        LocationAlarm revise = alarm.Copy();
                        revise.AlarmLevel = LocationAlarmLevel.正常;
                        if (realAlarms != null && realAlarms.Count > 0)
                        {
                            LocationAlarm alarm2 = realAlarms.Find(p => p.Id == item);
                            realAlarms.Remove(alarm2);
                        }
                        else
                        {
                            realAlarms = _bll.LocationAlarms.ToList();
                        }
                    }
                    if (removeList != null && removeList.Count != 0)
                    {
                        _bll.LocationAlarms.RemoveList(removeList);
                        SaveHisAlarms();
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                return null;
            }
            return reviseListT;
        }

    }
}

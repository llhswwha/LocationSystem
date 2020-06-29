using LocationServices.Locations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using TModel.BaseData;
using BLL;
using BLL.Blls.Location;
using LocationServices.Converters;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;
using Location.BLL.Tool;
using TModel.Location.AreaAndDev;
using DbModel.Location.Alarm;
using T_LocationAlarm = Location.TModel.Location.Alarm.LocationAlarm;
using TModel.Tools;
using DbModel.Tools;
using SignalRService.Hubs;
using TModel.Location.Alarm;
using TModel.FuncArgs;
using DbModel.LocationHistory.Alarm;

namespace LocationServices.Locations.Services
{
    public interface IAlarmService:INameEntityService<DevAlarm>
    {
        /// <summary>
        /// 获取定位告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        List<T_LocationAlarm> GetLocationAlarms(AlarmSearchArg arg);

        LocationAlarmInformation GetLocationAlarmByArgs(AlarmSearchArg arg);
        /// <summary>
        /// 获取设备告警列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        DeviceAlarmInformation GetDeviceAlarms(AlarmSearchArg arg);

        Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg);

        bool DeleteSpecifiedLocationAlarm(int id);

        bool DeleteLocationAlarm(List<int> idList);

        AllAlarms GetAllAlarmsByPerson(AlarmSearchArgAll args);
    }

    public class AlarmService : IAlarmService
    {
        private Bll db;
    
        public AlarmService()
        {
            db = Bll.NewBllNoRelation();
        }

        /// <summary>
        /// //实现加载全部设备告警到内存中
        /// </summary>
        public static void RefreshAlarmBuffer()
        {
            //if (allAlarms == null)
            {
                BLL.Bll bll = BLL.Bll.NewBllNoRelation();
                allAlarms = bll.DevAlarms.ToList();
                allLocationAlarmHistory = bll.LocationAlarmHistorys.ToList();
            }
        }
        public static List<DevAlarm> allAlarms;
        public static List<DbModel.LocationHistory.Alarm.LocationAlarmHistory> allLocationAlarmHistory;

        public DeviceAlarmInformation GetDeviceAlarms(AlarmSearchArg arg)
        {
           
            if (allAlarms == null)
            {
                RefreshAlarmBuffer();
            }

            DeviceAlarmInformation DevAlarm = new DeviceAlarmInformation();          

            DateTime start = DateTime.Now;
            var devs = db.DevInfos.ToList().ToTModel();
            if (devs == null || devs.Count == 0) return null;
            var dict = new Dictionary<int, DevInfo>();
            foreach (var item in devs)
            {
                if (item.ParentId != null)
                    dict.Add(item.Id, item);
            }
        
            List<DeviceAlarm> alarms = new List<DeviceAlarm>();
            List<DbModel.Location.Alarm.DevAlarm> alarms1 = null;
            DateTime now = DateTime.Now;
            DateTime todayStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            DateTime todayEnd = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
            //arg.Start = null;
            if (arg.Start != null && arg.End != null)
            {
                todayStart = Convert.ToDateTime(arg.Start);
                todayEnd = Convert.ToDateTime(arg.End);
            }
            else if (arg.Start != null)
            {
                todayStart = Convert.ToDateTime(arg.Start);
            }
            else if (arg.End != null)
            {
                todayEnd = Convert.ToDateTime(arg.End);
            }

            var startStamp = TimeConvert.ToStamp(todayStart);
            var endStamp = TimeConvert.ToStamp(todayEnd);
           
            if (arg.AreaId != 0)
            {
                var areas = db.Areas.GetAllSubAreas(arg.AreaId);//获取所有子区域
                if (arg.IsAll)
                {
                    alarms1 = allAlarms.FindAll(i => i.DevInfo != null && areas.Contains(i.DevInfo.ParentId));
                }
                else
                {
                    alarms1 = allAlarms.FindAll(i => i.DevInfo != null && areas.Contains(i.DevInfo.ParentId) && i.AlarmTimeStamp >= startStamp && i.AlarmTimeStamp <= endStamp);
                }

            }
            else
            {
                if (arg.IsAll)
                {
                    alarms1 = allAlarms;
                }
                else
                {
                    alarms1 = allAlarms.FindAll(i => i.AlarmTimeStamp >= startStamp && i.AlarmTimeStamp <= endStamp);
                }

            }
           
            if (alarms1 == null) return null;
            alarms = alarms1.ToTModel();
            alarms.Sort();
            if (alarms.Count == 0)
            {

            }
          
            foreach (var item in alarms)
            {
                try
                {
                    if (dict.ContainsKey(item.DevId))
                    {
                        var dev = dict[item.DevId];
                        item.SetDev(dev);//这里设置DevId,AreaId等
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }
         
            DevAlarm.devAlarmList = new List<DeviceAlarm>();            
            DeviceAlarmScreen(arg, alarms, DevAlarm.devAlarmList);
            DevAlarm.Total = DevAlarmTotal;
            
            TimeSpan time = DateTime.Now - start;
            DevAlarm.SetEmpty();
            return DevAlarm;                  
        }
        int DevAlarmTotal = 0;
        private void DeviceAlarmScreen(AlarmSearchArg arg, List<DeviceAlarm> ListInfo, List<DeviceAlarm> DevAlarmlist)
        {
            List<DeviceAlarm> DevAlarmLevelList = new List<DeviceAlarm>();
            if (arg.DevTypeName == "所有设备" && arg.Level == 0)
            {
                DevAlarmLevelList.AddRange(ListInfo);
            }
            else
            {
                foreach (var devAlarm in ListInfo)
                {
                    if (arg.DevTypeName == "所有设备" && arg.Level != 0)
                    {
                        if (devAlarm.Level == GetDevAlarmLevel(arg.Level))
                        {
                            DevAlarmLevelList.Add(devAlarm);
                        }
                    }
                    else if (arg.Level == 0 && arg.DevTypeName != "所有设备")
                    {
                        if (arg.DevTypeName == devAlarm.DevTypeName)
                        {
                            DevAlarmLevelList.Add(devAlarm);
                        }
                    }
                    else if (arg.DevTypeName != "所有设备" && arg.Level != 0)
                    {
                        if (arg.DevTypeName == devAlarm.DevTypeName && devAlarm.Level == GetDevAlarmLevel(arg.Level))
                        {
                            DevAlarmLevelList.Add(devAlarm);
                        }
                    }
                }
            }
            if (DevAlarmLevelList.Count == 0)
            {
                DevAlarmTotal = 1;
            }
            else
            {
                int maxPage = (int)Math.Ceiling((double)DevAlarmLevelList.Count / (double)arg.Page.Size);
                DevAlarmTotal = maxPage;
                if (arg.Page.Number > maxPage)
                {
                    arg.Page.Number = maxPage - 1;
                }
                var QueryData = DevAlarmLevelList.Skip(arg.Page.Size * arg.Page.Number).Take(arg.Page.Size);
                foreach (var devAlarm in QueryData)
                {
                    DevAlarmlist.Add(devAlarm);
                }
            }
        }

        /// <summary>
        /// 告警等级
        /// </summary>
        /// <returns></returns>
        public Abutment_DevAlarmLevel GetDevAlarmLevel(int level)
        {
            if (level == 1) return Abutment_DevAlarmLevel.高;
            else if (level == 2) return Abutment_DevAlarmLevel.中;
            else if (level == 3) return Abutment_DevAlarmLevel.低;
            else
            {
                return Abutment_DevAlarmLevel.未定;
            }

        }
        /// <summary>
        /// 根据id号，删除指定告警
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteSpecifiedLocationAlarm(int id)
        {
            BLL.Bll bll = new BLL.Bll(false, true, true);
            BLL.Buffers.AuthorizationBuffer ab = BLL.Buffers.AuthorizationBuffer.Instance(bll);
            List<int> idList = new List<int>() { id };
            return DeleteLocationAlarm(idList);
        }
        /// <summary>
        /// 批量删除告警
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public bool DeleteLocationAlarm(List<int>idList)
        {
            BLL.Bll bll = new BLL.Bll(false, true, true);
            BLL.Buffers.AuthorizationBuffer ab = BLL.Buffers.AuthorizationBuffer.Instance(bll);
            List<DbModel.Location.Alarm.LocationAlarm> reviseAlarms = ab.DeleteSpecifiedLocationAlarm(idList);
            if (reviseAlarms == null || reviseAlarms.Count == 0) return false;
            else
            {
                //新增，服务端消警后，把消警信息发给客户端
                var alarms = reviseAlarms.ToTModel().ToArray();
                AlarmHub.SendLocationAlarms(alarms);
                return true;
            }
        }

        public Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg)
        {
            Page<DeviceAlarm> page = new Page<DeviceAlarm>();
            page.Content = GetDeviceAlarms(arg).devAlarmList;
            return page;
        }

        public LocationAlarmInformation GetLocationAlarmByArgs(AlarmSearchArg arg)
        {
            if (allLocationAlarmHistory == null)
            {
                RefreshAlarmBuffer();
            }
            LocationAlarmInformation locationAlarm = new LocationAlarmInformation();
            List<T_LocationAlarm> alarmsTemp = TryGetAllLocationAlarms(arg);
            if (alarmsTemp == null || alarmsTemp.Count == 0) return new LocationAlarmInformation();
            alarmsTemp.Sort();
            LocationAlarmInformation devAlarmInfo = GetLocationAlarmPage(arg, alarmsTemp);
            return locationAlarm;
        }

        public List<T_LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            return TryGetAllLocationAlarms(arg);
        }
        /// <summary>
        /// 定位告警历史，获取对应切页
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public LocationAlarmInformation GetLocationAlarmPage(AlarmSearchArg arg, List<Location.TModel.Location.Alarm.LocationAlarm> list)
        {
            LocationAlarmInformation locationAlarmInfo = new LocationAlarmInformation();
            if (list == null || list.Count == 0) return locationAlarmInfo;

            locationAlarmInfo.locationAlarmList = new List<Location.TModel.Location.Alarm.LocationAlarm>();
            var LocationAlarmlist = locationAlarmInfo.locationAlarmList;
            if (list.Count == 0)
            {
                locationAlarmInfo.Total = 1;
            }
            else
            {
                if (arg.Page != null)
                {
                    int maxPage = (int)Math.Ceiling((double)list.Count / (double)arg.Page.Size);
                    locationAlarmInfo.Total = maxPage;
                    if (arg.Page.Number > maxPage)
                    {
                        arg.Page.Number = maxPage - 1;
                    }
                    var queryData = list.Skip(arg.Page.Size * arg.Page.Number).Take(arg.Page.Size);
                    var resultList = queryData.ToList();
                    LocationAlarmlist.AddRange(resultList);
                }
                else
                {
                    LocationAlarmlist.AddRange(list);
                }
            }
            locationAlarmInfo.SetEmpty();
            return locationAlarmInfo;
        }
        /// <summary>
        /// 根据参数，获取所有的定位告警
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private List<T_LocationAlarm> TryGetAllLocationAlarms(AlarmSearchArg arg)
        {
            try
            {
                if (allLocationAlarmHistory == null)
                {
                    RefreshAlarmBuffer();
                }
                var departments = db.Departments.ToDictionary();
                var persons = db.Personnels.ToDictionary();
                var cards = db.LocationCards.ToDictionary();
                foreach (var person in persons)
                {
                    if (person.Value.ParentId != null)
                    {
                        int id = (int)person.Key;
                        int pId = (int)person.Value.ParentId;
                        if (departments.ContainsKey(pId))
                        {
                            persons[id].Parent = departments[pId];
                        }
                    }
                }

                if (arg.IsAll == false)//IsAll==true代表查询实时告警
                {
                    var list = db.LocationAlarms.Where(p => p.AlarmLevel != 0).ToList();

                    SetAlarmPerson(list, persons);
                    SetAlarmTag(cards, list);
                    list.OrderByDescending(i => i.AlarmTimeStamp);
                    return list.ToWcfModelList();
                }
                else
                {
                    long timeStampStart = 0;
                    long timeStampEnd = long.MaxValue;
                    if (!string.IsNullOrEmpty(arg.Start))
                    {
                        DateTime start = arg.Start.ToDateTime();
                        start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0, 0);
                        timeStampStart = start.ToStamp();
                    }

                    if (!string.IsNullOrEmpty(arg.End))
                    {
                        DateTime end = arg.End.ToDateTime();
                        end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59, 999);
                        timeStampEnd = end.ToStamp();
                    }

                    //历史告警
                    var list1 = allLocationAlarmHistory.Where(p =>
                              p.AlarmLevel != 0 && p.AlarmTimeStamp >= timeStampStart && p.AlarmTimeStamp <= timeStampEnd)
                        .ToList();

                    List<DbModel.Location.Alarm.LocationAlarm> list3 = new List<DbModel.Location.Alarm.LocationAlarm>();
                    if (list1 != null)
                    {
                        foreach (var alarmHistory in list1)
                        {
                            var alarm = alarmHistory.ConvertToAlarm();
                            list3.Add(alarm);
                        }
                    }

                    //当前的事实告警
                    var list2 = db.LocationAlarms.Where(p =>
                        p.AlarmLevel != 0 && p.AlarmTimeStamp >= timeStampStart && p.AlarmTimeStamp <= timeStampEnd).ToList();
                    if (list2 != null)
                    {
                        list3.AddRange(list2);
                    }

                    SetAlarmPerson(list3, persons);
                    SetAlarmTag(cards, list3);
                    list3.OrderByDescending(i => i.AlarmTimeStamp);
                    List<Location.TModel.Location.Alarm.LocationAlarm> send = list3.ToWcfModelList();
                    if (send != null && send.Count() == 0)
                    {
                        send = null;
                    }
                    return send;
                }

            }
            catch (Exception e)
            {
                Log.Error(LogTags.DbGet, "GetLocationAlarms:" + e);
                return null;
            }
        }


        private static void SetAlarmTag(Dictionary<int, DbModel.Location.AreaAndDev.LocationCard> cards, List<DbModel.Location.Alarm.LocationAlarm> list)
        {
            foreach (var alarm in list)
            {
                if (alarm.LocationCardId != null)
                {
                    int pId = (int)alarm.LocationCardId;
                    if (cards.ContainsKey(pId))
                    {
                        alarm.LocationCard = cards[pId];
                    }
                }
            }
        }

        private static void SetAlarmPerson(List<DbModel.Location.Alarm.LocationAlarm> list, Dictionary<int, DbModel.Location.Person.Personnel> persons)
        {
            foreach (var alarm in list)
            {
                if (alarm.PersonnelId != null)
                {
                    int pId = (int)alarm.PersonnelId;
                    if (persons.ContainsKey(pId))
                    {
                        alarm.Personnel = persons[pId];
                    }
                }
            }
        }


        public IList<DevAlarm> GetListByName(string name)
        {
            throw new NotImplementedException();
        }

        public DevAlarm Delete(string id)
        {
            return db.DevAlarms.DeleteById(int.Parse(id));
        }

        public DevAlarm GetEntity(string id)
        {
            return db.DevAlarms.Find(int.Parse(id));
        }

        public List<DevAlarm> GetList()
        {
            var list = db.DevAlarms.ToList();
            return list;
        }

        public DevAlarm Post(DevAlarm item)
        {
          var result = db.DevAlarms.Add(item);
            return result ? item : null;
        }

        public DevAlarm Put(DevAlarm item)
        {
            return null;
        }

        public AllAlarms GetAllAlarmsByPerson(AlarmSearchArgAll args)
        {
            AllAlarms alarms = new AllAlarms();
            try
            {
                string sqlwhere1 = "";
                string sqlwhere2 = "";
                string personnels = "";
                if (args != null)
                {
                    if (args.personnels != null)
                    {
                        foreach (int person in args.personnels)
                        {
                            personnels += person.ToString() + ",";
                        }
                        personnels = personnels.Substring(0,personnels.Length-1);
                        sqlwhere1 += " and PersonnelId in ("+personnels+")";
                    }
                    if (args.startTime != null && args.endTime != null)
                    {
                        sqlwhere1 += string.Format(@"  and  AlarmTimeStamp>{0} and AlarmTimeStamp<{1}",args.startTime.ToStamp(),args.endTime.ToStamp());
                        sqlwhere2 += string.Format(@"  and  AlarmTimeStamp>{0} and AlarmTimeStamp<{1}", args.startTime.ToStamp(), args.endTime.ToStamp());
                    }

                }
                string sql1 = string.Format(@"select id,AlarmId,AlarmType,AlarmLevel,LocationCardId,PersonnelId,CardRoleId,AreaId,AuzId,AllAuzId,Content,AlarmTime,AlarmTimeStamp,HandleTime,HandleTimeStamp,`Handler`,HandleType from location.locationalarms    where 1=1  " + sqlwhere1);
                string sql2 = string.Format(@"select id,AlarmId,AlarmType,AlarmLevel,LocationCardId,PersonnelId,CardRoleId,areadid,AuzId,AllAuzId,Content,AlarmTime,AlarmTimeStamp,HandleTime,HandleTimeStamp,`Handler`,HandleType, HistoryTime, HistoryTimeStamp from locationhistory.locationalarmhistories  where 1=1 " +sqlwhere1);
                string sql3 = string.Format(@"select id,Abutment_Id,Title,Msg,`LEVEL`,`Code`,Src,DevInfoId,Device_desc,AlarmTime,AlarmTimeStamp  from location.devalarms    where 1=1 " + sqlwhere2);
                string sql4 = string.Format(@"select id,Abutment_Id,Title,Msg,`LEVEL`,`Code`,Src,DevInfoId,Device_desc,AlarmTime,AlarmTimeStamp,historyTime,historyTimeStamp from locationhistory.devalarmhistories where 1=1 "+sqlwhere2);
                List<DbModel.Location.Alarm.LocationAlarm> list1 = db.LocationAlarms.GetListBySql<DbModel.Location.Alarm.LocationAlarm>(sql1);
                List<LocationAlarmHistory> list2 = db.LocationAlarmHistorys.GetListBySql<LocationAlarmHistory>(sql2);
                List<DbModel.Location.Alarm.LocationAlarm> listalarm = new List<DbModel.Location.Alarm.LocationAlarm>();
                listalarm.AddRange(list1);
                foreach (LocationAlarmHistory alarmHis in list2)
                {
                    DbModel.Location.Alarm.LocationAlarm alarm = alarmHis.ConvertToAlarm();
                    listalarm.Add(alarm);
                }
                List<DevAlarm> list3 = db.DevAlarms.GetListBySql<DevAlarm>(sql3);
                List<DevAlarmHistory> devalarmHiss = db.DevAlarmHistorys.GetListBySql<DevAlarmHistory>(sql4);
                List<DevAlarm> devalarms = new List<DevAlarm>();
                devalarms.AddRange(list3);
                foreach (DevAlarmHistory devalarmhis in devalarmHiss)
                {
                    DevAlarm devalarm = devalarmhis.ConvertToDevAlarm();
                    devalarms.Add(devalarm);
                }
                alarms.alarmList = listalarm.ToTModel() ;
                alarms.devAlarmList = devalarms.ToTModel();
            }
            catch (Exception ex)
            {
                alarms.alarmList = new List<T_LocationAlarm>();
                alarms.devAlarmList = new List<DeviceAlarm>();
                Log.Error("AlarmService.GetAllAlarmsByPerson:"+ex.ToString());
            }
            return alarms;
        }
    }
}

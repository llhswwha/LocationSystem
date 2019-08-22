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
using DbModel.Tools;

namespace LocationServices.Locations.Services
{
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
        public static void RefreshDeviceAlarmBuffer()
        {
            //if (allAlarms == null)
            {
                BLL.Bll bll = BLL.Bll.NewBllNoRelation();
                allAlarms = bll.DevAlarms.ToList();
            }
        }
        public static List<DbModel.Location.Alarm.DevAlarm> allAlarms;

        public DeviceAlarmInformation GetDeviceAlarms(AlarmSearchArg arg)
        {
            if (allAlarms == null)
            {
                RefreshDeviceAlarmBuffer();
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


        public Page<DeviceAlarm> GetDeviceAlarmsPage(AlarmSearchArg arg)
        {
            Page<DeviceAlarm> page = new Page<DeviceAlarm>();
            page.Content = GetDeviceAlarms(arg).devAlarmList;
            return page;
        }

        public List<LocationAlarm> GetLocationAlarms(AlarmSearchArg arg)
        {
            var list = db.LocationAlarms.Where(p => p.AlarmLevel != 0).ToList();
            var persons = db.Personnels.ToList();
            foreach (var alarm in list)
            {
                alarm.Personnel = persons.Find(i => i.Id == alarm.PersonnelId);
            }
            return list.ToWcfModelList();
        }
    }
}

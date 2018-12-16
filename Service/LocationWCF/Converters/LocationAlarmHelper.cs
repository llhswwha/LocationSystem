using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
    public static class LocationAlarmHelper
    {

        #region Location.TModel.Location.Alarm.LocationAlarm <=> DbModel.Location.Alarm.LocationAlarm

        public static List<Location.TModel.Location.Alarm.LocationAlarm> ToWcfModelList(
            this List<DbModel.Location.Alarm.LocationAlarm> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.Alarm.LocationAlarm ToTModel(
            this DbModel.Location.Alarm.LocationAlarm item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.Alarm.LocationAlarm();
            item2.Id = item1.Id;
            item2.AlarmType = item1.AlarmType;
            item2.AlarmLevel = item1.AlarmLevel;
            item2.TagId = item1.LocationCardId ?? 0;
            item2.Tag = item1.LocationCard.ToTModel();
            item2.PersonnelId = item1.PersonnelId ?? 0;
            item2.Personnel = item1.Personnel.ToTModel();
            item2.Content = item1.Content;
            item2.CreateTime = item1.AlarmTime;
            item2.AlarmTimeStamp = item1.AlarmTimeStamp;
            item2.HandleTime = item1.HandleTime;
            item2.HandleTimeStamp = item1.HandleTimeStamp;
            item2.Handler = item1.Handler;
            item2.HandleType = item1.HandleType;
            item2.AlarmId = item1.AlarmId;
            item2.CardRoleId = item1.CardRoleId;
            return item2;
        }

        public static List<Location.TModel.Location.Alarm.LocationAlarm> ToTModel(
            this List<DbModel.Location.Alarm.LocationAlarm> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.Alarm.LocationAlarm>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Alarm.LocationAlarm ToDbModel(
            this Location.TModel.Location.Alarm.LocationAlarm item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Alarm.LocationAlarm();
            item2.Id = item1.Id;
            item2.AlarmType = item1.AlarmType;
            item2.AlarmLevel = item1.AlarmLevel;
            item2.LocationCardId = item1.TagId;
            item2.LocationCard = item1.Tag.ToDbModel();
            item2.PersonnelId = item1.PersonnelId;
            item2.Personnel = item1.Personnel.ToDbModel();
            item2.Content = item1.Content;
            item2.AlarmTime = item1.CreateTime;
            item2.AlarmTimeStamp = item1.AlarmTimeStamp;
            item2.HandleTime = item1.HandleTime;
            item2.HandleTimeStamp = item1.HandleTimeStamp;
            item2.Handler = item1.Handler;
            item2.HandleType = item1.HandleType;
            item2.AlarmId = item1.AlarmId;
            item2.CardRoleId = item1.CardRoleId;
            return item2;
        }

        public static List<DbModel.Location.Alarm.LocationAlarm> ToDbModel(
            this List<Location.TModel.Location.Alarm.LocationAlarm> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Alarm.LocationAlarm>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

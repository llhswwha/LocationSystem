using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
    public static class DevAlarmConvertHelper
    {
        #region Location.TModel.Location.Alarm.DeviceAlarm <=> DbModel.Location.Alarm.DevAlarm

        public static List<Location.TModel.Location.Alarm.DeviceAlarm> ToWcfModelList(
            this List<DbModel.Location.Alarm.DevAlarm> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.Alarm.DeviceAlarm ToTModel(this DbModel.Location.Alarm.DevAlarm item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.Alarm.DeviceAlarm();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Title = item1.Title;
            item2.Message = item1.Msg;
            item2.Level = item1.Level;
            item2.Code = item1.Code;
            item2.Src = item1.Src;
            item2.DevId = item1.DevInfoId;
            item2.SetDev(item1.DevInfo.ToTModel());
            //item2.Dev = item1.DevInfo.ToTModel();
            item2.Device_desc = item1.Device_desc;
            item2.CreateTime = item1.AlarmTime;
            item2.AlarmTimeStamp = item1.AlarmTimeStamp;
            if (item1.DevInfo != null)
                item2.AreaId = item1.DevInfo.ParentId ?? 0;
            item2.HistoryTime = item1.HistoryTime;
            item2.HistoryTimeStamp = item1.HistoryTimeStamp;
            return item2;
        }

        public static List<Location.TModel.Location.Alarm.DeviceAlarm> ToTModel(
            this List<DbModel.Location.Alarm.DevAlarm> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.Alarm.DeviceAlarm>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.Alarm.DevAlarm ToDbModel(this Location.TModel.Location.Alarm.DeviceAlarm item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.Alarm.DevAlarm();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Title = item1.Title;
            item2.Msg = item1.Message;
            item2.Level = item1.Level;
            item2.Code = item1.Code;
            item2.Src = item1.Src;
            item2.DevInfoId = item1.DevId;
            //item2.DevInfo = item1.Dev.ToDbModel();
            item2.Device_desc = item1.Device_desc;
            item2.AlarmTime = item1.CreateTime;
            item2.AlarmTimeStamp = item1.AlarmTimeStamp;
            return item2;
        }

        public static List<DbModel.Location.Alarm.DevAlarm> ToDbModel(
            this List<Location.TModel.Location.Alarm.DeviceAlarm> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.Alarm.DevAlarm>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

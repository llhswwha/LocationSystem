using System.Collections.Generic;
using DbModel.Tools;

namespace LocationServices.Converters
{
    public static class ArchorConvertHelper
    {
        #region TModel.Location.AreaAndDev.Archor <=> DbModel.Location.AreaAndDev.Archor
        public static List<TModel.Location.AreaAndDev.Archor> ToWcfModelList(this List<DbModel.Location.AreaAndDev.Archor> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
        public static TModel.Location.AreaAndDev.Archor ToTModel(this DbModel.Location.AreaAndDev.Archor item1)
        {
            if (item1 == null) return null;
            var item2 = new TModel.Location.AreaAndDev.Archor();
            item2.Id = item1.Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Type = item1.Type;
            item2.IsAutoIp = item1.IsAutoIp;
            item2.Ip = item1.Ip;
            item2.ServerIp = item1.ServerIp;
            item2.ServerPort = item1.ServerPort;
            item2.Power = item1.Power;
            item2.AliveTime = item1.AliveTime;
            item2.Enable = item1.Enable;
            item2.DevInfoId = item1.DevInfoId;
            item2.DevInfo = item1.DevInfo.ToTModel();
            return item2;
        }

        public static void EditProperty(this DbModel.Location.AreaAndDev.Archor item1, TModel.Location.AreaAndDev.Archor item2)
        {
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Type = item1.Type;
            item2.IsAutoIp = item1.IsAutoIp;
            item2.Ip = item1.Ip;
            item2.ServerIp = item1.ServerIp;
            item2.ServerPort = item1.ServerPort;
            item2.Power = item1.Power;
            item2.AliveTime = item1.AliveTime;
            item2.Enable = item1.Enable;
        }

        public static List<TModel.Location.AreaAndDev.Archor> ToTModel(this List<DbModel.Location.AreaAndDev.Archor> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TModel.Location.AreaAndDev.Archor>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }
        public static DbModel.Location.AreaAndDev.Archor ToDbModel(this TModel.Location.AreaAndDev.Archor item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Archor();
            item2.Id = item1.Id;
            EditProperty(item2,item1);
            item2.DevInfoId = item1.DevInfoId;
            item2.DevInfo = item1.DevInfo.ToDbModel();
            return item2;
        }


        public static List<DbModel.Location.AreaAndDev.Archor> ToDbModel(this List<TModel.Location.AreaAndDev.Archor> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Archor>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
        #endregion

    }
}

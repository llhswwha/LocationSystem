using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
    public static class TransfromConvertHelper
    {
        #region Location.TModel.Location.AreaAndDev.TransformM <=> DbModel.Tools.InitInfos.TransformM

        public static List<Location.TModel.Location.AreaAndDev.TransformM> ToWcfModelList(
            this List<DbModel.Tools.InitInfos.TransformM> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.TransformM ToTModel(
            this DbModel.Tools.InitInfos.TransformM item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.TransformM();
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.RX = item1.RX;
            item2.RY = item1.RY;
            item2.RZ = item1.RZ;
            item2.SX = item1.SX;
            item2.SY = item1.SY;
            item2.SZ = item1.SZ;
            item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            item2.IsOnLocationArea = item1.IsOnLocationArea;
            item2.IsCreateAreaByData = item1.IsCreateAreaByData;
            item2.IsRelative = item1.IsRelative;
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.TransformM> ToTModel(
            this List<DbModel.Tools.InitInfos.TransformM> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.TransformM>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Tools.InitInfos.TransformM ToDbModel(
            this Location.TModel.Location.AreaAndDev.TransformM item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Tools.InitInfos.TransformM();
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.RX = item1.RX;
            item2.RY = item1.RY;
            item2.RZ = item1.RZ;
            item2.SX = item1.SX;
            item2.SY = item1.SY;
            item2.SZ = item1.SZ;
            item2.IsOnAlarmArea = item1.IsOnAlarmArea;
            item2.IsOnLocationArea = item1.IsOnLocationArea;
            item2.IsCreateAreaByData = item1.IsCreateAreaByData;
            item2.IsRelative = item1.IsRelative;
            return item2;
        }

        public static List<DbModel.Tools.InitInfos.TransformM> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.TransformM> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Tools.InitInfos.TransformM>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

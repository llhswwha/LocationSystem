using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
    public static class PointConvertHelper
    {
        #region Location.TModel.Location.AreaAndDev.Point <=> DbModel.Location.AreaAndDev.Point

        public static List<Location.TModel.Location.AreaAndDev.Point> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Point> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Point ToTModel(this DbModel.Location.AreaAndDev.Point item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Point();
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Index = item1.Index;
            item2.BoundId = item1.BoundId;
            //item2.Bound = item1.Bound.ToTModel();
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Point> ToTModel(
            this List<DbModel.Location.AreaAndDev.Point> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Point>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Point ToDbModel(this Location.TModel.Location.AreaAndDev.Point item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Point();
            item2.Update(item1);
            return item2;
        }

        public static DbModel.Location.AreaAndDev.Point Update(this DbModel.Location.AreaAndDev.Point item2, Location.TModel.Location.AreaAndDev.Point item1)
        {
            if (item1 == null) return null;
            item2.Id = item1.Id;
            item2.X = item1.X;
            item2.Y = item1.Y;
            item2.Z = item1.Z;
            item2.Index = item1.Index;
            item2.BoundId = item1.BoundId;
            //item2.Bound = item1.Bound.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Point> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Point> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Point>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

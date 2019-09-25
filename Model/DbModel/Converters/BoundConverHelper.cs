using DbModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Converters
{
    public static class BoundConverHelper
    {
        #region Location.TModel.Location.AreaAndDev.Bound <=> DbModel.Location.AreaAndDev.Bound

        public static List<Location.TModel.Location.AreaAndDev.Bound> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.Bound> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Bound ToTModel(this DbModel.Location.AreaAndDev.Bound item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Bound();
            item2.Id = item1.Id;
            item2.MinX = item1.MinX;
            item2.MaxX = item1.MaxX;
            item2.MinY = item1.MinY;
            item2.MaxY = item1.MaxY;
            item2.MinZ = item1.MinZ;
            item2.MaxZ = item1.MaxZ;
            item2.ZeroX = item1.ZeroX;
            item2.ZeroY = item1.ZeroY;
            item2.Shape = item1.Shape;
            item2.IsRelative = item1.IsRelative;
            item2.Points = item1.Points.ToTModel();
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Bound> ToTModel(
            this List<DbModel.Location.AreaAndDev.Bound> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Bound>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.Bound ToDbModel(this Location.TModel.Location.AreaAndDev.Bound item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.Bound();
            item2.Update(item1);
            return item2;
        }

        public static DbModel.Location.AreaAndDev.Bound Update(this DbModel.Location.AreaAndDev.Bound item2, Location.TModel.Location.AreaAndDev.Bound item1)
        {
            if (item1 == null) return null;
            item2.Id = item1.Id;
            item2.MinX = item1.MinX;
            item2.MaxX = item1.MaxX;
            item2.MinY = item1.MinY;
            item2.MaxY = item1.MaxY;
            item2.MinZ = item1.MinZ;
            item2.MaxZ = item1.MaxZ;
            item2.ZeroX = item1.ZeroX;
            item2.ZeroY = item1.ZeroY;
            item2.Shape = item1.Shape;
            item2.IsRelative = item1.IsRelative;
            item2.Points = item1.Points.ToDbModel();
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.Bound> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Bound> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.Bound>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

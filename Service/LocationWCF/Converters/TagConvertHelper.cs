using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Tools;

namespace LocationServices.Converters
{
    public static class TagConvertHelper
    {
        #region Location.TModel.Location.AreaAndDev.Tag <=> DbModel.Location.AreaAndDev.LocationCard

        public static List<Location.TModel.Location.AreaAndDev.Tag> ToWcfModelList(
            this List<DbModel.Location.AreaAndDev.LocationCard> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.AreaAndDev.Tag ToTModel(
            this DbModel.Location.AreaAndDev.LocationCard item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.AreaAndDev.Tag();
            item2.Id = item1.Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.Describe = item1.Describe;
            item2.CardRoleId = item1.CardRoleId ?? 0;
            item2.IsActive = item1.IsActive;
            return item2;
        }

        public static List<Location.TModel.Location.AreaAndDev.Tag> ToTModel(
            this List<DbModel.Location.AreaAndDev.LocationCard> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.AreaAndDev.Tag>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbModel.Location.AreaAndDev.LocationCard ToDbModel(
            this Location.TModel.Location.AreaAndDev.Tag item1)
        {
            if (item1 == null) return null;
            var item2 = new DbModel.Location.AreaAndDev.LocationCard();
            item2.Update(item1);
            return item2;
        }

        public static DbModel.Location.AreaAndDev.LocationCard Update(
            this DbModel.Location.AreaAndDev.LocationCard item2,Location.TModel.Location.AreaAndDev.Tag item1)
        {
            if (item1 == null) return item2;
            item2.Id = item1.Id;
            //item2.Abutment_Id = item1.Abutment_Id;
            item2.Code = item1.Code;
            item2.Name = item1.Name;
            item2.Describe = item1.Describe;
            int roleId = item1.CardRoleId;

            if(roleId == 0)
            {
                item2.CardRoleId = null;
            }
            else
            {
                item2.CardRoleId = roleId;
            }

            //item2.CardRoleId = roleId;
            item2.IsActive = item1.IsActive;
            return item2;
        }

        public static List<DbModel.Location.AreaAndDev.LocationCard> ToDbModel(
            this List<Location.TModel.Location.AreaAndDev.Tag> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbModel.Location.AreaAndDev.LocationCard>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

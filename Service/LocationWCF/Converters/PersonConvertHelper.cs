using DbModel.Tools;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Nodes;
using TEntity = Location.TModel.Location.Person.Personnel;
using DbEntity = DbModel.Location.Person.Personnel;

namespace LocationServices.Converters
{
    public static class PersonConvertHelper
    {
        #region Location.TModel.Location.Person.Personnel <=> DbModel.Location.Person.Personnel

        public static List<TEntity> ToWcfModelList(
            this List<DbEntity> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TEntity ToTModel(this DbEntity item1)
        {
            if (item1 == null) return null;
            var item2 = new TEntity();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Name = item1.Name;
            item2.Sex = item1.Sex.ToString();
            item2.Photo = item1.Photo;
            item2.BirthDay = item1.BirthDay;
            item2.BirthTimeStamp = item1.BirthTimeStamp;
            item2.Nation = item1.Nation;
            item2.Address = item1.Address;
            item2.WorkNumber = item1.WorkNumber;
            item2.Email = item1.Email;
            item2.PhoneNumber = item1.Phone;
            item2.Mobile = item1.Mobile;
            item2.Enabled = item1.Enabled;
            item2.ParentId = item1.ParentId;
            item2.Pst = item1.Pst;
            return item2;
        }

        public static PersonNode ToTModelS(this DbEntity item1)
        {
            if (item1 == null) return null;
            var item2 = new PersonNode();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.Sex = item1.Sex.ToString();
            item2.ParentId = item1.ParentId;
            return item2;
        }

        public static PersonNode ToTModelS(this TEntity item1)
        {
            if (item1 == null) return null;
            var item2 = new PersonNode();
            item2.Id = item1.Id;
            item2.Name = item1.Name;
            item2.Sex = item1.Sex.ToString();
            item2.ParentId = item1.AreaId;
            return item2;
        }

        public static List<PersonNode> ToTModelS(
    this List<DbEntity> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<PersonNode>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModelS());
            }
            return list2;
        }

        public static List<TEntity> ToTModel(
            this List<DbEntity> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<TEntity>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static DbEntity ToDbModel(this TEntity item1)
        {
            if (item1 == null) return null;
            var item2 = new DbEntity();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Name = item1.Name;
            item2.Sex = item1.Sex == "男" ? Sexs.男 : item1.Sex == "女" ? Sexs.女 : Sexs.未知;
            item2.Photo = item1.Photo;
            item2.BirthDay = item1.BirthDay;
            item2.BirthTimeStamp = TimeConvert.DateTimeToTimeStamp((DateTime)item1.BirthDay);
            item2.Nation = item1.Nation;
            item2.Address = item1.Address;
            item2.WorkNumber = item1.WorkNumber;
            item2.Email = item1.Email;
            item2.Phone = item1.PhoneNumber;
            item2.Mobile = item1.Mobile;
            item2.Enabled = item1.Enabled;
            item2.ParentId = item1.ParentId;
            item2.Pst = item1.Pst;
            return item2;
        }

        public static List<DbEntity> ToDbModel(
            this List<TEntity> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<DbEntity>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }

        #endregion
    }
}

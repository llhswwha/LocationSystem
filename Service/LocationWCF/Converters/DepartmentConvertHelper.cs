using System.Collections.Generic;
using DbModel.Tools;
using TEntity = Location.TModel.Location.Person.Department;
using DbEntity = DbModel.Location.Person.Department;

namespace LocationServices.Converters
{
    public static class DepartmentConvertHelper
    {
        //Location.TModel.Location.Person.Department <=> DbModel.Location.AreaAndDev.Department

        public static List<TEntity> ToWcfModelList(this List<DbEntity> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static TEntity ToTModel(this DbEntity item1, bool onlyself = false)
        {
            try
            {
                if (item1 == null) return null;
                var item2 = new TEntity();
                item2.Id = item1.Id;
                item2.Abutment_Id = item1.Abutment_Id;
                item2.Name = item1.Name;
                if (onlyself == false)
                    item2.Parent = item1.Parent.ToTModel(true);
                item2.ParentId = item1.ParentId;
                item2.ShowOrder = item1.ShowOrder;
                item2.Type = (int)item1.Type;
                item2.Description = item1.Description;
                if (onlyself == false)
                    item2.Children = item1.Children.ToTModel(onlyself);
                if (onlyself == false)
                    item2.LeafNodes = item1.LeafNodes.ToTModel();
                return item2;
            }
            catch (System.Exception ex)
            {
                LogEvent.Error(ex);
                return null;
            }
            
        }

        public static List<TEntity> ToTModel(this List<DbEntity> list1,bool onlySelf=false)
        {
            if (list1 == null) return null;
            var list2 = new List<TEntity>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel(onlySelf));
            }
            return list2;
        }

        public static DbEntity ToDbModel(this TEntity item1, bool onlyself = false)
        {
            try
            {
                if (item1 == null) return null;
                var item2 = new DbEntity();
                item2.Id = item1.Id;
                item2.Abutment_Id = item1.Abutment_Id;
                item2.Name = item1.Name;
                if (onlyself == false)
                    item2.Parent = item1.Parent.ToDbModel(true);
                item2.ParentId = item1.ParentId;
                item2.ShowOrder = item1.ShowOrder;
                item2.Type = (DepartType)item1.Type;
                item2.Description = item1.Description;
                if (onlyself == false)
                    item2.Children = item1.Children.ToDbModel(onlyself);
                if (onlyself == false)
                    item2.LeafNodes = item1.LeafNodes.ToDbModel();
                return item2;
            }
            catch (System.Exception ex)
            {
                LogEvent.Error(ex);
                return null;
            }
        }

        public static List<DbEntity> ToDbModel(this List<TEntity> list1,bool onlySelf=false)
        {
            if (list1 == null) return null;
            var list2 = new List<DbEntity>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel(onlySelf));
            }
            return list2;
        }
    }
}

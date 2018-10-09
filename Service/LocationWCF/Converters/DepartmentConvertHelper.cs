using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Location.Person;
using DbModel.Tools;

namespace LocationServices.Converters
{
    public static class DepartmentConvertHelper
    {
        //Location.TModel.Location.Person.Department <=> DbModel.Location.AreaAndDev.Department

        public static List<Location.TModel.Location.Person.Department> ToWcfModelList(this List<Department> list1)
        {
            return list1.ToTModel().ToWCFList();
        }

        public static Location.TModel.Location.Person.Department ToTModel(this Department item1)
        {
            if (item1 == null) return null;
            var item2 = new Location.TModel.Location.Person.Department();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Name = item1.Name;
            item2.Parent = item1.Parent.ToTModel();
            item2.ParentId = item1.ParentId;
            item2.ShowOrder = item1.ShowOrder;
            item2.Type = (int)item1.Type;
            item2.Description = item1.Description;
            item2.Children = item1.Children.ToTModel();
            item2.LeafNodes = item1.LeafNodes.ToTModel();
            return item2;
        }

        public static List<Location.TModel.Location.Person.Department> ToTModel(this List<Department> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Location.TModel.Location.Person.Department>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToTModel());
            }
            return list2;
        }

        public static Department ToDbModel(this Location.TModel.Location.Person.Department item1)
        {
            if (item1 == null) return null;
            var item2 = new Department();
            item2.Id = item1.Id;
            item2.Abutment_Id = item1.Abutment_Id;
            item2.Name = item1.Name;
            item2.Parent = item1.Parent.ToDbModel();
            item2.ParentId = item1.ParentId;
            item2.ShowOrder = item1.ShowOrder;
            item2.Type = (DepartType)item1.Type;
            item2.Description = item1.Description;
            item2.Children = item1.Children.ToDbModel();
            item2.LeafNodes = item1.LeafNodes.ToDbModel();
            return item2;
        }

        public static List<Department> ToDbModel(this List<Location.TModel.Location.Person.Department> list1)
        {
            if (list1 == null) return null;
            var list2 = new List<Department>();
            foreach (var item1 in list1)
            {
                list2.Add(item1.ToDbModel());
            }
            return list2;
        }
    }
}

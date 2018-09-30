using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Location.TModel.ConvertCodes
{
    public class ConvertCodeGenerator
    {
        public string Code { get; set; }

        public Type TypeT { get; set; }

        public Type TypeDb { get; set; }

        public string TypeTName
        {
            get { return TypeT.FullName; }
        }

        public string TypeDbName
        {
            get { return TypeDb.FullName; }
        }

        public ConvertCodeGenerator(Type typeT, Type typeDb)
        {
            TypeT = typeT;
            TypeDb = typeDb;
        }

        private void GetToWcfModelList()
        {
            /*
        public static List<Tag> ToWcfModelList(this List<LocationCard> list1)
        {
            return list1.ToTModel().ToWCFList();
        }
             */
            Code += string.Format("        public static List<{0}> ToWcfModelList(this List<{1}> list1)\n", TypeTName, TypeDbName);
            Code += "        {\n";
            Code += "            return list1.ToTModel().ToWCFList();\n";
            Code += "        }\n";

        }

        private void GetToTModel()
        {
            GetToModel("ToTModel", TypeT, TypeDb);
        }

        private void GetToTModelList()
        {
            GetToModelList("ToTModel", TypeTName, TypeDbName);
        }

        private void GetToModelList(string method,string typeTo,string typeFrom)
        {
            Code += string.Format("        public static List<{0}> {2}(this List<{1}> list1)\n", typeTo, typeFrom, method);
            Code += "        {\n";
            Code += "            if (list1 == null) return null;\n";
            Code += string.Format("            var list2 = new List<{0}>();\n", typeTo);
            Code += "            foreach (var item1 in list1)\n";
            Code += "            {\n";
            Code += string.Format("                list2.Add(item1.{0}());\n", method);
            Code += "            }\n";
            Code += "            return list2;\n";
            Code += "        }\n";
        }

        private void GetToDbModel()
        {
            GetToModel("ToDbModel", TypeDb, TypeT);
        }

        private void GetToModel(string method,Type typeTo,Type typeFrom)
        {
            Code += string.Format("        public static {0} {2}(this {1} item1)\n", typeTo.FullName, typeFrom.FullName,
                method);
            Code += "        {\n";
            Code += "            if (item1 == null) return null;\n";
            Code += string.Format("            var item2 = new {0}();\n", typeTo.FullName);
            PropertyInfo[] propertyInfos2 = typeTo.GetProperties();
            foreach (var propertyInfo1 in propertyInfos2)
            {
                SetPropertyValue(method, typeFrom, propertyInfo1);
            }

            Code += "            return item2;\n";
            Code += "        }\n";
        }

        private PropertyInfo FindProperty(Type type, PropertyInfo property)
        {
            string propertyName = property.Name;
            ByNameAttribute byNameAttribute = Attribute.GetCustomAttribute(property, typeof(ByNameAttribute)) as ByNameAttribute;
            if (byNameAttribute != null)
            {
                propertyName = byNameAttribute.Name;
            }
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null) return propertyInfo;
            propertyInfo = type.GetProperty(propertyName.ToLower());//全小写
            if (propertyInfo != null) return propertyInfo;
            string property2 = propertyName[0].ToString().ToUpper() + propertyName.Substring(1);//小写转大写
            propertyInfo = type.GetProperty(property2);
            if (propertyInfo != null) return propertyInfo;
            string property3 = propertyName[0].ToString().ToLower() + propertyName.Substring(1);//大写转小写
            propertyInfo = type.GetProperty(property3);
            if (propertyInfo != null) return propertyInfo;
            return null;
        }

        private void SetPropertyValue(string method, Type typeFrom, PropertyInfo propertyInfo1)
        {
            PropertyInfo propertyInfo2 = FindProperty(typeFrom,propertyInfo1);
            if (propertyInfo2 != null)
            {
                if (propertyInfo1.PropertyType.FullName.StartsWith("System.")
                    || propertyInfo1.PropertyType.IsEnum) //基本类型
                {
                    if (propertyInfo1.PropertyType.FullName.Contains("System.Collections.Generic.List`1")) //列表
                    {
                        if (propertyInfo1.PropertyType.FullName.Contains("System.Collections.Generic.List`1[[System."))
                        {
                            SetPropertyValue(propertyInfo1.Name, propertyInfo2.Name);
                        }
                        else
                        {
                            SetPropertyValue(propertyInfo1.Name,propertyInfo2.Name, method);
                        }

                    }
                    else
                    {
                        SetPropertyValue(propertyInfo1.Name, propertyInfo2.Name);
                    }
                }
                else
                {
                    SetPropertyValue(propertyInfo1.Name, propertyInfo2.Name, method);
                }
            }
            else
            {
                Code += string.Format("            //item2.{0} = item1.{0};\n", propertyInfo1.Name);
            }
        }
        private void SetPropertyValue(string property1,string property2)
        {
            Code += string.Format("            item2.{0} = item1.{1};\n", property1, property2);
        }
        //private void SetPropertyValue(string property)
        //{
        //    Code += string.Format("            item2.{0} = item1.{0};\n", property);
        //}
        private void SetPropertyValue(string property1, string property2, string method)
        {
            Code += string.Format("            item2.{0} = item1.{1}.{2}();\n", property1, property2,
                                method);
        }

        private void GetToDbModelList()
        {
            GetToModelList("ToDbModel", TypeDbName, TypeTName);
        }

        public string GetCode()
        {
            Code = "";
            Code += string.Format("        #region {0} <=> {1}\n", TypeTName, TypeDbName);
            GetToWcfModelList();
            GetToTModel();
            GetToTModelList();
            GetToDbModel();
            GetToDbModelList();
            Code += string.Format("        #endregion\n");
            return Code;
        }

        public static List<Type> FindSimilarTypes(List<Type> types, Type type)
        {
            ByNameAttribute dbClass=Attribute.GetCustomAttribute(type, typeof (ByNameAttribute)) as ByNameAttribute;
            if (dbClass != null)
            {
                return FindSimilarTypes(types, dbClass.Name);
            }
            else
            {
                return FindSimilarTypes(types, type.Name);
            }
        }
        public static List<Type> FindSimilarTypes(List<Type> types, string name)
        {
            List<Type> types1 = types.FindAll(i => i.Name.ToLower() == name.ToLower());
            if (types1.Count > 0) return types1;
            List<Type> types2 = types.FindAll(i => i.Name.ToLower().Contains(name.ToLower()));
            if (types2.Count > 0) return types2;
            List<Type> types3 = types.FindAll(i => i.Name.ToLower().Contains(name.ToLower()+"s"));
            return types3;
        }

        public static Type FindSimilarType(List<Type> types1,Type type1)
        {
            List<Type> types = FindSimilarTypes(types1,type1);
            if (types.Count > 0)
            {
                return types[0];
            }
            return null;
        }

        public static string GetCode(List<Type> types1, List<Type> types2)
        {
            List<Type> types11 = new List<Type>();
            List<Type> types21 = new List<Type>(types2);

            string code = "";

            foreach (Type type1 in types1)
            {
                Type type2 = FindSimilarType(types2, type1);
                if (type2 == null)
                {
                    types11.Add(type1);
                    continue;
                }
                types21.Remove(type2);

                ConvertCodeGenerator coders = new ConvertCodeGenerator(type1, type2);
                code += coders.GetCode() + "\n";
            }

            string other = "";
            other += string.Format("//未找到DbModel的类({0}):\n//", types11.Count);
            foreach (Type type in types11)
            {
                other += type.Name + "; ";
            }
            other += "\n";

            other += string.Format("//未找到TModel的类({0}):\n//", types21.Count);
            foreach (Type type in types21)
            {
                other += type.Name + "; ";
            }
            other += "\n";

            code = "        #region ConvertCodeGenerator自动生成代码\n" + other + code + "        #endregion\n";
            return code;
        }
    }
}

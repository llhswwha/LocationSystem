using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Location.TModel.Location.AreaAndDev;

namespace TModelCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = typeof (Tag).Assembly;

            List<Type> types = new List<Type>();
            foreach (Type i in assembly.GetTypes())
            {
                if (i.BaseType == typeof(Attribute)) continue;

                if (i.IsClass
                    && !i.IsAbstract
                    && !i.FullName.Contains("Tools")
                    && !i.FullName.Contains("InitInfo")
                    && !i.FullName.Contains("ConvertCodes"))
                {
                    ObsoleteAttribute obsoleteAttribute = i.GetCustomAttribute<ObsoleteAttribute>();
                    if (obsoleteAttribute != null)
                    {
                        continue;
                    }
                    types.Add(i);
                }
            }

            string txt = "";
            foreach (Type type in types)
            {
                txt += CheckType(type);
            }

            Console.WriteLine(txt);
            Console.Read();
        }

        private static string CheckType(Type type)
        {
            bool flag = false;
            string txt = "";
            txt += string.Format("检查类型:{0}\n", type);
            DataContractAttribute attribute = type.GetCustomAttribute<DataContractAttribute>();
            if (attribute == null)
            {
                txt += string.Format("没有 DataContract ！！\n");
                flag = true;
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                DataMemberAttribute attribute2 = propertyInfo.GetCustomAttribute<DataMemberAttribute>();
                if (attribute2 == null)
                {
                    txt += string.Format("没有DataMember:{0}\n", propertyInfo);
                    flag = true;
                }
            }

            txt += string.Format("\n");
            if (flag)
            {
                return txt;
            }
            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Location.TModel.Tools
{
    public static class ConvertHelper
    {
        public static IList<T> CloneObjectList<T>(this IList<T> list) where T : class, new()
        {
            if (list == null) return null;
            IList<T> listNew = new List<T>();
            foreach (T item in list)
            {
                T itemNew = CloneObjectByBinary(item);
                listNew.Add(itemNew);
            }
            return listNew;
        }

        public static T CloneObjectByBinary<T>(this T obj) where T : class
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            return formatter.Deserialize(stream) as T;
        }

        public static object ToType(object value, Type type)
        {
            if (type == typeof (Int16)|| type == typeof(Int16?))
            {
                if (value == "") return null;
                return Convert.ToInt16(value);
            }
            else if (type == typeof(Int32) || type == typeof(Int32?))
            {
                if (value == "") return null;
                return Convert.ToInt32(value);
            }
            else if (type == typeof(Int64) || type == typeof(Int64?))
            {
                if (value == "") return null;
                return Convert.ToInt64(value);
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                if (value == "") return null;
                return Convert.ToDouble(value);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                if (value == "") return null;
                return (float)Convert.ToDouble(value);
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                if (value == "") return null;
                return Convert.ToBoolean(value);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                if (value == "") return null;
                return Convert.ToDateTime(value);
            }
            else if (type.BaseType == typeof(Enum))
            {
                var names= Enum.GetNames(type);
                var id = names.ToList().IndexOf(value.ToString());
                var values=Enum.GetValues(type);
                return values.GetValue(id);
            }
            else if (value == "")
            {
                if (type.BaseType == typeof(Object))
                {
                    return null;
                }
            }
            return value;
        }

        public static bool SetValueEx(this PropertyInfo pt, object obj,object value)
        {
            try
            {
                var vaule2 = ToType(value, pt.PropertyType);
                pt.SetValue(obj, vaule2,null);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}

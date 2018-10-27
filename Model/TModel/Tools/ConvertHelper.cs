using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Location.TModel.Tools
{
    public static class ConvertHelper
    {
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
            if (type == typeof (Int16))
            {
                return Convert.ToInt16(value);
            }
            if (type == typeof(Int32))
            {
                return Convert.ToInt32(value);
            }
            if (type == typeof(Int64))
            {
                return Convert.ToInt64(value);
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

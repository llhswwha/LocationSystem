using Location.IModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Tool
{
    public static class CloneHelper
    {
        public static T CloneObject<T>(this T obj) where T : class,new()
        {
            return CloneByDataContract(obj);
        }

        public static T CloneObjectByProperties<T>(this T obj) where T : new()
        {
            if (obj == null) return default(T);

            T objNew = new T();
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    DataMemberAttribute attribue = Attribute.GetCustomAttribute(property, typeof(DataMemberAttribute)) as DataMemberAttribute;
                    object value = property.GetValue(obj);
                    property.SetValue(objNew, value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return objNew;
        }


        public static IList<T> CloneObjectList<T>(this IList<T> list) where T : class, new()
        {
            if (list == null) return null;
            IList<T> listNew = new List<T>();
            foreach (T item in list)
            {
                T itemNew = CloneObject(item);
                listNew.Add(itemNew);
            }
            return listNew;
        }

        public static T CloneByDataContract<T>(this T obj)
        {
            var serializer = new DataContractSerializer(typeof(T), new DataContractSerializerSettings()
            {
                DataContractResolver = new ProxyDataContractResolver()
            });
            using (var stream = new MemoryStream())
            {
                // 反序列化
                serializer.WriteObject(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                T objNew = (T)serializer.ReadObject(stream);
                return objNew;
            }
        }

        public static T CloneObjectByBinary<T>(this T obj) where T : class
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            return formatter.Deserialize(stream) as T;
        }
    }
}

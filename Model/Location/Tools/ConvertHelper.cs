using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.Tools
{
    public static class ConvertHelper
    {
        public static List<T2> ConvertObjectList<T1, T2>(this IList<T1> list) where T2 : class, new()
        {
            if (list == null) return null;
            Stopwatch stopwatch=new Stopwatch();
            stopwatch.Start();
            List<T2> listNew = new List<T2>();
            foreach (T1 item in list)
            {
                T2 itemNew = ConvertObjectTo<T1, T2>(item);
                listNew.Add(itemNew);
            }
            stopwatch.Stop();
            LogEvent.Info("ConvertObjectList Time:" + stopwatch.Elapsed);
            return listNew;
        }

        public static T2 ConvertObjectTo<T1, T2>(this T1 obj) where T2 : class, new()
        {
            //return ConvertObjectByContract<T1, T2>(obj);//不行
            return ConvertObjectByReflect<T1, T2>(obj);//不行
        }

        public static T2 ConvertObjectByReflect<T1, T2>(this T1 obj1) where T2 : class, new()
        {
            try
            {
                Type type1 = typeof (T1);
                Type type2 = typeof (T2);
                T2 obj2 = new T2();
                //PropertyInfo[] propertyInfos1 = type1.GetProperties();
                PropertyInfo[] propertyInfos2 = type2.GetProperties();
                foreach (PropertyInfo p2 in propertyInfos2)
                {
                    PropertyInfo p1=type1.GetProperty(p2.Name);
                    object value = p1.GetValue(obj1);
                    p2.SetValue(obj2, value);
                }
                return obj2;
            }
            catch (Exception ex)
            {
                LogEvent.Info(ex.ToString());
                return default(T2);
            }
        }

        public static T2 ConvertObjectByContract<T1, T2>(this T1 obj)
        {
            try
            {
                var serializer1 = new DataContractSerializer(typeof (T1), new DataContractSerializerSettings()
                {
                    DataContractResolver = new ProxyDataContractResolver()
                });
                var serializer2 = new DataContractSerializer(typeof (T2), new DataContractSerializerSettings()
                {
                    DataContractResolver = new ProxyDataContractResolver()
                });
                using (var stream = new MemoryStream())
                {
                    // 反序列化
                    serializer1.WriteObject(stream, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    T2 objNew = (T2) serializer2.ReadObject(stream);
                    return objNew;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T2);
            }
        }
    }
}

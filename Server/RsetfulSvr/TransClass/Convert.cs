using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.IO;

namespace TransClass
{
    public class Convert<T>
    {

        //将json字符串，有时间字段的转换为可以序列化的格式，再序列化为类
        public static T JsonDeserialize(string jsonString)
        {
            //string pattern = @"(\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2})|(\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2})";
            //string pattern = @"(\d{4}-\d{2}-\d{2})|(\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2})";
            string pattern = @"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)(?:([\+-])(\d{2})\:(\d{2}))?Z?";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(pattern);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            return StringToClass(jsonString);
        }

        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        //将json字符串序列化为类
        public static T StringToClass(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);

            return obj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace TransClass
{
    public class GetMethod<T>
    {
        //通过Get方法获取的数据为json格式，序列化为类对象
        public static T Get(string strUrl)
        {
            T recv = Activator.CreateInstance<T>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = "GET";
            request.ContentType = "application/json;charest=UTF-8";
            request.UserAgent = null;
            //request.Timeout = 10000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retstring = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            recv = TransClass.Convert<T>.StringToClass(retstring);

            return recv;
        }

        //通过Get方法获取的数据为json格式，且有DateTime字段，序列化为类对象
        public static T GetHasDateTime(string strUrl)
        {
            T recv = Activator.CreateInstance<T>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = "GET";
            request.ContentType = "application/json;charest=UTF-8";
            request.UserAgent = null;
            //request.Timeout = 10000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retstring = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            recv = TransClass.Convert<T>.JsonDeserialize(retstring);

            return recv;
        }

        //通过Get方法获取的数据为json格式，但不序列化
        public static string GetNoSerialize(string strUrl)
        {
            T recv = Activator.CreateInstance<T>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Method = "GET";
            request.ContentType = "application/json;charest=UTF-8";
            request.UserAgent = null;
            //request.Timeout = 10000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retstring = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retstring;
        }
    }
}

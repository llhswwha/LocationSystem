using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CommunicationClass.SihuiThermalPowerPlant
{
    public class PostMethod<T>
    {
        public static T Post(string strUrl, T send)
        {
            T recv = Activator.CreateInstance<T>();
            string json = JsonConvert.SerializeObject(send);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            request.Method = "Post";
            request.ContentType = "application/json";
            //request.ContentLength = Encoding.UTF8.GetByteCount(json);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(json);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retstring = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            recv = CommunicationClass.SihuiThermalPowerPlant.Convert<T>.JsonDeserialize(retstring);
            return recv;
        }
    }
}

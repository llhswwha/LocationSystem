using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Location.BLL.Tool;

namespace WebApiLib
{
    public static class WebApiHelper
    {
        public static string GetString(string uri, string accept = "")
        {
            try
            {
                if (uri == null) return null;
                var client = new HttpClient();
                if (!string.IsNullOrEmpty(accept))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                }
                var resMsg = client.GetAsync(uri).Result;
                var result = resMsg.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
            }
        }

        public static T GetEntity<T>(string uri)
        {
            string result = GetString(uri);
            if (result == null) return default(T);
            T obj = JsonConvert.DeserializeObject<T>(result);
            return obj;
        }

        public static string PostEntity<T>(string uri, T data)
        {
            return Invoke(uri, data, "POST");
        }

        public static string PutEntity<T>(string uri, T data)
        {

            return Invoke(uri, data, "PUT");
        }

        public static string Invoke<T>(string uri, T data, string method)
        {
            string json = JsonConvert.SerializeObject(data);
            return Invoke(uri, json, method);
        }

        public static string Invoke(string uri, string data, string method)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.ContentType = "application/json";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            string location = response.Headers["Location"];
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return result;
        }

        public static string Delete(string uri)
        {
            var client = new HttpClient();
            var resMsg = client.DeleteAsync(uri).Result;
            var result = resMsg.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}

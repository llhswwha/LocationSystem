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

        public static bool IsSaveJsonToFile = false;

        public static string GetString(string uri, string accept = "")
        {
            try
            {
                if (uri == null) return null;
                Log.Info(LogTags.BaseData, "uri:" + uri);
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
                //Log.Error(LogTags.Server, string.Format("WebApiHelper.GetString:uri={0},error={1}",uri,ex.Message));
                //return null;
                throw ex;
            }
        }


        public static T GetEntity<T>(string url)
        {
            if (url == null) return default(T);
            if (url.Contains("?"))
            {
                url += "&offset=0&limit=10000";
            }
            else
            {
                url += "?offset=0&limit=10000";
            }

            string result = GetString(url);
            if (result == null) return default(T);

            if (IsSaveJsonToFile)
            {
                SaveJson(url, result);
            }

            if (result.Contains("404 Not Found"))
            {
                throw new Exception("404 Not Found");
            }
            T obj = JsonConvert.DeserializeObject<T>(result);
            return obj;
        }

        private static void SaveJson(string url, string result)
        {
            try
            {
                Uri uri = new Uri(url);
                DateTime now = DateTime.Now;
                string name = uri.Segments[uri.Segments.Length - 1];

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Json\\" + name + "__" +
                              now.ToString("yyyy_mm_dd_HH_MM_ss_fff");

                if (path.Length > 240)
                {
                    path = path.Substring(0, 240);
                }

                path += ".json";

                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                string content = "//" + url + "\n" + result;
                File.WriteAllText(path, content);
            }
            catch (Exception ex)
            {
                Log.Error("SaveJson:"+ex);
            }
        }

        public static string PostEntity<T>(string uri, T data, bool withResult)
        {
            return InvokeEntity(uri, data, "POST", withResult);
        }

        public static string PutEntity<T>(string uri, T data, bool withResult)
        {

            return InvokeEntity(uri, data, "PUT", withResult);
        }

        public static string InvokeEntity<T>(string uri, T data, string method, bool withResult)
        {
            string json = "";
            if (typeof(T) == typeof(string))
            {
                json = data+"";
            }
            else
            {
                json = JsonConvert.SerializeObject(data);
            }
            
            return Invoke(uri, json, method, withResult);
        }

        public static string Invoke(string uri, string data, string method,bool withResult)
        {
            string result = null;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = method;
                request.ContentType = "application/json";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(bytes, 0, bytes.Length);
                dataStream.Close();

                if (withResult)
                {
                    WebResponse response = request.GetResponse();//这里可能会卡住
                                                                 //string location = response.Headers["Location"];
                    result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                else
                {
                    result = null;//只管发送，发送好了，就不管结果
                }
            }
            catch (Exception ex)
            {
                Log.Error("WebApiHelper.Invoke:"+ex);
            }

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

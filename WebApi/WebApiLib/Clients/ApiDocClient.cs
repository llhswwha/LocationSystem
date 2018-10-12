using System;
using System.Net.Http;
using WebApiLib.ApiDocs;

namespace WebApiLib.Clients
{
    public class ApiDocClient
    {
        public ApiDocument Document { get; set; }

        public ApiDocClient()
        {
            
        }

        public ApiDocClient(ApiDocument doc)
        {
            Document = doc;
        }

        public string TestApis()
        {
            string testResult = "";
            foreach (ApiPath apiPath in Document.Paths)
            {
                string url = apiPath.GetUrl();

                if (string.IsNullOrEmpty(url))
                {
                    testResult += string.Format("path:\n{0}\nStatus:{1}\nResult:\n{2}\n", apiPath, "NoUri", "");
                    apiPath.SetResultState("NoUri");
                }
                else
                {
                    try
                    {
                        var client = new HttpClient();
                        HttpResponseMessage resMsg = client.GetAsync(url).Result;
                        var result = resMsg.Content.ReadAsStringAsync().Result;
                        //string resMsg=WebApiHelper.GetString(url);
                        testResult += string.Format("url:\n{0}\npath:\n{1}\nStatus:{2}\nResult:\n{3}\n", url, apiPath, resMsg.StatusCode, result);
                        apiPath.SetResultState(string.Format("[{0}]{1}", (int)resMsg.StatusCode,resMsg.StatusCode));
                    }
                    catch (Exception ex)
                    {
                        testResult += string.Format("url:\n{0}\npath:\n{1}\nStatus:{2}\nResult:\n{3}\n", url, apiPath, "Error", ex);
                        apiPath.SetResultState("Error");
                    }
                }

            }
            return testResult;
        }
    }
}

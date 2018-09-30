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
                    var client = new HttpClient();
                    HttpResponseMessage resMsg = client.GetAsync(url).Result;
                    var result = resMsg.Content.ReadAsStringAsync().Result;
                    //string resMsg=WebApiHelper.GetString(url);
                    testResult += string.Format("path:\n{0}\nStatus:{1}\nResult:\n{2}\n", apiPath, resMsg.StatusCode, result);
                    apiPath.SetResultState(resMsg.StatusCode.ToString());
                }

            }
            return testResult;
        }
    }
}

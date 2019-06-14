using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLib.Clients
{
    public class WebApiClient
    {
        public string BaseUri { get; set; }

        public string Accept { get; set; }

        public WebApiClient()
        {
            BaseUri = "http://<host>:<port>/";
        }

        public WebApiClient(string host, string port)
        {
            BaseUri = string.Format("http://{0}:{1}/", host, port);
        }

        public WebApiClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        public string GetString(string path="")
        {
            return WebApiHelper.GetString(string.Format("{0}{1}", BaseUri, path),Accept);
        }

        public string PostEntity<T>(string uri, T data, bool withResult)
        {
            string fullUri = string.Format("{0}{1}", BaseUri, uri);
            return WebApiHelper.PostEntity<T>(fullUri, data, withResult);
        }

        public string PutEntity<T>(string uri, T data, bool withResult)
        {
            string fullUri = string.Format("{0}{1}", BaseUri, uri);
            return WebApiHelper.PutEntity<T>(fullUri, data, withResult);
        }
    }
}

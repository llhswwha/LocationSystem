using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace WebApiLib.ApiDocs
{
    public class ApiDocument
    {
        public string BaseUri { get; set; }

        public List<ApiPath> Paths { get; set; } 

        public ApiDocument()
        {
            BaseUri = "http://<host>:<port>/";
        }

        public ApiDocument(string host, string port)
        {
            BaseUri = string.Format("http://{0}:{1}/", host, port);
        }

        public ApiDocument(string baseUri)
        {
            BaseUri = baseUri;
            
        }

        public bool LoadDoc(string htmlFilePath)
        {
            Paths = new List<ApiPath>();
            try
            {
                var doc = new XmlDocument();
                doc.Load(htmlFilePath);
                var tbodys = doc.GetElementsByTagName("tbody");
                var tbody = tbodys[0];
                foreach (XmlNode tr in tbody.ChildNodes)
                {
                    var path = new ApiPath(tr, this);
                    Paths.Add(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }
    }
}

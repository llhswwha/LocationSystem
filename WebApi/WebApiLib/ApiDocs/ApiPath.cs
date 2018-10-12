using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebApiLib.ApiDocs
{
    public class ApiPath
    {
        public string Name { get; set; }
        public string Uri { get; set; }

        public string Method { get; set; }

        public string User { get; set; }

        public string Describe { get; set; }

        public string Result { get; set; }

        public void SetResultState(string result)
        {
            Result = result;
        }


        private ApiDocument _doc;
        public ApiPath()
        {
            
        }

        public ApiPath(XmlNode node,ApiDocument doc)
        {
            _doc = doc;
            Name = node.ChildNodes[0].InnerText;
            Uri = node.ChildNodes[1].InnerText;
            Method = node.ChildNodes[2].InnerText;
            User = node.ChildNodes[3].InnerText;
            Describe = node.ChildNodes[4].InnerText;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Uri);
        }

        public string GetUrl(string baseUri)
        {
            if (string.IsNullOrEmpty(Uri))
            {
                return "";
            }
            
            string url= baseUri + Uri;
            if (url.Contains("{id}"))
            {
                url = url.Replace("{id}", "1");
            }
            return url;
        }

        public string GetUrl()
        {
            return GetUrl(_doc.BaseUri);
        }
    }
}

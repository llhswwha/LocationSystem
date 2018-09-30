using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Net;
using System.IO;
using WebLocation.Models.Rsetful;

namespace WebLocation.Controllers
{
    public class RsetfulClientController : Controller
    {
        public string Index()
        {
            return "1";
        }

        public ActionResult Create()
        {
            Uri postUr1 = new Uri("http://localhost:10743/Svr/RsetfulSvr/Create/");

            GenericFeed<User> feed = new GenericFeed<User>();
            User user = new User();
            user.Id = Guid.NewGuid().ToString();
            user.Title = new TextSyndicationContent("测试");
            user.Name = "测试";
            user.Mail = "ceshi";

            var items = new List<User>();
            items.Add(user);
            feed.Items = items;

            
            string syr = "dsada";
            var request = WebRequest.CreateDefault(postUr1);
            request.Method = "POST";
            MemoryStream ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms))
            {
                feed.SaveAsAtom10(writer);
            }

            request.ContentType = "application/atom+xml";
            request.ContentLength = ms.Length;

            using (var sw = request.GetRequestStream())
            {
                sw.Write(ms.ToArray(), 0, (int)ms.Length);
            }

            var response = request.GetResponse() as HttpWebResponse;
            var stream = response.GetResponseStream();
            var feed2 = SyndicationFeed.Load<GenericFeed<User>>(XmlReader.Create(stream));
            var result = (from item in feed2.Items select item as User).ToList<User>();

            stream.Close();
            response.Close();

            return View(result);
        }

        public string Query()
        {
            Uri queryUri = new Uri("http://localhost:10743/Svr/RsetfulSvr/GetAll/");
            //Uri queryUri = new Uri("http://localhost:10743/Svr/RsetfulSvr/Archors/");

            var request = WebRequest.CreateDefault(queryUri);
            request.Method = "GET";

            request.ContentType = "application/atom+xml, charest=\"utf-8\"";
            var response = request.GetResponse() as HttpWebResponse;
           
            var stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            return result;
            //var feed = SyndicationFeed.Load<GenericFeed<User>>(XmlReader.Create(stream));

            //stream.Close();
            //response.Close();

            //var result = (from item in feed.Items select item as User).ToList<User>();

            //return View(result);
        }
    }
}
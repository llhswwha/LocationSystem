using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace WebLocation.Models.Rsetful
{

    public class GenericFeed<T> : SyndicationFeed where T : SyndicationItem, new()
    {
        protected override SyndicationItem CreateItem()
        {
            return new T();
        }
    }

    public class User : SyndicationItem
    {
        private const string OUTERNS = "http://sample.cleversoft.com/user/1.0";

        public string Name
        {
            get
            {
                return GetValue<string>("Name");
            }
            set
            {
                SetValue("Name", value);
            }
        }

        public string Password
        {
            get
            {
                return GetValue<string>("Password");
            }
            set
            {
                SetValue("Password", value);
            }
        }

        public string Mail
        {
            get
            {
                return GetValue<string>("Mail");
            }
            set
            {
                SetValue("Mail", value);
            }
        }


        private T GetValue<T>(string outerName)
        {
            return base.ElementExtensions.ReadElementExtensions<T>(outerName, OUTERNS)[0];
        }

        private void SetValue(string outerName, object value)
        {
            base.ElementExtensions.Add(outerName, OUTERNS, value);
        }


        //public Guid ID { get; set; }
        //public string Name { get; set; }
        //public string Password { get; set; }
        //public string Mail { get; set; }
    }

    public class SyndicationFeedResult : ActionResult
    {
        SyndicationFeed feed;

        string format;

        public SyndicationFeedResult(SyndicationFeed feed, string format)
        {
            this.feed = feed;
            this.format = format;
        }

        public SyndicationFeedResult(SyndicationFeed feed) : this(feed, "atom")
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            SyndicationFeedFormatter f = format == "atom" ? (SyndicationFeedFormatter)new Atom10FeedFormatter(feed) : (SyndicationFeedFormatter)new Rss20FeedFormatter(feed);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                f.WriteTo(writer);
            }

            int nnn = 0;
        }
    }
}
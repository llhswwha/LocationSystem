using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RsetfulSvr.Controllers;
using Location.BLL;
using RsetfulSvr.Models;
using System.Threading;
using RsetfulSvr.Controllers.Client;

namespace RsetfulSvr
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LocationBll bll = new LocationBll();
            int mode = int.Parse(ConfigurationManager.AppSettings["DataInitMode"]);//0:EF,1:Sql
            bll.Init(mode);

            TagPositionController tp = new TagPositionController();

            Thread th2 = new Thread(ShowAlarmController.NSQListen);
            th2.Start();

            Reference rf = Reference.Instance();
            rf.Init();
        }
    }
}

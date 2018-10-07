using Location.BLL;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BLL;
using Location.BLL.Tool;

using WebLocation.Controllers;
using WebApiService;
using System.Web.Http;

namespace WebLocation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Info("== Application_Start ==");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);//WebApi
            //WebApiConfiguration.Configure(GlobalConfiguration.Configuration);//WebApi 这两种写法都行
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure();
            
            InitData();
        }

        private void InitData()
        {
            Log.Info("InitData");
            int mode = int.Parse(ConfigurationManager.AppSettings["DataInitMode"]); //-1:不初始化,0:EF,1:Sql
            Log.Info("DataInitMode:" + mode);
            if (mode >= 0)
            {
                Log.InfoStart("MvcApplication.InitData");
                Bll bll = new Bll();
                bll.Init(mode);
                Log.InfoEnd("MvcApplication.InitData");
            }
        }
    }
}

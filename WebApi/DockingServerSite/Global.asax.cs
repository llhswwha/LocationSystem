using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SihuiThermalPowerPlant.Controllers;
using System.ServiceModel.Web;
using System.Threading;

namespace SihuiThermalPowerPlant
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

            zonesController.Init();
            devicesController.Init();
            cardsController.Init();
            ticketsController.Init();
            eventsController.Init();
            sisController.Init();
            orgController.Init();

            //cardsController cards = new cardsController();
            //WebServiceHost cardsHost = new WebServiceHost(cards, new Uri("http://localhost:9347/api/cards"));
            //cardsHost.Open();


            //devicesController devices = new devicesController();
            //WebServiceHost devicesHost = new WebServiceHost(devices, new Uri("http://localhost:9347/api/devices"));
            //devicesHost.Open();

            //sisController sis = new sisController();
            //WebServiceHost sisHost = new WebServiceHost(sis, new Uri("http://localhost:9347/api/rt/sis"));
            //sisHost.Open();

            //zonesController zones = new zonesController();
            //WebServiceHost zonesHost = new WebServiceHost(zones, new Uri("http://localhost:9347/api/zones"));
            //zonesHost.Open();

            Thread th = new Thread(ProductRealTimeAlarm.ProductRealTimeAlarmInfo);
            th.Start();

        }
    }
}

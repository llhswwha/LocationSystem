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
using WebNSQLib;
using System.Threading;
using SignalRService.Hubs;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFServer;

namespace WebLocation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            XmlConfigurator.Configure();//这个没有的话，Log4Net无效

            Log.Info("== Application_Start ==");
            AreaRegistration.RegisterAllAreas();
            Log.Info("AreaRegistration");
            GlobalConfiguration.Configure(WebApiConfig.Register);//WebApi
            Log.Info("GlobalConfiguration");
            //WebApiConfiguration.Configure(GlobalConfiguration.Configuration);//WebApi 这两种写法都行
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Log.Info("FilterConfig");
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Log.Info("RouteConfig");
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Log.Info("BundleConfig");
            XmlConfigurator.Configure();
            Log.Info("XmlConfigurator");
            InitData();

            //if (receiveAlarmThread != null)
            //{
            //    RealAlarm ra = new RealAlarm();
            //    receiveAlarmThread = new Thread(ra.ReceiveRealAlarmInfo);
            //    receiveAlarmThread.Start();
            //    ra.MessageHandler.DevAlarmReceived += DevAlarmReceived;
            //}

            if (engineClient == null)
            {
                Log.Info("StartConnectEngine");

                EngineLogin login=new EngineLogin("127.0.0.1",2323,"127.0.0.1",3456);

                engineClient = new PositionEngineClient();
                engineClient.Logs = Logs;
                engineClient.StartConnectEngine(login);//todo:ip写到配置文件中
            }
        }

        private PositionEngineLog Logs = new PositionEngineLog();

        private PositionEngineClient engineClient;

        private Thread receiveAlarmThread;

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

        private void DevAlarmReceived(DbModel.Location.Alarm.DevAlarm obj)
        {
            AlarmHub.SendDeviceAlarms(obj.ToTModel());
        }
    }
}

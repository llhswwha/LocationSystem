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

            if (receiveAlarmThread != null)
            {
                RealAlarm ra = new RealAlarm();
                receiveAlarmThread = new Thread(ra.ReceiveRealAlarmInfo);
                receiveAlarmThread.Start();
                ra.MessageHandler.DevAlarmReceived += DevAlarmReceived;
            }

            //if (engineClient == null)
            //{
            //    engineClient = new PositionEngineClient();
            //    engineClient.Logs = Logs;
            //    engineClient.StartConnectEngine(0, "127.0.0.1", "127.0.0.1");//todo:ip写到配置文件中
            //}
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

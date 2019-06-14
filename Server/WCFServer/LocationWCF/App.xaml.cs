using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using DAL;
using log4net.Config;
using Location.BLL.Tool;
using TModel.Tools;
using LocationServer;
using LocationServer.Tools;
using System.Threading;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.Work;
using DbModel.LocationHistory.Work;
using DbModel.Tools;
using Location.TModel.Tools;
using LocationServices.Converters;
using DbModel.Location.AreaAndDev;
using WebApiLib.Clients;
using TModel.Models.Settings;
using LocationServices.Locations;
using WebNSQLib;
using SignalRService.Hubs;
using EngineClient;
using WebApiLib;

namespace LocationWCFServer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            XmlConfigurator.Configure();

            Log.StartWatch();
            Log.AppStart();
            Log.Info("App_OnStartup");

            //LocationDbLite db = new LocationDbLite();
            ////db.Database.Create();
            //var list = db.Books.ToList();
            //if (list.Count == 0)
            //{
            //    db.Books.Add(new Book() { Name = "Book1" });
            //    db.Books.Add(new Book() { Name = "Book2" });
            //    db.Books.Add(new Book() { Name = "Book3" });
            //    db.SaveChanges();
            //}

            //Bll db = new Bll();
            ////bll.InitDevModelAndType();

            //var devs = db.DevInfos.ToList();

            int mode = ConfigurationHelper.GetIntValue("ShowSqlLog");
            if (mode == 1)
            {
                BLL.Bll.ShowLog = true;
            }

            InitDbContext();

            InitData();
            
            AppContext.AutoStartServer= ConfigurationHelper.GetIntValue("AutoStartServer") ==0;
            AppContext.WritePositionLog = ConfigurationHelper.GetBoolValue("WritePositionLog");
            AppContext.PositionMoveStateWaitTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateWaitTime");
            AppContext.ParkName = ConfigurationHelper.GetValue("ParkName");
            AppContext.DatacaseWebApiUrl = ConfigurationHelper.GetValue("DatacaseWebApiUrl");
            LocationContext.LoadOffset(ConfigurationHelper.GetValue("LocationOffset"));

            EngineClientSetting.LocalIp = ConfigurationHelper.GetValue("Ip");
            EngineClientSetting.EngineIp = ConfigurationHelper.GetValue("EngineIp");
            EngineClientSetting.AutoStart = ConfigurationHelper.GetBoolValue("AutoConnectEngine");
            //SystemSetting setting = new SystemSetting();
            //XmlSerializeHelper.Save(setting,AppDomain.CurrentDomain.BaseDirectory + "\\default.xml");

            //WebApiHelper.IsSaveJsonToFile = ConfigurationHelper.GetBoolValue("IsSaveJsonToFile");

            RealAlarm.NsqLookupdUrl= ConfigurationHelper.GetValue("NsqLookupdUrl");
            RealAlarm.NsqLookupdTopic = ConfigurationHelper.GetValue("NsqLookupdTopic");
            RealAlarm.NsqLookupdChannel = ConfigurationHelper.GetValue("NsqLookupdChannel");
        }

        private string datacaseUrl = "ipms-demo.datacase.io";

        private static void InitDbContext()
        {
            string dbType = ConfigurationHelper.GetValue("DbSourceType");
            AppContext.InitDbContext(dbType);
        }

        private void InitData()
        {
            Log.Info("InitData");
            int mode = ConfigurationHelper.GetIntValue("DataInitMode"); //-1:不初始化,0:EF,1:Sql
            Log.Info("DataInitMode:" + mode);
            if (mode >= 0)
            {
                AppContext.InitDb(mode);
            }
        }
    }
}

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
            //Log.StartWatch();
            //Log.AppStart();
            //Log.Info("App_OnStartup");

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

            LocationContext.LoadOffset(ConfigurationHelper.GetValue("LocationOffset"));
        }

        private static void InitDbContext()
        {
            int mode = ConfigurationHelper.GetIntValue("DbSource");
            AppContext.InitDbContext(mode);
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

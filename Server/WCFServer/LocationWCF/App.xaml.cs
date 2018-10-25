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

            Log.Info("App_OnStartup");

            LocationDbLite db = new LocationDbLite();
            //db.Database.Create();
            var list = db.Books.ToList();
            if (list.Count == 0)
            {
                db.Books.Add(new Book() { Name = "Book1" });
                db.Books.Add(new Book() { Name = "Book2" });
                db.Books.Add(new Book() { Name = "Book3" });
                db.SaveChanges();
            }

            //InitDbContext();

            //InitData();
        }

        private static void InitDbContext()
        {
            int mode = ConfigurationManager.AppSettings["DbSource"].ToInt();
            Log.Info("DbSource:" + mode);
            if (mode == 0)
            {
            }
            else if (mode == 1)
            {
                LocationDb.Name = "LocationLite";
                LocationHistoryDb.Name = "LocationHisLite";
            }
        }

        private void InitData()
        {
            Log.Info("InitData");
            int mode = ConfigurationManager.AppSettings["DataInitMode"].ToInt(); //-1:不初始化,0:EF,1:Sql
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

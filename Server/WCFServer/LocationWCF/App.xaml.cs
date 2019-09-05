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
using System.IO;
using Location.Server;
using System.Diagnostics;

namespace LocationWCFServer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ErrorInfo\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = dir + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")+"_Error.txt";
            File.WriteAllText(path, "App.CurrentDomain_UnhandledException:" + e.ExceptionObject + "");
            //Log.Error("App.CurrentDomain_UnhandledException", e.ExceptionObject + "");

            string path2 = string.Format("{0}{1}_0.dmp", dir, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
            MiniDump.TryDump(path2, MiniDumpType.None);
            string path3 = string.Format("{0}{1}_1.dmp", dir, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
            MiniDump.TryDump(path3, MiniDumpType.WithFullMemory);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "None"), MiniDumpType.None);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "Normal"), MiniDumpType.Normal);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "WithDataSegs"), MiniDumpType.WithDataSegs);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "WithCodeSegs"), MiniDumpType.WithCodeSegs);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "WithHandleData"), MiniDumpType.WithHandleData);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "FilterMemory"), MiniDumpType.FilterMemory);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "ScanMemory"), MiniDumpType.ScanMemory);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "WithThreadInfo"), MiniDumpType.WithThreadInfo);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "WithFullMemoryInfo"), MiniDumpType.WithFullMemoryInfo);
            //MiniDump.TryDump(string.Format("{0}{1}.dmp", dir, "WithFullMemory"), MiniDumpType.WithFullMemory);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DateTime dtDay = DateTime.Now;
            DateTime dtNextDay = DateTime.Now.AddDays(1);
            DateTime dtThirdDay = DateTime.Now.AddDays(2);
            int nHour = dtDay.Hour;

                string strDay = dtDay.ToString("yyyyMMdd");
                strDay = "p" + strDay;
                string strSqlSelect = "select PARTITION_NAME from INFORMATION_SCHEMA.PARTITIONS where table_name='positions'";

                dtNextDay = dtNextDay.Date;
                long lTime = Location.TModel.Tools.TimeConvert.ToStamp(dtNextDay);
                //string strSqlAdd = "ALTER TABLE positions ADD PARTITION (PARTITION " + strDay + " values less than(" + Convert.ToString(lTime) + "));";

                //DbRawSqlQuery<string> result1 = DbHistory.Database.SqlQuery<string>(strSqlSelect + ";");
                //List<string> lst = result1.ToList();
                //if (lst.Count == 0 || lst[0] == null)
                //{
                //    strSqlAdd = "alter table positions partition by range(DateTimeStamp) (PARTITION " + strDay + " values less than(" + Convert.ToString(lTime) + "));";
                //}

                //strSqlSelect += " and PARTITION_NAME = '" + strDay + "';";

                //DbRawSqlQuery<string> result2 = DbHistory.Database.SqlQuery<string>(strSqlSelect + ";");
                //List<string> lst2 = result2.ToList();
                //if (lst2.Count == 0)
                //{
                //    DbHistory.Database.ExecuteSqlCommand(strSqlAdd);
                //}


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
            AppContext.PositionMoveStateOfflineTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateOfflineTime");
            AppContext.LowPowerFlag = ConfigurationHelper.GetIntValue("LowPowerFlag");

            AppContext.UrlMaxLength = ConfigurationHelper.GetIntValue("UrlMaxLength");
            

            AppContext.ParkName = ConfigurationHelper.GetValue("ParkName");
            AppContext.DatacaseWebApiUrl = ConfigurationHelper.GetValue("DatacaseWebApiUrl");
            AppContext.ShowUnLocatedAreaPoint = ConfigurationHelper.GetBoolValue("ShowUnLocatedAreaPoint");

            AppContext.LogTextBoxMaxLength= ConfigurationHelper.GetIntValue("LogTextBoxMaxLength",10000);

            AppContext.MoveMaxSpeed = ConfigurationHelper.GetDoubleValue("MoveMaxSpeed", 20);
            AppContext.FilterTodayWhenStart = ConfigurationHelper.GetBoolValue("FilterTodayWhenStart");
            AppContext.FilterMoreThanMaxSpeedInterval = ConfigurationHelper.GetIntValue("FilterMoreThanMaxSpeedInterval",3600);
            

            LocationContext.LoadOffset(ConfigurationHelper.GetValue("LocationOffset"));
            LocationContext.LoadInitOffset(ConfigurationHelper.GetValue("InitTopoOffset"));
            LocationContext.Power = ConfigurationHelper.GetIntValue("InitTopoPower");

            EngineClientSetting.LocalIp = ConfigurationHelper.GetValue("Ip");
            EngineClientSetting.EngineIp = ConfigurationHelper.GetValue("EngineIp");
            EngineClientSetting.AutoStart = ConfigurationHelper.GetBoolValue("AutoConnectEngine");
            AppContext.PosEngineKeepAliveInterval= ConfigurationHelper.GetIntValue("PosEngineKeepAliveInterval",1000);


            //SystemSetting setting = new SystemSetting();
            //XmlSerializeHelper.Save(setting,AppDomain.CurrentDomain.BaseDirectory + "\\default.xml");

            //WebApiHelper.IsSaveJsonToFile = ConfigurationHelper.GetBoolValue("IsSaveJsonToFile");

            RealAlarm.NsqLookupdUrl= ConfigurationHelper.GetValue("NsqLookupdUrl");
            RealAlarm.NsqLookupdTopic = ConfigurationHelper.GetValue("NsqLookupdTopic");
            RealAlarm.NsqLookupdChannel = ConfigurationHelper.GetValue("NsqLookupdChannel");

            DbModel.AppSetting.AddHisPositionInterval = ConfigurationHelper.GetIntValue("AddHisPositionInterval",30) * 1000;//单位是s，

            KillOtherServers();
        }

        /// <summary>
        /// 关闭其他服务端进程，确保只能打开一个服务端进程。
        /// 之所以不是关闭自己是因为，可能存在服务端错误关闭（崩溃、或者出问题）后台线程还存在，但是客户端已经不能连接了。
        /// </summary>
        private static void KillOtherServers()
        {
            try
            {
                Process process = Process.GetCurrentProcess();
                string processName = process.ProcessName;
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 1)
                {
                    foreach (Process process1 in processes)
                    {
                        if (process1.HasExited) continue;
                        if (process1.Id != process.Id)
                        {
                            process1.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.Server,"KillOtherServers:"+ ex.ToString());
            }
            
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
            Log.Info(LogTags.Server,"DataInitMode:" + mode);
            if (mode >= 0)
            {
                AppContext.InitDb(mode);
            }
        }
    }
}

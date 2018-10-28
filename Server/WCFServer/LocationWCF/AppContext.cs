using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Location.BLL.Tool;
using DAL;
using System.IO;

namespace LocationServer
{
    public static class AppContext
    {
        public static Bll GetLocationBll()
        {
            return new Bll(false, true, false);
        }

        public static int DbSource = 0;

        public static void InitDbContext(int mode)
        {
            DbSource = mode;
            Log.Info("InitDbContext:" + mode);
            if (mode == 0)
            {
                LocationDb.IsSqlite = false;
                LocationDb.Name = "LocationConnection";
                LocationHistoryDb.IsSqlite = false;
                LocationHistoryDb.Name = "LocationHistoryConnection";
            }
            else if (mode == 1)
            {
                LocationDb.IsSqlite = true;
                LocationDb.Name = "LocationLite";
                LocationHistoryDb.IsSqlite = true;
                LocationHistoryDb.Name = "LocationHisLite";
            }
        }

        public static void InitDb(int mode, bool isForce = false)
        {
            Log.InfoStart("InitDb");
            Bll bll = new Bll();
            if (isForce)
            {
                bll.Db.Database.Create();
                bll.DbHistory.Database.Create();
            }
            bll.Init(mode);
            Log.InfoEnd("InitDb");
        }

        public static void InitDbAsync(int mode,Action callBack,bool isForce=false)
        {
            Log.InfoStart("InitDb");
            Bll bll;
            if (isForce&& DbSource==1)
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                string file1 = dir + "location.db";
                if (File.Exists(file1))
                {
                    File.Delete(file1);
                }
                string file2 = dir + "locationHis.db";
                if (File.Exists(file2))
                {
                    File.Delete(file2);
                }

                bll = new Bll();
            }
            else
            {
                bll = new Bll();
                if (isForce)
                {
                    bll.Db.Database.Create();
                    bll.DbHistory.Database.Create();
                }
            }
            bll.InitAsync(mode,()=>
            {
                Log.InfoEnd("InitDb");
                if (callBack != null)
                {
                    callBack();
                }
            });
            
        }

        public static void InitDbAsync(int dbsource, int initMode,Action callback)
        {
            InitDbContext(dbsource);
            InitDbAsync(initMode, callback,true);
        }

        public static void InitDb(int dbsource,int initMode)
        {
            InitDbContext(dbsource);
            InitDb(initMode);
        }
    }
}

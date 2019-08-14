using System;
using System.Linq;
using Location.BLL.Tool;
using System.Threading;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace BLL
{
    public partial class Bll
    {
        private static Thread PartitionThread = null;
        private bool bPartitionInitFlag = true;

        public void Init(int mode)
        {
            InitDb();
            InitDbData(mode);
        }

        public void InitAsync(int mode,Action<Bll> callBack)
        {
            Thread thread = new Thread(() =>
              {
                  InitDb();
                  InitDbData(mode);
                  if (callBack != null)
                  {
                      callBack(this);
                  }
              });
            thread.IsBackground = true;
            thread.Start();
        }

        public void InitDb()
        {
            Log.InfoStart(LogTags.DbGet, "InitDb");
            //List<Department> list = Departments.ToList();
            int count = Departments.DbSet.Count();//调试时，第一次使用EF获取数据要占用15-20s的时间，部署后不会。
            Log.Info("Count:" + count);
            int count2 = DbHistory.U3DPositions.Count();
            Log.Info("Count2:" + count2);
            InitPartitionThread();
            Log.InfoEnd("InitDb");
        }

        private void InitPartitionThread()
        {
            if (PartitionThread == null)
            {
                PartitionThread = new Thread(InsertPartitionInfo);
                PartitionThread.IsBackground = true;
                PartitionThread.Start();
            }
        }

        public bool HasData()
        {
            return Departments.ToList().Count > 0;
        }

        public void InitDbData(int mode, bool isForce = false)
        {
            DbInitializer initializer = new DbInitializer(this);
            initializer.InitDbData(mode, isForce);
        }

        private void InsertPartitionInfo()
        {
            bool bPartitionFlag = true;

            while (true)
            {
                DateTime dtDay = DateTime.Now;
                DateTime dtNextDay = DateTime.Now.AddDays(1);
                DateTime dtThirdDay = DateTime.Now.AddDays(2);
                int nHour = dtDay.Hour;

                if (nHour == 23)
                {
                    bPartitionFlag = true;
                }

                if (bPartitionInitFlag)
                {
                    string strDay = dtDay.ToString("yyyyMMdd");
                    strDay = "p" + strDay;
                    string strSqlSelect = "select PARTITION_NAME from INFORMATION_SCHEMA.PARTITIONS where table_name='positions'";

                    dtNextDay = dtNextDay.Date;
                    long lTime = Location.TModel.Tools.TimeConvert.ToStamp(dtNextDay);
                    string strSqlAdd = "ALTER TABLE positions ADD PARTITION (PARTITION " + strDay + " values less than(" + Convert.ToString(lTime) + "));";
                    
                    DbRawSqlQuery<string> result1 = DbHistory.Database.SqlQuery<string>(strSqlSelect + ";");
                    List<string> lst = result1.ToList();
                    if (lst.Count == 0 || lst[0] == null)
                    {
                        strSqlAdd = "alter table positions partition by range(DateTimeStamp) (PARTITION " + strDay + " values less than(" + Convert.ToString(lTime) + "));";
                    }

                    strSqlSelect += " and PARTITION_NAME = '" + strDay + "';";

                    DbRawSqlQuery<string> result2 = DbHistory.Database.SqlQuery<string>(strSqlSelect + ";");
                    List<string> lst2 = result2.ToList();
                    if (lst2.Count == 0)
                    {
                        DbHistory.Database.ExecuteSqlCommand(strSqlAdd);
                    }
                }

                if (bPartitionFlag)
                {
                    string strDay = dtNextDay.ToString("yyyyMMdd");
                    strDay = "p" + strDay;
                    string strSqlSelect = "select PARTITION_NAME from INFORMATION_SCHEMA.PARTITIONS where table_name='positions' and PARTITION_NAME = '" + strDay + "';";

                    dtThirdDay = dtThirdDay.Date;
                    long lTime = Location.TModel.Tools.TimeConvert.ToStamp(dtThirdDay);
                    string strSqlAdd = "ALTER TABLE positions ADD PARTITION (PARTITION " + strDay + " values less than(" + Convert.ToString(lTime) + "));";
                    
                    DbRawSqlQuery<string> result2 = DbHistory.Database.SqlQuery<string>(strSqlSelect + ";");
                    List<string> lst2 = result2.ToList();
                    if (lst2.Count == 0)
                    {
                        DbHistory.Database.ExecuteSqlCommand(strSqlAdd);
                    }
                }

                bPartitionInitFlag = false;
                bPartitionFlag = false;

                Thread.Sleep(1000*60*30);
                
            }
        }
    }
}

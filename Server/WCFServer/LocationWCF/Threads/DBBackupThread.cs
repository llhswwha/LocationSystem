using Base.Common.Threads;
using Location.BLL.Tool;
using LocationServer.Tools;
using LocationServer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    public class FileComparer : System.Collections.IComparer
    {
        int System.Collections.IComparer.Compare(Object o1, Object o2)
        {
            FileInfo fi1 = o1 as FileInfo;
            FileInfo fi2 = o2 as FileInfo;
            return fi1.CreationTime.CompareTo(fi2.CreationTime);//文件或目录的创建日期
        }

    }
    public class DBBackupThread: IntervalTimerThread
    {
        private float DBBackupDelayDays = 1;//默认一天备份一次

        private int MaxDBSaveCount = 60;
        public DBBackupThread(float days)
            :base(
                 TimeSpan.FromHours(10)
                 , TimeSpan.FromMinutes(10))//程序启动后10分中，立刻执行一次，之后没过10小时执行一次
        {
            DBBackupDelayDays = days;
            MaxDBSaveCount = ConfigurationHelper.GetIntValue("MaxDBSaveCount");
        }
        public override bool TickFunction()
        {
            try
            {
                if(DBBackupDelayDays<=0)
                {
                    Log.Info("DBBackupDelayDays is less than 0,CancelBackup...");
                    return false;
                }
                if (MaxDBSaveCount < 0) MaxDBSaveCount = 0;
                string dbSavePath = MySqlBackUpWindow.dbSavePath;
                if (string.IsNullOrEmpty(dbSavePath)) return false;
                //默认保留60个数据库文件，超出则清除           
                FileInfo[] filesBack = new DirectoryInfo(dbSavePath).GetFiles("*.sql");
                List<FileInfo> infoList = new List<FileInfo>();
                if (filesBack != null)
                {
                    //1.先按创建时间排序
                    FileComparer fc = new FileComparer();
                    Array.Sort(filesBack, fc);
                    infoList = filesBack.ToList();
                    //2.如果超过最大保存数，则删除最早保存的数据库                                
                    if (infoList != null && infoList.Count > MaxDBSaveCount)
                    {
                        int offset = infoList.Count - MaxDBSaveCount;
                        for (int i = offset - 1; i >= 0; i--)
                        {
                            FileInfo fileInfo = infoList[i];
                            if (File.Exists(fileInfo.FullName))
                            {
                                infoList.Remove(fileInfo);
                                File.Delete(fileInfo.FullName);
                                Log.Info("DataBaseBackup delete:" + fileInfo.FullName);
                            }
                        }
                    }
                }
                //3.按时间排序后，取最后一个文件的创建时间，和当前比较。如果小于备份间隔天数，就不备份
                bool isNeedBacking = true;
                if(infoList!=null&&infoList.Count>0)
                {
                    FileInfo latestFile = infoList[infoList.Count - 1];
                    TimeSpan span = DateTime.Now - latestFile.CreationTime;
                    if (span.TotalDays < DBBackupDelayDays)
                    {
                        isNeedBacking = false;
                    }
                }              
                if (isNeedBacking)
                {
                    //4.备份location和locationhistory数据库
                    MySqlBackUpWindow.BackupSqlInSqlBackupDirectory(MySqlBackUpWindow.LocationMySql);
                    MySqlBackUpWindow.BackupSqlInSqlBackupDirectory(MySqlBackUpWindow.LocationHistoryMySql);
                }
                return true;
            }catch(Exception e)
            {
                Log.Info("Error.DBBackupThread.Exception:"+e.ToString());
                return false;
            }
        }
        protected override void DoBeforeWhile()
        {

        }
    }

}

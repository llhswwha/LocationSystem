using BLL;
using Location.BLL.Tool;
using LocationServer.Tools;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocationServer.Windows
{
    /// <summary>
    /// MySqlBackUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MySqlBackUpWindow : Window
    {
        public static string LocationMySql = "location";
        public static string LocationHistoryMySql = "locationhistory";
        private static string sqlSuffixName = ".sql";

        private static string savePath_Location = "";
        private static string savePath_LocationHistory = "";

        public static string dbSavePath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\SqlBackup\\";
        public MySqlBackUpWindow()
        {
            InitializeComponent();
            InitRecoverGrid();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }        

        private void ButtonLocationBack_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            if(!bll.Db.Database.Exists())
            {
                MessageBox.Show("数据库:Location 不存在，无法进行备份！");
                return;
            }
            string tips = "数据库备份过程中，请勿关闭程序！\n点击确定开始备份，点击取消则退出备份";
            if (MessageBox.Show(tips, "提示",MessageBoxButton.OKCancel)==MessageBoxResult.OK)
            {
                BackupByDBName(LocationMySql, LocationPathText);//备份Location数据库
            }
            else
            {
                MessageBox.Show("数据库备份已取消","提示");
            }            
        }

        private void ButtonLocationHistoryBack_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            if (!bll.DbHistory.Database.Exists())
            {
                MessageBox.Show("数据库:LocationHistory 不存在，无法进行备份！");
                return;
            }
            string tips = "数据库备份过程中，请勿关闭程序！\n点击确定开始备份，点击取消则退出备份";
            if (MessageBox.Show(tips, "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                BackupByDBName(LocationHistoryMySql, LocationHistoryPathText); //备份locationhistory数据库
            }
            else
            {
                MessageBox.Show("数据库备份已取消", "提示");
            }           
        }

        private void ButtonLocationAndHistoryBack_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            string tips = "数据库备份过程中，请勿关闭程序！\n点击确定开始备份，点击取消则退出备份";
            if (MessageBox.Show(tips, "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (!bll.Db.Database.Exists())
                {
                    MessageBox.Show("数据库:Location 不存在，无法进行备份！");
                    return;
                }
                BackupByDBName(LocationMySql, LocationPathText, () =>
                {
                    if (!bll.DbHistory.Database.Exists())
                    {
                        MessageBox.Show("数据库:LocationHistory 不存在，无法进行备份！");
                        return;
                    }
                    BackupByDBName(LocationHistoryMySql, LocationHistoryPathText);
                });
            }
            else
            {
                MessageBox.Show("数据库备份已取消", "提示");
            }          
        }
        private bool isBackupNow;
        //通过数据库名称，备份数据库
        private void BackupByDBName(string dbName,TextBox logBox,Action onComplete=null)
        {
            if(logBox!=null)logBox.Text = dbName + "数据库备份中，请耐心等待...";
            if (isBackupNow)
            {
                MessageBox.Show(string.Format("数据库:{0} 备份中，请耐心等待...",dbName));
                return;
            }
            isBackupNow = true;
            bool isBackupSuccess = true;
            Worker.Run(() =>
            {
                BackupSqlInSqlBackupDirectory(dbName);
            }, () =>
            {
                isBackupNow = false;
                if(isBackupSuccess)
                {
                    string savePath = dbName.ToLower() == LocationMySql.ToLower() ? savePath_Location : savePath_LocationHistory;
                    if (logBox != null) logBox.Text = string.Format("{0}数据库备份成功，地址：{1}", LocationMySql, savePath);
                    InitRecoverGrid();//备份成功后，刷新还原列表
                }               
                if (onComplete != null) onComplete();              
            }, exp =>
            {
                isBackupSuccess = false;
                isBackupNow = false;
                if (logBox != null) logBox.Text = string.Format("{0}数据库备份失败，错误原因：{1}", LocationMySql, exp.ToString());
                if (onComplete != null) onComplete();
            });
        }

        /// <summary>
        /// 备份数据库（默认保存至Data/SqlBackup文件夹内）
        /// </summary>
        /// <param name="databaseName">location/locationhistory</param>
        /// <param name="ip"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="port"></param>
        public static void BackupSqlInSqlBackupDirectory(string databaseName,string ip = "localhost",string userName="root",string pwd="123456",string port="3306")
        {
            //备份数据库的路径
            if(string.IsNullOrEmpty(dbSavePath)) dbSavePath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\SqlBackup\\";
            BackupSqlByFullPath(dbSavePath, databaseName,ip,userName,pwd,port);
        }
        /// <summary>
        /// 备份数据库在指定文件夹
        /// </summary>
        /// <param name="fullPath">文件夹路径，例如：G:\Location\Server\WCFServer\LocationWCF</param>
        /// <param name="dbName">数据库名称:location/locationhistory</param>
        /// <param name="ip"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="port"></param>
        public static void BackupSqlByFullPath(string fullPath,string dbName, string ip = "localhost", string userName = "root", string pwd = "123456", string port = "3306")
        {
            //调用mysqldump备份mysql数据库的语句
            string backupsql = string.Format("mysqldump --host={0} --default-character-set=utf8 --lock-tables  --routines --force --port={3} --user={1} --password={2} --quick  ", ip, userName, pwd, port);
            ////mysqldump的路径
            //string mysqldump = @"E:\mysql-5.7.24-winx64\bin";
            //需要备份的数据库名称
            string strDB = dbName;
            //备份数据库的路径
            string strDBpath = fullPath;

            //判断备份的数据库路径是否存在
            if (!Directory.Exists(strDBpath))
            {
                Directory.CreateDirectory(strDBpath);
            }
            //备份数据库
            if (!string.IsNullOrEmpty(strDB))
            {
                string fileNamePath = strDBpath + DateTime.Now.ToString("yyyyMMdd") + strDB;
                string fileFullPath = strDBpath + DateTime.Now.ToString("yyyyMMdd") + strDB + ".sql";
                if (File.Exists(fileFullPath))
                {
                    fileFullPath = TryGetFileNameIndex(1, fileNamePath, ".sql");
                }
                SetTextByDataBase(strDB, fileFullPath);
                DateTime now = DateTime.Now;
                string cmd = backupsql + " " + strDB + " >" + fileFullPath;
                string result = RunCmd(cmd);
                string logValue = (DateTime.Now - now).TotalSeconds.ToString();
                Log.Info(string.Format("Backup DB-{0} finish,costTime:{1}s",dbName, logValue));
            }
        }

        /// <summary>
        /// 保存成功后，记录保存路径地址
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="fullPath"></param>
        private static void SetTextByDataBase(string dataBaseName,string fullPath)
        {
            if(dataBaseName.ToLower()== LocationMySql.ToLower())
            {
                savePath_Location = fullPath;
            }
            else if(dataBaseName.ToLower() == LocationHistoryMySql.ToLower())
            {
                savePath_LocationHistory = fullPath;
            }
        }

        private static string RunCmd(string strCmd)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.WorkingDirectory = mysqldumPath;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(strCmd);
            p.StandardInput.WriteLine("exit");
            return p.StandardError.ReadToEnd();
        }

        /// <summary>
        /// 通过文件名称还原数据库,文件需在Data\SqlBackup文件夹中
        /// </summary>
        /// <param name="fileName">文件名称: 20200408location或者20200408location.sql均可</param>
        /// <param name="dbName">数据库名称：locaiton 或 locationhistory</param>
        /// <param name="ip"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="port"></param>
        public static void RecoverDateByFileName(string fileName,string dbName, string ip = "localhost", string userName = "root", string pwd = "123456", string port = "3306")
        {
            if (string.IsNullOrEmpty(dbSavePath)) dbSavePath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\SqlBackup\\";
            string filePath = dbSavePath+fileName;
            RecoverDBByFullPath(filePath,dbName,ip,userName,pwd,port);
        }
        /// <summary>
        /// 通过文件完整路径，还原数据库
        /// </summary>
        /// <param name="fileFullName">G:\Location\Server\WCFServer\LocationWCF\bin\Debug\Data\SqlBackup\20200408location.sql</param>
        /// <param name="dbName"></param>
        /// <param name="ip"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="port"></param>
        public static void RecoverDBByFullPath(string fileFullName, string dbName, string ip = "localhost", string userName = "root", string pwd = "123456", string port = "3306")
        {
            //如果数据库为空，创建默认数据库
            CheckEmptyDB();
            //调用mysqldump备份mysql数据库的语句
            string backupsql = string.Format("mysql --host={0} --default-character-set=utf8  --port={3} --user={1} --password={2} ", ip, userName, pwd, port);
            ////mysql的路径
            //string mysqldump = @"E:\mysql-5.7.24-winx64\bin";
            //需要备份的数据库名称
            string strDB = dbName;
            if (!fileFullName.Contains(sqlSuffixName)) fileFullName += sqlSuffixName;
            if (!File.Exists(fileFullName))
            {
                throw new Exception("文件不存在，路径：" + fileFullName);
            }
            else
            {
                string cmd = backupsql + " " + strDB + " < \"" + fileFullName + "\"";
                DateTime now = DateTime.Now;
                string result = RunCmd(cmd);
                string costTime = (DateTime.Now - now).TotalSeconds.ToString();
                Log.Info(string.Format("还原数据库成功，耗时:{0}秒", costTime));
            }
        }
        /// <summary>
        /// 如果数据库为空，则创建默认数据库
        /// </summary>
        private static void CheckEmptyDB()
        {
            try
            {
                Log.Info("SqlBackup.CheckEmptyDB...");
                Bll bll = Bll.NewBllNoRelation();
                bll.Db.Database.CreateIfNotExists();
                bll.DbHistory.Database.CreateIfNotExists();
            }
            catch(Exception e)
            {
                Log.Info("CheckEmptyDB.Exception:"+e.ToString());
            }           
        }
       
        /// <summary>
        /// 如有重复文件，则后缀+1  locaiton.sql->location1.sql
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="filePath"></param>
        /// <param name="suffixName"></param>
        /// <returns></returns>
        private static string TryGetFileNameIndex(int currentIndex,string filePath,string suffixName)
        {
            string fileName = filePath + currentIndex.ToString() + suffixName;
            if(File.Exists(fileName))
            {
                currentIndex++;
                return TryGetFileNameIndex(currentIndex,filePath,suffixName);
            }
            else
            {
                return fileName;
            }
        }

        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private bool isRecovering;//是否正在还原数据库
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(isRecovering)
            {
                MessageBox.Show("数据库还原中，请耐心等待...");
                return;
            }           
            if (DataGrid1.SelectedItem == null)
            {
                MessageBox.Show("未选中任何数据库", "提示");
            }
            else
            {
                isRecovering = true;
                DBBackupPath fileT = DataGrid1.SelectedItem as DBBackupPath;
                string path = fileT.Path;
                DateTime timeNow = DateTime.Now;
                bool isRecoverSuccess = true;//线程内出现异常后，执行异常回调后，又会执行结束回调。增加这个标志位区分是否成功
                RecoverInfoText.Text = "数据库正在还原中，请耐心等待。请勿关闭当前页面!";
                Worker.Run(() =>
                {
                    string[] pathSplit = path.Split('\\');
                    string dbTargetName = LocationMySql;//需要还原的数据库名称
                    if (pathSplit != null && pathSplit.Length > 0)
                    {
                        string finalName = pathSplit[pathSplit.Length - 1].ToLower();
                        if (finalName.Contains(LocationHistoryMySql.ToLower())) dbTargetName = LocationHistoryMySql;
                    }
                    else
                    {
                        if (path.Contains(LocationHistoryMySql.ToLower())) dbTargetName = LocationHistoryMySql;
                    }
                    string tips = string.Format("文件路径：{0} 目标数据库名称：{1}\n点击确认开始还原，点击取消则退出。", path, dbTargetName);                  
                    if (MessageBox.Show(tips, "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        RecoverDBByFullPath(path, dbTargetName);
                    }
                    else
                    {
                        isRecoverSuccess = false;
                    }
                }, () =>
                {
                    isRecovering = false;                    
                    if (isRecoverSuccess)
                    {
                        string costTime = (DateTime.Now - timeNow).TotalSeconds.ToString();
                        string successInfo = string.Format("数据库还原成功,耗时：{0} 秒", costTime);
                        RecoverInfoText.Text = successInfo;
                        MessageBox.Show(successInfo);
                    }
                    else
                    {
                        RecoverInfoText.Text = "数据库还原已取消...";
                    }
                }, exp =>
                {
                    isRecoverSuccess = false;
                    isRecovering = false;
                    string errorInfo = string.Format("Error:数据库还原失败，错误原因：{0}", exp.ToString());                   
                    MessageBox.Show(errorInfo);
                    RecoverInfoText.Text = errorInfo;
                });
            }                       
        }

        private void InitRecoverGrid()
        {
            if (string.IsNullOrEmpty(dbSavePath)) dbSavePath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\SqlBackup\\";
            string[] files = Directory.GetFiles(dbSavePath, "*.sql");
            if (files == null || files.Length == 0) return;
            DBBackupPath[] pathes = DBBackupPath.GetPathes(files);
            if (pathes != null) DataGrid1.ItemsSource = pathes;           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(isRecovering)
            {
                MessageBox.Show("有数据库正在还原，请等待还原完成后再执行删除操作！");
                return;
            }
            //删除选中数据库
            if (DataGrid1.SelectedItem == null)
            {
                MessageBox.Show("未选中任何数据库", "提示");
            }
            else
            {
                DBBackupPath fileT = DataGrid1.SelectedItem as DBBackupPath;
                string path = fileT.Path;
                string tips = string.Format("即将删除备份数据库文件，文件路径：{0}", path);
                if (MessageBox.Show(tips, "提示", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        InitRecoverGrid();
                    }
                    else
                    {
                        MessageBox.Show("删除失败，路径下文件不存在！");
                    }
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(dbSavePath))
            {
                MessageBox.Show("数据库备份文件夹为空，dbSavePath is null!");
            }
            else
            {
                System.Diagnostics.Process.Start(dbSavePath);
            }           
        }

        private void RecoverInfoText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    /// <summary>
    /// 数据库备份路径
    /// </summary>
    public class DBBackupPath
    {
        public string Path { get; set; }
        
        public static DBBackupPath[] GetPathes(string[] files)
        {
            if (files == null) return null;
            List<DBBackupPath> pathes = new List<DBBackupPath>();
            foreach(var item in files)
            {
                DBBackupPath back = new DBBackupPath();
                back.Path = item;
                pathes.Add(back);
            }
            return pathes.ToArray();
        }
    }
}

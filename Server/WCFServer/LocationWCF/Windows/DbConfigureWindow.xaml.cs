using BLL;
using BLL.Tools;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
using DbModel.Tools;
using DbModel.Tools.InitInfos;
using LocationClient.Tools;
using Location.Model.InitInfos;
using DbModel.Location.AreaAndDev;
using DbModel.CADEntitys;
using Point = DbModel.Location.AreaAndDev.Point;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using DbModel.Location.Settings;
using LocationServer.Tools;
using TModel.Tools;
using DbModel;
using IModel.Enums;
using WPFClientControlLib;
using DbModel.Location.Alarm;
using DbModel.LocationHistory.Alarm;
using LocationServices.Locations;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for DbConfigureWindow.xaml
    /// </summary>
    public partial class DbConfigureWindow : Window
    {
        public DbConfigureWindow()
        {
            InitializeComponent();
            //Debug.Listeners.Add(new TraceListener());
            //Log.NewLogEvent += Log_NewLogEvent;
            logTbController.Init(TbConsole, LogTags.DbInit);
            Log.StartWatch();
            this.Closing += DbConfigureWindow_Closing   ;
        }

        LogTextBoxController logTbController = new LogTextBoxController();

        private void DbConfigureWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.StopWatch();
            logTbController.Dispose();
            //Log.NewLogEvent -= Log_NewLogEvent;
        }

        private void MenuInitMSSql_Click(object sender, RoutedEventArgs e)
        {
            //var aa = new AuthorizationArea();
            //aa.Id = 1;
            //aa.Name = "aa";
            //aa.ParentId = 0;
            //aa.Children.Add(new AuthorizationArea() { Id = 2, ParentId = 1, Name = "bb" });
            //XmlSerializeHelper.Save(aa, AppDomain.CurrentDomain.BaseDirectory + "\\Data\\AuthorizationTree.xml");


            //var xf = new XmlFile();
            //xf.Id = 1;
            //xf.Name = "aa";
            //xf.ParentId = 0;
            //xf.Children.Add(new XmlFile() { Id = 2, ParentId = 1, Name = "bb" });
            //XmlSerializeHelper.Save(xf, AppDomain.CurrentDomain.BaseDirectory + "\\Data\\XmlFile.xml");


            //AppContext.DeleteDb(0);
            AppContext.InitDbAsync("SqlServer", 0, (bll) =>
             {
                 InitImage(bll);
                 MessageBox.Show("初始化完成");
             });
        }

        private void InitImage(Bll bll)
        {
            string strName = "顶视图";
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Images\\顶视图.png";
            byte[] byteArray = ImageHelper.LoadImageFile(path);
            bll.Pictures.Update(strName, byteArray);
        }

        private void MenuInitSqlite_Click(object sender, RoutedEventArgs e)
        {
            AppContext.DeleteDb(1);
            AppContext.InitDbAsync("SQLite", 0, (bll) =>
            {
                InitImage(bll);
                MessageBox.Show("初始化完成");
            });
        }

        private void MenuInitMySql_Click(object sender, RoutedEventArgs e)
        {
            AppContext.DeleteDb(1);
            AppContext.InitDbAsync("MySql", 0, (bll) =>
            {
                InitImage(bll);
                MessageBox.Show("初始化完成");
            });
        }

        private void MenuInitTopo_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                AreaTreeInitializer initializer = new AreaTreeInitializer(new Bll());
                initializer.InitTopoFromXml();
            }, () => { MessageBox.Show("完成"); },null, LogTags.DbInit);
        }

        private void MenuRemoveArchor_Click(object sender, RoutedEventArgs e)
        {
            DevInfoHelper.RemoveArchorDev();
            MessageBox.Show("完成");
        }

        private void MenuImportDevs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuInitAA_OnClick(object sender, RoutedEventArgs e)
        {
            
            Worker.Run(() =>
            {
                DbInitializer initializer = new DbInitializer(new Bll());
                initializer.InitAuthorization();
            }, () =>
            {
                MessageBox.Show("完成");
            });
        }

        private void MenuRealPos_OnClick(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                DbInitializer initializer = new DbInitializer(new Bll());
                initializer.InitRealTimePositions();
            }, () =>
            {
                MessageBox.Show("完成");
            });
        }

        private void MenuPersonAndCard_OnClick(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                DbInitializer initializer = new DbInitializer(new Bll());
                initializer.InitCardAndPerson();
            }, () =>
             {
                 MessageBox.Show("完成");
             });
            
            
        }

        private void MenuDeleteSqlServer_Click(object sender, RoutedEventArgs e)
        {
            AppContext.DeleteDb(0);
        }

        private void MenuExportArchorData_Click(object sender, RoutedEventArgs e)
        {
            LocationDeviceList list = new LocationDeviceList();
            list.DepList = new List<LocationDevices>();

            Dictionary<int, List<Archor>> dict = new Dictionary<int, List<Archor>>();

            Bll bll = new Bll();
            var archorList=bll.Archors.ToList();
            foreach (var item in archorList)
            {
                int pId = (int)item.ParentId;
                if (!dict.ContainsKey(pId))
                {
                    dict[pId] = new List<Archor>();
                }
                dict[pId].Add(item);
            }

            foreach (var item in dict.Keys)
            {
                var area = bll.Areas.Find(item);
                var archors = dict[item];
                LocationDevices devs = new LocationDevices();
                devs.DevList = new List<LocationDevice>();
                devs.Name = area.Name;

                list.DepList.Add(devs);
                foreach (var archor in archors)
                {
                    var dev = new LocationDevice();
                    dev.AbsolutePosX = archor.X.ToString();
                    dev.AbsolutePosY = archor.Y.ToString();
                    dev.AbsolutePosZ = archor.Z.ToString();
                    dev.AnchorId = archor.Code;
                    dev.IP = archor.Ip;
                    dev.Name = archor.Name;
                    dev.XPos = archor.DevInfo.PosX.ToString();
                    dev.YPos = archor.DevInfo.PosY.ToString();
                    dev.ZPos= archor.DevInfo.PosZ.ToString();
                    devs.DevList.Add(dev);
                }
            }

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\基站信息\\基站信息.xml";

            XmlSerializeHelper.Save(list,filePath);

            FileInfo fi = new FileInfo(filePath);
            Process.Start(fi.Directory.FullName);
        }

        private void LoadCADShapeList_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker2 = new BackgroundWorker();
            worker2.DoWork += Worker2_DoWork;
            worker2.WorkerReportsProgress = true;
            worker2.ProgressChanged += Worker2_ProgressChanged;
            worker2.RunWorkerAsync();
        }

        private void Worker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarEx1.Value = e.ProgressPercentage;
        }

        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker1 = sender as BackgroundWorker;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\CADAreaInfo.xml";

            CADAreaList list = XmlSerializeHelper.LoadFromFile<CADAreaList>(filePath);
            list.LineToBlock();

            Bll bll = new Bll();
            var areas = bll.Areas.ToList(false);
            List<Point> newPoints = new List<Point>();
            List<Area> newBounds = new List<Area>();
            List<Area> newAreas = new List<Area>();
            int count = 0;
            for (int i1 = 0; i1 < list.Count; i1++)
            {
                CADArea item = list[i1];
                var area = areas.Find(i => i.Name == item.Name);
                if (area != null)
                {
                    count += item.Shapes.Count;
                }
            }

            

            int index = 0;
             for (int i1 = 0; i1 < list.Count; i1++)
            {
                CADArea item = list[i1];
                var area = areas.Find(i => i.Name == item.Name);
                if (area != null)
                {
                    for (int i = 0; i < item.Shapes.Count; i++)
                    {
                        index++;
                        CADShape sp = item.Shapes[i];
                        Bound bound = new Bound();
                        bool r1 = bll.Bounds.Add(bound);
                        if (r1)
                        {
                            Area newArea = new Area();
                            newArea.Name = sp.Name;
                            newArea.Type = AreaTypes.CAD;
                            newArea.ParentId = area.Id;
                            newArea.InitBound = bound;
                            var r2 = bll.Areas.Add(newArea);
                            if (r2)
                            {
                                var pointList = new List<Point>();
                                foreach (var pt in sp.Points)
                                {
                                    var point = new Point();
                                    point.X = (float)pt.X / 1000 - 0.1f;
                                    point.Y = (float)pt.Y / 1000 - 0.1f;
                                    point.BoundId = bound.Id;
                                    var r3 = bll.Points.Add(point);
                                    pointList.Add(point);
                                }
                                bound.Shape = 0;
                                bound.IsRelative = true;
                                bound.SetInitBound(pointList.ToArray(), area.InitBound.MinZ, (float)area.InitBound.GetHeight());

                                bool r4 = bll.Bounds.Edit(bound);
                                newArea.SetBound(bound);
                                bll.Areas.Edit(newArea);
                            }
                        }
                        
                        int percent = (int)((index + 0.0) / count * 100);
                        worker1.ReportProgress(percent);
                    }
                }
            }

            //bll.Areas.AddRange(newAreas);
        }

        private void ClearCADShapeList_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker1 = new BackgroundWorker();
            worker1.DoWork += Worker1_DoWork;
            worker1.WorkerReportsProgress = true;
            worker1.ProgressChanged += Worker1_ProgressChanged;
            worker1.RunWorkerAsync();
        }

        private void Worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarEx1.Value = e.ProgressPercentage;
        }

        private void Worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker1 = sender as BackgroundWorker;
            Bll bll = new Bll();
            var areas = bll.Areas.ToList(false);
            var cadAreas = areas.FindAll(i => i.Type == AreaTypes.CAD);
            //bll.Areas.RemoveList(cadAreas);
            for (int i = 0; i < cadAreas.Count; i++)
            {
                Area item = cadAreas[i];
                var r1 = bll.Points.RemoveList(item.InitBound.Points);
                var r3 = bll.Areas.DeleteById(item.Id);
                var r2 = bll.Bounds.DeleteById((int)item.InitBoundId);
                int percent = (int)((i+0.0) / cadAreas.Count*100);
                worker1.ReportProgress(percent);
            }
        }

        private void MenuArchorSetting_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = new Bll();
            var list2 = DbInfoHelper.GetArchorSettings();
            bll.ArchorSettings.Clear();
            bll.ArchorSettings.AddRange(list2);
            MessageBox.Show("完成");
        }

        private void MenuSaveTop_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            AreaTreeInitializer initializer = new AreaTreeInitializer(bll);
            initializer.SaveInitInfoXml(false);
            MessageBox.Show("完成");
        }

        private void MenuMergeArchorInfo_Click(object sender, RoutedEventArgs e)
        {
            var win = new AreaTreeWindow();
            if (win.ShowDialog() == true)
            {
                var area=win.SelectedArea;
                var archors=DbInfoHelper.GetArchors().FindAll(i => i.ParentId == area.Id);
                var devs = DbInfoHelper.GetDevInfos().FindAll(i => i.ParentId == area.Id);
                foreach (var item in archors)
                {
                    item.DevInfo = devs.Find(i => i.Id == item.DevInfoId);
                }
                var archorSettings = DbInfoHelper.GetArchorSettings().FindAll(i => i.FloorName == area.Name);
                foreach (var item in archorSettings)
                {
                    item.Archor = archors.Find(i => i.Id == item.ArchorId);
                }

                Bll bll = new Bll();
                var archors2=bll.Archors.FindAll(i => i.ParentId == area.Id);
                var devs2 = bll.DevInfos.FindAll(i => i.ParentId == area.Id);
                var archorSettings2 = bll.ArchorSettings.FindAll(i => i.FloorName == area.Name);

                bll.Archors.RemoveList(archors2);
                bll.DevInfos.RemoveList(devs2);
                bll.ArchorSettings.RemoveList(archorSettings2);

                bll.DevInfos.AddRange(devs);
                foreach (var item in archors)
                {
                    item.DevInfoId = item.DevInfo.Id;
                }
                bll.Archors.AddRange(archors);

                foreach (var item in archorSettings)
                {
                    if(item.Archor!=null)
                        item.ArchorId = item.Archor.Id;
                }
                bll.ArchorSettings.AddRange(archorSettings);
            }
        }

        private void MenuLoadOutArchorPoints_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            var archors=bll.Archors.ToList().Where(i => i.ParentId == 2).ToList();
            var devs = bll.DevInfos.ToList().Where(i => i.ParentId == 2).ToList();
            var archorSettings = bll.ArchorSettings.ToList();
            var newDevs = new List<DevInfo>();
            var newArchors = new List<Archor>();
            var newArchorSettings1 = new List<ArchorSetting>();
            var newArchorSettings2 = new List<ArchorSetting>();
            var notArchors = new List<string>();
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\GPSPoints.txt";
            string[] lines = File.ReadAllLines(path);
            foreach (var item in lines)
            {
                string line = item.Trim();
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(',');
                string name = parts[0].ToLower();
                double x = parts[2].ToDouble();
                double y = parts[3].ToDouble();
                double z = parts[4].ToDouble();


                var archor = archors.Find(i => i.Code.ToLower() == name || i.Name.ToLower() == name);
                if (archor != null)
                {
                    if (archor.Name == "4")
                    {
                        int j = 0;
                    }
                    newArchors.Add(archor);
                    archors.Remove(archor);
                    archor.X = x;
                    archor.Y = z;
                    archor.Z = y;
                    var dev = devs.Find(i => i.Id == archor.DevInfoId);
                    dev.PosX = (float)x;
                    dev.PosY = (float)z;
                    dev.PosZ = (float)y;
                    newDevs.Add(dev);

                    var archorSetting = archorSettings.Find(i => i.ArchorId == archor.Id||i.Code==archor.Code);
                    if (archorSetting != null)
                    {
                        newArchorSettings1.Add(archorSetting);
                    }
                    else
                    {
                        archorSetting = new ArchorSetting();
                        newArchorSettings2.Add(archorSetting);
                    }
                    archorSetting.ZeroX = "0";
                    archorSetting.ZeroY = "0";
                    archorSetting.AbsoluteX = x.ToString("F3");
                    archorSetting.AbsoluteY = y.ToString("F3");
                    archorSetting.RelativeX = x.ToString("F3");
                    archorSetting.RelativeY = y.ToString("F3");
                    archorSetting.AbsoluteHeight = z;
                    archorSetting.RelativeHeight = z;
                    archorSetting.RelativeMode = RelativeMode.CAD坐标;
                    archorSetting.Name = archor.Name;
                    archorSetting.Code = archor.Code;
                    archorSetting.ArchorId = archor.Id;
                }
                else
                {
                    notArchors.Add(line);
                }
            }
            bll.Archors.EditRange(newArchors);
            bll.DevInfos.EditRange(newDevs);
            //bll.Db.SaveChanges();
            bll.ArchorSettings.EditRange(newArchorSettings1);
            bll.ArchorSettings.AddRange(newArchorSettings2);
        }

        private void MenuSaveOutArchorPoints_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            var archorSettings = bll.ArchorSettings.Where(i=>string.IsNullOrEmpty(i.BuildingName)&&!string.IsNullOrEmpty(i.Code) && i.AbsoluteHeight !=2).ToList();
            string txt = "";
            foreach (var item in archorSettings)
            {
                txt += string.Format("{0},,{1},{2},{3}\r\n", item.Code, item.AbsoluteX, item.AbsoluteY, item.AbsoluteHeight);
            }
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\GPSPoints2.txt";
            File.WriteAllText(path, txt);
        }

        private void MenuInitDevs_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            AreaTreeInitializer initializer = new AreaTreeInitializer(bll);
            initializer.InitDevs();
            MessageBox.Show("完成");
        }

        private void MenuInitTags_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            DbInitializer initializer = new DbInitializer(bll);
            initializer.InitTagPositions(true,true);
            MessageBox.Show("完成");
        }

        private void MenuLoadKKS_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            DbInitializer initializer = new DbInitializer(bll);
            //initializer.InitAllKKSCode();
            MessageBox.Show("完成");
        }

        private void DevSaveTop_Click(object sender, RoutedEventArgs e)
        {
            DevBackupHelper backup = new DevBackupHelper();
            backup.BackupDevInfo(()=> 
            {
                MessageBox.Show("备份完成...");
            });           
        }

        private void MenuDeleteMySqlServer_Click(object sender, RoutedEventArgs e)
        {
            AppContext.DeleteDb(2);
            MessageBox.Show("删除MySql完成");
        }

        private void MenuInitPic_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            DbInitializer initializer = new DbInitializer(bll);
            initializer.InitHomePage();
            MessageBox.Show("完成");
        }

        private void DepSaveTop_Click(object sender, RoutedEventArgs e)
        {
            Tools.DepartmentsBackupHelper backup = new Tools.DepartmentsBackupHelper();
            backup.BackupDepartmentsInfo(() =>
            {
                MessageBox.Show("备份完成...");
            });
        }

        private void PersonSaveTop_Click(object sender, RoutedEventArgs e)
        {
            Tools.PersonBackupHelper backup = new Tools.PersonBackupHelper();
            backup.BackupPersonInfo(() =>
            {
                MessageBox.Show("备份完成...");
            });
        }

        private void EntranceGuardCardSaveTop_Click(object sender, RoutedEventArgs e)
        {
            Tools.EntranceGuardCardBackupHelper backup = new Tools.EntranceGuardCardBackupHelper();
            backup.BackupEntranceGuardCardInfo(() =>
            {
                MessageBox.Show("备份完成...");
            });
        }

        private void MenuInitMySqlKKSAndDevMonitorNodes_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = new Bll();
            DbInitializer dil = new DbInitializer(bll);
            dil.InitKKSCode();
            AreaTreeInitializer ati = new AreaTreeInitializer(bll);
            ati.PubInitDevMonitorNode();
        }

        private void MenuInitMySqlKKSCode_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                DbInitializer dil = new DbInitializer(bll);
                dil.InitKKSCode();
                MessageBox.Show("初始化KKSCode点完成");
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void MenuInitMySqlDevMonitorNodes_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                AreaTreeInitializer ati = new AreaTreeInitializer(bll);
                ati.PubInitDevMonitorNode();
                MessageBox.Show("初始化监控节点完成");
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void MenuSaveTopCAD_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = Bll.NewBllNoRelation();
            AreaTreeInitializer initializer = new AreaTreeInitializer(bll);
            initializer.SaveInitInfoXml();
            MessageBox.Show("完成");
        }

        private void MenuInitMySqlNoramlDev_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                AreaTreeInitializer area = new AreaTreeInitializer(bll);
                area.InitDevInfo(false);
                MessageBox.Show("更新设备、门禁、摄像头信息完成。");
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void MenuInitTopoDev_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                AreaTreeInitializer initializer = new AreaTreeInitializer(new Bll());
                initializer.InitAreaAndDev();
            }, () =>
            {
                MessageBox.Show("完成");
            });
        }

        private void DbConfigureWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Title = AppSetting.ParkName;
        }

        private void MenuLoadCADArchorList_OnClick(object sender, RoutedEventArgs e)
        {

            string dir = AppDomain.CurrentDomain.BaseDirectory + "Data\\InitInfos\\" + AppSetting.ParkName +
                         "\\CADAnchors";
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (dirInfo.Exists == false)
            {
                MessageBox.Show("不存在文件夹:" + dir);
                return;
            }
            Worker.Run(() =>
            {
                Bll bll = new Bll();
                bll.Archors.Clear();
                var anchorDevs = bll.DevInfos.Where(i => i.Local_TypeCode == TypeCodes.Archor);
                bll.DevInfos.RemoveList(anchorDevs);
                AreaTreeInitializer initializer = new AreaTreeInitializer(bll);
                FileInfo[] files = dirInfo.GetFiles("*.xml");
                foreach (FileInfo file in files)
                {
                    CADAnchorList archorList = XmlSerializeHelper.LoadFromFile<CADAnchorList>(file.FullName);
                    if (archorList != null)
                    {
                        var area = bll.Areas.FindByName(archorList.ParentName);
                        if (area != null)
                        {
                            List<Archor> anchors = new List<Archor>();
                            foreach (var item in archorList.Anchors)
                            {
                                Archor anchor = new Archor();
                                anchor.Code = item.Name;
                                anchor.Name = item.Name;
                                var p = item.GetCenter();
                                anchor.X = p.X;
                                anchor.Z = p.Y;
                                anchor.Y = 2;
                                anchors.Add(anchor);
                                Log.Info(LogTags.DbInit, "初始化基站:" + item.Name);
                                initializer.AddArchorDev(anchor, area);
                            }

                            //initializer.AddArchorDevs(anchors, area);

                        }
                        else
                        {
                            Log.Error("未找到区域:" + archorList.ParentName);
                        }
                    }
                    else
                    {
                        Log.Error("解析失败:" + file.FullName);
                    }
                }
            }, () => { MessageBox.Show("完成"); });
            
        }

        private void MenuClearRepeatDev_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                AreaTreeInitializer initializer = new AreaTreeInitializer(new Bll());
                initializer.ClearRepeatDev(LogTags.DbInit);
            }, () =>
            {
                //MessageBox.Show("完成");
            });
        }

        private void MenuLoadDevTypeModelList_OnClick(object sender, RoutedEventArgs e)
        {
            int modelCount = 0;
            int typeCount = 0;
            Worker.Run(() =>
            {
                var bll = AppContext.GetLocationBll();
                DbInitializer initializer = new DbInitializer(bll);
                initializer.AddDevModelTypeByExcel((m, t) =>
                {
                    modelCount = m;
                    typeCount = t;
                });
            }, () =>
            {
                MessageBox.Show(string.Format("新增模型信息完成：modelCount:{0} typeCount:{1}", modelCount, typeCount));
            });

        }

        private void MenuImportArchorData_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                DbConfigureHelper.LoadArchorList(LogTags.DbInit);
            }, () =>
            {
                MessageBox.Show("完成");
            });
            
        }

        private void MenuArchorSettingImport_OnClick(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                Bll bll = new Bll();
                AreaTreeInitializer area = new AreaTreeInitializer(bll);
                area.InitLocationDevice();
                MessageBox.Show("导入基站位置信息成功。");
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private void MenuClearPowerAlarm_Click(object sender, RoutedEventArgs e)
        {
            int removeCount = 0;
            Worker.Run(() =>
            {                
                using (var _bll = Bll.NewBllNoRelation())
                {
                    List<LocationAlarmHistory> alarmList = _bll.LocationAlarmHistorys.ToList();
                    if(alarmList!=null&&alarmList.Count>0)
                    {
                        List<LocationAlarmHistory> powerAlarms = alarmList.FindAll(i=>i.AlarmType==LocationAlarmType.低电告警);
                        if(powerAlarms!=null&&powerAlarms.Count>0)
                        {
                            Log.Info("开始移除低电告警历史，耗时可能较长，完成后将有弹窗提示。");
                            removeCount = powerAlarms.Count;
                            _bll.LocationAlarmHistorys.RemoveList(powerAlarms);
                        }
                    }

                }
            }, () =>
            {
                MessageBox.Show(string.Format("移除低电告警历史完成，移除数量：{0}", removeCount));
                LocationService.RefreshDeviceAlarmBuffer(LogTags.Server);//实现加载全部设备告警到内存中
            });          
        }

        private void MenuAddDevInfoIp_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime recordT = DateTime.Now;
            Worker.Run(() =>
            {
                using (var db = Bll.NewBllNoRelation())
                {
                    var devList = db.DevInfos.ToList();
                    if (devList == null || devList.Count == 0) return;
                    var cameraInfoList = db.Dev_CameraInfos.ToList();
                    var archorList = db.Archors.ToList();
                    foreach (var item in devList)
                    {
                        string typeCode = item.Local_TypeCode.ToString();
                        if (TypeCodeHelper.IsCamera(typeCode))
                        {
                            Dev_CameraInfo infoT = cameraInfoList.Find(i => i.DevInfoId == item.Id);
                            if (infoT != null) item.IP = infoT.Ip;
                        }
                        else if (TypeCodeHelper.IsLocationDev(typeCode))
                        {
                            Archor archorT = archorList.Find(i => i.DevInfoId == item.Id);
                            if (archorT != null) item.IP = archorT.Ip;
                        }
                    }
                    db.DevInfos.EditRange(devList);
                }
            }, () =>
            {
                string costTime = string.Format("CostTime:{0}ms", (DateTime.Now - recordT).TotalMilliseconds);
                Log.Info("A-InitInfo", costTime);
                MessageBox.Show("设备信息，IP补充完成,建议重启服务端!");
            });

            Worker.Run(()=> 
            {
                LocationService.RefreshDeviceAlarmBuffer(LogTags.Server);//刷新缓存信息
            },()=> { });                       
        }
        /// <summary>
        /// 去除Personnel表中，名字的下划线（庄风\n）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuModifyPersonnel_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = Bll.NewBllNoRelation())
            {
                var personList = db.Personnels.ToList();
                int num = 0;
                foreach(var person in personList)
                {
                    string nameT = person.Name;
                    nameT.Trim();
                    if(nameT.Contains("\n"))
                    {
                        num++;
                        person.Name = nameT.Replace("\n", "");
                    }           
                }
                db.Personnels.EditRange(personList);
                MessageBox.Show(string.Format("名称去除下划线完成，共修改{0}个异常数据!",num));
            }
        }

        private void MenuBackupMySql_Click(object sender, RoutedEventArgs e)
        {
            MySqlBackUpWindow window = new MySqlBackUpWindow();
            window.Show();
        }

        private void MenuPersonAndCard_Mock_OnClick(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                DbInitializer initializer = new DbInitializer(new Bll());
                initializer.InitCardAndPerson_Mock();
            }, () =>
            {
                MessageBox.Show("完成");
            });

        }
    }
}

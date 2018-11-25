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
            Log.NewLogEvent += Log_NewLogEvent;
            Log.StartWatch();
            this.Closing += DbConfigureWindow_Closing   ;
        }

        private void DbConfigureWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.StopWatch();
            Log.NewLogEvent -= Log_NewLogEvent;
        }

        private void Log_NewLogEvent(string obj)
        {
            this.Dispatcher.Invoke(new Action(() => {
                TbConsole.Text = obj + "\n" + TbConsole.Text;
                //TbConsole.AppendText(obj);
            }));
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


            AppContext.DeleteDb(0);
            AppContext.InitDbAsync(0, 0, (bll) =>
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
            AppContext.InitDbAsync(1, 0, (bll) =>
            {
                InitImage(bll);
                MessageBox.Show("初始化完成");
            });
        }

        private void MenuInitTopo_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(()=>
            {
                AreaTreeInitializer initializer=new AreaTreeInitializer(new Bll());
                initializer.InitAreaAndDev();
                MessageBox.Show("完成");
            });
            thread.Start();
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
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitAuthorization();
            MessageBox.Show("完成");
        }

        private void MenuRealPos_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitRealTimePositions();
            MessageBox.Show("完成");
        }

        private void MenuPersonAndCard_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitCardAndPerson();
            MessageBox.Show("完成");
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
                                bound.IsRectangle = true;
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
                var r2 = bll.Bounds.DeleteById(item.InitBoundId);
                int percent = (int)((i+0.0) / cadAreas.Count*100);
                worker1.ReportProgress(percent);
            }
        }

        private void MenuArchorSetting_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

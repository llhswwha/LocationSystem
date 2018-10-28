using DbModel.Location.AreaAndDev;
using ExcelLib;
using Location.TModel.FuncArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocationServer.Controls
{
    /// <summary>
    /// ArchorListExportControl.xaml 的交互逻辑
    /// </summary>
    public partial class ArchorListExportControl : UserControl
    {
        public ArchorListExportControl()
        {
            InitializeComponent();
        }

        private void MenuExport_OnClick(object sender, RoutedEventArgs e)
        {
            ExcelHelper.ExportList(list, new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "//Archors.xls"));
        }

        private List<ArchorSetting> list;
        private List<Archor> archors;
        private BackgroundWorker worker;
        public void LoadData()
        {
            ProgressBar1.Visibility = Visibility.Visible;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        Area area;

        public void LoadData(Area area)
        {
            this.area = area;
            LoadData();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataGrid1.ItemsSource = list;
            //ProgressBar1.Visibility = Visibility.Hidden;
            ProgressBar1.Stop();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is Exception)
            {
                Exception ex = e.UserState as Exception;
                MessageBox.Show(ex.ToString());
            }

            ProgressBar1.Value = e.ProgressPercentage;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker.ReportProgress(0);
            var bll = AppContext.GetLocationBll();
            if (area == null)
            {
                archors = bll.Archors.ToList();
            }
            else
            {
                archors = bll.Archors.Where(i => i.DevInfo.ParentId == area.Id);
            }
            
            var devs = bll.DevInfos.ToList();
            var areas = bll.Areas.ToList();
            var bounds = bll.Bounds.ToList();
            var points = bll.Points.ToList();
            list = new List<ArchorSetting>();
            for (int i = 0; i < archors.Count; i++)
            {
                try
                {
                    var archor = archors[i];
                    ArchorSetting item = new ArchorSetting();
                    item.Id = archor.Id;
                    item.Code = archor.Code;
                    item.Name = archor.Name;
                    //var dev = archor.DevInfo;//大量循环获取sql数据的话采用按需加载的方式会慢很多
                    var dev = devs.Find(j => j.Id == archor.DevInfoId);//应该采用全部事先获取并从列表中搜索的方式，具体680多个，从35s变为1s
                    //var area = dev.Parent; 
                    var area = areas.Find(j => j.Id == dev.ParentId);
                    if (dev.ParentId == 2) //电厂
                    {
                        item.RelativeMode = (int)RelativeMode.Absolute;

                        item.AbsoluteX = archor.X.ToString("F2");
                        item.AbsoluteY = archor.Z.ToString("F2");
                        item.Height = archor.Y;

                        var floor = area;
                        item.AreaName = floor.Name;
                        item.AreaMinX = floor.InitBound.MinX.ToString("F2");
                        item.AreaMinY = floor.InitBound.MinY.ToString("F2");

                        item.ParkZeroX = item.AreaMinX;
                        item.ParkZeroY = item.AreaMinY;

                        item.RelativeX = (archor.X - floor.InitBound.MinX).ToString("F2");
                        item.RelativeY = (archor.Z - floor.InitBound.MinY).ToString("F2");


                    }
                    else
                    {
                        item.RelativeMode = (int)RelativeMode.Floor;

                        item.RelativeX = archor.X.ToString("F2");
                        item.RelativeY = archor.Z.ToString("F2");
                        item.Height = archor.Y;

                        var floor = area;

                        item.AreaName = floor.Name;

                        //var building = floor.Parent;
                        var building = areas.Find(j => j.Id == floor.ParentId);

                        var minX = floor.InitBound.MinX + building.InitBound.MinX;
                        var minY = floor.InitBound.MinY + building.InitBound.MinY;
                        item.AreaMinX = minX.ToString("F2");
                        item.AreaMinY = minY.ToString("F2");

                        item.AbsoluteX = (archor.X + minX).ToString("F2");
                        item.AbsoluteY = (archor.Z + minY).ToString("F2");

                        var rooms = areas.FindAll(j => j.ParentId == floor.Id);
                        var inRooms = rooms.FindAll(j => j.InitBound != null && j.InitBound.Contains(archor.X, archor.Z));
                        if (inRooms.Count > 0)
                        {
                            if (inRooms.Count == 1)
                            {
                                item.RoomName = inRooms[0].Name;
                                item.RoomMinX = inRooms[0].InitBound.MinX.ToString("F2");
                                item.RoomMinY = inRooms[0].InitBound.MinY.ToString("F2");
                            }
                            else
                            {
                                foreach (var inRoom in inRooms)
                                {
                                    item.RoomName = inRoom.Name + ";";
                                }
                                item.RoomMinX = inRooms[0].InitBound.MinX.ToString("F2");
                                item.RoomMinY = inRooms[0].InitBound.MinY.ToString("F2");
                            }
                        }
                    }
                    list.Add(item);

                    double percent = i * 100f / archors.Count;
                    worker.ReportProgress((int)percent);
                }
                catch (Exception ex)
                {
                    worker.ReportProgress(0, ex);
                }
            }
        }


        private void MenuCalculate_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in list)
            {

            }
        }

    }
}

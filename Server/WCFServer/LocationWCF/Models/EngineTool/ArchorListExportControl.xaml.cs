using DbModel.Location.AreaAndDev;
using ExcelLib;
using Location.TModel.FuncArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using BLL;
using DbModel.Engine;
using DbModel.Location.Settings;
using TModel.Tools;

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
            string fileName = string.Format("Archors_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\"+ fileName);
            list.Sort();
            ExcelHelper.ExportList(list, file);
            Process.Start(file.Directory.FullName);
        }

        private List<ArchorSetting> list;
        private List<Archor> archors;
        private BackgroundWorker worker;
        public void LoadData()
        {
            if (worker == null)
            {
                ProgressBar1.Visibility = Visibility.Visible;
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            else
            {
                
            }
        }

        int areaId;

        public void LoadData(int areaId)
        {
            this.areaId = areaId;
            LoadData();
        }

        public void Clear()
        {
            worker = null;
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

        List<ArchorSetting> listAdd = new List<ArchorSetting>();
        List<ArchorSetting> listEdit = new List<ArchorSetting>();

        public bool CalculateAll = false;

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker.ReportProgress(0);
            var bll = new Bll(false, false, false, false);
            if (areaId == 0)
            {
                archors = bll.Archors.ToList();
            }
            else
            {
                archors = bll.Archors.Where(i => i.ParentId == areaId);
            }

            var devs = bll.DevInfos.ToList();
            var areas = bll.Areas.GetWithBoundPoints(true);
            list = new List<ArchorSetting>();
            var list2 = bll.ArchorSettings.ToList();
            var list3 = new List<ArchorSetting>();
            for (int i = 0; i < archors.Count; i++)
            {
                try
                {
                    var archor = archors[i];
                    ArchorSetting archorSetting = bll.ArchorSettings.GetByArchor(archor);
                    if (archorSetting == null)
                    {
                        if (CalculateAll)
                        {
                            archorSetting = new ArchorSetting(archor.GetCode(), archor.Id);
                            listAdd.Add(archorSetting);
                            archorSetting.Name = archor.Name;
                            //var dev = archor.DevInfo;//大量循环获取sql数据的话采用按需加载的方式会慢很多
                            var dev = devs.Find(j => j.Id == archor.DevInfoId); //应该采用全部事先获取并从列表中搜索的方式，具体680多个，从35s变为1s
                                                                                //var area = dev.Parent; 
                            var floor = areas.Find(j => j.Id == dev.ParentId);
                            var building = areas.Find(j => j.Id == floor.ParentId);
                            if (building.Children == null)
                            {
                                building.Children = areas.FindAll(j => j.ParentId == building.Id);
                            }
                            var x = dev.PosX;
                            var y = dev.PosZ;
                            if (floor.IsPark()) //电厂
                            {
                                archorSetting.RelativeMode = RelativeMode.相对园区;
                                archorSetting.RelativeHeight = archor.Y;
                                archorSetting.AbsoluteHeight = archor.Y;
                                var park = floor;
                                var leftBottom = park.InitBound.GetLeftBottomPoint();
                                archorSetting.SetZero(leftBottom.X, leftBottom.Y);
                                archorSetting.SetRelative((x - leftBottom.X), (y - leftBottom.Y));
                                archorSetting.SetAbsolute(x, y);
                            }
                            else //机房
                            {
                                //var floor = area;
                                //var building = floor.Parent;
                                archorSetting.RelativeHeight = archor.Y;
                                archorSetting.AbsoluteHeight = (archor.Y + building.GetFloorHeight(floor.Id));

                                var minX = floor.InitBound.MinX + building.InitBound.MinX;
                                var minY = floor.InitBound.MinY + building.InitBound.MinY;

                                archorSetting.AbsoluteX = (x + minX).ToString("F3");
                                archorSetting.AbsoluteY = (y + minY).ToString("F3");

                                var room = Bll.GetDevRoom(floor, dev);
                                if (room != null)
                                {
                                    archorSetting.RelativeMode = RelativeMode.相对机房;
                                    var roomX = room.InitBound.MinX;
                                    var roomY = room.InitBound.MinY;
                                    archorSetting.SetPath(room, floor, building);
                                    archorSetting.SetZero(roomX, roomY);
                                    archorSetting.SetRelative((x - roomX), (y - roomY));
                                    archorSetting.SetAbsolute((minX + x), (minY + y));
                                }
                                else
                                {
                                    archorSetting.RelativeMode = RelativeMode.相对楼层;
                                    archorSetting.SetPath(null, floor, building);
                                    archorSetting.SetZero(0, 0);
                                    archorSetting.SetRelative(x, y);
                                    archorSetting.SetAbsolute((minX + x), (minY + y));
                                }
                            }
                        }
                       
                    }
                    else
                    {
                        bool r=archorSetting.CalAbsolute();
                        if (r == false)
                        {
                            list3.Add(archorSetting);
                        }
                        listEdit.Add(archorSetting);
                    }
                    if (archorSetting != null)
                    {

                        archorSetting.SetExtensionInfo(LocationContext.OffsetX, LocationContext.OffsetY);

                        if (ShowAll)
                        {
                            //if (archorSetting.RelativeHeight != 2)//过滤掉2m的基站 未测量位置坐标的
                                list.Add(archorSetting);
                        }
                        else
                        {
                            if (archorSetting.RelativeHeight != 2)//过滤掉2m的基站 未测量位置坐标的
                                list.Add(archorSetting);
                        }
                        
                    }

                    double percent = i*100f/archors.Count;
                    worker.ReportProgress((int) percent);
                }
                catch (Exception ex)
                {
                    worker.ReportProgress(0, ex);
                }
            }
        }

        private void MenuSave_OnClick(object sender, RoutedEventArgs e)
        {
            var bll = AppContext.GetLocationBll();
            if (bll.ArchorSettings.AddRange(listAdd) == false)
            {
                MessageBox.Show("保存失败");
            }
            else
            {
                MessageBox.Show("保存成功");
            }
        }

        private void MenuSave2_OnClick(object sender, RoutedEventArgs e)
        {
            var bll = AppContext.GetLocationBll();

            

            var list0 = bll.bus_anchors.ToList();
            var list1 = new List<bus_anchor>();
            var list2 = new List<bus_anchor>();
            foreach (var setting in list)
            {
                var item = list0.Find(i => i.anchor_id == setting.Code);
                if (item == null)
                {
                    item = new bus_anchor();
                    list1.Add(item);
                }
                else
                {
                    list2.Add(item);
                }
                item.anchor_x = (int) (setting.AbsoluteX.ToDouble()*100);
                item.anchor_y = (int) (setting.AbsoluteY.ToDouble()*100);
                item.anchor_z = (int) (setting.AbsoluteHeight*100);
            }

            if (bll.bus_anchors.AddRange(list1) && bll.bus_anchors.EditRange(list2))
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        private void MenuCalculate_Click(object sender, RoutedEventArgs e)
        {
            CalculateAll = true;
            worker = null;
            LoadData();
        }

        public bool ShowAll = false;

        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnGetAreas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbAreas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll = true;
            worker = null;
            LoadData();
        }
    }
}

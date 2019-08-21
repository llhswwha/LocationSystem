using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using DbModel;
using LocationServices.Locations.Services;
using WebApiCommunication.ExtremeVision;
using System.IO;
using Location.BLL.Tool;
using WPFClientControlLib;
using LocationServer.Tools;

namespace LocationServer.Windows
{
    /// <summary>
    /// CameraAlarmManagerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CameraAlarmManagerWindow : Window
    {
        public CameraAlarmManagerWindow()
        {
            InitializeComponent();
        }

        LogTextBoxController controller;// = new LogTextBoxController();
        private CameraAlarmService service = new CameraAlarmService();

        private void CameraAlarmManagerWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            controller = new LogTextBoxController(TbLog, LogTags.ExtremeVision);
        }

        private void DataGrid1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CameraAlarmInfo info=DataGrid1.SelectedItem as CameraAlarmInfo;
            if (info == null) return;
            CameraAlarmInfo detail=service.GetCameraAlarmDetail(info.id);
            GetImage(detail.pic_data);
            TbData.Text = detail.data + "";
        }

        public void GetImage(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                Image1.Source = null;
                Log.Error(LogTags.ExtremeVision, "没有告警图片！base64 is empty！");
                return;
            }
            base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(base64);
            //string imagebase64 = base64.Substring(base64.IndexOf(",") + 1);

            MemoryStream memStream = new MemoryStream(bytes);
            System.Drawing.Image mImage = System.Drawing.Image.FromStream(memStream);
            string path = AppDomain.CurrentDomain.BaseDirectory + "1.jpg";
            mImage.Save(path);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(bytes);
            bi.EndInit();
            Image1.Source = bi;
        }

        private void MenuSaveCameraAlarmPicture_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuSaveCameraAlarmPicture2_OnClick(object sender, RoutedEventArgs e)
        {
            CameraAlarmService service = new CameraAlarmService();
            service.SeparateImages_PicToFile(() =>
            {
                MessageBox.Show("完成");
            });
        }

        private void MenuOpenPictureDir_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(AppSetting.CameraAlarmPicSaveDir);
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            //可以删除全部告警
            List<CameraAlarmInfo> list = new List<CameraAlarmInfo>();
            foreach (var item in DataGrid1.SelectedItems)
            {
                CameraAlarmInfo info = item as CameraAlarmInfo;
                if (info == null) continue;
                list.Add(info);
            }
            Worker.Run(() =>
            {
                foreach (CameraAlarmInfo item in list)
                {
                    service.RemoveAlarm(item);
                }
            }, () => { LoadData(); });
        }

        private void MenuSaveAlarmToJson_OnClick(object sender, RoutedEventArgs e)
        {
            service.AlarmSaveToJsonAll();
        }

        private void MenuLoadAlarmFromJson_OnClick(object sender, RoutedEventArgs e)
        {
            Worker.Run(()=>
            {
                service.LoadAlarmFromJson();//从json文件中读取告警，不知道为什么，发生了7月9号到7月22号，告警数据丢失的问题，但是7月20号有数据库备份。
            },()=>
            {
                LoadData();
            });
           
           
        }

        private void MenuOpenJsonDir_OnClick(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = CameraAlarmService.GetJsonDir();
            Process.Start(dir.FullName);
        }

        private void MenuLoad_OnClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (DataGrid1 == null) return;
                var list = service.GetAllCameraAlarms(false);
                if (list == null)
                {
                    MessageBox.Show("加载数据失败，GetAllCameraAlarms list == null");
                    return;
                }
                var type = CbType.SelectedIndex;
                if (type == 0)
                {
                    DataGrid1.ItemsSource = list;
                }
                else
                {
                    DataGrid1.ItemsSource = list.FindAll(i => i.AlarmType == type);
                }

                Dictionary<string, FileInfo> fileDict = new Dictionary<string, FileInfo>();


                int fileCount = 0;
                var files = service.GetAllPictureFiles();
                if (files != null)
                {
                    fileCount = files.Length;
                    //Log.Info(LogTags.ExtremeVision, " fileCount:" + files.Length);
                    foreach (var item in files)
                    {
                        fileDict.Add(item.Name, item);
                    }
                }
               
                var picList = service.GetPictureCount();
                Log.Info(LogTags.ExtremeVision, string.Format("AlarmCount:{0},fileCount:{1}, PictureCount:{2}", list.Count, fileCount, picList));

                List<CameraAlarmInfo> noPicList = new List<CameraAlarmInfo>();
                List<CameraAlarmInfo> havePicList = new List<CameraAlarmInfo>();
                foreach (var item in list)
                {
                    if (fileDict.ContainsKey(item.pic_name))
                    {
                        havePicList.Add(item);
                        fileDict.Remove(item.pic_name);//删除已经有的
                    }
                    else
                    {
                        noPicList.Add(item);
                    }
                    //service.GetCameraAlarmDetail
                }

                //DataGrid1.ItemsSource = havePicList;

                var picNoAlarms = fileDict.Values.ToList();//剩下来的没有告警的图片（在当前数据库中）
                Log.Info(LogTags.ExtremeVision, string.Format(" picAlarms:{0}, picNoAlarms:{1}", havePicList.Count, picNoAlarms.Count));
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.ExtremeVision,ex.ToString());
                MessageBox.Show("加载数据失败:" + ex.Message);
            }
            
        }

        private void MenuParseData_Click(object sender, RoutedEventArgs e)
        {
            service.SeparateImages(() =>
            {
                service.SeparateImages_PicToFile(() =>
                {
                    //MessageBox.Show("完成");

                    Worker.Run(() =>
                    {
                        service.LoadAlarmFromJson();//从json文件中读取告警，不知道为什么，发生了7月9号到7月22号，告警数据丢失的问题，但是7月20号有数据库备份。
                    }, () =>
                    {
                        LoadData();
                        MessageBox.Show("完成");
                    });
                });
                //MessageBox.Show("完成");
            }, AppSetting.CameraAlarmPicSaveMode, 10);
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void MenuMenuClearAll_Click(object sender, RoutedEventArgs e)
        {
            //可以删除全部告警
            List<CameraAlarmInfo> list = DataGrid1.ItemsSource as List<CameraAlarmInfo>;
            Worker.Run(() =>
            {
                foreach (CameraAlarmInfo item in list)
                {
                    service.RemoveAlarm(item);
                }
            }, () => { LoadData(); });
        }
    }
}

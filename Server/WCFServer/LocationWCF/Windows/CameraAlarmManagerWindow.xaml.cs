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

        private CameraAlarmService service = new CameraAlarmService();

        private void CameraAlarmManagerWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var list=service.GetAllCameraAlarms(true);
            DataGrid1.ItemsSource = list;
        }

        private void DataGrid1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CameraAlarmInfo info=DataGrid1.SelectedItem as CameraAlarmInfo;
            if (info == null) return;
            CameraAlarmInfo detail=service.GetCameraAlarmDetail(info.id);

        }

        private void MenuSaveCameraAlarmPicture_OnClick(object sender, RoutedEventArgs e)
        {
            service.SeparateImages_ToPictures(() =>
            {
                MessageBox.Show("完成");
            });
        }

        private void MenuSaveCameraAlarmPicture2_OnClick(object sender, RoutedEventArgs e)
        {
            CameraAlarmService service = new CameraAlarmService();
            service.SeparateImages_ToFile(() =>
            {
                MessageBox.Show("完成");
            });
        }

        private void MenuOpenPictureDir_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(AppSetting.CameraAlarmPicSaveDir);
        }
    }
}

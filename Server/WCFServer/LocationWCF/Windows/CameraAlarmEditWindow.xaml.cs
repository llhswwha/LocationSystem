using DbModel.Location.Alarm;
using Location.BLL.Tool;
using LocationServices.Locations.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using WebApiCommunication.ExtremeVision;

namespace LocationServer.Windows
{
    /// <summary>
    /// CameraAlarmEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CameraAlarmEditWindow : Window
    {
        private CameraAlarmInfo info;
        private CameraAlarmService service = new CameraAlarmService();

        public CameraAlarmEditWindow(CameraAlarmInfo info)
        {
            InitializeComponent();
            this.info = info;
            lbdevName.Content = info.DevName;
            lbdevIp.Content = info.DevIp;
            lbdevId.Content = info.DevID;
            txtAlarmType.Text =info.AlarmType.ToString();
            lbid.Content = info.id;
            lbstarinfo.Content = info.startInfo;
            lbtime.Content = info.time;
            lbaid.Content = info.aid;
            lbcid.Content = info.cid;
            lbcidurl.Content = info.cid_url;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CameraAlarmInfo camera = this.info;
                camera.AlarmType = int.Parse(txtAlarmType.Text);
                CameraAlarmJson cameraJson = new CameraAlarmJson();
                cameraJson.Id = camera.id;
                string json = JsonConvert.SerializeObject(camera);
                cameraJson.Json = Encoding.UTF8.GetBytes(json);
                BLL.Bll bll = BLL.Bll.NewBllNoRelation();
                bll.CameraAlarmJsons.Edit(cameraJson);
            }
            catch (Exception ex)
            {
                Log.Info("saveCameraJson:"+ex.ToString());
            }
        }
    }
}

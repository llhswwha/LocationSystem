using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.BaseData;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using Location.BLL.Tool;
using LocationServer.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WebApiLib;
using WebApiLib.Clients;
using WPFClientControlLib;

namespace LocationServer.Windows
{
    /// <summary>
    /// SyncAllDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SyncAllDataWindow : Window
    {
        public SyncAllDataWindow()
        {
            InitializeComponent();
            

            logController.Init(TbLogs, LogTags.BaseData);
        }

        LogTextBoxController logController = new LogTextBoxController();

        private void Window_Closed(object sender, EventArgs e)
        {
            logController.Dispose();
        }

        private void MenuSync_Click(object sender, RoutedEventArgs e)
        {
            Sync();
        }

        private void MenuCreateSimulateData_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                BaseDataSimulator simulator = new BaseDataSimulator();
                simulator.SaveDepToOrg();
                simulator.SavePersonnelToUser();
                simulator.SaveAreaToZone();
                simulator.SaveDevInfoToDevice();
                simulator.SaveGuardCardToCard();
            }, () =>
             {
                 Log.Info(LogTags.BaseData, "创建完成!");
                 MessageBox.Show("创建完成");
             });
        }

        private string datacaseUrl = "ipms-demo.datacase.io";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datacaseUrl = AppContext.DatacaseWebApiUrl;
            client = new BaseDataClient(datacaseUrl, null, "api");

            //Sync();
            GetDate();

            Bll bll = Bll.Instance();
            List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();
            dg_camera.ItemsSource = dclst;
        }

        private void Sync()
        {
            Worker.Run(() =>
            {
                try
                {
                    //bool isSave1 = true;
                    bool isSave2 = true;

                    //区域
                    //zoneList = client.GetZoneList();
                    areaList = client.GetAreaList(isSave2);

                    //组织
                    //orgList = client.GetOrgList();
                    depList = client.GetDepList(isSave2);

                    //人员
                    //userList = client.GetUserList();
                    personnelList = client.GetPersonnelList(isSave2);

                    //设备
                    //deviceList = client.GetDeviceList(null, null, null);
                    devInfoList = client.GetDevInfoList(null, null, null, isSave2);

                    //门禁
                    //cardList = client.GetCardList();
                    guardCardList = client.GetGuardCardList(isSave2);

                    //告警事件
                    eventList = client.GetEventList(null, null, null, null);
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                Log.Info(LogTags.BaseData, "同步完成!");
                MessageBox.Show("同步完成");
            });
        }

        BaseDataClient client;

        List<zone> zoneList;
        List<Area> areaList;
        List<org> orgList;
        List<Department> depList;
        List<user> userList;
        List<Personnel> personnelList;
        List<device> deviceList;
        List<DevInfo> devInfoList;
        List<cards> cardList;
        List<EntranceGuardCard> guardCardList;
        List<events> eventList;

        private void MenuGet_Click(object sender, RoutedEventArgs e)
        {
            GetDate();
        }

        private void GetDate()
        {
            Worker.Run(() =>
            {
                try
                {
                    WebApiHelper.IsSaveJsonToFile = true;//保存数据到文件中
                    bool isSave1 = true;
                    bool isSave2 = false;

                    //区域
                    zoneList = client.GetZoneList();
                    //areaList = client.GetAreaList(isSave2);

                    //组织
                    orgList = client.GetOrgList();
                    //depList = client.GetDepList(isSave2);

                    //人员
                    userList = client.GetUserList();
                    //personnelList = client.GetPersonnelList(isSave2);

                    //设备
                    deviceList = client.GetDeviceList(null, null, null);
                    //devInfoList = client.GetDevInfoList(null, null, null, isSave2);

                    //门禁
                    cardList = client.GetCardList();
                    //guardCardList = client.GetGuardCardList(isSave2);

                    //告警事件
                    eventList = client.GetEventList(null, null, null, null);

                    WebApiHelper.IsSaveJsonToFile = false;
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.BaseData, "Worker_DoWork:" + ex);
                }
            }, () =>
            {
                dg_zone.ItemsSource = zoneList;
                dg_org.ItemsSource = orgList;
                dg_user.ItemsSource = userList;
                dg_dev.ItemsSource = deviceList;
                dg_event.ItemsSource = eventList;
            });
        }

        private void MenuGetRtsp_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                var list1 = client.GetCameraInfoList(null, null, null, true);
                var list2 = client.GetCameraInfoList("1021,1022,1023", null, null, true);
            }, () =>
            {
                Bll bll = Bll.Instance();
                List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();
                dg_camera.ItemsSource = dclst;
                Log.Info(LogTags.BaseData, "完成!");
                MessageBox.Show("完成");
            });
        }

        private void MenuCreateRealData_Click(object sender, RoutedEventArgs e)
        {
            Worker.Run(() =>
            {
                BaseDataInitializer initializer = new BaseDataInitializer();
                deviceList=initializer.InitDevices();

            }, () =>
            {
                dg_zone.ItemsSource = zoneList;
                dg_org.ItemsSource = orgList;
                dg_user.ItemsSource = userList;
                dg_dev.ItemsSource = deviceList;
                dg_event.ItemsSource = eventList;

                Log.Info(LogTags.BaseData, "完成!");
                MessageBox.Show("完成");
            });
        }
    }
}

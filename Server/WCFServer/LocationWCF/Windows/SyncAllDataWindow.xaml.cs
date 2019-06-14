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
            Location.BLL.Tool.Log.NewLogEvent += Log_NewLogEvent;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Location.BLL.Tool.Log.NewLogEvent -= Log_NewLogEvent;
        }

        private int MaxLength = 100000;
        private int MaxLength2 = 50000;

        private string logs = "";

        private void Log_NewLogEvent(string tag, string log)
        {
            if (logs.Length > MaxLength)
            {
                logs = logs.Substring(0, MaxLength2);
            }

            //string[] parts = log.Split('|');
            if (tag == LogTags.BaseData)
            {
                logs = log + "\n" + logs;
                TbLogs.Dispatcher.Invoke(() =>
                {
                    TbLogs.Text = logs;
                });
            }
        }

        private void MenuSync_Click(object sender, RoutedEventArgs e)
        {
            Sync();
        }

        private void MenuCreateSimulateData_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker simulateWorker = new BackgroundWorker();
            simulateWorker.DoWork += SimulateWorker_DoWork;
            simulateWorker.RunWorkerCompleted += SimulateWorker_RunWorkerCompleted;
            simulateWorker.RunWorkerAsync();
        }

        private void SimulateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.Info(LogTags.BaseData, "创建完成!");
            MessageBox.Show("创建完成");
        }

        private void SimulateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            client.SaveDepToOrg();
            client.SavePersonnelToUser();
            client.SaveAreaToZone();
            client.SaveDevInfoToDevice();
            client.SaveGuardCardToCard();
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
            BackgroundWorker syncWorker = new BackgroundWorker();
            syncWorker.DoWork += SyncWorker_DoWork;
            syncWorker.RunWorkerCompleted += SyncWorker_RunWorkerCompleted;
            syncWorker.RunWorkerAsync();
        }

        private void SyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //dg_zone.ItemsSource = zoneList;
            //dg_org.ItemsSource = orgList;
            //dg_user.ItemsSource = userList;
            //dg_dev.ItemsSource = deviceList;
            //dg_event.ItemsSource = eventList;
            Log.Info(LogTags.BaseData, "同步完成!");
            MessageBox.Show("同步完成");
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

        private void SyncWorker_DoWork(object sender, DoWorkEventArgs e)
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
        }

        private void MenuGet_Click(object sender, RoutedEventArgs e)
        {
            GetDate();
        }

        private void GetDate()
        {
            BackgroundWorker getWorker = new BackgroundWorker();
            getWorker.DoWork += GetWorker_DoWork;
            getWorker.RunWorkerCompleted += GetWorker_RunWorkerCompleted;  
            getWorker.RunWorkerAsync();
        }

        private void GetWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dg_zone.ItemsSource = zoneList;
            dg_org.ItemsSource = orgList;
            dg_user.ItemsSource = userList;
            dg_dev.ItemsSource = deviceList;
            dg_event.ItemsSource = eventList;
        }

        private void GetWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WebApiHelper.IsSaveJsonToFile = true;
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
        }

        private void MenuGetRtsp_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker rtspWorker = new BackgroundWorker();
            rtspWorker.DoWork += RtspWorker_DoWork;
            rtspWorker.RunWorkerCompleted += RtspWorker_RunWorkerCompleted;
            rtspWorker.RunWorkerAsync();
        }

        private void RtspWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Bll bll = Bll.Instance();
            List<Dev_CameraInfo> dclst = bll.Dev_CameraInfos.ToList();
            dg_camera.ItemsSource = dclst;
            Log.Info(LogTags.BaseData, "完成!");
            MessageBox.Show("完成");
        }

        private void RtspWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var list = client.GetCameraInfoList(null, null, null, true);
        }
    }
}

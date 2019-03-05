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
using Location.TModel.FuncArgs;
using Location.TModel.Location.Alarm;
using LocationClient.WebApi;
using SignalRClientLib;
using Location.TModel.Location.AreaAndDev;
using LocationClient.Tools;
using TModel.Location.AreaAndDev;
using TModel.Location.Person;
using WCFServiceForWPF.LocationServices;
using TModel.LocationHistory.AreaAndDev;
using TModel.Location.Work;
using Location.TModel.Location.Person;

namespace LocationWCFClient.Windows
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private string SignalRServerURI = SignalRAppContext.ServerUrl;

        private AlarmHub alarmHub;

        public ClientWindow()
        {
            InitializeComponent();
        }

        LocationServiceClient client;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = AppContext.Instance.Client.InnerClient;
            var treeRoot1= client.GetPhysicalTopologyTree(0);
            var treeRoot2 = client.GetDepartmentTree();
            ResourceTreeView1.LoadData(treeRoot1, treeRoot2);
            ResourceTreeView1.TopoTree.Tree.SelectedItemChanged += Tree_SelectedItemChanged;

             var devList = client.GetDevInfos(null);
            DeviceListBox1.LoadData(devList);

            var personList = client.GetPersonList();
            PersonListBox1.LoadData(personList);

            var tagList = client.GetTags();
            TagListBox1.LoadData(tagList);

            var archorList = client.GetArchors();
            AchorListBox1.LoadData(archorList);

            AppContext.Instance.CallbackClient.LocAlarmsReceved += CallbackClient_LocAlarmsReceved;

            DeviceAlarm[] devAlarms= client.GetDeviceAlarms(new AlarmSearchArg());
            ShowDeviceAlarms(devAlarms);

            LocationAlarm[] locAlarms = client.GetLocationAlarms(new AlarmSearchArg());
            ShowLocationAlarms(locAlarms);

            InitAlarmHub();
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item == null) return;
            AreaCanvas1.ShowDev = true;
            //AreaCanvas1.ShowArea(item.Tag as PhysicalTopology);
        }

        protected List<LocationAlarm> LocationAlarms = new List<LocationAlarm>();

        private void ShowLocationAlarms(LocationAlarm[] locAlarms)
        {
            LocationAlarms.AddRange(locAlarms);
            DataGridLocationAlarms.ItemsSource = null;
            DataGridLocationAlarms.ItemsSource = LocationAlarms;
        }

        private void ShowDeviceAlarms(DeviceAlarm[] devAlarms)
        {
            DeviceAlarms.AddRange(devAlarms);
            DeviceAlarmListBox1.LoadData(DeviceAlarms.ToArray());
        }

        protected List<DeviceAlarm> DeviceAlarms = new List<DeviceAlarm>();

        private async void InitAlarmHub()
        {
            alarmHub = new AlarmHub(SignalRServerURI);
            alarmHub.GetDeviceAlarms += AlarmHub_GetDeviceAlarms;
            alarmHub.GetLocationAlarms += AlarmHub_GetLocationAlarms;
            await alarmHub.Start();
        }

        private void AlarmHub_GetLocationAlarms(LocationAlarm[] obj)
        {
            this.Dispatcher.Invoke(() =>
                    ShowLocationAlarms(obj)
                );
        }

        private void AlarmHub_GetDeviceAlarms(DeviceAlarm[] obj)
        {
            this.Dispatcher.Invoke(() =>
                    ShowDeviceAlarms(obj)
                );
        }

        private void CallbackClient_LocAlarmsReceved(Location.TModel.Location.Alarm.LocationAlarm[] obj)
        {
            MessageBox.Show("告警推送");
        }

        private void MenuSignalR_Click(object sender, RoutedEventArgs e)
        {
            var window = new EchoHubWindow();
            window.Show();
        }

        private void MenuChatHubMenu_Click(object sender, RoutedEventArgs e)
        {
            var window = new ChatHubWindow();
            window.Show();
        }

        private void MenuAlarm_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AlarmWindow();
            window.Show();
        }

        private void MenuWebApi_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new WebApiWindow();
            window.SetUrl(AppContext.Instance.GetWebApiUrl());
            window.Show();
        }

        private void BtnModifyArchor_OnClick(object sender, RoutedEventArgs e)
        {
            Archor Archor = new Archor();
            Archor.Code = "85G8";
            Archor.Name = "测试";
            Archor.X = 1000;
            Archor.Y = 1000;
            Archor.Z = 1000;
            Archor.Type = 0;
            Archor.IsAutoIp = true;
            Archor.Ip = "127.0.0.1";
            Archor.ServerIp = "127.0.0.1";
            Archor.ServerPort = 40010;
            Archor.Power = 100;
            Archor.AliveTime = 100;
            Archor.Enable = 0;
            Archor.DevInfoId = 10;

            client.EditBusAnchor(Archor,1);
        }

        private void BtnModifyTag_OnClick(object sender, RoutedEventArgs e)
        {
            Tag Tag = new Tag();

            Tag.Code = "0004";
            Tag.Name = "卡3";
            Tag.Describe = "测试";

            client.EditBusTag(Tag);
        }

        private void BtnModifyPicture_OnClick(object sender, RoutedEventArgs e)
        {
            string strName = "顶视图";
            //string strInfo = "还贷款萨丹哈";
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Images\\顶视图.png";
            byte[] byteArray =  ImageHelper.LoadImageFile(path);
            Picture pc = new Picture();
            pc.Name = strName;
            pc.Info = byteArray;
            bool bReturn = client.EditPictureInfo(pc);
            if (bReturn)
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        private void BtnGetPicture_OnClick(object sender, RoutedEventArgs e)
        {
            string strName = "顶视图";
            Picture pc = client.GetPictureInfo(strName);
            byte[] byteArray = pc.Info;
            System.Drawing.Image image = ImageHelper.BytesToImage(byteArray);
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Images\\顶视图2.png";
            image.Save(path);
            BitmapImage bitmapImage = ImageHelper.ToBitmapImage(byteArray);
            Image1.Source = bitmapImage;
        }

        private void BtnClearPicture_OnClick(object sender, RoutedEventArgs e)
        {
            Image1.Source = null;
        }

        private void BtnGetAreaStatistics_OnClick(object sender, RoutedEventArgs e)
        {
            AreaStatistics recv = client.GetAreaStatistics(1);
            int PersonNum = recv.PersonNum;
            int DevNum = recv.DevNum;
            int LocationAlarmNum = recv.LocationAlarmNum;
            int DevAlarmNum = recv.DevAlarmNum;
        }

        private void GetNearbyPerson_Currency_OnClick(object sender, RoutedEventArgs e)
        {
            NearbyPerson[] lst = client.GetNearbyPerson_Currency(1,200);
        }

        private void GetNearbyPerson_Alarm_OnClick(object sender, RoutedEventArgs e)
        {
            NearbyPerson[] lst = client.GetNearbyPerson_Alarm(1,200);
        }

        private void GetNearbyDev_Currency_OnClick(object sender, RoutedEventArgs e)
        {
            NearbyDev[] lst = client.GetNearbyDev_Currency(1,200,0);
        }

        private void GetNearbyCamera_Alarm_OnClick(object sender, RoutedEventArgs e)
        {
            NearbyDev[] lst = client.GetNearbyCamera_Alarm(1,200);
        }

        private void GetEntranceActionInfoByPerson24Hours_OnClick(object sender, RoutedEventArgs e)
        {
            EntranceGuardActionInfo[] lst = client.GetEntranceActionInfoByPerson24Hours(1);
        }

        private void MenuGetRealPos_Click(object sender, RoutedEventArgs e)
        {
            var list=client.GetRealPositons();
            DataGridRealPos.ItemsSource = list;
        }

        private void MenuGetHisPos_Click(object sender, RoutedEventArgs e)
        {
            var list = client.GetHistoryPositons();
            //DataGridHisPos.ItemsSource = list;
            DataGridRealPos.ItemsSource = list;
        }

        private void GetDevMonitorInfoByKKS_Click(object sender, RoutedEventArgs e)
        {
            Dev_Monitor sd = client.GetDevMonitorInfoByKKS("20MAA 12CT121A", true);
            int nnn = 0;
        }

        private void GetInspectionTrack_Click(object sender, RoutedEventArgs e)
        {
        //    DateTime dt = DateTime.Now;
        //    DateTime dtTime = DateTime.Now.AddHours(-10);
        //    InspectionTrack[] lst = client.Getinspectionlist(dtTime, false);
        //    DateTime dt2 = DateTime.Now;
        //    int nn = 0;
        }

        private void GetAreaBasicList_Click(object sender, RoutedEventArgs e)
        {
            PhysicalTopology pt = client.GetPhysicalTopologyTree(0);
            int nn = 0;
        }

        private void GetAreaDevInfoList_Click(object sender, RoutedEventArgs e)
        {
            PhysicalTopology pt = client.GetPhysicalTopologyTree(1);
            int nn = 0;
        }

        private void GetAreaPersonnelInfoList_Click(object sender, RoutedEventArgs e)
        {
            PhysicalTopology pt = client.GetPhysicalTopologyTree(2);
            int nn = 0;
        }

        private void GetAreaDevPersonnelInfoList_Click(object sender, RoutedEventArgs e)
        {
            PhysicalTopology pt = client.GetPhysicalTopologyTree(3);
            int nn = 0;
        }


        #region 测试基础信息平台
        //获取人员列表
        private void GetUserList_Click(object sender, RoutedEventArgs e)
        {
            Personnel[] lst = client.GetUserList();
            int nn = 0;
        }

        //获取部门列表
        private void GetorgList_Click(object sender, RoutedEventArgs e)
        {
            Department[] lst = client.GetorgList();
            int nn = 0;
        }

        //获取区域列表
        private void GetzonesList_Click(object sender, RoutedEventArgs e)
        {
            PhysicalTopology[] lst = client.GetzonesList();
            int nn = 0;
        }

        //获取单个区域信息
        private void GetSingleZonesInfo_Click(object sender, RoutedEventArgs e)
        {
            PhysicalTopology lst = client.GetSingleZonesInfo(1,7);
            int nn = 0;
        }

        //获取指定区域下设备列表
        private void GetZoneDevList_Click(object sender, RoutedEventArgs e)
        {
            Location.TModel.Location.AreaAndDev.DevInfo[] lst = client.GetZoneDevList(1);
            int nn = 0;
        }

        //获取设备列表
        private void GetDeviceList_Click(object sender, RoutedEventArgs e)
        {
            Location.TModel.Location.AreaAndDev.DevInfo[] lst = client.GetDeviceList(null,null,null);
            int nn = 0;
        }

        //获取单个设备信息
        private void GetSingleDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            Location.TModel.Location.AreaAndDev.DevInfo lst = client.GetSingleDeviceInfo(1);
            int nn = 0;
        }

        //获取单台设备操作历史
        private void GetSingleDeviceActionHistory_Click(object sender, RoutedEventArgs e)
        {
            client.GetSingleDeviceActionHistory(1, null, null);
            int nn = 0;
        }

        //获取门禁卡列表
        private void GetCardList_Click(object sender, RoutedEventArgs e)
        {
            client.GetCardList();
            int nn = 0;
        }

        //获取门禁卡操作历史
        private void GetSingleCardActionHistory_Click(object sender, RoutedEventArgs e)
        {
            client.GetSingleCardActionHistory(1, null, null);
            int nn = 0;
        }

        //获取两票列表
        private void GetTicketsList_Click(object sender, RoutedEventArgs e)
        {
            client.GetTicketsList("1", null, null);
            int nn = 0;
        }

        //获取指定的两票详情
        private void GetTicketsDetail_Click(object sender, RoutedEventArgs e)
        {
            client.GetTicketsDetail(1, null, null);
            int nn = 0;
        }

        //获取告警事件列表
        private void GeteventsList_Click(object sender, RoutedEventArgs e)
        {
            client.GeteventsList(null, null, null,null);
            int nn = 0;
        }

        //获取SIS传感数据
        private void GetSomesisList_Click(object sender, RoutedEventArgs e)
        {
            client.GetSomesisList("k01,k02");
            int nn = 0;
        }

        //获取SIS历史数据
        private void GetSomeSisHistoryList_Click(object sender, RoutedEventArgs e)
        {
            client.GetSomeSisHistoryList("k01", true);

            client.GetSomeSisHistoryList("k01", false);
            int nn = 0;
        }

        //获取SIS采样历史数据
        private void GetSisSamplingHistoryList_Click(object sender, RoutedEventArgs e)
        {
            client.GetSisSamplingHistoryList("k01,k02");
            int nn = 0;
        }

        //获取巡检轨迹列表
        private void Getinspectionlist_Click(object sender, RoutedEventArgs e)
        {
            client.Getinspectionlist(1, 1, false);
            int nn = 0;
        }

        //获取巡检节点列表
        private void Getcheckpoints_Click(object sender, RoutedEventArgs e)
        {
            client.Getcheckpoints(134);
            int nn = 0;
        }

        //获取巡检结果列表
        private void Getcheckresults_Click(object sender, RoutedEventArgs e)
        {
            client.Getcheckresults(469, "100012");
            int nn = 0;
        }

        #endregion


    }
}

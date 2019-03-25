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
using System.Windows.Threading;

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
            if (locAlarms == null) return;
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
            List<Dev_Monitor> lst = new List<Dev_Monitor>();
            List<Dev_Monitor> nolst = new List<Dev_Monitor>();
            Dictionary<string, Dev_Monitor> hasdict = new Dictionary<string, Dev_Monitor>();
            Dictionary<string, Dev_Monitor> nodictHasMon = new Dictionary<string, Dev_Monitor>();
            Dictionary<string, Dev_Monitor> nodictNoMon = new Dictionary<string, Dev_Monitor>();

            Dev_Monitor sd = client.GetDevMonitorInfoByKKS("10LCE16", true);
            Dev_Monitor sd2 = client.GetDevMonitorInfoByKKS("11MKG08", true);
            Dev_Monitor sd3 = client.GetDevMonitorInfoByKKS("11MKF21", true);
            Dev_Monitor sd4 = client.GetDevMonitorInfoByKKS("10LCE13", true);
            Dev_Monitor sd5 = client.GetDevMonitorInfoByKKS("10PGA11", true);
            Dev_Monitor sd6 = client.GetDevMonitorInfoByKKS("10MAJ21", true);
            Dev_Monitor sd7 = client.GetDevMonitorInfoByKKS("10PGA34", true);
            Dev_Monitor sd8 = client.GetDevMonitorInfoByKKS("21MKF21", true);
            Dev_Monitor sd9 = client.GetDevMonitorInfoByKKS("20MAA50", true);
            Dev_Monitor sd10 = client.GetDevMonitorInfoByKKS("20PGA11", true);
            Dev_Monitor sd11 = client.GetDevMonitorInfoByKKS("20PGA34", true);
            Dev_Monitor sd12 = client.GetDevMonitorInfoByKKS("20LCE13", true);
            Dev_Monitor sd13 = client.GetDevMonitorInfoByKKS("21MKG08", true);
            Dev_Monitor sd14 = client.GetDevMonitorInfoByKKS("20MAV10", true);
            Dev_Monitor sd15 = client.GetDevMonitorInfoByKKS("20MAJ21", true);
            Dev_Monitor sd16 = client.GetDevMonitorInfoByKKS("20PAH12", true);
            Dev_Monitor sd17 = client.GetDevMonitorInfoByKKS("20LCA12", true);
            Dev_Monitor sd18 = client.GetDevMonitorInfoByKKS("20MAC01", true);
            Dev_Monitor sd19 = client.GetDevMonitorInfoByKKS("20LCE16", true);
            Dev_Monitor sd20 = client.GetDevMonitorInfoByKKS("21MKG30", true);
            Dev_Monitor sd21 = client.GetDevMonitorInfoByKKS("10PAH12", true);
            Dev_Monitor sd22 = client.GetDevMonitorInfoByKKS("10PCB11", true);
            Dev_Monitor sd23 = client.GetDevMonitorInfoByKKS("10MAV30", true);
            Dev_Monitor sd24 = client.GetDevMonitorInfoByKKS("10MAV10", true);
            Dev_Monitor sd25 = client.GetDevMonitorInfoByKKS("10MAX01", true);
            Dev_Monitor sd26 = client.GetDevMonitorInfoByKKS("10LCA12", true);
            Dev_Monitor sd27 = client.GetDevMonitorInfoByKKS("10LCE15", true);
            Dev_Monitor sd28 = client.GetDevMonitorInfoByKKS("10LCA40", true);
            Dev_Monitor sd29 = client.GetDevMonitorInfoByKKS("10LCA20", true);
            Dev_Monitor sd30 = client.GetDevMonitorInfoByKKS("10MBH20", true);
            Dev_Monitor sd31 = client.GetDevMonitorInfoByKKS("10QEA20", true);
            Dev_Monitor sd32 = client.GetDevMonitorInfoByKKS("10MBA41", true);
            Dev_Monitor sd33 = client.GetDevMonitorInfoByKKS("10PGA26", true);
            Dev_Monitor sd34 = client.GetDevMonitorInfoByKKS("11MKG08", true);
            Dev_Monitor sd35 = client.GetDevMonitorInfoByKKS("11MKG30", true);
            Dev_Monitor sd36 = client.GetDevMonitorInfoByKKS("10MBV10", true);
            Dev_Monitor sd37 = client.GetDevMonitorInfoByKKS("10MAV41", true);
            Dev_Monitor sd38 = client.GetDevMonitorInfoByKKS("20PCB11", true);
            Dev_Monitor sd39 = client.GetDevMonitorInfoByKKS("20MAV30", true);
            Dev_Monitor sd40 = client.GetDevMonitorInfoByKKS("20MAX01", true);
            Dev_Monitor sd41 = client.GetDevMonitorInfoByKKS("20LCE15", true);
            Dev_Monitor sd42 = client.GetDevMonitorInfoByKKS("20LCA40", true);
            Dev_Monitor sd43 = client.GetDevMonitorInfoByKKS("20LCA20", true);
            Dev_Monitor sd44 = client.GetDevMonitorInfoByKKS("20MBH20", true);
            Dev_Monitor sd45 = client.GetDevMonitorInfoByKKS("20QEA20", true);
            Dev_Monitor sd46 = client.GetDevMonitorInfoByKKS("20MBA41", true);
            Dev_Monitor sd47 = client.GetDevMonitorInfoByKKS("20PGA26", true);
            Dev_Monitor sd48 = client.GetDevMonitorInfoByKKS("21MKG08", true);
            Dev_Monitor sd49 = client.GetDevMonitorInfoByKKS("20MBV10", true);
            Dev_Monitor sd50 = client.GetDevMonitorInfoByKKS("20MAV41", true);
            Dev_Monitor sd51 = client.GetDevMonitorInfoByKKS("10MAA50", true);
            Dev_Monitor sd52 = client.GetDevMonitorInfoByKKS("10MAC01", true);
            lst.Add(sd);
            lst.Add(sd2);
            lst.Add(sd3);
            lst.Add(sd4);
            lst.Add(sd5);
            lst.Add(sd6);
            lst.Add(sd7);
            lst.Add(sd8);
            lst.Add(sd9);
            lst.Add(sd10);
            lst.Add(sd11);
            lst.Add(sd12);
            lst.Add(sd13);
            lst.Add(sd14);
            lst.Add(sd15);
            lst.Add(sd16);
            lst.Add(sd17);
            lst.Add(sd18);
            lst.Add(sd19);
            lst.Add(sd20);
            lst.Add(sd21);
            lst.Add(sd22);
            lst.Add(sd23);
            lst.Add(sd24);
            lst.Add(sd25);
            lst.Add(sd26);
            lst.Add(sd27);
            lst.Add(sd28);
            lst.Add(sd29);
            lst.Add(sd30);
            lst.Add(sd31);
            lst.Add(sd32);
            lst.Add(sd33);
            lst.Add(sd34);
            lst.Add(sd35);
            lst.Add(sd36);
            lst.Add(sd37);
            lst.Add(sd38);
            lst.Add(sd39);
            lst.Add(sd40);
            lst.Add(sd41);
            lst.Add(sd42);
            lst.Add(sd43);
            lst.Add(sd44);
            lst.Add(sd45);
            lst.Add(sd46);
            lst.Add(sd47);
            lst.Add(sd48);
            lst.Add(sd49);
            lst.Add(sd50);
            lst.Add(sd51);
            lst.Add(sd52);

            bool bReturn = false;
            string strKKS1 = "";
            string strKKS2 = "";
            string strKKS3 = "";
            foreach (Dev_Monitor item in lst)
            {
                bReturn = GetHasOrNo(item);
                if (bReturn)
                {
                    hasdict[item.KKSCode] = item;
                    strKKS1 += item.KKSCode + "\n";
                }
                else
                {
                    nolst.Add(item);
                }
            }

            foreach (Dev_Monitor item2 in nolst)
            {
                bReturn = HasMonitorNode(item2);
                if (bReturn)
                {
                    nodictHasMon[item2.KKSCode] = item2;
                    strKKS2 += item2.KKSCode + "\n";
                }
                else
                {
                    nodictNoMon[item2.KKSCode] = item2;
                    strKKS3 += item2.KKSCode + "\n";
                }
            }

            int nnn = 0;
        }

        private bool GetHasOrNo(Dev_Monitor dvm)
        {
            bool bReturn = false;

            if (dvm.MonitorNodeList == null || dvm.MonitorNodeList.Count() == 0)
            {
                bReturn = false;
            }
            else
            {
                foreach (DevMonitorNode item2 in dvm.MonitorNodeList)
                {
                    if (item2.Value != "" && item2.Value != null && item2.Value != "null")
                    {
                        bReturn = true;
                        break;
                    }
                }
            }

            if (bReturn)
            {
                return bReturn;
            }
            else
            {
                if (dvm.ChildrenList == null || dvm.ChildrenList.Count() == 0)
                {
                    bReturn = false;
                    return bReturn;
                }
                else
                {
                    foreach (Dev_Monitor item in dvm.ChildrenList)
                    {
                        bReturn = GetHasOrNo(item);
                        if (bReturn)
                        {
                            return bReturn;
                        }
                    }
                }
            }

            return bReturn;
        }

        private bool HasMonitorNode(Dev_Monitor dvm)
        {
            bool bReturn = false;
            if (dvm.MonitorNodeList == null || dvm.MonitorNodeList.Count() == 0)
            {
                bReturn = false;
            }
            else
            {
                bReturn = true;
            }

            if (bReturn)
            {
                return bReturn;
            }
            else
            {
                if (dvm.ChildrenList != null && dvm.ChildrenList.Count() > 0)
                {
                    foreach (Dev_Monitor item in dvm.ChildrenList)
                    {
                        bReturn = HasMonitorNode(item);
                        if (bReturn)
                        {
                            return bReturn;
                        }
                    }
                }
            }

            return bReturn;
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
            client.GetSomesisList("FW.J0GCK14AP001_F,N2DCS.20MAV42FP116A_DL12");
            int nn = 0;
        }

        //获取SIS历史数据
        private void GetSomeSisHistoryList_Click(object sender, RoutedEventArgs e)
        {
            client.GetSomeSisHistoryList("k01", true);

            client.GetSomeSisHistoryList("N2DCS.20MAV42FP116A_DL12", false);
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
        DispatcherTimer timer;
        private void TestGetTags_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1f);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var tags=client.GetTags();
        }
        #region 初始化数据库表
        private void InitKksTable_Click(object sender, RoutedEventArgs e)
        {
            client.InitKksTable();
            int nn = 0;
        }

        #endregion

        private void TestGetHomePage_Click(object sender, RoutedEventArgs e)
        {
            byte[] ds = client.GetHomePageByName("11.png");
            int nn = 0;
        }

        private void TestGetHomePageName_Click(object sender, RoutedEventArgs e)
        {
            string[] ds = client.GetHomePageNameList();
            int nn = 0;
        }

        private void EscapeKKS_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
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
using System.Windows.Threading;
using WCFClientLib;
using WCFServiceForWPF.LocationCallbackServices;
using WCFServiceForWPF.LocationServices;

namespace LocationWCFClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private LocationServiceClient locationServiceClient;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGetMaps_Click(object sender, RoutedEventArgs e)
        {
            //if (locationServiceClient == null)
            //{
            //    MessageBox.Show("先连接");
            //    return;
            //}
            //try
            //{
                

            //    //Map[] maps = locationServiceClient.GetMaps(2);
            //    //DataGrid1.ItemsSource = maps;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            
        }

        private void BtnGetAreas_Click(object sender, RoutedEventArgs e)
        {
            //if (locationServiceClient == null)
            //{
            //    MessageBox.Show("先连接");
            //    return;
            //}

            //try
            //{
            //    //DataGrid1.ItemsSource = locationServiceClient.GetAreas(1);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}

        }

        private void BtnHello_Click(object sender, RoutedEventArgs e)
        {
            if (Client1 == null)
            {
                MessageBox.Show("先连接");
                return;
            }
            try
            {
                TbResult.Text = Client1.InnerClient.Hello("abc");
                //User user = locationServiceClient.GetUser();
                //TbResult.Text += user.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnConnect_OnClick(object sender, RoutedEventArgs e)
        {
            GetLocationServiceClient();
        }

        private LocationClient Client1;

        private void GetLocationServiceClient()
        {
            Client1=new LocationClient(TbHost.Text, "8733");
        }

        public int UserId = 1;

        private LocationCallbackClient Client2;
        //private LocationAlarmServiceClient alarmServiceClient;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetLocationServiceClient();

            GetLocationCallbacksClient();
        }

        private void GetLocationCallbacksClient()
        {
            Client2=new LocationCallbackClient(TbHost.Text,"8734");
            bool r=Client2.Connect();
            //alarmServiceClient = Client2.Client;
        }

        private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGetDeps_Click(object sender, RoutedEventArgs e)
        {
            if (Client1 == null)
            {
                MessageBox.Show("先连接");
                return;
            }

            try
            {
                var deps= Client1.InnerClient.GetDepartmentList();


                //bool r = deps[0].Children[0] == deps[1];
                DataGrid1.ItemsSource = deps;
                TreeView1.ItemsSource = deps;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnGetDepRoot_Click(object sender, RoutedEventArgs e)
        {
            if (Client1 == null)
            {
                MessageBox.Show("先连接");
                return;
            }

            try
            {
                var dep = Client1.InnerClient.GetDepartmentTree();
                TreeView1.ItemsSource = dep.Children;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FindAreas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //IList<Area> lare = locationServiceClient.FindAreas("区域1");
                //int n = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetArea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Area are = locationServiceClient.GetArea(2);
                //int n = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AddArea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Area are = new Area();
                //are.Name = "区域3";
                //are.MapId = 2;
                ////are.MinX = -100;
                ////are.MaxX = 3000;
                ////are.MinY = -100;
                ////are.MaxY = 1500;
                ////are.MinZ = 700;
                ////are.MaxZ = 800;
                //bool br = locationServiceClient.AddArea(are);
                //int n = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void EditArea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Area are = new Area();
                //are.Id = 4;
                //are.Name = "区域5";
                //are.MapId = 2;
                ////are.MinX = -200;
                ////are.MaxX = 3000;
                ////are.MinY = -200;
                ////are.MaxY = 1500;
                ////are.MinZ = 700;
                ////are.MaxZ = 800;
                //bool br = locationServiceClient.EditArea(are);
                //int n = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DeleteArea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //bool br = locationServiceClient.DeleteArea(3);
                //int n = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnGetPositions_OnClick(object sender, RoutedEventArgs e)
        {
            StartInsertPositionTimer();
        }


        private DispatcherTimer timerGetRealPos;

        public void StartInsertPositionTimer()
        {
            if (timerGetRealPos == null)
            {
                timerGetRealPos = new DispatcherTimer();
                timerGetRealPos.Interval = TimeSpan.FromMilliseconds(400);
                timerGetRealPos.Tick += TimerGetRealPos_Tick;
            }
            timerGetRealPos.Start();
        }

        private void TimerGetRealPos_Tick(object sender, EventArgs e)
        {
            //DataGridRealPosition.ItemsSource=client.GetRealPositons();
            DataGridRealPosition.ItemsSource = Client1.InnerClient.GetRealPositonsByTags(new string[] {"0003"});
        }

        private void BtnSavePosition3D_OnClick(object sender, RoutedEventArgs e)
        {
            if (insert3DPosTimer == null)
            {
                insert3DPosTimer = new DispatcherTimer();
                insert3DPosTimer.Interval = TimeSpan.FromMilliseconds(200);
                insert3DPosTimer.Tick += Insert3DPosTimer_Tick;
            }
            insert3DPosTimer.Start();
        }

        private void Insert3DPosTimer_Tick(object sender, EventArgs e)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();

            U3DPosition pos = new U3DPosition();
            Client1.InnerClient.AddU3DPosition(new U3DPosition[] { pos});

            watch1.Stop();

            Tb3DPosResult.Text = GetLogText(string.Format("写入数据 用时:{0}", watch1.Elapsed))+"\n"+ Tb3DPosResult.Text;

        }

        private string GetLogText(string msg)
        {
            return DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg;
        }

        private DispatcherTimer insert3DPosTimer;

        private void BtnGetTopoTree_OnClick(object sender, RoutedEventArgs e)
        {
            if (Client1 == null)
            {
                MessageBox.Show("先连接");
                return;
            }

            try
            {
                PhysicalTopology root = Client1.InnerClient.GetPhysicalTopologyTree();
                TreeView1.ItemsSource = root.Children;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetLocationHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Client2.Disconnect();
        }

        private void GetDevInfo_OnClick(object sender, EventArgs e)
        {
            //string[] Result = infoServiceClient.GetInfo(1);
            //string txt = "";
            //foreach (string strInfo in Result)
            //{
            //    txt += strInfo + "\n";
            //}
            ////Tb3DPosResult.Text = GetLogText(txt);
            //MessageBox.Show("收到设备信息:" + txt);
        }

        public string[] GetInfoFromRsetful(int i)
        {
            string[] DevInfos = new string[] { };
            DevInfos[0] = "abcdefgh";
            return DevInfos;
        }

        private void BtnGetTicketList_OnClick(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = Client1.InnerClient.GetTicketList(1, DateTime.Now, DateTime.Now);
        }

        private void BtnGetTicketDetail_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void TestWork_Click(object sender, RoutedEventArgs e)
        {
            LocationServiceClient client=Client1.InnerClient;

            OperationTicket[] lst1 = client.GetOperationTicketList();
            WorkTicket[] lst2 = client.GetWorkTicketList();
            MobileInspectionDev[] lst3 = client.GetMobileInspectionDevList();
            MobileInspection[] lst4 = client.GetMobileInspectionList();
            PersonnelMobileInspection[] lst5 = client.GetPersonnelMobileInspectionList();
            OperationItemHistory[] lst6 = client.GetOperationItemHistoryList();
            WorkTicketHistory[] lst7 = client.GetWorkTicketHistoryList();
            PersonnelMobileInspectionHistory[] lst8 = client.GetPersonnelMobileInspectionHistoryList();
        }
    }
}

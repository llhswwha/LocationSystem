using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using BLL;
using DbModel.LocationHistory.Data;
using Location.BLL;
using LocationServices.Tools;
using LocationWCFServer;
using LocationWCFService;

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for LocationTestBox.xaml
    /// </summary>
    public partial class LocationTestBox : UserControl
    {
        private PositionEngineClient engineClient;

        public PositionEngineLog Logs { get; set; }

        public LocationTestBox()
        {
            InitializeComponent();
        }

        private void StartTestInsertPositions()
        {
            int mockCount = int.Parse(TbMockTagPowerCount0.Text);

            using (var positionBll = AppContext.GetLocationBll())
            {
                positionBll.InitTagPosition(mockCount);
            }

            //StartGetPosInfoTimer();
            StartInserttTestDataTimer();
            StartInsertPositionTimer();
        }

        public void StartConnectEngine()
        {
            Location.BLL.Tool.Log.Info("开始连接定位引擎");
            int mockCount = int.Parse(TbMockTagPowerCount0.Text);
            if (engineClient == null)
            {
                EngineLogin login=new EngineLogin("127.0.0.1",2323, "192.168.10.155",3456);
                engineClient = PositionEngineClient.Instance();
                engineClient.Logs = Logs;
                engineClient.MockCount = mockCount;
                engineClient.StartConnectEngine(login);//todo:ip写到配置文件中
            }
            StartInsertPositionTimer();
        }

        private string GetLogText(string msg)
        {
            return DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg;
        }

        StopwatchTextBox _stopwatchTextBox;

        public void StartInsertPositionTimer()
        {
            if (_stopwatchTextBox == null)
            {
                _stopwatchTextBox = new StopwatchTextBox(TbTimer);
                _stopwatchTextBox.Start();
            }
        }

        async void TestInsertData2Async()
        {
            //200
            using (var positionBll = AppContext.GetLocationBll())
            {
                Stopwatch watch1 = new Stopwatch();
                watch1.Start();

                List<Position> positions = GenerateMockPositions(positionBll);

                watch1.Stop();
                Logs.WriteLogLeft("生成模拟数据 用时:" + watch1.Elapsed);

                Stopwatch watch = new Stopwatch();
                watch.Start();

                await positionBll.AddPositionsAsyc(positions);

                watch.Stop();
                Logs.WriteLogLeft(string.Format("插入{0}数据 用时:{1}", positions.Count, watch.Elapsed));
            }
        }

        private List<Position> GenerateMockPositions(Bll positionBll)
        {
            int tagPowerCount = int.Parse(TbMockTagPowerCount.Text);//标签倍数 2*100=200
            var tags = positionBll.InitTagPosition(tagPowerCount);
            int mockCount = int.Parse(TbMockPosPowerCount.Text);//位置信息倍数 200*100=20000
            List<Position> positions = PositionMocker.GetMockPosition(tags, mockCount);
            return positions;
        }

        private DispatcherTimer timerInserttTestData;

        /// <summary>
        /// 产生位置信息
        /// </summary>
        private void StartInserttTestDataTimer()
        {
            if (timerInserttTestData == null)
            {
                timerInserttTestData = new DispatcherTimer();
                timerInserttTestData.Interval = TimeSpan.FromMilliseconds(250);
                timerInserttTestData.Tick += TimerInserttTestData_Tick;
            }
            timerInserttTestData.Start();
        }

        bool istestInsertBusy = false;

        private void TimerInserttTestData_Tick(object sender, EventArgs e)
        {
            if (istestInsertBusy == false)
            {
                istestInsertBusy = true;

                TestInsertData2Async();

                //using (LocationBll positionBll = new LocationBll(false,false))
                //{
                //    List<Position> positions = GenerateMockPositions(positionBll);
                //    Positions.AddRange(positions);
                //}

                istestInsertBusy = false;
            }
            else
            {
                Logs.WriteLogLeft(GetLogText(string.Format("等待")));
            }
        }

        private void TestInsertData()
        {
            using (var positionBll = AppContext.GetLocationBll())
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                positionBll.InitTagPosition(0);

                int mockCount = int.Parse(TbMockCount1.Text);

                List<Position> positions = PositionMocker.GetMockPosition("00002", mockCount);
                positionBll.AddPositions(positions);

                watch.Stop();
                Logs.WriteLogLeft("用时:" + watch.Elapsed);
            }
        }

        private void BtnStopConnectEngine_OnClick(object sender, RoutedEventArgs e)
        {
            StopConnectEngine();
        }

        private void BtnNewDb_OnClick(object sender, RoutedEventArgs e)
        {
            StopConnectEngine();

            Thread.Sleep(1000);

            //if (positionBll != null)
            //{
            //    positionBll.Dispose();
            //}
            //positionBll = new LocationBll();
            Logs.WriteLogLeft("重新生成");
        }

        public void StopConnectEngine()
        {
            if (engineClient != null)
            {
                engineClient.Stop();
                engineClient = null;
            }
            if (_stopwatchTextBox != null)
            {
                _stopwatchTextBox.Stop();
                _stopwatchTextBox = null;
            }
        }

        private void BtnTestInsertData_OnClick(object sender, RoutedEventArgs e)
        {
            TestInsertData();
        }

        private void BtnTestInsertData2_OnClick(object sender, RoutedEventArgs e)
        {
            //TestInsertData2();
            TestInsertData2Async(); //异步方式
        }

        private void BtnTestInsertData3_OnClick(object sender, RoutedEventArgs e)
        {
            StartTestInsertPositions();
        }

        private void BtnGeneratePosition_OnClick(object sender, RoutedEventArgs e)
        {
            //GeneratePosition();
        }
    }
}

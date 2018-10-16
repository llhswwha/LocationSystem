using Location.BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using LocationWCFService;
using System.Linq;
using BLL;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using Location.TModel.Location.Alarm;
using LocationServices.Locations;
using LocationServices.Tools;
using LocationServer;
using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;
using WebNSQLib;
using LocationServices.Converters;
using System.Threading;
using Location.TModel.Location.AreaAndDev;
using TModel.Location.Data;

//using Web.Sockets.Core;

namespace LocationWCFServer
{
    public partial class MainWindow
    {
        private PositionEngineClient engineClient;

        private void StartConnectEngine()
        {
           Log.Info("开始连接定位引擎");
            int mockCount = int.Parse(TbMockTagPowerCount0.Text);
            if (engineClient == null)
            {
                engineClient = new PositionEngineClient();
                engineClient.Logs = Logs;
                engineClient.StartConnectEngine(mockCount, "127.0.0.1", "127.0.0.1");//todo:ip写到配置文件中
            }

            StartInsertPositionTimer();

        }

        private Bll GetLocationBll()
        {
            return new Bll(false,true,false);
        }

        private void StartTestInsertPositions()
        {
            int mockCount = int.Parse(TbMockTagPowerCount0.Text);

            using (var positionBll = GetLocationBll())
            {
                positionBll.InitTagPosition(mockCount);
            }

            //StartGetPosInfoTimer();
            StartInserttTestDataTimer();
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
            using (var positionBll = GetLocationBll())
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

        private void StopConnectEngine()
        {
            if (engineClient != null)
            {
                engineClient.Stop();
            }
            if (_stopwatchTextBox != null)
            {
                _stopwatchTextBox.Stop();
            }
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
            using (var positionBll = GetLocationBll())
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

        
        /// <summary>
        /// 门禁设备，绑定设备信息（For: DevInfo.Path）
        /// </summary>
        /// <param name="devList"></param>
        /// <param name="doorAccessList"></param>
        private void BindingDevInfo(List<DevInfo>devList,List<Dev_DoorAccess>doorAccessList)
        {
            foreach(var door in doorAccessList)
            {
                DevInfo info = devList.Find(i=>i.DevID==door.DevID);
                if (info != null) door.DevInfo = info;
            }
        }
        private void Mh_DevAlarmReceived(DbModel.Location.Alarm.DevAlarm obj)
        {
            AlarmHub.SendDeviceAlarms(obj.ToTModel());
        }

        private void BtnSendMessage_OnClick(object sender, RoutedEventArgs e)
        {
            string msg = TbMessage.Text;
            ChatHub.SendToAll(msg);
        }

        private void BtnCreateHistoryPos_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = GetLocationBll();

            Position pos = PositionMocker.GetRandomPosition("223");
            pos.PersonnelID = 112;
            bll.Positions.Add(pos);

            DataGridHistoryPosList.ItemsSource = bll.Positions.ToList();
        }

        private void BtnGetHistoryList_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = GetLocationBll();
            DataGridHistoryPosList.ItemsSource = bll.Positions.ToList();
        }
    }
}

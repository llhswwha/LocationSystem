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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LocationService service = new LocationService();
            var devlist = service.GetAllDevInfos();
            DeviceListBox1.LoadData(devlist.ToArray());

            DeviceListBox1.AddMenu("告警", (se, arg) =>
            {
                //MessageBox.Show("告警" + DeviceListBox1.CurrentDev.Name);
                //todo:告警事件推送
                var dev=DeviceListBox1.CurrentDev;
                DeviceAlarm alarm = new DeviceAlarm()
                {
                    Id = dev.Id,
                    Level = Abutment_DevAlarmLevel.低,
                    Title = "告警" + dev.Id,
                    Message = "设备告警1",
                    CreateTime = new DateTime(2018, 8, 28, 9, 5, 34)
                }.SetDev(dev);
                AlarmHub.SendDeviceAlarms(alarm);
            });
            DeviceListBox1.AddMenu("消警", (se, arg) =>
            {
                //MessageBox.Show("消警" + DeviceListBox1.CurrentDev.Name);
                var dev = DeviceListBox1.CurrentDev;
                DeviceAlarm alarm = new DeviceAlarm()
                {
                    Id = dev.Id,
                    Level = Abutment_DevAlarmLevel.低,
                    Title = "消警" + dev.Id,
                    Message = "设备消警1",
                    CreateTime = new DateTime(2018, 8, 28, 9, 5, 34)
                }.SetDev(dev);
                AlarmHub.SendDeviceAlarms(alarm);
            });

            RealAlarm ra = new RealAlarm();
            Thread th = new Thread(ra.ReceiveRealAlarmInfo);
            th.Start();
            ra.MessageHandler.DevAlarmReceived += Mh_DevAlarmReceived;

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
    }
}

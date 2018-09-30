using Location.BLL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Coldairarrow.Util.Sockets;
using log4net.Config;
using LocationWCFService;
using LocationServices.LocationCallbacks;
using System.Linq;
using BLL;
using DbModel.LocationHistory.Data;
//using Web.Sockets.Core;

namespace LocationWCFServer
{
    public partial class MainWindow
    {
        private PositionEngineDA engineDa;

        private void StartConnectEngine()
        {
           Log.Info("开始连接定位引擎");
            int mockCount = int.Parse(TbMockTagPowerCount0.Text);
            if (engineDa == null)
            {
                //engineDa = new PositionEngineDA("192.168.10.155", "192.168.10.19");//todo:ip写到配置文件中
                engineDa = new PositionEngineDA("127.0.0.1", "127.0.0.1");//todo:ip写到配置文件中
                engineDa.MockCount = mockCount;
                engineDa.MessageReceived += EngineDa_MessageReceived;
                engineDa.PositionRecived += EngineDa_PositionRecived;
                engineDa.PositionListRecived += EngineDa_PositionListRecived;
            }

            using (var positionBll = GetLocationBll())
            {
                positionBll.InitTagPosition(mockCount);
            }

            engineDa.Start();

            StartInsertPositionTimer();

        }

        private Bll GetLocationBll()
        {
            return new Bll(false,true,false);
        }

        private void EngineDa_PositionListRecived(List<Position> posList)
        {
            //Positions.AddRange(posList);
            //InsertPostions(posList);//在这里插入不会越来越慢，但是还是想分开。
            //InsertPostionsAsync(posList);

            foreach (var item in posList)
            {
                if (item != null)
                {
                    Positions.Add(item);
                }
                else
                {
                    Log.Warn("EngineDa_PositionListRecived item==null");
                }
            }
        }

        private void EngineDa_PositionRecived(Position obj)
        {
            
        }

        private void EngineDa_MessageReceived(string obj)
        {
            WriteLogLeft(GetLogText(obj));
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

        private Thread insertThread;

        private string GetLogText(string msg)
        {
            return DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg;
        }


        public List<Position> Positions = new List<Position>();

        private DispatcherTimer timerInsertPosition;

        Stopwatch insertWatch = new Stopwatch();

        public void StartInsertPositionTimer()
        {
            if (timerInsertPosition == null)
            {
                timerInsertPosition = new DispatcherTimer();
                timerInsertPosition.Interval = TimeSpan.FromMilliseconds(250);
                timerInsertPosition.Tick += (sender, e) =>
                {
                    //InsertPostions(); //同步
                    //InsertPostionsAsync();//异步，出异常时改成同步来调试
                    //这个异步会越来越慢 为什么呢？
                    TbTimer.Text = insertWatch.Elapsed.ToString();
                };
            }
            insertWatch.Start();

            timerInsertPosition.Start();


            insertThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    InsertPostions();
                }
            });
            insertThread.Start();
        }

        public bool isBusy = false;//没有这个标志位的话，很容易导致子线程间干扰

        private void InsertPostions()
        {
            lock (Positions)
            {
                if (!isBusy && Positions.Count > 0)
                {
                    isBusy = true;
                    WriteLogRight(GetLogText(string.Format("写入{0}条数据 Start", Positions.Count)));
                    try
                    {
                        List<Position> posList2 = new List<Position>();
                        posList2.AddRange(Positions);
                        if (InsertPostions(posList2))
                        {
                            Positions.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("InsertPostions", ex);
                    }

                    isBusy = false;
                }
                else
                {
                    if (Positions.Count > 0)
                        WriteLogRight(GetLogText(string.Format("等待 当前{0}条数据", Positions.Count)));
                }
            }
        }

        private bool InsertPostions(List<Position> posList2)
        {
            bool r = false;
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();

            using (var positionBll = GetLocationBll())
            {
                var personnels = positionBll.Personnels.ToList();
                var tagToPersons=positionBll.LocationCardToPersonnels.ToList();
                var tags = positionBll.LocationCards.ToList();
                //List<Tag> tagList = positionBll.Db.Tags.ToList();

                //foreach (Personnel item in personnels)
                //{
                //    item.Tag = tagList.Find(i => i.Id == item.TagId);
                //}

                foreach (Position pos in posList2)
                {
                    if (pos == null) continue;
                    try
                    {
                        var tag = tags.Find(i => i.Code == pos.Code);
                        var ttp = tagToPersons.Find(i => i.LocationCardId == tag.Id);
                        var personnelT = personnels.Find(i => i.Id == ttp.PersonnelId);
                        if (personnelT != null)
                        {
                            pos.PersonnelID = personnelT.Id;
                        }
                    }
                    catch
                    {
                        int i = 0;
                    }
                }

                r=positionBll.AddPositions(posList2);
                if (r)
                {
                    foreach (Position p in posList2)
                    {
                        if (p == null) continue;
                        if (p.X >= 10 && p.X <= 30 && p.Y >= 50 && p.Y <= 70 && p.Z >= 80 && p.Z <= 100)
                        {
                            LocationCallbackService.NotifyServiceStop();
                        }
                    }

                }
            }

            watch1.Stop();
            WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", posList2.Count, watch1.Elapsed)));
            return r;
        }

        private async void InsertPostionsAsync()
        {
            if (!isBusy && Positions.Count > 0)
            {
                isBusy = true;

                WriteLogRight(GetLogText(string.Format("写入{0}条数据 Start", Positions.Count)));
                List<Position> posList2 = new List<Position>();
                posList2.AddRange(Positions);
                Positions.Clear();

                InsertPostionsAsync(posList2);

                isBusy = false;
            }
            else
            {
                if (Positions.Count > 0)
                    WriteLogRight(GetLogText(string.Format("等待 当前{0}条数据", Positions.Count)));
            }
        }

        private async void InsertPostionsAsync(List<Position> posList)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();

            using (var positionBll = GetLocationBll())
            {
                await positionBll.AddPositionsAsyc(posList);
            }

            watch1.Stop();
            WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", posList.Count, watch1.Elapsed)));
        }

        //LocationBll positionBll = new LocationBll(false,false);

        private void GeneratePosition()
        {
            if (BtnGeneratePosition.Content == "开始产生位置信息")
            {
                BtnGeneratePosition.Content = "停止产生位置信息";
                StartGeneratePositionTimer();
            }
            else
            {
                BtnGeneratePosition.Content = "开始产生位置信息";
                if (timerGeneratePosition != null)
                    timerGeneratePosition.Stop();
            }
        }

        private DispatcherTimer timerGeneratePosition;

        /// <summary>
        /// 产生位置信息
        /// </summary>
        private void StartGeneratePositionTimer()
        {
            if (timerGeneratePosition == null)
            {
                timerGeneratePosition = new DispatcherTimer();
                timerGeneratePosition.Interval = TimeSpan.FromMilliseconds(250);
                timerGeneratePosition.Tick += TimerGeneratePosition_Tick;
            }
            timerGeneratePosition.Start();
        }



        private void TimerGeneratePosition_Tick(object sender, EventArgs e)
        {
            //Position pos = PositionMocker.GetRandomPosition("");
        }


        //void TestInsertData2()
        //{
        //    using (var positionBll = GetLocationBll())
        //    {
        //        //200

        //        Stopwatch watch1 = new Stopwatch();
        //        watch1.Start();

        //        int tagPowerCount = int.Parse(TbMockTagPowerCount.Text); //标签倍数 2*100=200
        //        var tags = positionBll.InitTagPosition(tagPowerCount);
        //        int mockCount = int.Parse(TbMockPosPowerCount.Text); //位置信息倍数 200*100=20000
        //        List<Position> positions = PositionMocker.GetMockPosition(tags, mockCount);

        //        watch1.Stop();
        //        WriteLogLeft( "生成模拟数据 用时:" + watch1.Elapsed);

        //        Stopwatch watch = new Stopwatch();
        //        watch.Start();

        //        positionBll.AddPositions(positions);

        //        watch.Stop();
        //        WriteLogLeft("插入数据 用时:" + watch.Elapsed);
        //    }
        //}

        async void TestInsertData2Async()
        {
            //200
            using (var positionBll = GetLocationBll())
            {
                Stopwatch watch1 = new Stopwatch();
                watch1.Start();

                List<Position> positions = GenerateMockPositions(positionBll);

                watch1.Stop();
                WriteLogLeft("生成模拟数据 用时:" + watch1.Elapsed);

                Stopwatch watch = new Stopwatch();
                watch.Start();

                await positionBll.AddPositionsAsyc(positions);

                watch.Stop();
                WriteLogLeft(string.Format("插入{0}数据 用时:{1}", positions.Count, watch.Elapsed));
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
            if (engineDa != null)
            {
                engineDa.Stop();
            }
            if (timerInsertPosition != null)
            {
                timerInsertPosition.Stop();
                timerInsertPosition = null;
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
                WriteLogLeft(GetLogText(string.Format("等待")));
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
                WriteLogLeft("用时:" + watch.Elapsed);
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //

        }
    }
}

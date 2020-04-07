using BLL;
using DbModel.LocationHistory.Data;
using Location.BLL.Tool;
using LocationServices.LocationCallbacks;
using LocationWCFServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using DbModel.Location.Authorizations;
using BLL.Buffers;
using DbModel.Location.Alarm;
using LocationServer;
using BLL.Tools;
using WebNSQLib;
using LocationServices.Locations;
using System.Collections.Concurrent;

namespace LocationServices.Tools
{
    public class FaintAlarmLogin
    {
        public string FaintScope { get; set; }
        public int FaintTime { get; set; }
        public int FaintIntervalTime { get; set; }

        public FaintAlarmLogin()
        {



            FaintScope = "0.5";
            FaintTime = 20;
            FaintIntervalTime = 1;
        }

    }


    public class PositionEngineClient
    {


        public static PositionEngineClient Single = null;

        public static PositionEngineClient Instance()
        {
            if (Single == null)
            {
                Single = new PositionEngineClient();
            }

            return Single;
        }

        private PositionEngineClient()
        {
            StaticEvents.DbDataChanged += StaticEvents_DbDataChanged;
        }

        private void StaticEvents_DbDataChanged(DataChangArg arg)
        {
            //if (ab != null)
            //{
            //    ab.ForceLoadData();
            //}

            //if (bll != null)
            //{
            //    bll.UpdateBuffer();
            //}
        }

        public PositionEngineLog Logs { get; set; }

        public void WriteLogLeft(string txt)
        {
            if (Logs == null)
            {
                Logs = new PositionEngineLog();
            }
            Logs.WriteLogLeft(txt);
            if (AppContext.WritePositionLog)
            {
                Log.Info(LogTags.Engine, txt);
            }
        }

        public void WriteLogRight(string txt, bool isError = false)
        {
            if (Logs == null)
            {
                Logs = new PositionEngineLog();
            }
            Logs.WriteLogRight(txt, isError);
        }

        public PositionEngineDA engineDa;

        //public System.Collections.Concurrent.list

        public ConcurrentBag<Position> Positions = new ConcurrentBag<Position>();

        public int MockCount = 0;

        public bool IsWriteToDb = true;

        public int psCount = 0;

        public void StartConnectEngine(EngineLogin login)
        {
            //Log.Info(LogTags.Engine,"StartConnectEngine:" + login.EngineIp);
            WriteLogLeft(GetLogText("StartConnectEngine:" + login.EngineIp));
            //int mockCount = int.Parse(TbMockTagPowerCount0.Text);
            if (engineDa == null)
            {
                //engineDa = new PositionEngineDA("192.168.10.155", "192.168.10.19");//todo:ip写到配置文件中
                engineDa = new PositionEngineDA(login);//todo:ip写到配置文件中
                engineDa.MockCount = MockCount;
                //engineDa.MessageReceived += EngineDa_MessageReceived;
                engineDa.MessageReceived += (msg) =>
                {
                    psCount++;
                    string m = string.Format("{0}||{1}", psCount, msg);
                    WriteLogLeft(GetLogText(m));
                };
                //engineDa.PositionRecived += EngineDa_PositionRecived;
                engineDa.PositionListRecived += EngineDa_PositionListRecived;
            }
            engineDa.Start();

            if (IsWriteToDb)
            {
                InitTagPosition(MockCount);
                StartInsertPositionTimer();
            }
        }

        private void InitTagPosition(int mockCount)
        {
            ThreadTool.Start(() =>
            {
                try
                {
                    using (var positionBll = GetLocationBll())
                    {
                        positionBll.InitTagPosition(mockCount);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("PositionEngineClient.InitTagPosition", ex);
                }

            });
        }

        private Thread insertThread;

        public bool isBusy = false;//没有这个标志位的话，很容易导致子线程间干扰

        public void StartInsertPositionTimer()
        {
            insertThread = ThreadTool.Start(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);//300ms插入一次数据库
                    //todo:1.插入数据库时间写入配置文件,2.实时数据弄一个缓存，让客户端从缓存中取。
                    InsertPostions();
                }
            });
        }

        //private int count = 0;


        private string GetLogText(string msg)
        {
            //count++;
            return string.Format("[{0}]{1}",DateTime.Now.ToString("HH:mm:ss.fff") , msg);
        }

        /// <summary>
        /// 删除重复的数据
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        private ConcurrentBag<Position> RemoveRepeatPosition(ConcurrentBag<Position> list1)
        {
            Dictionary<string, Position> dict = new Dictionary<string, Position>();
            foreach (Position pos in list1)
            {
                if (pos == null) continue;
                try
                {
                    dict[pos.Code] = pos;
                }
                catch (Exception ex)
                {
                    Log.Error(LogTags.Engine, "RemoveRepeatPosition:" + ex);
                }
            }
            //return dict.Values.ToList();
            ConcurrentBag<Position> posBagT = new ConcurrentBag<Position>(dict.Values);
            return posBagT;
        }

        public void TestInsertPostions()
        {
            insertThread = ThreadTool.Start(() =>
            {
                List<Position> posList2 = new List<Position>();
                posList2.AddRange(Positions);
                Positions = new ConcurrentBag<Position>();

                foreach (Position position in posList2)
                {
                    List<Position> posList3 = new List<Position>();
                    posList3.Add(position);//模拟状态一个一个添加

                    if (InsertPostions(posList3))
                    {
                        WriteLogRight(GetLogText(string.Format("写入{0}条数据", posList3.Count)));
                    }
                    else
                    {
                        CloseBll();
                        Thread.Sleep(300);
                        WriteLogRight(GetLogText(string.Format("写入失败 当前有{0}条数据 error:{1}", posList2.Count, ErrorMessage)), true);
                    }
                }

            });
        }

        public void ClearPositions()
        {
            Positions = new ConcurrentBag<Position>();
        }

        public bool EnableInsertPosition = true;

        public bool IsSimulate = false;

        public int insertCount = 0;

        public void InsertPostions()
        {
            lock (Positions)
            {
                if (!isBusy && Positions.Count > 0)
                {
                    isBusy = true;

                    try
                    {
                        if (EnableInsertPosition == false) //模拟状态不删除数据
                        {
                            return;
                        }
                        if (IsSimulate)//模拟数据
                        {
                            List<Position> posList2 = new List<Position>();
                            posList2.AddRange(Positions);
                            Positions = new ConcurrentBag<Position>();
                            posList2.Reverse();//发现Positions(ConcurrentBag<>)里面数据位置是倒过来的，是和Add顺序相反的


                            foreach (Position pos in posList2)
                            {
                                List<Position> posList3 = new List<Position>();
                                posList3.Add(pos);

                                insertCount++;

                                if (InsertPostions(posList3))
                                {
                                    WriteLogRight(GetLogText(string.Format("写入{0}条数据,{1}", posList3.Count, insertCount)));
                                    //Positions.Clear();

                                }
                                else
                                {
                                    CloseBll();
                                    Thread.Sleep(300);
                                    WriteLogRight(
                                        GetLogText(string.Format("写入失败 当前有{0}条数据 error:{1}", posList3.Count, ErrorMessage)),
                                        true);
                                }
                            }
                        }
                        else
                        {
                            Positions = RemoveRepeatPosition(Positions); //删除重复的数据 这个原来在InsertPostions也有

                            List<Position> posList2 = new List<Position>();
                            posList2.AddRange(Positions);
                            if (InsertPostions(posList2))
                            {
                                WriteLogRight(GetLogText(string.Format("写入{0}条数据", posList2.Count)));
                                //Positions.Clear();
                                Positions = new ConcurrentBag<Position>();
                            }
                            else
                            {
                                CloseBll();
                                Thread.Sleep(300);
                                WriteLogRight(
                                    GetLogText(string.Format("写入失败 当前有{0}条数据 error:{1}", posList2.Count, ErrorMessage)),
                                    true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("InsertPostions", ex);
                        //Positions.Clear();
                        Thread.Sleep(100);
                        try
                        {
                            Positions = new ConcurrentBag<Position>();
                        }
                        catch (Exception e)
                        {
                            Log.Error("PositionEngineClient.InsertPostions.Exception", e);
                        }
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

        //Bll bll;

        AuthorizationBuffer ab;

        public void CloseBll()
        {

            try
            {
                //if (bll != null)
                //{
                //    bll.Dispose();
                //    bll = null;
                //}
            }
            catch (Exception ex)
            {
                Log.Error("CloseBll", ex);
            }
        }

        public Thread LocationAlarmThread;

        private Thread FaintAlarmThread;

        ConcurrentBag<Position> alarmPosLit = null;

        private FaintAlarmLogin faintAL = null;

        private void LocationAlarmFunction()
        {
            List<LocationAlarm> UdpAlarm = new List<LocationAlarm>();
            List<LocationAlarm> UdpAlarm2 = new List<LocationAlarm>();

            DateTime dt = DateTime.Now;
            long dl = Location.TModel.Tools.TimeConvert.ToStamp(DateTime.Now);


            while (true)
            {
                if (alarmPosLit != null)
                {
                    try
                    {
                        Log.Info("LocationAlarm", "判断定位告警:" + alarmPosLit.Count);
                        NewAlarms = ab.GetNewAlarms(alarmPosLit.ToList(), UdpAlarm);

                        //LocationAlarm lc = new LocationAlarm();
                        //lc.Id = 13;
                        //lc.AlarmType = LocationAlarmType.求救信号;
                        //lc.AlarmTime = DateTime.Now;
                        //lc.AlarmTimeStamp = dl;
                        //UdpAlarm.Add(lc);

                        if (UdpAlarm != null&& UdpAlarm.Count > 0)
                        {
                           // Log.Info("UdpAlarm", "求救信号:"+ UdpAlarm.Count);
                            SendUdpAlarm(UdpAlarm, UdpAlarm2);
                        
                            if (UdpAlarm2.Count == 0)
                            {
                                UdpAlarm2 = new List<LocationAlarm>(UdpAlarm);
                            }

                            UdpAlarm.Clear();
                        }
                        if (NewAlarmsFired != null)
                        {
                            if(NewAlarms!=null&&NewAlarms.Count!=0)
                            {
                                Log.Info("LocationAlarm", "NewAlarmsFired:" + NewAlarms.Count);
                                NewAlarmsFired(NewAlarms);
                            }                      
                        }
                        alarmPosLit = null;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("LocationAlarmThread", ex);
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }

            }
        }

        List<LocationCard> lcList;

        public void SendUdpAlarm(List<LocationAlarm> SendAlarm, List<LocationAlarm> SendAlarm2)
        {
            if (SendAlarm == null || SendAlarm.Count <= 0)
            {
                return;
            }

            //using()
            if (lcList == null)
            {
                using (var bll = GetLocationBll())
                {
                    lcList = bll.LocationCards.ToList();
                }
            }

            DateTime dtNow = DateTime.Now;
            long timeStamp = 0;
            long timeStamp2 = Location.TModel.Tools.TimeConvert.ToStamp(DateTime.Now);
            long timeStampCha = 3500;
            List<LocationAlarm> lst = new List<LocationAlarm>();
            List<LocationAlarm> lst2 = new List<LocationAlarm>();

            foreach (LocationAlarm item in SendAlarm2)
            {
                int alarmId = item.Id;

                LocationAlarm la = SendAlarm.Find(p=>p.Id == alarmId);
                if (la != null)
                {
                    timeStamp = item.AlarmTimeStamp;
                    long lc = timeStamp2 - timeStamp;
                    if (lc >= timeStampCha)
                    {
                        item.AlarmTimeStamp = timeStamp2;
                    }
                    else
                    {
                        SendAlarm.Remove(la);
                    }
                }
                else
                {
                    lst.Add(item);
                }
            }

            foreach (LocationAlarm item in lst)
            {
                SendAlarm2.Remove(item);
            }

            foreach (LocationAlarm item in SendAlarm)
            {
                int alarmId = item.Id;
                LocationAlarm la = SendAlarm2.Find(p=>p.Id == alarmId);
                if (la == null)
                {
                    SendAlarm2.Add(item);
                }

            }

            foreach (LocationAlarm item in SendAlarm)
            {
                LocationAlarmLevel level = item.AlarmLevel;

                if (level == LocationAlarmLevel.正常)
                {
                    continue;
                }

                int? id = item.LocationCardId;
                if (id == null)
                {
                    continue;
                }

                int id2 = (int)id;
                String strData = GetUdpAlarmInfo(id2, lcList);
                if (strData == "")
                {
                    continue;
                }

                Log.Info("打印Udp告警信号信号：" + Convert.ToString(item.Id) + " " + dtNow.ToString());
                engineDa.SendAlarm(strData);
            }
        }

        private string GetUdpAlarmInfo(int id, List<LocationCard> lcList)
        {
            string strData = "";

            LocationCard lc = lcList.Find(p => p.Id == id);
            if (lc == null)
            {
                //lc = bll.LocationCards.Find(p => p.Id == id);
                //if (lc != null)
                //{
                //    lcList.Add(lc);
                //}
            }

            if (lc == null)
            {
                return strData;
            }

            if (lc != null)
            {
                strData = "{\"cmdType\":\"02\",\"data\":{\"tagId\":\"" + lc.Code + "\",\"opType\":\"03\"}}";
            }
            
            return strData;
        }

        public void StartFaintAlarm(FaintAlarmLogin fal)
        {
            try
            {
                faintAL = fal;
                if (FaintAlarmThread == null)
                {
                    FaintAlarmThread = new Thread(FaintAlarmFunction);
                    FaintAlarmThread.IsBackground = true;
                    FaintAlarmThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Error("PositionEngineClient.StartFaintAlarm  ", ex);
            }
        }

        public void StopFaintAlarm()
        {
            try
            {
                if (FaintAlarmThread != null)
                {
                    FaintAlarmThread.Abort();
                    FaintAlarmThread = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("PositionEngineClient.StopFaintAlarm  ", ex);
            }
        }

        private void FaintAlarmFunction()
        {
            string nFaintScope = faintAL.FaintScope;
            int nFaintTime = faintAL.FaintTime;
            int nFaintIntervalTime = faintAL.FaintIntervalTime;
            nFaintIntervalTime = nFaintIntervalTime * 60 * 1000;

            List<LocationAlarm> newAlarms2 = new List<LocationAlarm>();
            while (true)
            {
                try
                {
                    //if (bll == null)
                    //{
                    //    bll = GetLocationBll();
                    //}

                    using (var bll = GetLocationBll())
                    {
                        if (ab == null)
                        {
                            ab = AuthorizationBuffer.Instance(bll);
                        }

                        Log.Info("FaintAlarm", "判断晕倒告警");
                        newAlarms2 = ab.GetFaintAlarm(nFaintScope, nFaintTime);
                        if (newAlarms2.Count <= 0)
                        {
                            Thread.Sleep(nFaintIntervalTime);
                            continue;
                        }

                        if (NewAlarmsFired != null)
                        {
                            Log.Info("FaintAlarm", "NewAlarmsFired:" + newAlarms2.Count);
                            NewAlarmsFired(newAlarms2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("FaintAlarmThread", ex);
                }

                Thread.Sleep(nFaintIntervalTime);
            }
        }

        private Bll bll;
        private bool InsertPostions(List<Position> list1)
        {
            try
            {
                //if (list1.Count < 20) return false;
                bool r = false;
                Stopwatch watch1 = new Stopwatch();
                watch1.Start();
                if (bll == null)
                {
                    bll = GetLocationBll();
                }

                //using (var bll = GetLocationBll())  //用完就释放，会导致定位数据缓存被清空，无法存入历史数据库
                //{
                    if (ab == null)
                    {
                        ab = AuthorizationBuffer.Instance(bll);
                    }

                    r = bll.AddPositionsEx(list1);
                    //todo:添加定位权限判断
                    if (r)
                    {
                        List<Position> temp = new List<Position>(list1);
                        //alarmPosLit = temp;
                        alarmPosLit = new ConcurrentBag<Position>();
                        foreach (var item in temp)
                        {
                            if(alarmPosLit!=null)alarmPosLit.Add(item);
                        }
                        if (LocationAlarmThread == null)
                        {
                            LocationAlarmThread = new Thread(LocationAlarmFunction);
                            LocationAlarmThread.IsBackground = true;
                            LocationAlarmThread.Start();
                        }
                        
                        //AlarmHub.SendLocationAlarms(obj.ToTModel().ToArray());
                    }

                    SendNsqPos(list1);

                    watch1.Stop();
                    WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", list1.Count, watch1.Elapsed)));

                    if (r == false)
                    {
                        ErrorMessage = bll.ErrorMessage;
                    }
                    return r;
                //}
            }
            catch (Exception ex)
            {

                Log.Error("InsertPostions", ex);
                return false;
            }
            
        }

        public string ErrorMessage;

        public event Action<List<LocationAlarm>> NewAlarmsFired;

        public List<LocationAlarm> NewAlarms = new List<LocationAlarm>();

        //private async void InsertPostionsAsync()
        //{
        //    if (!isBusy && Positions.Count > 0)
        //    {
        //        isBusy = true;

        //        WriteLogRight(GetLogText(string.Format("写入{0}条数据 Start", Positions.Count)));
        //        List<Position> posList2 = new List<Position>();
        //        posList2.AddRange(Positions);
        //        Positions.Clear();

        //        InsertPostionsAsync(posList2);

        //        isBusy = false;
        //    }
        //    else
        //    {
        //        if (Positions.Count > 0)
        //            WriteLogRight(GetLogText(string.Format("等待 当前{0}条数据", Positions.Count)));
        //    }
        //}

        SynchronizedPosition nsq;

        private async void InsertPostionsAsync(List<Position> posList)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();

            using (var positionBll = GetLocationBll())
            {
                await positionBll.AddPositionsAsyc(posList);
            }

            SendNsqPos(posList);

            watch1.Stop();
            WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", posList.Count, watch1.Elapsed)));
        }

        private void SendNsqPos(List<Position> posList)
        {
            if (nsq == null)
            {
                nsq = new SynchronizedPosition(AppContext.DatacaseWebApiUrl + ":4151");
            }

            nsq.SendPositionMsgAsync(posList);
        }

        public bool Stop()
        {
            try
            {
                StaticEvents.DbDataChanged -= StaticEvents_DbDataChanged;
                if (engineDa != null)
                {
                    engineDa.Stop();
                    engineDa = null;
                }
                if (insertThread != null)
                {
                    try
                    {
                        insertThread.Abort();
                        insertThread = null;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("PositionEngineClient.Stop1",ex);
                    }
                }

                if (LocationAlarmThread != null)
                {
                    try
                    {
                        LocationAlarmThread.Abort();
                        LocationAlarmThread = null;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("PositionEngineClient.Stop2", ex);
                    }
                }

                isBusy = false;
                //Positions.Clear();
                Positions = new ConcurrentBag<Position>();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("PositionEngineClient.Stop", ex);
                return false;
            }
        }


        private void EngineDa_PositionListRecived(List<Position> posList)
        {
            //lock (Positions)//这里有lock的话会导致线程堵住，去掉发现似乎并不需要
            {
                try
                {
                    foreach (var item in posList)
                    {

                        if (item != null)
                        {
                            Positions.Add(item);//发现Positions里面数据是倒过来的
                        }
                        else
                        {
                            Log.Warn("EngineDa_PositionListRecived item==null");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("EngineDa_PositionListRecived", ex);
                }
            }
        }

        private Bll GetLocationBll()
        {
            return new Bll(false, true, true);
        }
    }
}

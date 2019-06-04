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

namespace LocationServices.Tools
{
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
            if (bll != null)
            {
                bll.UpdateBuffer();
            }
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
                Log.Info(txt);
            }
        }

        public void WriteLogRight(string txt)
        {
            if (Logs == null)
            {
                Logs = new PositionEngineLog();
            }
            Logs.WriteLogRight(txt);
        }

        public PositionEngineDA engineDa;

        //public System.Collections.Concurrent.list

        public List<Position> Positions = new List<Position>();

        public int MockCount = 0;

        public bool IsWriteToDb = true;

        public void StartConnectEngine(EngineLogin login)
        {
            Log.Info("StartConnectEngine:"+ login.EngineIp);
            //int mockCount = int.Parse(TbMockTagPowerCount0.Text);
            if (engineDa == null)
            {
                //engineDa = new PositionEngineDA("192.168.10.155", "192.168.10.19");//todo:ip写到配置文件中
                engineDa = new PositionEngineDA(login);//todo:ip写到配置文件中
                engineDa.MockCount = MockCount;
                //engineDa.MessageReceived += EngineDa_MessageReceived;
                engineDa.MessageReceived += (obj) =>
                {
                    WriteLogLeft(GetLogText(obj));
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



        private string GetLogText(string msg)
        {
            return DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg;
        }

        /// <summary>
        /// 删除重复的数据
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        private List<Position> RemoveRepeatPosition(List<Position> list1)
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
                    Log.Error("RemoveRepeatPosition", ex);
                }
            }
            return dict.Values.ToList();
        }

        private void InsertPostions()
        {
            lock (Positions)
            {
                if (!isBusy && Positions.Count > 0)
                {
                    isBusy = true;
                    
                    try
                    {
                        Positions = RemoveRepeatPosition(Positions);//删除重复的数据 这个原来在InsertPostions也有

                        List<Position> posList2 = new List<Position>();
                        posList2.AddRange(Positions);
                        if (InsertPostions(posList2))
                        {
                            WriteLogRight(GetLogText(string.Format("写入{0}条数据", Positions.Count)));
                            Positions.Clear();
                        }
                        else
                        {
                            WriteLogRight(GetLogText(string.Format("当前有{0}条数据", Positions.Count)));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("InsertPostions", ex);
                        Positions.Clear();
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

        Bll bll;

        AuthorizationBuffer ab;
        
        private bool InsertPostions(List<Position> list1)
        {
            //if (list1.Count < 20) return false;
            bool r = false;
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            if (bll == null)
            {
                bll = GetLocationBll();
            }
            if (ab == null)
            {
                ab = AuthorizationBuffer.Instance(bll);
            }
            //using (var bll = GetLocationBll())
            {
                r = bll.AddPositionsEx(list1);
                //todo:添加定位权限判断
                if (r)
                {
                    NewAlarms = ab.GetNewAlarms(list1);
                    if (NewAlarmsFired != null)
                    {
                        NewAlarmsFired(NewAlarms);
                    }
                    //AlarmHub.SendLocationAlarms(obj.ToTModel().ToArray());
                }
            }

            SendNsqPos(list1);

            watch1.Stop();
            WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", list1.Count, watch1.Elapsed)));
            return r;
        }

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
                nsq = new SynchronizedPosition(LocationService.url + ":4151");
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
                }
                if (insertThread != null)
                {
                    insertThread.Abort();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
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
                            Positions.Add(item);
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

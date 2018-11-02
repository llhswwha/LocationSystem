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

namespace LocationServices.Tools
{
    public class PositionEngineClient
    {
        public PositionEngineLog Logs { get; set; }

        public void WriteLogLeft(string txt)
        {
            if (Logs == null)
            {
                Logs = new PositionEngineLog();
            }
            Logs.WriteLogLeft(txt);
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
                    Thread.Sleep(100);
                    InsertPostions();
                }
            });
        }



        private string GetLogText(string msg)
        {
            return DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg;
        }


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

        private bool InsertPostions(List<Position> list1)
        {
            bool r = false;
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            using (var bll = GetLocationBll())
            {
                r = bll.AddPositionsEx(list1);
                //todo:添加定位权限判断
                if (r)
                {
                    foreach (Position p in list1)
                    {
                        if (p == null) continue;
                        //1.找出区域相关的所有权限
                        //2.判断当前定位卡是否有权限进入该区域
                        //  2.1找的卡所在的标签角色
                        //  2.2判断该组是否是在权限内
                        //  2.3不在则发出警告，进入非法区域
                        //  2.4默认标签角色CardRole 1.超级管理员、巡检人员、管理人员、施工人员、参观人员
                        //p.AreaId
                    }
                }
            }

            watch1.Stop();
            WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", list1.Count, watch1.Elapsed)));
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

        public bool Stop()
        {
            try
            {
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
            lock (Positions)
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
            return new Bll(false, true, false);
        }
    }
}

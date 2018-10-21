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

        public void StartConnectEngine(int mockCount,string engineIp, string localIp)
        {
            Log.Info("StartConnectEngine:"+engineIp);
            //int mockCount = int.Parse(TbMockTagPowerCount0.Text);
            if (engineDa == null)
            {
                //engineDa = new PositionEngineDA("192.168.10.155", "192.168.10.19");//todo:ip写到配置文件中
                engineDa = new PositionEngineDA(engineIp, localIp);//todo:ip写到配置文件中
                engineDa.MockCount = mockCount;
                //engineDa.MessageReceived += EngineDa_MessageReceived;
                engineDa.MessageReceived += (obj) =>
                {
                    Logs.WriteLogLeft(GetLogText(obj));
                };
                //engineDa.PositionRecived += EngineDa_PositionRecived;
                engineDa.PositionListRecived += EngineDa_PositionListRecived;
            }

            using (var positionBll = GetLocationBll())
            {
                positionBll.InitTagPosition(mockCount);
            }

            engineDa.Start();

            StartInsertPositionTimer();
        }

        private Thread insertThread;

        public bool isBusy = false;//没有这个标志位的话，很容易导致子线程间干扰

        public void StartInsertPositionTimer()
        {
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

        private bool InsertPostions(List<Position> list1)
        {
            bool r = false;
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();

            using (var positionBll = GetLocationBll())
            {
                var personnels = positionBll.Personnels.ToList();
                var tagToPersons = positionBll.LocationCardToPersonnels.ToList();
                var tags = positionBll.LocationCards.ToList();
                var archors = positionBll.Archors.ToList();//基站
                

                list1 = RemoveRepeatPosition(list1);

                //var tagPositions = positionBll.LocationCardPositions.ToList();//实时位置
                ////剔除位置信息不变的部分
                //List<Position> list2 = new List<Position>();
                //foreach (var pos in list1)
                //{
                //    var tagPos = tagPositions.Find(i => i.Code == pos.Code);
                //    if (tagPos != null)
                //    {
                //        double distance = (tagPos.X - pos.X)*(tagPos.X - pos.X) + (tagPos.Z - pos.Z)*(tagPos.Z - pos.Z);
                //        if (distance > 1)
                //        {
                //            list2.Add(pos);
                //        }
                //    }
                //}
                ////todo:怎么利用能够判断位置信息不变的部分呢

                //处理定位引擎位置信息，添加关联人员信息
                foreach (Position pos in list1)
                {
                    if (pos == null) continue;
                    try
                    {
                        var tag = tags.Find(i => i.Code == pos.Code);//标签
                        if (tag == null) continue;
                        var ttp = tagToPersons.Find(i => i.LocationCardId == tag.Id);//关系
                        if (ttp == null) continue;
                        var personnelT = personnels.Find(i => i.Id == ttp.PersonnelId);//人员
                        if (personnelT != null)
                        {
                            pos.PersonnelID = personnelT.Id;
                        }

                        if (pos.IsSimulate)//是模拟程序数据
                        {
                            var relativeArchors=archors.FindAll(i => ((i.X - pos.X)*(i.X - pos.X) + (i.Z - pos.Z)*(i.Z - pos.Z)) < 100).ToList();
                            if (relativeArchors == null) continue;
                            foreach (var archor in relativeArchors)
                            {
                                pos.AddArchor(archor.Code);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        int i = 0;
                    }
                }

                r = positionBll.AddPositions(list1);
                if (r)
                {
                    foreach (Position p in list1)
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

        private Bll GetLocationBll()
        {
            return new Bll(false, true, false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Coldairarrow.Util.Sockets;
using DbModel.LocationHistory.Data;
using Location.BLL.Tool;

namespace LocationWCFServer
{
    /// <summary>
    /// 定位引擎对接获取定位信息
    /// </summary>
    public class PositionEngineDA
    {
        private LightUDP ludp2;

        private Thread aliveThread;

        public int MockCount = 100;

        public string EngineIp { get; set; }

        public string LocalIp { get; set; }

        public PositionEngineDA(string engineIp,string localIp)
        {
            EngineIp = engineIp;
            LocalIp = localIp;
        }

        
        public void Start()
        {
            Log.Info("PositionEngineDA.Start");
            Stop();
            if (aliveThread == null)
            {
                aliveThread = new Thread(() =>
                {
                    while (true)
                    {
                        InitUdp();
                        SendAlive();
                        Thread.Sleep(500);
                    }
                });
                aliveThread.Start();
            }
        }

        private void SendAlive()
        {
            //Log.Info("PositionEngineDA.SendAlive");
            byte[] data = Encoding.UTF8.GetBytes("1");
            IPAddress ip = IPAddress.Parse(EngineIp);
            int port = EnginePort;
            ludp2.Send(data, new IPEndPoint(ip, port));

            if (ludp2.IsReceiving == false)//假如是后启动的定位引擎 虽然定位引擎能收到数据 但是这里无法收到定位数据
            {
                ludp2.InitReceive();
            }
        }

        public int EnginePort = 3456;
        public int LocalPort = 2323;

        private void InitUdp()
        {
            if (ludp2 == null)
            {
                Log.Info("PositionEngineDA.InitUdp");
                ludp2 = new LightUDP(IPAddress.Parse(LocalIp), LocalPort); //建立UDP  监听端口
                ludp2.DGramRecieved += Ludp2_DGramRecieved;
            }
        }

        private void Ludp2_DGramRecieved(object sender, BUDPGram dgram)
        {
            string msg = Encoding.UTF8.GetString(dgram.data);
            if (MessageReceived != null)
            {
                MessageReceived(msg);
            }
            Position pos = new Position();
            if (pos.Parse(msg))
            {
                if (PositionRecived != null)
                {
                    PositionRecived(pos);
                }
                List<Position> posList = AddPositions(pos);
                if (PositionListRecived != null)
                {
                    PositionListRecived(posList);
                }
            }
        }

        public event Action<string> MessageReceived;

        public event Action<Position> PositionRecived;

        public event Action<List<Position>> PositionListRecived;

        private List<Position> AddPositions(Position pos)
        {
            List<Position> posList = new List<Position>();
            posList.Add(pos);

            //WriteLine(TbResult2, GetLogText("创建模拟数据 Start"));
            //todo:创建模拟数据 一个标签变成10、50、100个标签。
            for (int i = 0; i < MockCount; i++)
            {
                Position posT = new Position();
                posT.Id = pos.Id + i;
                posT.Code = pos.Code + i;
                posT.X = pos.X + 0.1f * i;
                posList.Add(posT);
            }
            //WriteLine(TbResult2, GetLogText("创建模拟数据 End"));
            return posList;
        }

        public void Stop()
        {
            try
            {
                if (aliveThread != null)
                {
                    aliveThread.Abort();
                    aliveThread = null;
                }

                if (ludp2 != null)
                {
                    ludp2.Close();
                    ludp2 = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("PositionEngineDA.Stop", ex);
            }

        }
    }
}

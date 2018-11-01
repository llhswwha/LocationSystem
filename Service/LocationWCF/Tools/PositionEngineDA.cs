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
    public class EngineLogin
    {
        public string EngineIp { get; set; }

        public string LocalIp { get; set; }

        public int EnginePort { get; set; }
        public int LocalPort { get; set; }

        public EngineLogin(string localIp, int localPort,string engineIp,int enginePort)
        {
            EngineIp = engineIp;
            EnginePort = enginePort;
            LocalIp = localIp;
            LocalPort = localPort;
        }

        public EngineLogin()
        {
            EngineIp = "127.0.0.1";
            EnginePort = 3456;
            LocalIp = "127.0.0.1";
            LocalPort = 2323;
        }

        public bool Valid()
        {
            string[] parts1 = EngineIp.Split('.');
            string[] parts2= LocalIp.Split('.');
            if (parts1.Length == 4 && parts2.Length == 4)
            {
                return parts1[0] == parts2[0] && parts1[1] == parts2[1] && parts1[2] == parts2[2];
            }
            return true;
        }
    }

    /// <summary>
    /// 定位引擎对接获取定位信息
    /// </summary>
    public class PositionEngineDA
    {
        private LightUDP ludp2;

        private Thread aliveThread;

        public int MockCount = 100;

        public EngineLogin Login { get; set; }
        public PositionEngineDA()
        {
            Login = new EngineLogin();
        }
        public PositionEngineDA(EngineLogin login)
        {
            Login = login;
        }

        
        public void Start()
        {
            Log.Info("PositionEngineDA.Start");
            Stop();
            if (aliveThread == null)
            {
                aliveThread = new Thread(KeepAlive);
                aliveThread.Start();
            }
        }

        private void KeepAlive()
        {
            Log.Info("PositionEngineDA.KeepAlive");
            while (true)
            {
                try
                {
                    InitUdp();
                    SendAlive();
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    //Log.Error("PositionEngineDA.KeepAlive", ex);
                    //break;
                }
            }
        }

        private void SendAlive()
        {
            //Log.Info("PositionEngineDA.SendAlive");
            byte[] data = Encoding.UTF8.GetBytes("1");
            IPAddress ip = IPAddress.Parse(Login.EngineIp);
            ludp2.Send(data, new IPEndPoint(ip, Login.EnginePort));

            if (ludp2.IsReceiving == false)//假如是后启动的定位引擎 虽然定位引擎能收到数据 但是这里无法收到定位数据
            {
                ludp2.InitReceive();
            }
        }

        private void InitUdp()
        {
            if (ludp2 == null)
            {
                Log.Info("PositionEngineDA.InitUdp");
                ludp2 = new LightUDP(IPAddress.Parse(Login.LocalIp), Login.LocalPort); //建立UDP  监听端口
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
            if (msg.Contains("002"))
            {
                Log.Info("002");
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

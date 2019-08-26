using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using BLL;
using Coldairarrow.Util.Sockets;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using Location.BLL.Tool;
using LocationServer;

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
            return IpHelper.IsSameDomain(EngineIp, LocalIp);
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
            Log.Info(LogTags.Engine, "PositionEngineDA.Start");
            Stop();
            if (aliveThread == null)
            {
                aliveThread = new Thread(KeepAlive);
                aliveThread.IsBackground = true;
                aliveThread.Start();
            }

            aliveInterval = AppContext.PosEngineKeepAliveInterval;
        }

        public int aliveInterval = 750;//心跳包的时间间隔

        private void KeepAlive()
        {
            Log.Info(LogTags.Engine, "PositionEngineDA.KeepAlive");
            while (true)
            {
                try
                {
                    InitUdp();
                    SendAlive();
                }
                catch (Exception ex)
                {
                    //Log.Error("PositionEngineDA.KeepAlive", ex);
                    //break;
                }
                Thread.Sleep(aliveInterval);
            }
        }

        /// <summary>
        /// 心跳包发送的字符
        /// </summary>
        public string AliveText = "0";//当前项目所有标签的ID都是09开头的，发送1作为心跳包，有一定概率返回19开头的数据，改成0

        private void SendAlive()
        {
            //Log.Info(LogTags.Engine,"PositionEngineDA.SendAlive");
            byte[] data = Encoding.UTF8.GetBytes(AliveText);
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
                Log.Info(LogTags.Engine,"PositionEngineDA.InitUdp");
                ludp2 = new LightUDP(IPAddress.Parse(Login.LocalIp), Login.LocalPort); //建立UDP  监听端口
                //ludp2.DGramRecieved += Ludp2_DGramRecieved;
                ludp2.DGramListRecieved += Ludp2_DGramListRecieved;
            }
        }

        private void Ludp2_DGramListRecieved(object sender, List<BUDPGram> dgramList)
        {
            List<Position> posList = new List<Position>();
            foreach (var dgram in dgramList)
            {
                string msg = Encoding.UTF8.GetString(dgram.data);
                if (dgram.data.Length>1&&msg.StartsWith("1"))
                {

                }
                if (MessageReceived != null)
                {
                    MessageReceived(msg);
                }
                Position pos = new Position();
                if (pos.Parse(msg, LocationContext.OffsetX, LocationContext.OffsetY))
                {
                    posList.Add(pos);
                }
            }
            if (PositionListRecived != null)
            {
                PositionListRecived(posList);
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
            if (pos.Parse(msg,LocationContext.OffsetX, LocationContext.OffsetY))
            {
                //if (PositionRecived != null)
                //{
                //    PositionRecived(pos);
                //}
                List<Position> posList = AddMockPosition(pos);
                if (PositionListRecived != null)
                {
                    PositionListRecived(posList);
                }
            }
        }

        public event Action<string> MessageReceived;

        //public event Action<Position> PositionRecived;

        public event Action<List<Position>> PositionListRecived;

        private List<Position> AddMockPosition(Position pos)
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

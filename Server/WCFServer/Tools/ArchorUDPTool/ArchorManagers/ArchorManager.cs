using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArchorUDPTool.Commands;
using ArchorUDPTool.Models;
using Coldairarrow.Util.Sockets;
using DbModel.Tools;
using TModel.Tools;

namespace ArchorUDPTool
{

    public class ArchorManager
    {
        Dictionary<string, LightUDP> udps = new Dictionary<string, LightUDP>();

        private int archorPort;

        CommandResultManager resultList;

        public UDPArchorList archorList;

        public CommandResultGroup AddArchor(System.Net.IPEndPoint iep, byte[] data)
        {
            lock (resultList.Groups)
            {
                if (archorList == null)
                {
                    archorList = new UDPArchorList();
                    //archorList.DataUpdated += (archor) =>
                    //{

                    //};
                    //archorList.DataAdded += (archor) =>
                    //{
                    //    //if (ArchorListChanged != null)
                    //    //{
                    //    //    ArchorListChanged(archorList);
                    //    //}
   
                    //};
                }

                UDPArchorList list = new UDPArchorList();
                foreach(var item in resultList.Groups)
                {
                    list.Add(item.Archor);
                    item.Archor.Num = list.Count;
                }

                CommandResultGroup group =resultList.Add(iep, data);
                if (ArchorUpdated != null)
                {
                    ArchorUpdated(group.Archor);
                }
                if (ArchorListChanged != null)
                {
                    ArchorListChanged(list);
                }
                archorList = list;
                return group;
            }

        }

        public string GetStatistics()
        {
            return resultList.GetStatistics();
        }

        public event Action<UDPArchorList> ArchorListChanged;

        public event Action<UDPArchor> ArchorUpdated;

        public event Action<string> LogChanged;

        public Stopwatch Stopwatch;

        public void StopTime()
        {
            if (Stopwatch == null)
            {
                Stopwatch = new Stopwatch();
            }
            Stopwatch.Stop();
        }

        public TimeSpan GetTimeSpan()
        {
            if (Stopwatch == null)
            {
                return TimeSpan.FromMilliseconds(0);
            }
            return Stopwatch.Elapsed;
        }

        private void StartTime()
        {
            StopTime();
            Stopwatch.Reset();
            Stopwatch.Start();
        }

        public class ScanArg
        {
            public string ipsText;
            public string port;
            public bool OneIPS;
            public bool ScanList;
            public bool Ping;
            public string[] cmds;
        }

        public void ScanArchors(ScanArg arg)
        {
            var ips = arg.ipsText.Split(';');
            archorPort = arg.port.ToInt();
            if (resultList == null)
            {
                resultList = new CommandResultManager();
            }
            StartTime();

            List<string> localIps = GetLocalIps(ips);

            if (arg.OneIPS)
            {
                List<string> allIps = new List<string>();
                foreach (var ip in localIps)
                {
                    var ipList = IpHelper.GetIPS(ip);
                    allIps.AddRange(ipList);
                }
                ScanArchors(arg.cmds, allIps.ToArray());
            }
            else if (arg.ScanList)
            {
                List<string> allIps = new List<string>();
                foreach (var archor in archors.ArchorList)
                {
                    allIps.Add(archor.ArchorIp);
                }
                ScanArchors(arg.cmds, allIps.ToArray());
            }
            else
            {
                ScanArchors(arg.cmds, localIps.ToArray());
            }
        }

        private List<string> GetLocalIps(string[] ips)
        {
            List<string> localIps = new List<string>();
            foreach (var ip in ips)
            {
                var localIp = IpHelper.GetLocalIp(ip);
                if (localIp != null)
                {
                    localIps.Add(ip);
                    AddLog("存在IP段:" + ip);
                }
                else
                {
                    AddLog("不存在IP段:" + ip);
                }
            }
            return localIps;
        }

        public string[] Ips;
        public string[] Cmds;

        private void ScanArchors(string cmd, string[] ips)
        {
            ScanArchors(new string[] { cmd }, ips);
        }

        private void ScanArchor(string cmd, string ip)
        {
            ScanArchors(new string[] { cmd }, new string[] { ip });
        }

        private void ScanArchors(string[] cmds, string[] ips)
        {
            this.Cmds = cmds;
            this.Ips = ips;
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();

            //Ping ping = new Ping();
            //ping.
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (PercentChanged != null)
            {
                PercentChanged(e.ProgressPercentage);
            }
        }

        public Action<int> PercentChanged;

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int j = 0; j < Ips.Length; j++)
            {
                string ip = Ips[j];
                var localIp = IpHelper.GetLocalIp(ip);

                var udp = GetLightUDP(localIp);
                int sleepTime = 200;
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), archorPort);

                int i = 0;
                foreach (var cmd in Cmds)
                {
                    string log = string.Format("发送 :: [{0}]:{1}", ipEndPoint, cmd);
                    AddLog(log);
                    udp.SendHex(cmd, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    i++;
                }
                Thread.Sleep(sleepTime * i);

                int percent = (int)((j + 1.0) / Ips.Length * 100);
                worker.ReportProgress(percent,Ips.Length);
            }
        }

        public string Log = "";

        private void AddLog(string log)
        {
            Log = log + "\n" + Log;
            if (LogChanged != null)
            {
                LogChanged(Log);
            }
        }

        private LightUDP GetLightUDP(IPAddress localIp)
        {
            LightUDP udp = null;
            var id = localIp.ToString();
            if (udps.ContainsKey(id))
            {
                udp = udps[id];
            }
            else
            {
                udp = new LightUDP(localIp, 1111);
                udp.DGramRecieved += Udp_DGramRecieved;
                udps[id] = udp;
            }
            return udp;
        }

        internal void SaveArchorList(string path)
        {
            XmlSerializeHelper.Save(archorList, path);
        }

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            //string hex = ByteHelper.byteToHexStr(dgram.data);
            ////string str = Encoding.UTF7.GetString(dgram.data);
            //string txt = string.Format("[{0}]:{1}", dgram.iep, hex);
            //AddLog(txt);

            var group=AddArchor(dgram.iep, dgram.data);

            string txt = string.Format("收到 :: {0}", group.ToString());
            AddLog(txt);
        }

        public void SendCmd(string cmd)
        {
            foreach (var archor in archorList)
            {
                var localIp = IpHelper.GetLocalIp(archor.Ip);
                var udp = GetLightUDP(localIp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(archor.Ip), archorPort);
                udp.SendHex(cmd, ipEndPoint);
            }
        }

        public void SetServerIp251()
        {
            foreach (var archor in archorList)
            {
                var localIp = IpHelper.GetLocalIp(archor.Ip);
                var udp = GetLightUDP(localIp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(archor.Ip), archorPort);
                var cmd = "";
                if (archor.Ip.StartsWith("192.168.3."))
                {
                    if(archor.ServerIp!="192.168.3.251")
                        cmd = UDPCommands.ServerIp3251;
                }
                else if (archor.Ip.StartsWith("192.168.4."))
                {
                    if (archor.ServerIp != "192.168.4.251")
                        cmd = UDPCommands.ServerIp4251;
                }
                else if(archor.Ip.StartsWith("192.168.5."))
                {
                    if (archor.ServerIp != "192.168.5.251")
                        cmd = UDPCommands.ServerIp5251;
                }
                udp.SendHex(cmd, ipEndPoint);
            }
        }

        public void SetServerIp253()
        {
            foreach (var archor in archorList)
            {
                var localIp = IpHelper.GetLocalIp(archor.Ip);
                var udp = GetLightUDP(localIp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(archor.Ip), archorPort);
                var cmd = "";
                if (archor.Ip.StartsWith("192.168.3."))
                {
                    if (archor.ServerIp != "192.168.3.253")
                        cmd = UDPCommands.ServerIp3253;
                }
                else if (archor.Ip.StartsWith("192.168.4."))
                {
                    if (archor.ServerIp != "192.168.4.253")
                        cmd = UDPCommands.ServerIp4253;
                }
                else if (archor.Ip.StartsWith("192.168.5."))
                {
                    if (archor.ServerIp != "192.168.5.253")
                        cmd = UDPCommands.ServerIp5253;
                }
                udp.SendHex(cmd, ipEndPoint);
            }
        }

        internal void ClearBuffer()
        {
            resultList = new CommandResultManager();
        }

        internal void ScanArchor(UDPArchor archor)
        {
            ScanArchors(UDPCommands.GetAll().ToArray(), new string[] { archor.GetClientIP() });
        }

        internal void GetArchorInfo(UDPArchor archor,string key)
        {
            //key = key.ToLower();
            if (key == "Id")
            {
                ScanArchor(UDPCommands.GetId, archor.GetClientIP());
            }
            else if (key == "Ip")
            {
                ScanArchor(UDPCommands.GetIp, archor.GetClientIP());
            }
            else if (key == "Type")
            {
                ScanArchor(UDPCommands.GetType, archor.GetClientIP());
            }
            else if (key == "ServerIp")
            {
                ScanArchor(UDPCommands.GetServerIp, archor.GetClientIP());
            }
            else if (key == "ServerPort")
            {
                ScanArchor(UDPCommands.GetServerPort, archor.GetClientIP());
            }
            else if (key == "Mask")
            {
                ScanArchor(UDPCommands.GetMask, archor.GetClientIP());
            }
            else if (key == "Gateway")
            {
                ScanArchor(UDPCommands.GetGateway, archor.GetClientIP());
            }
            else if (key == "DHCP")
            {
                ScanArchor(UDPCommands.GetDHCP, archor.GetClientIP());
            }
            else if (key == "SoftVersion")
            {
                ScanArchor(UDPCommands.GetSoftVersion, archor.GetClientIP());
            }
            else if (key == "HardVersion")
            {
                ScanArchor(UDPCommands.GetHardVersion, archor.GetClientIP());
            }
            else if (key == "Power")
            {
                ScanArchor(UDPCommands.GetPower, archor.GetClientIP());
            }
            else if (key == "MAC")
            {
                ScanArchor(UDPCommands.GetMAC, archor.GetClientIP());
            }

        }

        internal void SetArchorInfo(UDPArchor archor, string key)
        {
            if (key == "id")
            {

            }
            ScanArchors(UDPCommands.GetAll().ToArray(), new string[] { archor.GetClientIP() });
        }

        ArchorDevList archors;

        internal void LoadList(ArchorDevList archors)
        {
            this.archors = archors;
            resultList = new CommandResultManager();
            foreach (var item in archors.ArchorList)
            {
                var group=resultList.Add(item);
                //group.Archor.Ip = item.ArchorIp;
                group.Archor.Area = item.InstallArea;
            }

            UDPArchorList list = new UDPArchorList();
            foreach (var item in resultList.Groups)
            {
                list.Add(item.Archor);
                item.Archor.Num = list.Count;
            }

            archorList = list;
            if (ArchorListChanged != null)
            {
                ArchorListChanged(list);
            }
        }

        internal void LoadArchorList(string path)
        {
            archorList = XmlSerializeHelper.LoadFromFile<UDPArchorList>(path);

            resultList = new CommandResultManager();
            foreach (var item in archorList)
            {
                var group = resultList.Add(item);
            }

            if (ArchorListChanged != null)
            {
                ArchorListChanged(archorList);
            }
        }
    }
}

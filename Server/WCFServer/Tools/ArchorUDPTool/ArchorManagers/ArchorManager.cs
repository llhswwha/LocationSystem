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
using ArchorUDPTool.Tools;
using Coldairarrow.Util.Sockets;
using DbModel.Tools;
using TModel.Tools;
using ArchorUDPTool.ArchorManagers;
using System.IO;
using System.Windows;

namespace ArchorUDPTool
{

    public class ArchorManager
    {
        Dictionary<string, LightUDP> udps = new Dictionary<string, LightUDP>();

        public int archorPort;

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

                CommandResultGroup group = resultList.Add(iep, data);
                archorList = OnDataReceive(group);
                return group;
            }

        }

        public string GetStatistics()
        {
            return resultList.GetStatistics();
        }

        public event Action<UDPArchorList, UDPArchor> ArchorListChanged;

        Dictionary<string, UDPArchor> progressList = new Dictionary<string, UDPArchor>();

        //public event Action<UDPArchor> ArchorUpdated;

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
            public int PingLength;
            public int PingWaitTime;
            public int PingCount;
            public string[] cmds;
        }

        public ScanArg arg;

        public void ScanArchors(ScanArg arg, UDPArchorList list)
        {
            if (arg == null) return;
            this.arg = arg;
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
                if (list != null)
                {
                    foreach (var archor in list)
                    {
                        allIps.Add(archor.GetIp());
                    }
                }
                else if (archors != null)
                    foreach (var archor in archors.ArchorList)
                    {
                        allIps.Add(archor.ArchorIp);
                    }
                else if (archorList != null)
                {
                    foreach (var archor in archorList)
                    {
                        allIps.Add(archor.GetClientIP());
                    }
                }

                ScanArchors(arg.cmds, allIps.ToArray());
            }
            else
            {
                ScanArchors(arg.cmds, localIps.ToArray());
            }
        }

        public IPAddress LocalIp;

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

        public IPAddress GetLocalIp(string ip)
        {
            var localIp = IpHelper.GetLocalIp(ip);
            if (localIp == null)
            {
                localIp = LocalIp;
            }

            return localIp;
        }

        public string[] Ips;
        public string[] Cmds;

        private void ScanArchors(string cmd, string[] ips)
        {
            ScanArchors(new string[] {cmd}, ips);
        }

        private void ScanArchor(string cmd, string ip)
        {
            ScanArchors(new string[] {cmd}, new string[] {ip});
        }

        private void ScanArchors(string[] cmds, string[] ips)
        {
            this.Cmds = cmds;
            this.Ips = ips;

            if (arg.Ping)
            {
                resultList.ClearPing(Ips);
                if (pingEx == null)
                {
                    pingEx = new PingEx();
                    pingEx.ProgressChanged += PingEx_ProgressChanged;
                    pingEx.Error += (ex) => { AddLog(ex.ToString()); };
                }

                pingEx.WaitTime = arg.PingWaitTime;
                pingEx.SetData(arg.PingLength);
                pingEx.PingRange(Ips, arg.PingCount);
            }
            else
            {
                //BackgroundWorker worker = new BackgroundWorker();
                //worker.WorkerSupportsCancellation = true;
                //worker.WorkerReportsProgress = true;
                //worker.DoWork += Worker_DoWork;
                //worker.ProgressChanged += Worker_ProgressChanged;
                //worker.RunWorkerAsync();

                udpscanWorker = new UDPScanWorker(this, Ips);
                udpscanWorker.Start();
            }

            //Ping ping = new Ping();
            //ping.
        }

        UDPScanWorker udpscanWorker;

        //BackgroundWorker worker;

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnPercentChanged(e.ProgressPercentage);
        }

        public void OnPercentChanged(int p)
        {
            if (PercentChanged != null)
            {
                PercentChanged(p);
            }
        }

        public event Action<int> PercentChanged;

        PingEx pingEx;

        public int CmdSleepTime = 10;

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IsCancel = false;
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int j = 0; j < Ips.Length; j++)
            {
                string ip = Ips[j];

                //if (arg.Ping)
                //{
                //    if (pingEx == null)
                //    {
                //        pingEx = new PingEx();
                //        pingEx.ProgressChanged += PingEx_ProgressChanged;
                //    }
                //    pingEx.Ping(ip, 4);
                //}

                var localIp = GetLocalIp(ip);

                if (localIp != null)
                {
                    var udp = GetLightUDP(localIp);
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), archorPort);

                    foreach (var cmd in Cmds)
                    {
                        AddLog(string.Format("发送 :: [{0}]:{1}", ipEndPoint, cmd));
                        udp.SendHex(cmd, ipEndPoint);
                        Thread.Sleep(CmdSleepTime);
                    }

                    Thread.Sleep(CmdSleepTime * Cmds.Length);
                }

                int percent = (int) ((j + 1.0) / Ips.Length * 100);
                worker.ReportProgress(percent, Ips.Length);

                if (IsCancel)
                {
                    return;
                }
            }
        }

        private void PingEx_ProgressChanged(int arg1, PingResult arg2)
        {
            AddLog(arg2.Line);
            if (arg2.Type == 1)
            {
                var group = resultList.GetByIp(arg2.Ip);
                if (group == null)
                {
                    group = resultList.Add(arg2.Ip + ":Ping");
                    group.Archor.Ping = arg2.GetResult();
                }
                else
                {
                    group.Archor.Ping = arg2.GetResult();
                }

                //AddLog()

                archorList = OnDataReceive(group);
            }
            else
            {
                if (PercentChanged != null)
                {
                    PercentChanged(arg1);
                }
            }
        }

        public string Log = "";

        int logCount = 0;

        private void AddLog(string log)
        {
            logCount++;
            log = "[" + logCount + "]" + log;
            Log = log + "\n" + Log;
            if (Log.Length > 2000)
            {
                Log = Log.Substring(0, 1000);
            }

            if (LogChanged != null)
            {
                LogChanged(Log);
            }
        }

        public static int port = 1115;
        public static int count = 0;

        public LightUDP GetLightUDP(IPAddress localIp)
        {
            count++;
            LightUDP udp = null;
            var id = localIp.ToString();
            if (udps.ContainsKey(id))
            {
                udp = udps[id];
            }
            else
            {
                udp = LightUDP.Create(localIp, port + count);
                udp.DGramRecieved += Udp_DGramRecieved;
                udps[id] = udp;
            }

            return udp;
        }

        public void SaveArchorList(string path)
        {
            var list = GetMaxArchorList();
            XmlSerializeHelper.Save(list, path);

            savedList = list;
        }

        public static UDPArchorList LoadArchorListResult()
        {
            if (savedList == null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
                FileInfo fi = new FileInfo(path);
                savedList = XmlSerializeHelper.LoadFromFile<UDPArchorList>(path);
            }

            return savedList;
        }

        public void SaveArchorListResult()
        {
            var list = GetMaxArchorList();
            savedList = list;
        }

        public static UDPArchorList savedList;

        private void Udp_DGramRecieved(object sender, BUDPGram dgram)
        {
            //string hex = ByteHelper.byteToHexStr(dgram.data);
            ////string str = Encoding.UTF7.GetString(dgram.data);
            //string txt = string.Format("[{0}]:{1}", dgram.iep, hex);
            //AddLog(txt);

            var group = AddArchor(dgram.iep, dgram.data);
            AddLog(string.Format("收到 :: {0}", group.ToString()));
        }

        public void SendCmd(string cmd, int port)
        {
            archorPort = port;
            ThreadTool.Start(() =>
            {
                foreach (var archor in archorList)
                {
                    SendCmd(cmd, archor);
                    Thread.Sleep(100);
                }
            });

        }

        public void SendCmd(string cmd, int port, UDPArchorList list)
        {
            archorPort = port;
            ThreadTool.Start(() =>
            {
                foreach (var archor in list)
                {
                    SendCmd(cmd, archor);
                    Thread.Sleep(100);
                }
            });

        }

        private void SendCmd(string cmd, UDPArchor archor)
        {
            var archorIp = archor.GetClientIP();
            var localIp = GetLocalIp(archorIp);
            if (localIp == null) return;
            var udp = GetLightUDP(localIp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(archorIp), archorPort);
            udp.SendHex(cmd, ipEndPoint);
            AddLog(string.Format("发送 :: [{0}]:{1}", ipEndPoint, cmd));
        }

        public void ResetAll(int port)
        {
            AddLog(string.Format("开始重启"));
            archorPort = port;
            SendCmd(UDPCommands.Restart, port);
        }

        public void ResetAll(int port, UDPArchorList list)
        {
            AddLog(string.Format("开始重启"));
            archorPort = port;
            SendCmd(UDPCommands.Restart, port, list);
        }

        public void Reset(params UDPArchor[] archors)
        {
            AddLog(string.Format("开始重启"));
            foreach (var archor in archors)
            {
                SendCmd(UDPCommands.Restart, archor);
            }
        }

        public void SetServerIp251(int port)
        {
            archorPort = port;
            foreach (var archor in archorList)
            {
                var localIp = GetLocalIp(archor.Ip);
                if (localIp != null)
                {
                    var udp = GetLightUDP(localIp);
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(archor.Ip), archorPort);
                    var cmd = "";
                    if (archor.Ip.StartsWith("192.168.3."))
                    {
                        if (archor.ServerIp != "192.168.3.251")
                            cmd = SetCommands.GetCmd("192.168.3.251");
                    }
                    else if (archor.Ip.StartsWith("192.168.4."))
                    {
                        if (archor.ServerIp != "192.168.4.251")
                            cmd = SetCommands.GetCmd("192.168.4.251");
                    }
                    else if (archor.Ip.StartsWith("192.168.5."))
                    {
                        if (archor.ServerIp != "192.168.5.251")
                            cmd = SetCommands.GetCmd("192.168.5.251");
                    }

                    udp.SendHex(cmd, ipEndPoint);
                    AddLog(string.Format("发送 :: [{0}]:{1}", ipEndPoint, cmd));
                }

            }
        }

        public void SetServerIp253(int port)
        {
            archorPort = port;
            foreach (var archor in archorList)
            {
                string archorIp = archor.GetClientIP();
                var localIp = IpHelper.GetLocalIp(archorIp);
                if (localIp == null)
                {
                    continue;
                }

                var udp = GetLightUDP(localIp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(archorIp), archorPort);
                var cmd = "";
                if (archorIp.StartsWith("192.168.3."))
                {
                    if (archor.ServerIp != "192.168.3.253")
                        cmd = SetCommands.GetCmd("192.168.3.253");
                }
                else if (archorIp.StartsWith("192.168.4."))
                {
                    if (archor.ServerIp != "192.168.4.253")
                        cmd = SetCommands.GetCmd("192.168.4.253");
                }
                else if (archorIp.StartsWith("192.168.5."))
                {
                    if (archor.ServerIp != "192.168.5.253")
                        cmd = SetCommands.GetCmd("192.168.5.253");
                }

                udp.SendHex(cmd, ipEndPoint);
                AddLog(string.Format("发送 :: [{0}]:{1}", ipEndPoint, cmd));
            }
        }

        internal void ClearBuffer()
        {
            resultList = new CommandResultManager();
        }

        internal void ScanArchor(params UDPArchor[] archors)
        {
            var ips = new List<string>();
            foreach (UDPArchor item in archors)
            {
                ips.Add(item.GetClientIP());
            }

            ScanArchors(UDPCommands.GetAll().ToArray(), ips.ToArray());
        }

        internal void GetArchorInfo(UDPArchor archor, string key)
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

        internal void UnCheckAll()
        {
            if (archorList != null)
            {
                foreach (var item in archorList)
                {
                    item.IsChecked = false;
                }
            }
        }

        internal void CheckAll()
        {
            if (archorList != null)
            {
                foreach (var item in archorList)
                {
                    item.IsChecked = true;
                }
            }
        }

        internal void Cancel()
        {
            if (arg == null) return;
            if (arg.Ping)
            {
                pingEx.Cancel();
            }
            //else if(worker!=null)
            //{
            //    worker.CancelAsync();
            //    IsCancel = true;
            //}

            else if (udpscanWorker != null)
            {
                udpscanWorker.Stop();
                IsCancel = true;
            }
        }

        private bool IsCancel = false;

        public List<LightUDP> serverUdps = new List<LightUDP>();

        internal void StartListen()
        {

            //var servers = archorList.ServerList;
            //foreach (var server in servers)
            //{
            //    var localIp = IpHelper.GetLocalIp(server.Ip);
            //    if (localIp != null && localIp.Address.ToString() == server.Ip)
            //    {
            //        LightUDP udp = new LightUDP(server.Ip, server.Port);
            //        udp.DGramRecieved += Udp_DGramRecieved1;
            //        serverUdps.Add(udp);
            //    }
            //}

            string[] ips = new string[] {"192.168.3.253", "192.168.4.253", "192.168.5.253"};
            foreach (var ip in ips)
            {
                var localIp = IpHelper.GetLocalIp(ip);
                if (localIp != null && localIp.ToString() == ip)
                {
                    LightUDP udp = new LightUDP(ip, 1998);
                    udp.DGramRecieved += Udp_DGramRecieved1;
                    serverUdps.Add(udp);
                }
            }

        }

        public UDPArchorValueList valueList = new UDPArchorValueList();

        private void Udp_DGramRecieved1(object sender, BUDPGram dgram)
        {
            if (resultList == null)
            {
                resultList = new CommandResultManager();
            }

            var group = resultList.Add(dgram.iep, dgram.data);
            group.Archor.Value = ByteHelper.byteToHexStr(dgram.data);

            valueList.Add(dgram.iep, dgram.data);
            AddLog(string.Format("收到 :: {0}", group.ToString()));


            archorList = OnDataReceive(group);
        }

        private UDPArchorList OnDataReceive(CommandResultGroup group)
        {
            UDPArchorList list = GetResultArchorList();
            //if (ArchorUpdated != null)
            //{
            //    ArchorUpdated(group.Archor);
            //}

            OnArchorListChanged(list, group.Archor);
            return list;
        }

        public UDPArchorList GetResultArchorList()
        {
            UDPArchorList list = new UDPArchorList();
            foreach (var item in resultList.Groups)
            {
                list.Add(item.Archor);
                item.Archor.Num = list.Count;
            }

            return list;
        }

        public void StopListen()
        {
            foreach (var item in serverUdps)
            {
                item.Close();
            }

            serverUdps.Clear();
        }

        internal void SetArchorInfo(UDPArchor archor, string key)
        {
            if (key == "id")
            {

            }

            ScanArchors(UDPCommands.GetAll().ToArray(), new string[] {archor.GetClientIP()});
        }

        ArchorDevList archors;

        public void LoadList(ArchorDevList archors)
        {
            this.archors = archors;
            resultList = new CommandResultManager();
            if (archors.ArchorList != null)
                foreach (var item in archors.ArchorList)
                {
                    var group = resultList.Add(item);
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
            OnArchorListChanged(list, null);
        }

        private void OnArchorListChanged(UDPArchorList a, UDPArchor item)
        {
            if (item != null && item.Id != null)
            {
                //if(!progressList.Contains(item))
                //    progressList.Add(item);

                if (!progressList.ContainsKey(item.Id))
                {
                    progressList.Add(item.Id, item);
                    //Log.Info(Name, string.Format("maxCount:{0}", progressList.Count));
                }

                count++;

                //Log.Info(Name, "ArchorListChanged:" + count);
            }

            if (ArchorListChanged != null)
            {
                ArchorListChanged(a, item);
            }


        }

        public UDPArchorList GetMaxArchorList()
        {
            UDPArchorList list = new UDPArchorList();
            foreach (var item in progressList)
            {
                list.Add(item.Value);
            }

            return list;
        }

        public void ClearMaxArchorList()
        {
            progressList.Clear();
        }

        internal void LoadArchorList(string path)
        {
            archorList = XmlSerializeHelper.LoadFromFile<UDPArchorList>(path);

            resultList = new CommandResultManager();
            foreach (var item in archorList)
            {
                var group = resultList.Add(item);
            }

            OnArchorListChanged(archorList, null);
        }

        public void Close()
        {
            try
            {
                Cancel();
                if (udps.Values != null)
                    foreach (var item in udps.Values)
                    {
                        if (item == null) continue;
                        item.Close();
                    }

                if (serverUdps != null)
                    foreach (var item in serverUdps)
                    {
                        if (item == null) continue;
                        item.Close();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}

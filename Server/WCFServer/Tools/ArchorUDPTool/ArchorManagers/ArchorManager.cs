using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
                    //    if (NewArchorAdded != null)
                    //    {
                    //        NewArchorAdded(archor);
                    //    }
                    //};
                }

                UDPArchorList list = new UDPArchorList();
                foreach(var item in resultList.Groups)
                {
                    list.Add(item.Archor);
                    item.Archor.Num = list.Count;
                }

                CommandResultGroup group =resultList.Add(iep, data);
                //int r=archorList.AddOrUpdate(group.Archor);
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

        public event Action<UDPArchor> NewArchorAdded;

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

        public void ScanArchors(string ipsText, string port,params string[] cmds)
        {
            var ips = ipsText.Split(';');
            archorPort = port.ToInt();
            if (resultList == null)
            {
                resultList = new CommandResultManager();
            }

            StartTime();
            ScanArchors(cmds, ips);
        }

        private void ScanArchors(string[] cmds, string[] ips)
        {
            Thread thread = ThreadTool.Start(() =>
            {
                foreach (var ip in ips)
                {
                    var localIp = IpHelper.GetLocalIp(ip);
                    if (localIp != null)
                    {
                        AddLog("存在IP段:" + ip);

                        var udp = GetLightUDP(localIp);
                        int sleepTime = 200;
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), archorPort);

                        int i = 0;
                        foreach (var cmd in cmds)
                        {
                            udp.SendHex(cmd, ipEndPoint);
                            Thread.Sleep(sleepTime);
                            i++;
                        }
                        Thread.Sleep(sleepTime * i);
                    }
                    else
                    {
                        AddLog("不存在IP段:" + ip);
                        //MessageBox.Show("当前电脑不存在IP段:" + ips);
                    }
                }
            });
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

            string txt = string.Format("{0}", group.ToString());
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
    }
}

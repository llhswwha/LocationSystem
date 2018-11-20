using System;
using System.Collections.Generic;
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

        public UDPArchorList AddArchor(System.Net.IPEndPoint iep, byte[] data)
        {
            resultList.Add(iep, data);
            archorList = new UDPArchorList();
            foreach (var item in resultList.Groups)
            {
                archorList.Add(item.Archor);
                item.Archor.Num = archorList.Count;
            }
            if (ArchorListChanged != null)
            {
                ArchorListChanged();
            }
            return archorList;
        }

        public event Action ArchorListChanged;

        public event Action<string> LogChanged;

        public void SearchArchor(string ipsText, string port)
        {
            resultList = new CommandResultManager();
            archorPort = port.ToInt();
            var ips = ipsText.Split(';');
            foreach (var ip in ips)
            {
                var localIp = IpHelper.GetLocalIp(ip);
                if (localIp != null)
                {
                    AddLog("存在IP段:" + ip);

                    var udp = GetLightUDP(localIp);
                    int sleepTime = 200;
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), archorPort);
                    udp.SendHex(UDPCommands.GetServerIp, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetId, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetIp, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetPort, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetArchorType, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetDHCP, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetMask, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetGateway, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetSoftVersion, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetHardVersion, ipEndPoint);
                    Thread.Sleep(sleepTime);
                    udp.SendHex(UDPCommands.GetPower, ipEndPoint);
                    Thread.Sleep(sleepTime);

                    Thread.Sleep(1000);
                }
                else
                {
                    AddLog("不存在IP段:" + ip );
                    //MessageBox.Show("当前电脑不存在IP段:" + ips);
                }
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
            string hex = ByteHelper.byteToHexStr(dgram.data);
            //string str = Encoding.UTF7.GetString(dgram.data);
            string txt = string.Format("[{0}]:{1}", dgram.iep, hex);
            AddLog(txt);

            AddArchor(dgram.iep, dgram.data);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Commands
{
    public static class SetCommands
    {
        public static List<SetCommand> Cmds = new List<SetCommand>();

        static SetCommands()
        {
            AddCmd("192.168.5.1", "10 01 20 03 04 C0 A8 05 01 91 9C 9D BE");
            AddCmd("192.168.10.155", "10 01 20 03 04 C0 A8 0A 9B 06 DE FB 2B");
            AddCmd("192.168.3.251", "10 01 20 03 04 C0 A8 03 FB 9A AE 21 3A");
            AddCmd("192.168.4.251", "10 01 20 03 04 C0 A8 04 FB D5 EF B7 FD");
            AddCmd("192.168.5.251", "10 01 20 03 04 C0 A8 05 FB CC F4 86 BC");
            AddCmd("192.168.3.253", "10 01 20 03 04 C0 A8 03 FD 73 CD 84 0F");
            AddCmd("192.168.4.253", "10 01 20 03 04 C0 A8 04 FD 3C 8C 12 C8");
            AddCmd("192.168.5.253", "10 01 20 03 04 C0 A8 05 FD 25 97 23 89");
            AddCmd("172.16.100.25", "10 01 20 03 04 AC 10 64 19 44 AB B8 1D");
            AddCmd("172.16.100.207", "10 01 20 03 04 AC 10 64 CF 2B 1B CF FC");
        }

        public static SetCommand AddCmd(string name, string cmd)
        {
            SetCommand ipCmd = new SetCommand();
            ipCmd.Name = name;
            ipCmd.Cmd = cmd;
            Cmds.Add(ipCmd);
            return ipCmd;
        }

        public static string GetCmd(string name)
        {
            var cmd = Cmds.Find(i => i.Name == name);
            if (cmd == null) return "";
            return cmd.Name;
        }
    }

    public class SetCommand
    {
        public string Name { get; set; }

        public string Cmd { get; set; }
    }

    public static class UDPCommands
    {
        public static string GetId = "10:01:10:01:00:1b:43:95:cb";
        public static string GetIdR = "1001C00102";

        public static string GetIp = "10 01 10 02 00 30 6E C6 08";
        public static string GetIpR = "1001C00204";

        public static string GetMask = "10 01 10 0C 00 AE ED EB 86";
        public static string GetMaskR = "1001C00C04";
        public static string GetGateway = "10 01 10 0D 00 B7 F6 DA C7";
        public static string GetGatewayR = "1001C00D04";
        public static string GetServerIp = "10 01 10 03 00 29 75 F7 49";
        public static string GetServerIpR = "1001C00304";
        public static string GetServerPort = "10:01:10:0a:00:f8:b7:4c:00";
        public static string GetServerPortR = "1001C00A02";
        public static string GetType = "10:01:10:13:00:63:b7:e5:18";
        public static string GetTypeR = "1001C01301";

        public static string GetDHCP = "10 01 10 0E 00 9C DB 89 04";
        public static string GetDHCPR = "1001C00E01";

        public static string GetSoftVersion = "10:01:10:05:00:7f:2f:50:cf";
        public static string GetSoftVersionR = "1001C005";

        public static string GetHardVersion = "10:01:10:06:00:54:02:03:0c";
        public static string GetHardVersionR = "1001C006";

        /// <summary>
        /// 功率
        /// </summary>
        public static string GetPower = "10:01:10:04:00:66:34:61:8e";

        public static string GetPowerR = "1001C00401";

        public static string GetMAC = "10:01:10:09:00:d3:9a:1f:c3";

        public static string GetMACR = "1001C00906";//10:01:c0:09:06:00:00:2e:5b:49:9c:16:ed:01:3f


        public static string Restart = "10 01 20 0B 00 C5 C7 98 D1";

        public static List<string> GetAll()
        {
            return new List<string>() {
                GetId,
                GetIp,
                GetServerIp,
                GetServerPort,
                GetType,
                GetDHCP,
                GetMask,
                GetGateway,
                GetSoftVersion,
                GetHardVersion,
                GetPower,
                GetMAC
            };
        }
    }

    public class UDPCommand
    {
        public string Name { get; set; }

        public string Hex { get; set; }

        public UDPCommand()
        {

        }

        public UDPCommand(string name, string hex)
        {
            Name = name;
            Hex = hex;
        }
    }

    public class CommandList : List<UDPCommand>
    {

    }
}

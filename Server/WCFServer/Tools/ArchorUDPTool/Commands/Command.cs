using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Commands
{
    public class UDPCommands
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
        public static string GetPort = "10:01:10:0a:00:f8:b7:4c:00";
        public static string GetPortR = "1001C00A02";
        public static string GetArchorType = "10:01:10:13:00:63:b7:e5:18";
        public static string GetArchorTypeR = "1001C01301";

        public static string GetDHCP = "10 01 10 0E 00 9C DB 89 04";
        public static string GetDHCPR = "1001C00E01";

        public static string GetSoftVersion = "10:01:10:05:00:7f:2f:50:cf";
        public static string GetSoftVersionR = "1001C00502";

        public static string GetHardVersion = "10:01:10:06:00:54:02:03:0c";
        public static string GetHardVersionR = "1001C00601";

        /// <summary>
        /// 功率
        /// </summary>
        public static string GetPower = "10:01:10:04:00:66:34:61:8e";

        public static string GetPowerR = "1001C00401";


        public static string Restart = "10 01 20 0B 00 C5 C7 98 D1";

        public static string ServerIp1 = "10 01 20 03 04 C0 A8 05 01 91 9C 9D BE";//192.168.5.1
        public static string ServerIp2 = "10 01 20 03 04 C0 A8 0A 9B 06 DE FB 2B";//192.168.10.155
        public static string ServerIp3 = "10 01 20 03 04 C0 A8 03 FB 9A AE 21 3A";//192.168.3.251
        public static string ServerIp4 = "10 01 20 03 04 C0 A8 04 FB D5 EF B7 FD";//192.168.4.251
        public static string ServerIp5 = "10 01 20 03 04 C0 A8 05 FB CC F4 86 BC";//192.168.5.251

        public static List<string> GetAll()
        {
            return new List<string>() { GetId, GetIp, GetArchorType, GetDHCP,GetSoftVersion,GetServerIp,GetPort,GetMask,GetGateway };
        }
    }

    public class UDPCommand
    {
        public string Name { get; set;}

        public string Hex { get; set; }

        public UDPCommand()
        {

        }

        public UDPCommand(string name,string hex)
        {
            Name = name;
            Hex = hex;
        }
    }

    public class CommandList:List<UDPCommand>
    {

    }
}

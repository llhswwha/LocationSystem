using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Commands
{
    public class UDPCommands
    {
        public static string GetMask = "10 01 10 0C 00 AE ED EB 86";
        public static string GetGateway = "10 01 10 0D 00 B7 F6 DA C7";

        public static string GetIp = "10 01 10 02 00 30 6E C6 08";
        public static string GetIpR = "1001C00204";

        public static string GetServerIp = "10 01 10 03 00 29 75 F7 49";
        public static string GetServerIpR = "1001C00304";
        public static string GetDHCP = "10 01 10 0E 00 9C DB 89 04";
        public static string GetPort = "10:01:10:0a:00:f8:b7:4c:00";//
        public static string GetPortR = "1001C00A02";//
        public static string GetId = "10:01:10:01:00:1b:43:95:cb";//
        public static string GetIdR = "1001C00102";//
        public static string GetArchorType = "10:01:10:13:00:63:b7:e5:18";
        public static string GetSoftVersion = "10:01:10:05:00:7f:2f:50:cf";

        public static string GetMaskR = "";
        public static string GetGatewayR = "";
        public static string GetDHCPR = "";
        public static string GetArchorTypeR = "";
        public static string GetSoftVersionR = "";

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchorUDPTool.Models
{
    public class UDPArchor
    {
        public string Id { get; set; }
        public string Ip { get; set; }

        public string ServerIp { get; set; }

        public long ServerPort { get; set; }

        public int Type { get; set; }

        public string Mask { get; set; }

        public string Gateway { get; set; }

        public bool DHCP { get; set; }

        public string SoftVersion { get; set; }

        public string HardVersion { get; set; }

        public int Power { get; set; }
    }
}

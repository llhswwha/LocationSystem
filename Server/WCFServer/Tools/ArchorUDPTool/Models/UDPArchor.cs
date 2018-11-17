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
    }
}

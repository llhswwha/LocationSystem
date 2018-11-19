using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArchorUDPTool.Models
{
    public class UDPArchor
    {
        [XmlAttribute]
        public int Num { get; set; }

        [XmlAttribute]
        public string Client { get; set; }

        public string GetClientIP()
        {
            int id = Client.IndexOf(':');
            string ip = Client.Substring(0, id);
            return ip;
        }

        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string Ip { get; set; }


        [XmlAttribute]
        public string Path1 { get; set; }


        [XmlAttribute]
        public string Path2 { get; set; }

        [XmlAttribute]
        public string ServerIp { get; set; }
        [XmlAttribute]
        public long ServerPort { get; set; }


        [XmlAttribute]
        public int Type { get; set; }
        [XmlAttribute]
        public string Mask { get; set; }
        [XmlAttribute]
        public string Gateway { get; set; }
        [XmlAttribute]
        public bool DHCP { get; set; }
        [XmlAttribute]
        public string SoftVersion { get; set; }
        [XmlAttribute]
        public string HardVersion { get; set; }
        [XmlAttribute]
        public int Power { get; set; }
    }
}

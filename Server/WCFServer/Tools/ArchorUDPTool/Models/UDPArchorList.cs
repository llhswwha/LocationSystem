using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArchorUDPTool.Models
{
    [XmlRoot("UDPArchorList")]
    public class UDPArchorList:List<UDPArchor>
    {
    }
}

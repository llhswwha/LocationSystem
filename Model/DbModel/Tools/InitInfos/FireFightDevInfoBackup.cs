using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Tools.InitInfos
{

    [XmlType("DevBackupList")]
    public class FireFightDevInfoBackupList
    {
        [XmlElement("DeviceList")]
        public List<FireFightDevInfoBackup> DevList;
    }

    public class FireFightDevInfoBackup : IComparable<FireFightDevInfoBackup>
    {
        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("ParentName")]
        public string ParentName;

        [XmlAttribute("ParentId")]
        public string ParentId;

        [XmlAttribute("Code")]
        public string Code;

        [XmlAttribute("Local_TypeCode")]
        public string Local_TypeCode;

        [XmlAttribute("Abutment_Type")]
        public string Abutment_Type;

        public int CompareTo(FireFightDevInfoBackup other)
        {
            return (this.ParentId + Name).CompareTo(other.ParentId + other.Name);
        }
    }
}

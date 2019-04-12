using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Tools.InitInfos
{
    [XmlType("EntranceGuardCardInfoBackupList")]
    public class EntranceGuardCardInfoBackupList
    {
        [XmlElement("PerList")]
        public List<EntranceGuardCardInfoBackup> EcList;
    }

    public class EntranceGuardCardInfoBackup : IComparable<EntranceGuardCardInfoBackup>
    {
        [XmlAttribute("Abutment_Id")]
        public int Abutment_Id { get; set; }

        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("State")]
        public int State { get; set; }
        
        public int CompareTo(EntranceGuardCardInfoBackup other)
        {
            return (this.Code).CompareTo(other.Code);
        }
    }
}

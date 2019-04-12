using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Tools.InitInfos
{
    [XmlType("DepartmentInfoBackupList")]
    public class DepartmentInfoBackupList
    {
        [XmlElement("DepList")]
        public List<DepartmentInfoBackup> DepList;
    }

    public class DepartmentInfoBackup : IComparable<DepartmentInfoBackup>
    {
        //Abutment_Id\ParentId\Abutment_ParentId

        [XmlAttribute("Abutment_Id")]
        public int Abutment_Id { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("ParentId")]
        public int ParentId { get; set; }
        [XmlAttribute("Abutment_ParentId")]
        public int Abutment_ParentId { get; set; }
        [XmlAttribute("ShowOrder")]
        public int ShowOrder { get; set; }
        [XmlAttribute("Type")]
        public DepartType Type { get; set; }
        [XmlAttribute("Description")]
        public string Description { get; set; }

        public int CompareTo(DepartmentInfoBackup other)
        {
            return (this.ParentId + Name).CompareTo(other.ParentId + other.Name);
        }
    }

}

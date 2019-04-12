using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Tools.InitInfos
{
    [XmlType("PersonnelInfoBackupList")]
    public class PersonnelInfoBackupList
    {
        [XmlElement("PerList")]
        public List<PersonnelInfoBackup> PerList;
    }

    public class PersonnelInfoBackup : IComparable<PersonnelInfoBackup>
    {
        [XmlAttribute("Abutment_Id")]
        public int Abutment_Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Sex")]
        public Sexs Sex { get; set; }

        [XmlAttribute("Photo")]
        public string Photo { get; set; }

        [XmlAttribute("BirthDay")]
        public DateTime BirthDay { get; set; }

        [XmlAttribute("BirthTimeStamp")]
        public long BirthTimeStamp { get; set; }

        [XmlAttribute("Nation")]
        public string Nation { get; set; }

        [XmlAttribute("Address")]
        public string Address { get; set; }

        [XmlAttribute("WorkNumber")]
        public int WorkNumber { get; set; }

        [XmlAttribute("Email")]
        public string Email { get; set; }

        [XmlAttribute("Phone")]
        public string Phone { get; set; }

        [XmlAttribute("Mobile")]
        public string Mobile { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("ParentId")]
        public int ParentId { get; set; }

        [XmlAttribute("Pst")]
        public string Pst { get; set; }
        
        public int CompareTo(PersonnelInfoBackup other)
        {
            return (this.ParentId + Name).CompareTo(other.ParentId + other.Name);
        }
    }
}

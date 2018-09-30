using Location.IModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System;
using Location.IModel.Locations;

namespace Location.Model
{
    /// <summary>
    /// 机构
    /// </summary>
    [DataContract]
    public class Department : ITreeNodeEx<Department, Personnel>, IDepartment
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name="机构名")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "排序")]
        public int ShowOrder { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [XmlIgnore]
        [Display(Name = "上级机构")]
        public virtual Department Parent { get; set; }

        [DataMember]
        [ForeignKey("ParentId")]
        public virtual List<Department> Children { get; set; }

        /// <summary>
        /// 叶子节点：人员信息
        /// </summary>
        [DataMember]
        [ForeignKey("ParentId")]
        public virtual List<Personnel> LeafNodes { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}

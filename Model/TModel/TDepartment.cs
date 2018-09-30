using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Location.TModel
{
    /// <summary>
    /// 机构
    /// </summary>
    [DataContract]
    public class TDepartment //: ITreeNode<Department>, IDepartment
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ShowOrder { get; set; }

        //[XmlIgnore]
        //public virtual TDepartment Parent { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        //[DataMember]
        //public virtual List<TDepartment> Children { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

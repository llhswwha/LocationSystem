using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Location.IModel;
using Location.TModel.ConvertCodes;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Person;
using Location.TModel.Tools;

namespace TModel.Location.Nodes
{
    /// <summary>
    /// 人员信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class PersonNode : INode
    {
        private Tag _tag;

        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string Sex { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public Department Parent { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }


        [DataMember]
        public int? TagId { get; set; }

        [DataMember]
        public virtual Tag Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                if (value != null)
                {
                    TagId = value.Id;
                }
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

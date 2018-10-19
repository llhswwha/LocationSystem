using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Location.IModel;
using TModel.Tools;

namespace TModel.Location.Nodes
{

    [DataContract]
    [Serializable]
    public class DevNode : INode
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        [DataMember]
        public string KKS { get; set; }


        /// <summary>
        /// 本地设备ID
        /// </summary>
        [DataMember]
        public string DevID { get; set; }

        /// <summary>
        /// 本地设备类型编号
        /// </summary>
        [DataMember]
        public int TypeCode { get; set; }

        private string _typeName = "";

        [DataMember]
        public string TypeName
        {
            get
            {
                if (_typeName == "")
                {
                    _typeName = DevTypeHelper.GetTypeName(TypeCode);
                }
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }
    }
}

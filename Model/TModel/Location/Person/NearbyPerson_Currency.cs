using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.Person
{
    /// <summary>
    /// 附近人员（通用）
    /// </summary>
    [DataContract][Serializable]
    public class NearbyPerson_Currency
    {
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public int? WorkNumber { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        [DataMember]
        public float? Distance { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public string DepartMent { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [DataMember]
        public string Position { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        public float X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        public float Y { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        [DataMember]
        public float Z { get; set; }


        public NearbyPerson_Currency()
        {
            id = 0;
            Name = "";
            WorkNumber = 0;
            Distance = 0;
            DepartMent = "";
            Position = "";
            X = 0;
            Y = 0;
            Z = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// 附近设备
    /// </summary>
    [DataContract]
    [Serializable]
    public class NearbyDev
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// 所属区域名称
        /// </summary>
        [DataMember]
        public string Area { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        [DataMember]
        public float Distance { get; set; }

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

        public NearbyDev()
        {
            id = 0;
            Name = "";
            TypeName = "";
            Area = "";
            Distance = 0;
            X = 0;
            Y = 0;
            Z = 0;
        }
    }

    public class DevDistanceCompare : IComparer<NearbyDev>
    {
        public int Compare(NearbyDev d1, NearbyDev d2)
        {
            return d1.Distance.CompareTo(d2.Distance);
        }
    }
}

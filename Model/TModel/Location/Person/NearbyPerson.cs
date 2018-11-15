using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.Location.Person
{
    /// <summary>
    /// 附近人员
    /// </summary>
    [DataContract][Serializable]
    public class NearbyPerson
    {
        /// <summary>
        /// 人员Id
        /// </summary>
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
        public float Distance { get; set; }

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


        public NearbyPerson()
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

        public NearbyPerson Clone()
        {
            NearbyPerson send = new NearbyPerson();
            send.id = id;
            send.Name = Name;
            send.WorkNumber = WorkNumber;
            send.Distance = Distance;
            send.DepartMent = DepartMent;
            send.Position = Position;
            send.X = X;
            send.Y = Y;
            send.Z = Z;

            return send;
        }
    }

    public class PersonDistanceCompare:IComparer<NearbyPerson>
    {
        public int Compare(NearbyPerson p1, NearbyPerson p2)
        {
            return p1.Distance.CompareTo(p2.Distance);
        }
    }
}

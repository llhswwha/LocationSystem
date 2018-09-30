using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DbModel.Location.AreaAndDev;

namespace DbModel.Tools.InitInfos
{
    /// <summary>
    /// 物体的位置，角度，大小比例信息
    /// </summary>
    [DataContract]
    public class TransformM
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double X { get; set; }

        /// <summary>
        /// 底面距离地面的高度
        /// </summary>
        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public double Z { get; set; }

        [DataMember]
        public double RX { get; set; }

        [DataMember]
        public double RY { get; set; }

        [DataMember]
        public double RZ { get; set; }

        /// <summary>
        /// X方向大小
        /// </summary>
        [DataMember]
        public double SX { get; set; }

        /// <summary>
        /// Y方向大小
        /// </summary>
        [DataMember]
        public double SY { get; set; }

        /// <summary>
        /// Z方向大小
        /// </summary>
        [DataMember]
        public double SZ { get; set; }


        [DataMember]
        public bool IsRelative { get; set; }

        /// <summary>
        /// 创建区域范围是否通过数据，还是物体自身的大小
        /// </summary>
        [DataMember]
        public bool IsCreateAreaByData { get; set; }

        /// <summary>
        /// 是否是告警区域范围
        /// </summary>
        [DataMember]
        public bool IsOnAlarmArea { get; set; }

        /// <summary>
        /// 是否是定位区域
        /// </summary>
        [DataMember]
        public bool IsOnLocationArea { get; set; }

        public TransformM()
        {
            
        }

        public TransformM(Bound bound)
        {
            IsRelative = bound.IsRelative;
            X = (bound.MinX + bound.MaxX)/2.0;
            Z = (bound.MinY + bound.MaxY)/2.0;//Z和Y调换一下
            Y = (bound.MinZ + bound.MaxZ)/2.0;

            SX = (bound.MaxX - bound.MinX);
            SZ = (bound.MaxY - bound.MinY);//Z和Y调换一下
            SY = (bound.MaxZ - bound.MinZ);
        }
    }
}

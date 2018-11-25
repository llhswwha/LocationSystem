using DbModel.Tools;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using IModel;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 位置点
    /// </summary>
    public class Point: IVector2
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        [Display(Name = "X")]
        public float X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        [Display(Name = "Y")]
        public float Y { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        [DataMember]
        [Display(Name = "Z")]
        public float Z { get; set; }

        /// <summary>
        /// Index
        /// </summary>
        [DataMember]
        [Display(Name = "Index")]
        public int Index { get; set; }

        /// <summary>
        /// 边界
        /// </summary>
        [DataMember]
        [Display(Name = "边界")]
        public int BoundId { get; set; }
        public virtual Bound Bound { get; set; }

        public Point()
        {
            
        }

        public Point(float x, float y,int index)
        {
            X = x;
            Y = y;
            Index = index;
        }

        public Point(float x, float y, float z, int index)
        {
            X = x;
            Y = y;
            Z = z;
            Index = index;
        }

        public Point(double x, double y, double z, int index)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
            Index = index;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
            Index = p.Index;
        }

        public Point Clone()
        {
            Point copy = this.CloneObjectByBinary();
            return copy;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}",X,Y);
        }
    }
}
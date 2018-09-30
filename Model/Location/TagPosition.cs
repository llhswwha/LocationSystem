using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 标签的位置信息（实时位置）
    /// </summary>
    [DataContract]
    public class TagPosition
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        [Key]
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }
        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        public double Z { get; set; }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [DataMember]
        public long Time { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [DataMember]
        public int Power { get; set; }

        /// <summary>
        /// 序号（新的卡才有的）
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// 不知道什么信息 格式是 0:0:0:0:0 或者 0:0:0:0:1。
        /// 感觉是卡不动时会发1，动时发0。可能用:分开，不同位有不同作用
        /// 补充：卡大约20秒中不动后，会发0:0:0:0:1，然后再不动大约10秒后，不发位置信息
        /// </summary>
        [DataMember]
        public string Flag { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        public List<string> Archors { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层
        /// </summary>
        [DataMember]
        public int? TopoNodes { get; set; }

        public TagPosition()
        {
            Archors = new List<string>();
        }

        public TagPosition(string tag)
        {
            this.Tag = tag;
        }

        public TagPosition(Position pos)
        {
            this.Tag = pos.Tag;
            Edit(pos);
        }

        public void Edit(Position pos)
        {
            this.X = pos.X;
            this.Y = pos.Y;
            this.Z = pos.Z;
            this.Time = pos.Time;
            this.Power = pos.Power;
            this.Number = pos.Number;
            this.Flag = pos.Flag;
            this.Archors = pos.Archors;
            this.TopoNodes = pos.TopoNodeId;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 标签的位置信息（实时位置）
    /// </summary>
    public class TagPosition
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        [Key]
        public string Tag { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// 序号（新的卡才有的）
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 不知道什么信息 格式是 0:0:0:0:0 或者 0:0:0:0:1。
        /// 感觉是卡不动时会发1，动时发0。可能用:分开，不同位有不同作用
        /// </summary>
        public string Flag { get; set; }

        public TagPosition()
        {

        }

        public TagPosition(Position pos)
        {
            this.Tag = pos.Tag;
            this.X = pos.X;
            this.Y = pos.Y;
            this.Z = pos.Z;
            this.Time = pos.Time;
            this.Power = pos.Power;
            this.Number = pos.Number;
            this.Flag = pos.Flag;
        }
    }
}

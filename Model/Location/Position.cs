using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 位置信息 (历史位置记录）
    /// </summary>
    public class Position
    {
        public int Id { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
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

        public bool Parse(string info)
        {
            try
            {
                string[] parts = info.Split(',');
                int length = parts.Length;
                if (length <= 1) return false;//心跳包回拨
                Tag = parts[0];
                X = double.Parse(parts[1]);
                Y = double.Parse(parts[2]);
                Z = double.Parse(parts[3]);
                Time = long.Parse(parts[4]);
                if (length > 4)
                    Power = int.Parse(parts[5]);
                if (length > 5)
                    Number = int.Parse(parts[6]);
                if (length > 6)
                    Flag = parts[7];
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}

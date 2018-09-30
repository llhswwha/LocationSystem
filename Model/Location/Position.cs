using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 位置信息 (历史位置记录）
    /// </summary>
    [DataContract]
    public class Position
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [DataMember]
        public int PersonnelID { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        [Required]
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public double Z { get; set; }


        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [Required]
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
        /// 基站所在的区域、建筑、楼层编号Id
        /// </summary>
        [DataMember]
        public int? TopoNodeId { get; set; }

        public Position()
        {
            //Archors = new List<string>();
        }

        public bool Parse(string info)
        {
            try
            {
                string[] parts = info.Split(',');
                int length = parts.Length;
                if (length <= 1) return false;//心跳包回拨
                Tag = parts[0];
                X = double.Parse(parts[1]);//平面位置
                Z = double.Parse(parts[2]);//平面位置
                Y = double.Parse(parts[3]);//高度位置，为了和Unity坐标信息一致，Y为高度轴
                Time = long.Parse(parts[4]);
                if (length > 5)
                    Power = int.Parse(parts[5]);
                if (length > 6)
                    Number = int.Parse(parts[6]);
                if (length > 7)
                    Flag = parts[7];
                if (length > 8)
                    Archors = parts[8].Split('@').ToList();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}

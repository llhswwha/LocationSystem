using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.ConvertCodes;
using Location.TModel.Tools;
using Base.Common.Tools;

namespace Location.TModel.LocationHistory.Data
{
    /// <summary>
    /// 位置信息 (历史位置记录）
    /// </summary>
    [DataContract]
    [Serializable]
    public class Position
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [DataMember]
        //[Display(Name = "人员ID")]
        public int? PersonnelID { get; set; }

        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        //[Display(Name = "定位卡编号")]
        //[Required]
        public string Code { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        //[Display(Name = "X")]
        public float X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        //[Display(Name = "Y")]
        public float Y { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        [DataMember]
        //[Display(Name = "Z")]
        public float Z { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        //[Display(Name = "时间")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [DataMember]
        //[Display(Name = "时间戳")]
        [ByName("DateTimeStamp")]
        public long Time { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [DataMember]
        //[Display(Name = "电量")]
        public int Power { get; set; }

        /// <summary>
        /// 电量状态,0表示正常，1表示弱电
        /// </summary>
        //[Display(Name = "电量状态")]
        [DataMember]
        public int PowerState { get; set; }

        /// <summary>
        /// 区域状态，0:在定位区域，1:不在定位区域
        /// </summary>
        //[Display(Name = "区域状态")]
        [DataMember]
        public int AreaState { get; set; }

        /// <summary>
        /// 运动状态，0:运动，1:待机状态，2:静止状态
        /// </summary>
        //[Display(Name = "区域状态")]
        [DataMember]
        public int MoveState { get; set; }

        /// <summary>
        /// 序号（新的卡才有的）
        /// </summary>
        [DataMember]
        //[Display(Name = "序号")]
        public int Number { get; set; }

        /// <summary>
        /// 不知道什么信息 格式是 0:0:0:0:0 或者 0:0:0:0:1。
        /// 感觉是卡不动时会发1，动时发0。可能用:分开，不同位有不同作用
        /// 补充：卡大约20秒中不动后，会发0:0:0:0:1，然后再不动大约10秒后，不发位置信息
        /// </summary>
        [DataMember]
        //[Display(Name = "信息")]
        public string Flag { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        //[Display(Name = "参与计算的基站编号")]
        public List<string> Archors { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        public string ArchorsText { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层编号Id
        /// </summary>
        [DataMember]
        //[Display(Name = "基站所在的区域、建筑、楼层编号Id")]
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
                Code = parts[0];
                X = float.Parse(parts[1]);//平面位置
                Z = float.Parse(parts[2]);//平面位置
                Y = float.Parse(parts[3]);//高度位置，为了和Unity坐标信息一致，Y为高度轴
                Time = long.Parse(parts[4]);
                DateTime = TimeConvert.ToDateTime(Time / 1000);

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
                LogEvent.Info(ex.ToString());
                return false;
            }
        }

        public override string ToString()
        {
            return Code;
        }

        public Position Clone()
        {
            Position copy = new Position();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }


    [DataContract]
    [Serializable]
    public class PositionList
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Count { get; set; }

        //[DataMember]
        //public List<Position> Items { get; set; }
    }

    [DataContract]
    [Serializable]
    public class Pos
    {
        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        //[Display(Name = "定位卡编号")]
        //[Required]
        public string Code { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        //[Display(Name = "X")]
        public float X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [DataMember]
        //[Display(Name = "Y")]
        public float Y { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        [DataMember]
        //[Display(Name = "Z")]
        public float Z { get; set; }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [DataMember]
        //[Display(Name = "时间戳")]
        [ByName("DateTimeStamp")]
        public long Time { get; set; }
    }
}

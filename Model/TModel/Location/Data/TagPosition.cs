using System.Collections.Generic;
using System.Runtime.Serialization;
using Location.TModel.ConvertCodes;
using System;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;

namespace Location.TModel.Location.Data
{
    /// <summary>
    /// 标签的位置信息（实时位置）
    /// </summary>
    [ByName("LocationCardPosition")]
    [DataContract] [Serializable]
    public class TagPosition
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        //[Key]
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public float X { get; set; }

        [DataMember]
        public float Y { get; set; }
        /// <summary>
        /// X
        /// </summary>
        [DataMember]
        public float Z { get; set; }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [DataMember]
        public long Time { get; set; }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [DataMember]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [DataMember]
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
        public int? AreaId { get; set; }

        ///// <summary>
        ///// 基站所在的区域、建筑、楼层
        ///// </summary>
        //[DataMember]
        //public string AreaPath { get; set; }

        /// <summary>
        /// 关联人员的Id
        /// </summary>
        [DataMember]
        public int? PersonId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [DataMember]
        public Tag TagEntity { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        [DataMember]
        public Personnel Person { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [DataMember]
        public PhysicalTopology Area { get; set; }

        public TagPosition()
        {
            Archors = new List<string>();
        }

        public TagPosition(string tag)
        {
            this.Tag = tag;
        }

        //public TagPosition(Position pos)
        //{
        //    this.Tag = pos.Tag;
        //    Edit(pos);
        //}

        //public void Edit(Position pos)
        //{
        //    this.X = pos.X;
        //    this.Y = pos.Y;
        //    this.Z = pos.Z;
        //    this.Time = pos.Time;
        //    this.Power = pos.Power;
        //    this.Number = pos.Number;
        //    this.Flag = pos.Flag;
        //    this.Archors = pos.Archors;
        //    this.TopoNodes = pos.TopoNodeId;
        //}

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", X, Y, Z);
        }

    }
}

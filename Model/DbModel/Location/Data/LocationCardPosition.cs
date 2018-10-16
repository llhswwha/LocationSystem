﻿using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.Location.Data
{
    /// <summary>
    /// 定位卡的位置信息（实时位置）
    /// </summary>
    [DataContract]
    public class LocationCardPosition
    {
        private DateTime _dateTime;

        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡编号")]
        [Key]
        public string Code { get; set; }

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
        /// 时间
        /// </summary>
        [DataMember]
        [Display(Name = "时间")]
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value;
                DateTimeStamp = TimeConvert.DateTimeToTimeStamp(value);
            }
        }

        /// <summary>
        /// 时间戳（毫秒）
        /// </summary>
        [DataMember]
        [Display(Name = "时间戳")]
        public long DateTimeStamp { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [DataMember]
        [Display(Name = "电量")]
        public int Power { get; set; }

        /// <summary>
        /// 序号（新的卡才有的）
        /// </summary>
        [DataMember]
        [Display(Name = "序号")]
        public int Number { get; set; }

        /// <summary>
        /// 不知道什么信息 格式是 0:0:0:0:0 或者 0:0:0:0:1。
        /// 感觉是卡不动时会发1，动时发0。可能用:分开，不同位有不同作用
        /// 补充：卡大约20秒中不动后，会发0:0:0:0:1，然后再不动大约10秒后，不发位置信息
        /// </summary>
        [DataMember]
        [Display(Name = "信息")]
        public string Flag { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        [Display(Name = "参与计算的基站编号")]
        public List<string> Archors { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层
        /// </summary>
        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层")]
        public int? AreaId { get; set; }

        /// <summary>
        /// 关联人员的Id
        /// </summary>
        [DataMember]
        [Display(Name = "关联人员")]
        public int? PersonId { get; set; }

        public LocationCardPosition()
        {
            Archors = new List<string>();
        }

        public LocationCardPosition(string code)
        {
            this.Code = code;
        }

        public LocationCardPosition(Position pos)
        {
            this.Code = pos.Code;
            Edit(pos);
        }

        public void Edit(Position pos)
        {
            this.X = pos.X;
            this.Y = pos.Y;
            this.Z = pos.Z;
            this.DateTimeStamp = pos.DateTimeStamp;
            this.DateTime = pos.DateTime;
            this.Power = pos.Power;
            this.Number = pos.Number;
            this.Flag = pos.Flag;
            this.Archors = pos.Archors;
            this.AreaId = pos.TopoNodeId;
        }

        public LocationCardPosition Clone()
        {
            LocationCardPosition copy = this.CloneObjectByBinary();
            return copy;
        }

    }
}

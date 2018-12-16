using DbModel.LocationHistory.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.Data
{
    /// <summary>
    /// 定位卡的位置信息（实时位置）
    /// </summary>
    [DataContract]
    public class LocationCardPosition:IId<string>
    {
        private DateTime _dateTime;

        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡编号")]
        [MaxLength(32)]
        [Column("Code")]
        [Key]
        public string Id { get; set; }

        [DataMember]
        [Display(Name = "标签卡Id")]
        public int? CardId { get; set; }

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
        /// 电量状态,0表示正常，1表示弱电
        /// </summary>
        [Display(Name = "电量状态")]
        public int PowerState { get; set; }

        /// <summary>
        /// 区域状态，0:在定位区域，1:不在定位区域
        /// </summary>
        [Display(Name = "区域状态")]
        public int AreaState { get; set; }

        /// <summary>
        /// 运动状态，0:运动，1:待机状态，2:静止状态，3.长时间不动
        /// </summary>
        [Display(Name = "区域状态")]
        public int MoveState { get; set; }

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
        [MaxLength(16)]
        public string Flag { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        [Display(Name = "参与计算的基站编号")]
        [NotMapped]
        public List<string> Archors { get; set; }

        /// <summary>
        /// 参与计算的基站编号
        /// </summary>
        [DataMember]
        [Display(Name = "参与计算的基站编号")]
        [MaxLength(128)]
        public string ArchorsText { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层
        /// </summary>
        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层")]

        public int? AreaId { get; set; }

        /// <summary>
        /// 关联的人员
        /// </summary>
        [DataMember]
        [Display(Name = "关联的人员")]
        public int? PersonId { get; set; }

        public LocationCardPosition()
        {
            Archors = new List<string>();
        }

        public LocationCardPosition(string code)
        {
            DateTime = DateTime.Now;
            this.Id = code;
        }

        public LocationCardPosition(Position pos)
        {
            this.Id = pos.Code;
            Edit(pos);
        }

        public void Edit(Position pos)
        {
            this.CardId = pos.CardId;
            this.X = pos.X;
            this.Y = pos.Y;
            this.Z = pos.Z;
            this.DateTimeStamp = pos.DateTimeStamp;
            this.DateTime = pos.DateTime;
            this.Power = pos.Power;
            this.Number = pos.Number;
            this.Flag = pos.Flag;
            this.Archors = pos.Archors;
            this.AreaId = pos.AreaId;
            this.AreaState = pos.AreaState;
            this.PowerState = pos.PowerState;
            this.MoveState = pos.MoveState;
            this.ArchorsText = pos.ArchorsText;
            //this.AreaPath = pos.AreaPath;
        }

        public void Edit(LocationCardPosition pos)
        {
            this.CardId = pos.CardId;
            this.X = pos.X;
            this.Y = pos.Y;
            this.Z = pos.Z;
            this.DateTimeStamp = pos.DateTimeStamp;
            this.DateTime = pos.DateTime;
            this.Power = pos.Power;
            this.Number = pos.Number;
            this.Flag = pos.Flag;
            this.Archors = pos.Archors;
            this.AreaId = pos.AreaId;
            this.AreaState = pos.AreaState;
            this.PowerState = pos.PowerState;
            this.MoveState = pos.MoveState;
            this.ArchorsText = pos.ArchorsText;
            //this.AreaPath = pos.AreaPath;
        }

        public LocationCardPosition Clone()
        {
            LocationCardPosition copy = this.CloneObjectByBinary();
            return copy;
        }

    }
}

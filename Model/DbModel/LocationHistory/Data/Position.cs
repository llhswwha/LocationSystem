using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using DbModel.Location.Data;
using DbModel.Location.Settings;
using Location.TModel.Tools;

namespace DbModel.LocationHistory.Data
{
    /// <summary>
    /// 位置信息 (历史位置记录）
    /// </summary>
    [DataContract]
    public class Position
    {
        private DateTime _dateTime;

        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "标签卡Id")]
        public int? CardId { get; set; }

        [DataMember]
        [Display(Name = "标签角色Id")]
        public int? RoleId { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [DataMember]
        [Display(Name = "人员ID")]
        public int? PersonnelID { get; set; }

        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡编号")]
        [Required]
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
        /// 电量状态,0表示正常，1表示弱电
        /// </summary>
        [Display(Name = "电量状态")]
        public int PowerState { get; set; }

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
        [NotMapped]
        public List<string> Archors { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层编号Id
        /// </summary>
        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层编号Id")]
        public int? AreaId { get; set; }

        [DataMember]
        public string AreaPath { get; set; }

        /// <summary>
        /// 模拟数据
        /// </summary>
        [NotMapped]
        public bool IsSimulate { get; set; }

        public Position()
        {
            //Archors = new List<string>();
        }

        public void SetTime()
        {
            DateTime now = DateTime.Now;
            DateTimeStamp = TimeConvert.DateTimeToTimeStamp(now);
            DateTime now2 = TimeConvert.TimeStampToDateTime(DateTimeStamp);
        }

        public void AddArchor(string archor)
        {
            if (Archors == null)
            {
                Archors = new List<string>();
            }
            if (string.IsNullOrEmpty(archor)) return;
            Archors.Add(archor);
        }

        public string _info;

        public bool Parse(string info)
        {
            try
            {
                _info = info;
                string[] parts = info.Split(new[] { ',' });
                int length = parts.Length;
                if (length <= 1) return false;//心跳包回拨
                Code = parts[0];
                X = float.Parse(parts[1]);//平面位置
                Z = float.Parse(parts[2]);//平面位置
                Y = float.Parse(parts[3]);//高度位置，为了和Unity坐标信息一致，Y为高度轴
                DateTimeStamp = long.Parse(parts[4]);
                DateTime = TimeConvert.TimeStampToDateTime(DateTimeStamp);
                long DateTimeStamp2 = TimeConvert.DateTimeToTimeStamp(DateTime);

                if (length > 5)
                    Power = int.Parse(parts[5]);
                if (length > 6)
                    Number = int.Parse(parts[6]);
                if (length > 7)
                    Flag = parts[7];
                if (length > 8)
                {
                    string archors = parts[8];
                    Archors = archors.Split(new [] { '@'},StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (Archors.Count > 1)
                    {
                        Console.Write("Archors.Count > 1");
                    }
                    IsSimulate = archors == "@0000" || string.IsNullOrEmpty(archors);
                }

                if (Power >= 400)
                {
                    PowerState = 0;
                }
                else
                {
                    PowerState = 1;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public string GetText()
        {
            string archors = "";
            if (Archors != null)
            {
                archors = string.Join(",", Archors.ToArray());
            }
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Code, X, Y, Z, DateTimeStamp, Power, Number,
                Flag, archors);
        }

        public override string ToString()
        {
            return Code;
        }

        public Position Clone()
        {
            Position copy = this.CloneObjectByBinary();
            return copy;
        }

        public void SetProperty(LocationCardPosition pos)
        {
            Code = pos.Code;
            X = pos.X;
            Y = pos.Z;
            Z = pos.Y;
            Power = pos.Power;
            Number = pos.Number;
            Flag = pos.Flag;
            Archors = pos.Archors;
        }
    }
}

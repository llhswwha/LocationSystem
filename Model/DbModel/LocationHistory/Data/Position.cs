using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.Location.Settings;
using Location.IModel;
using Location.TModel.Tools;

namespace DbModel.LocationHistory.Data
{
    /// <summary>
    /// 位置信息 (历史位置记录）
    /// </summary>
    [DataContract]
    public class Position : IId
    {
        private DateTime _dateTime;

        /// <summary>
        /// 主键Id
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        [Index]
        [DataMember]
        [Display(Name = "标签卡Id")]
        public int? CardId { get; set; }

        [DataMember]
        [Display(Name = "标签角色Id")]
        public int? RoleId { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [Index]
        [DataMember]
        [Display(Name = "人员ID")]
        public int? PersonnelID { get; set; }

        /// <summary>
        /// 定位卡编号
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡编号")]
        [MaxLength(16)]
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
        public DateTime DateTime { get; set; }
        //{
        //    get { return _dateTime; }
        //    set
        //    {
        //        _dateTime = value;
        //        DateTimeStamp = TimeConvert.DateTimeToTimeStamp(value);
        //    }
        //}

        ///// <summary>
        ///// 时间戳（毫秒）
        ///// </summary>
        [Key, Column(Order = 2)]
        [Index]
        [DataMember]
        [Display(Name = "时间戳")]
        public long DateTimeStamp { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [DataMember]
        [Display(Name = "电量")]
        public int Power { get; set; }


        public Area SetArea(params Area[] areas)
        {
            //if (ToString() == "0918")
            //{

            //}
            Area areaNode = null;
            if (areas == null)
            {
                AllAreaId = null;
                AreaPath = "";
                AreaState = 1;
                return areaNode;
            }
            Areas = areas;
            //AreaId = areas.Id;
            //AreaPath = area.Name;
            //AreaState = area.IsOnLocationArea ? 0 : 1;
            AreaState = 1;
            AllAreaId = "";
            AreaPath = "";
            foreach (var area in Areas)//同时处于一个告警区域和一个定位区域时 人员区域怎么判断？ 同时处于两个区域时 人员区域怎么判断
            {
                if (area.IsOnLocationArea)
                {
                    AreaState = 0;
                }
                if (area.Type != Tools.AreaTypes.范围)
                {
                    AreaId = area.Id;
                    areaNode = area;
                }
                AllAreaId += area.Id + ";";
                AreaPath = area.Name + ";";
            }
            return areaNode;
        }

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
        /// 运动状态，0:运动，1:待机状态，2:静止状态
        /// </summary>
        [Display(Name = "区域状态")]
        public int MoveState { get; set; }

        /// <summary>
        /// 
        /// 
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
        [MaxLength(64)]
        public string ArchorsText { get; set; }

        /// <summary>
        /// 基站所在的区域、建筑、楼层编号Id
        /// </summary>
        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层编号Id")]
        public string AllAreaId { get; set; }

        [DataMember]
        [Display(Name = "基站所在的区域、建筑、楼层编号Id")]
        public int? AreaId { get; set; }

        public bool IsAreaNull()
        {
            //return (AreaId == null || AreaId == 0);
            return (AllAreaId == null || AllAreaId == "");
        }

        public bool IsInArea(int areaId)
        {
            if (Areas != null)
            {
                foreach (var item in Areas)
                {
                    if (item.Id == areaId)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(AllAreaId)) return false;
                string[] parts = AllAreaId.Split(';');
                foreach (var item in parts)
                {
                    if(item== areaId + "")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsInArea(List<int> areaIds)
        {
            foreach (var areaId in areaIds)
            {
                if (Areas != null)
                {
                    foreach (var item in Areas)
                    {
                        if (item.Id == areaId)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    string[] parts = AllAreaId.Split(';');
                    foreach (var item in parts)
                    {
                        if (item == areaId + "")
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 基站所在的区域
        /// </summary>
        [NotMapped]
        public Area[] Areas { get; set; }


        [DataMember]
        [MaxLength(64)]
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

        /// <summary>
        /// 解析位置信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="offsetX">偏移量X 和定位引擎约定好具体偏移数值</param>
        /// <param name="offsetY">偏移量Y 和定位引擎约定好具体偏移数值</param>
        /// <returns></returns>
        public bool Parse(string info,float offsetX, float offsetY)
        {
            try
            {
                _info = info;
                string[] parts = info.Split(new[] { ',' });
                int length = parts.Length;
                if (length <= 1) return false;//心跳包回拨
                Code = parts[0];
                if (Code.StartsWith("1"))
                {

                }
                float x = float.Parse(parts[1]);
                float y = float.Parse(parts[2]);
                if (x < offsetX)
                {
                    X = x + offsetX; //平面位置
                    Z = y + offsetY; //平面位置
                }
                else //模拟数据是可以没有偏移量的 看模拟程序的版本
                {
                    X = x;
                    Z = y;
                }
               
                Y = float.Parse(parts[3]);//高度位置，为了和Unity坐标信息一致，Y为高度轴
                DateTimeStamp = long.Parse(parts[4]);
                DateTime = TimeConvert.TimeStampToDateTime(DateTimeStamp);
                TimeSpan time1 = DateTime.Now - DateTime;
                long DateTimeStamp2 = TimeConvert.DateTimeToTimeStamp(DateTime);

                if (length > 5)
                    Power = int.Parse(parts[5]);
                if (length > 6)
                    Number = int.Parse(parts[6]);
                if (length > 7)
                    Flag = parts[7];
                if (length > 8)
                {
                    ArchorsText = parts[8];
                    Archors = ArchorsText.Split(new [] { '@'},StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (Archors.Count > 1)
                    {
                        Console.Write("Archors.Count > 1");
                    }
                    IsSimulate = ArchorsText == "@0000" || string.IsNullOrEmpty(ArchorsText);
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
            Code = pos.Id;
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

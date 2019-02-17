using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Location.TModel.Tools;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;
using DbModel.LocationHistory.Data;
using Location.IModel;
using System.ComponentModel.DataAnnotations.Schema;
using DbModel.Location.Person;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 具体权限分配记录
    /// </summary>
    public class AreaAuthorizationRecord:IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [MaxLength(128)]
        [Required]
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        [Display(Name = "描述")]
        [MaxLength(128)]
        [XmlAttribute]
        public string Description { get; set; }

        /// <summary>
        /// 0 表示按时间长度设置权限，1 表示按时间点范围设置权限
        /// </summary>
        [DataMember]
        [Display(Name = "种类")]
        [XmlAttribute]
        public TimeSettingType TimeType { get; set; }

        /// <summary>
        /// 权限起始时间点
        /// </summary>
        [DataMember]
        [Display(Name = "起始时间点")]
        [DataType(DataType.Time)]
        [XmlAttribute]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 权限结束时间点
        /// </summary>
        [DataMember]
        [Display(Name = "结束时间点")]
        [DataType(DataType.Time)]
        [XmlAttribute]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 权限时长,单位是 分钟
        /// </summary>
        [DataMember]
        [Display(Name = "时长")]
        [XmlAttribute]
        public int TimeSpan { get; set; }

        public void SetTimeSpane()
        {
            if (StartTime == null) return;
            if (EndTime == null) return;
            DateTime start = (DateTime)StartTime;
            DateTime end = (DateTime)EndTime;
            var span = end - start;
            TimeSpan = (int)span.TotalMinutes;
        }

        /// <summary>
        /// 延迟时间，单位是 分钟
        /// </summary>
        [DataMember]
        [Display(Name = "延迟时间")]
        [XmlAttribute]
        public int DelayTime { get; set; }

        /// <summary>
        /// 误差距离，单位是 米
        /// </summary>
        [DataMember]
        [Display(Name = "误差距离")]
        [XmlAttribute]
        public int ErrorDistance { get; set; }

        /// <summary>
        /// 重复天数
        /// </summary>
        [DataMember]
        [Display(Name = "重复天数")]
        [Required]
        [XmlAttribute]
        public RepeatDay RepeatDay { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [DataMember]
        [Display(Name = "区域")]
        [XmlAttribute]
        public int AreaId { get; set; }

        //[NotMapped]
        [XmlIgnore]
        public virtual Area Area { get; set; }

        /// <summary>
        /// 进出类型，一般使用 AreaAccessType.EnterLeave ，只有特定的危险区域用 AreaAccessType.Leave
        /// </summary>
        [Display(Name = "进出类型")]
        [XmlAttribute]
        public AreaAccessType AccessType { get; set; }

        /// <summary>
        /// 当前只用到 AreaRangeType.Single
        /// </summary>
        [XmlAttribute]
        public AreaRangeType RangeType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Display(Name = "创建时间")]
        [XmlIgnore]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        [Display(Name = "修改时间")]
        [XmlIgnore]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataMember]
        [Display(Name = "删除时间")]
        [XmlIgnore]
        public DateTime? DeleteTime { get; set; }

        [DataMember]
        [Display(Name = "标签角色")]
        [XmlAttribute]
        public int CardRoleId { get; set; }     

        public virtual CardRole CardRole { get; set; }

        [DataMember]
        [Display(Name = "原权限Id")]
        [XmlAttribute]
        public int AuthorizationId { get; set; }

        public AreaAuthorizationRecord()
        {

        }

        public AreaAuthorizationRecord(AreaAuthorization aa,CardRole role)
        {
            this.Name = aa.Name;
            this.Description = aa.Description;
            this.TimeType = aa.TimeType;
            this.StartTime = aa.StartTime;
            this.EndTime = aa.EndTime;
            this.TimeSpan = aa.TimeSpan;
            this.DelayTime = aa.DelayTime;
            this.ErrorDistance = aa.ErrorDistance;
            this.RepeatDay = aa.RepeatDay;
            this.AreaId = aa.AreaId;
            //this.Area = aa.Area;
            this.AccessType = aa.AccessType;
            this.RangeType = aa.RangeType;
            this.CreateTime = DateTime.Now;
            this.ModifyTime = DateTime.Now;
            if (role != null)
            {
                this.CardRoleId = role.Id;
            }
            this.AuthorizationId = aa.Id;
        }

        public AreaAuthorizationRecord Clone()
        {
            AreaAuthorizationRecord copy = new AreaAuthorizationRecord();
            copy = this.CloneObjectByBinary();

            return copy;
        }

        public bool IsValid(Position p)
        {
            //1.角色和区域已经在外面过滤过了，这里假设角色和区域相同
            //2.时间是否在有效范围内
            //3.AccessType
            //4.RangeType
            return true;
        }

        public bool IsTimeValid(DateTime dtBegin, DateTime dtEnd, int nTimeStamp)
        {
            bool bReturn = false;

            if (TimeType == TimeSettingType.时间长度)
            {
                if (nTimeStamp <= (TimeSpan + DelayTime))
                {
                    bReturn = true;
                }
            }
            else
            {
                dtBegin = dtBegin.AddMinutes(DelayTime);
                dtEnd = dtEnd.AddMinutes(-DelayTime);
                int nStart = Convert.ToInt32(StartTime.ToString("HHmmss"));
                int nEnd = Convert.ToInt32(EndTime.ToString("HHmmss"));
                int nBegin = Convert.ToInt32(dtBegin.ToString("HHmmss"));
                int nEnd2 = Convert.ToInt32(dtEnd.ToString("HHmmss"));

                if (nBegin >= nStart && nEnd2 <= nEnd)
                {
                    bReturn = true;
                }
            }
            
            return bReturn;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Name, AreaId, CardRoleId);
        }
    }
}

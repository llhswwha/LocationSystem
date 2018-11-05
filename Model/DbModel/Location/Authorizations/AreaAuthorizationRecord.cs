using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Authorizations;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 具体权限分配记录
    /// </summary>
    public class AreaAuthorizationRecord
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        [Display(Name = "描述")]
        public string Description { get; set; }

        /// <summary>
        /// 0 表示按时间长度设置权限，1 表示按时间点范围设置权限
        /// </summary>
        [DataMember]
        [Display(Name = "种类")]
        public TimeSettingType TimeType { get; set; }

        /// <summary>
        /// 权限起始时间点
        /// </summary>
        [DataMember]
        [Display(Name = "起始时间点")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 权限结束时间点
        /// </summary>
        [DataMember]
        [Display(Name = "结束时间点")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 权限时长,单位是 分钟
        /// </summary>
        [DataMember]
        [Display(Name = "时长")]
        public int? TimeSpan { get; set; }

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
        public int DelayTime { get; set; }

        /// <summary>
        /// 误差距离，单位是 米
        /// </summary>
        [DataMember]
        [Display(Name = "误差距离")]
        public int ErrorDistance { get; set; }

        /// <summary>
        /// 重复天数
        /// </summary>
        [DataMember]
        [Display(Name = "重复天数")]
        [Required]
        public RepeatDay RepeatDay { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [DataMember]
        [Display(Name = "区域")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        public AreaAccessType AccessType { get; set; }

        public AreaRangeType RangeType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        [Display(Name = "修改时间")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataMember]
        [Display(Name = "删除时间")]
        public DateTime? DeleteTime { get; set; }

        [DataMember]
        [Display(Name = "标签角色")]
        public virtual CardRole CardRole { get; set; }

        [DataMember]
        [Display(Name = "标签角色")]
        public int? CardRoleId { get; set; }

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
            this.Area = aa.Area;
            this.AccessType = aa.AccessType;
            this.RangeType = aa.RangeType;
            this.CreateTime = DateTime.Now;
            this.ModifyTime = DateTime.Now;
            if (role != null)
            {
                this.CardRole = role;
                this.CardRoleId = role.Id;
            }
        }

        public AreaAuthorizationRecord Clone()
        {
            AreaAuthorizationRecord copy = new AreaAuthorizationRecord();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}

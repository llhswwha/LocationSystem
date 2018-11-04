using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.Location.Work
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AreaAuthorization
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
            DateTime end = (DateTime) EndTime;
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

        public AreaAuthorization Clone()
        {
            AreaAuthorization copy = new AreaAuthorization();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }

    public enum RepeatDay
    {
        All=0,Mon=1,Tues=2,Wed=4,Thur=8,Fri=16,Sat=32,Sun=64,
    }

    public enum AreaRangeType
    {
        All,//特殊，全部区域
        WithParent,//从根节点到自身节点
        Single,//只有自身   
        WithChildren,//自身和子节点（递归下去）
        AllRelative,//父节点、自身、子节点（递归下去）
    }

    /// <summary>
    /// 进出类型
    /// </summary>
    public enum AreaAccessType
    {
        EnterLeave,//可以进出
        Enter,//可以进入,不能出去
        Leave,//可以出去,不能进去
        None,//不能进去不能出去
    }
}

using DbModel.Tools;
using System;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.TModel.Location.AreaAndDev;

namespace TModel.Location.Work
{
    /// <summary>
    /// 权限
    /// </summary>
    public class JurisDiction
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        //[Display(Name = "名称")]
        //[Required]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        //[Display(Name = "描述")]
        public string Describe { get; set; }

        /// <summary>
        /// 0 表示按时间长度设置权限，1 表示按时间点范围设置权限
        /// </summary>
        [DataMember]
        //[Display(Name = "权限种类")]
        public TimeSettingType nFlag { get; set; }

        /// <summary>
        /// 权限起始时间点
        /// </summary>
        [DataMember]
        //[Display(Name = "权限起始时间点")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 权限结束时间点
        /// </summary>
        [DataMember]
        //[Display(Name = "权限结束时间点")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 权限时长,单位是 分钟
        /// </summary>
        [DataMember]
        //[Display(Name = "权限时长")]
        public int? nTimeLength { get; set; }

        /// <summary>
        /// 延迟时间，单位是 分钟
        /// </summary>
        [DataMember]
        //[Display(Name = "延迟时间")]
        public int DelayTime { get; set; }

        /// <summary>
        /// 误差距离，单位是 米
        /// </summary>
        [DataMember]
        //[Display(Name = "误差距离")]
        public int ErrorDistance { get; set; }

        /// <summary>
        /// 重复天数
        /// </summary>
        [DataMember]
        //[Display(Name = "重复天数")]
        //[Required]
        public string RepeatType { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [DataMember]
        //[Display(Name = "区域")]
        public int AreaId { get; set; }
        public PhysicalTopology Area { get; set; }

        /// <summary>
        /// 定位卡
        /// </summary>
        [DataMember]
        //[Display(Name = "定位卡")]
        public int LocationCardId { get; set; }
        public Tag LocationCard { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        //[Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        //[Display(Name = "修改时间")]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataMember]
        //[Display(Name = "删除时间")]
        public DateTime? DeleteTime { get; set; }

        public JurisDiction Clone()
        {
            JurisDiction copy = new JurisDiction();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}

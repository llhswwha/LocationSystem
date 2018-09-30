using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.LocationHistory.Alarm
{
    /// <summary>
    /// 设备历史告警
    /// </summary>
    public class DevAlarmHistory
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        [Display(Name = "内容")]
        public string Msg { get; set; }

        /// <summary>
        /// 告警级别(字典：报警事件级别,0 未定，1低，2中，3高)
        /// </summary>
        [DataMember]
        [Display(Name = "告警级别")]
        public Abutment_DevAlarmLevel? Level { get; set; }

        /// <summary>
        /// 代码(各系统上报的原始编码)
        /// </summary>
        [DataMember]
        [Display(Name = "代码")]
        public string Code { get; set; }

        /// <summary>
        /// 来源(字典：报警事件来源，0 未知，1 视频监控，2 门禁，3消防，11 SIS，12人员定位)
        /// </summary>
        [DataMember]
        [Display(Name = "来源")]
        public Abutment_DevAlarmSrc Src { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [DataMember]
        [Display(Name = "设备")]
        public int DevInfoId { get; set; }

        /// <summary>
        /// 设备说明
        /// </summary>
        [DataMember]
        [Display(Name = "设备说明")]
        public string Device_desc { get; set; }

        /// <summary>
        /// 设备告警产生时间
        /// </summary>
        [DataMember]
        [Display(Name = "设备告警产生时间")]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 告警时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "告警时间戳")]
        public long AlarmTimeStamp { get; set; }

        /// <summary>
        /// 历史记录产生时间
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录产生时间")]
        public DateTime HistoryTime { get; set; }

        /// <summary>
        /// 历史记录时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录时间戳")]
        public long HistoryTimeStamp { get; set; }


        public DevAlarmHistory Clone()
        {
            return this.CloneObjectByBinary();
        }
    }
}

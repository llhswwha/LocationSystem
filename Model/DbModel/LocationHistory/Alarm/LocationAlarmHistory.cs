using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;
using DbModel.Location.Alarm;

namespace DbModel.LocationHistory.Alarm
{
    /// <summary>
    /// 定位告警历史数据
    /// </summary>
    public class LocationAlarmHistory:IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [Key]
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember]
        [MaxLength(40)]
        public string AlarmId { get; set; }

        /// <summary>
        /// 告警类型：0:区域告警，1:消失告警，2:低电告警，3:传感器告警，4:重启告警，5:非法拆卸
        /// </summary>
        [DataMember]
        [Display(Name = "告警类型")]
        public LocationAlarmType AlarmType { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        [DataMember]
        [Display(Name = "告警等级")]
        public LocationAlarmLevel AlarmLevel { get; set; }

        /// <summary>
        /// 告警定位卡
        /// </summary>
        [DataMember]
        [Display(Name = "告警定位卡")]
        public int LocationCardId { get; set; }
     
        /// <summary>
        /// 告警人员
        /// </summary>
        [DataMember]
        [Display(Name = "告警人员")]
        public int PersonnelId { get; set; }

        /// <summary>
        /// 定位卡角色
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡角色")]
        public int CardRoleId { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember]
        [Display(Name = "告警")]
        public int? AreadId { get; set; }

        /// <summary>
        /// 告警规则
        /// </summary>
        [DataMember]
        [Display(Name = "告警规则")]
        public int? AuzId { get; set; }

        [DataMember]
        [MaxLength(100)]
        public string AllAuzId { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        [DataMember]
        [Display(Name = "告警内容")]
        [MaxLength(512)]
        public string Content { get; set; }

        /// <summary>
        /// 告警时间
        /// </summary>
        [DataMember]
        [Display(Name = "告警时间")]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 告警时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "时间戳")]
        public long AlarmTimeStamp { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        [Display(Name = "处理时间")]
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// 处理时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "处理时间戳")]
        public long HandleTimeStamp { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        [Display(Name = "处理人")]
        [MaxLength(128)]
        public string Handler { get; set; }

        /// <summary>
        /// 处理类型：误报，忽略，确认
        /// </summary>
        [DataMember]
        [Display(Name = "处理类型")]
        public LocationAlarmHandleType HandleType { get; set; }

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

        public LocationAlarmHistory Clone()
        {
            return this.CloneObjectByBinary();
        }

        public LocationAlarm ConvertToAlarm()
        {
            LocationAlarm history = new LocationAlarm();
            //history.Id = this.Id;
            history.AlarmId = this.AlarmId;
            history.AlarmType = this.AlarmType;
            history.AlarmLevel = this.AlarmLevel;
            history.LocationCardId = this.LocationCardId;
            history.PersonnelId = this.PersonnelId;
            history.AreaId = this.AreadId;
            history.CardRoleId = this.CardRoleId;
            history.Content = this.Content;
            history.AlarmTime = this.AlarmTime;
            history.AlarmTimeStamp = this.AlarmTimeStamp;
            history.HandleTime = this.HandleTime;
            history.HandleTimeStamp = this.HandleTimeStamp;
            history.Handler = this.Handler;
            history.HandleType = this.HandleType;
            history.AuzId = this.AuzId;
            history.AllAuzId = this.AllAuzId;
            //history.HistoryTime = DateTime.Now;
            //history.HistoryTimeStamp = TimeConvert.ToStamp(history.HistoryTime);

            return history;
        }
    }
}

using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Alarm;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DbModel.LocationHistory.Data;
using Location.TModel.Tools;

namespace DbModel.Location.Alarm
{
    /// <summary>
    /// 定位告警
    /// </summary>
    [DataContract]
    public class LocationAlarm
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

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
        public int? LocationCardId { get; set; }
        [DataMember]
        public virtual LocationCard LocationCard { get; set; }

        /// <summary>
        /// 告警人员
        /// </summary>
        [DataMember]
        [Display(Name = "告警人员")]
        public int? PersonnelId { get; set; }
        [DataMember]
        public virtual Personnel Personnel { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember]
        [Display(Name = "告警")]
        public int? AreadId  { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        [DataMember]
        [Display(Name = "告警内容")]
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
        public string Handler { get; set; }

        /// <summary>
        /// 处理类型：误报，忽略，确认
        /// </summary>
        [DataMember]
        [Display(Name = "处理类型")]
        public LocationAlarmHandleType HandleType { get; set; }

        public LocationAlarm()
        {
            SetTime();
        }

        private void SetTime()
        {
            AlarmTime = DateTime.Now;
            AlarmTimeStamp = TimeConvert.DateTimeToTimeStamp(AlarmTime);
        }

        public LocationAlarm(Position p,string content)
        {
            SetTime();
            AlarmType = LocationAlarmType.区域告警;
            AlarmLevel = LocationAlarmLevel.四级告警;
            LocationCardId = p.CardId;
            PersonnelId = p.PersonnelID;
            AreadId = p.AreaId;
            Content = content;
        }

        public LocationAlarm SetPerson(LocationCardToPersonnel p)
        {
            LocationCardId = p.LocationCardId;
            LocationCard = p.LocationCard;
            PersonnelId = p.PersonnelId;
            Personnel = p.Personnel;

            return this;
        }

        public LocationAlarm Clone()
        {
            LocationAlarm copy = new LocationAlarm();
            copy = this.CloneObjectByBinary();
            if (this.LocationCard != null)
            {
                copy.LocationCard = this.LocationCard;
            }

            if (this.Personnel != null)
            {
                copy.Personnel = this.Personnel;
            }

            return copy;
        }

        public LocationAlarmHistory RemoveToHistory()
        {
            LocationAlarmHistory history = new LocationAlarmHistory();
            history.Id = this.Id;
            history.AlarmType = this.AlarmType;
            history.AlarmLevel = this.AlarmLevel;
            history.LocationCardId = this.LocationCardId ?? 0;
            history.PersonnelId = this.PersonnelId ?? 0;
            history.AreadId = this.AreadId;
            history.Content = this.Content;
            history.AlarmTime = this.AlarmTime;
            history.AlarmTimeStamp = this.AlarmTimeStamp;
            history.HandleTime = this.HandleTime;
            history.HandleTimeStamp = this.HandleTimeStamp;
            history.Handler = this.Handler;
            history.HandleType = this.HandleType;
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

            return history;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}",Content,AreadId,PersonnelId);
        }

    }
}

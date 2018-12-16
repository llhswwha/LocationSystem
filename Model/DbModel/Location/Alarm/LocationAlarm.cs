using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Alarm;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DbModel.Location.Work;
using DbModel.LocationHistory.Data;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.Alarm
{
    /// <summary>
    /// 定位告警
    /// </summary>
    [DataContract]
    public class LocationAlarm:IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
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
        public int? AreadId  { get; set; }

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

        public LocationAlarm()
        {
            SetTime();
        }

        private void SetTime()
        {
            AlarmTime = DateTime.Now;
            AlarmTimeStamp = TimeConvert.DateTimeToTimeStamp(AlarmTime);

            HandleTime = new DateTime(2000,1,1);
            HandleTimeStamp = TimeConvert.DateTimeToTimeStamp(HandleTime);
        }

        public LocationAlarm(Position p,AreaAuthorizationRecord aar,string content, LocationAlarmLevel level)
        {
            SetTime();
            AlarmType = LocationAlarmType.区域告警;
            AlarmLevel = level;
            LocationCardId = p.CardId;
            PersonnelId = p.PersonnelID;
            CardRoleId = p.RoleId??0;
            //if (p.AreaId == 0)
            //{
            //    Console.WriteLine("p.AreaId == 0");
            //}
            AreadId = p.AreaId;
            if (aar != null)
            {
                AuzId = aar.Id;//触发告警的权限规则
                AllAuzId += aar.Id;
            }
            Content = content;
            AlarmId = Guid.NewGuid().ToString();
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

        public void Update(LocationAlarm alarm)
        {
            //this.Id = alarm.Id;
            this.AlarmType = alarm.AlarmType;
            this.AlarmLevel = alarm.AlarmLevel;
            this.LocationCardId = alarm.LocationCardId;
            this.PersonnelId = alarm.PersonnelId;
            this.AreadId = alarm.AreadId;
            this.CardRoleId = alarm.CardRoleId;
            this.Content = alarm.Content;
            this.AlarmTime = alarm.AlarmTime;
            this.AlarmTimeStamp = alarm.AlarmTimeStamp;
            this.HandleTime = alarm.HandleTime;
            this.HandleTimeStamp = alarm.HandleTimeStamp;
            this.Handler = alarm.Handler;
            this.HandleType = alarm.HandleType;
            this.AuzId = alarm.AuzId;
            this.AllAuzId = alarm.AllAuzId;
        }

        public LocationAlarmHistory RemoveToHistory()
        {
            LocationAlarmHistory history = new LocationAlarmHistory();
            //history.Id = this.Id;
            history.AlarmId = this.AlarmId;
            history.AlarmType = this.AlarmType;
            history.AlarmLevel = this.AlarmLevel;
            history.LocationCardId = this.LocationCardId ?? 0;
            history.PersonnelId = this.PersonnelId ?? 0;
            history.AreadId = this.AreadId;
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
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

            return history;
        }

        public override string ToString()
        {
            return string.Format("Content:{0},AreaId:{1},PersonId:{2}",Content,AreadId,PersonnelId);
        }

        public string GetAlarmId()
        {
            return "" + AuzId+AlarmType + AlarmLevel + LocationCardId + PersonnelId + AreadId + Content;
        }

    }
}

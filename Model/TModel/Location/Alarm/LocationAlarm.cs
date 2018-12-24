using System;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.ConvertCodes;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;
using Location.TModel.Tools;

namespace Location.TModel.Location.Alarm
{
    /// <summary>
    /// 定位告警
    /// </summary>
    [DataContract] [Serializable]
    public class LocationAlarm
    {
        private LocationAlarmType _alarmType;

        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        [DataMember]
        public string AlarmId { get; set; }

        /// <summary>
        /// 告警类型：0:区域告警，1:消失告警，2:低电告警，3:传感器告警，4:重启告警，5:非法拆卸
        /// </summary>
        [DataMember]
        //[Display(Name = "告警类型")]
        public LocationAlarmType AlarmType
        {
            get { return _alarmType; }
            set
            {
                _alarmType = value;
                TypeName = value.ToString();
            }
        }

        [DataMember]
        //[Display(Name = "告警类型")]
        public string TypeName { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        [DataMember]
        //[Display(Name = "告警等级")]
        public LocationAlarmLevel AlarmLevel { get; set; }

        /// <summary>
        /// 告警定位卡
        /// </summary>
        [DataMember]
        [ByName("LocationCardId")]
        public int TagId { get; set; }

        [DataMember]
        [ByName("LocationCard")]
        public  Tag Tag { get; set; }

        /// <summary>
        /// 告警人员
        /// </summary>
        [DataMember]
        public int PersonnelId { get; set; }

        [DataMember]
        public virtual Personnel Personnel { get; set; }

        /// <summary>
        /// 定位卡角色
        /// </summary>
        [DataMember]
        public int CardRoleId { get; set; }

        [DataMember]
        public int AreaId { get; set; }
        
        /// <summary>
        /// 告警内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 告警时间
        /// </summary>
        [DataMember]
        //[Display(Name = "告警时间")]
        [ByName("AlarmTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 告警时间戳
        /// </summary>
        [DataMember]
        //[Display(Name = "时间戳")]
        public long AlarmTimeStamp { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        //[Display(Name = "处理时间")]
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// 处理时间戳
        /// </summary>
        [DataMember]
        //[Display(Name = "处理时间戳")]
        public long HandleTimeStamp { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        //[Display(Name = "处理人")]
        public string Handler { get; set; }

        /// <summary>
        /// 处理类型：误报，忽略，确认
        /// </summary>
        [DataMember]
        //[Display(Name = "处理类型")]
        public LocationAlarmHandleType HandleType { get; set; }

        public LocationAlarm()
        {
            CreateTime = DateTime.Now;
            AlarmTimeStamp = TimeConvert.DateTimeToTimeStamp(CreateTime);
        }

        //public LocationAlarm SetPerson(LocationCardToPersonnel p)
        //{
        //    LocationCardId = p.LocationCardId;
        //    LocationCard = p.LocationCard;
        //    PersonnelId = p.PersonnelId;
        //    Personnel = p.Personnel;

        //    return this;
        //}

        public LocationAlarm SetPerson(Personnel p)
        {
            Personnel = p;
            if(p!=null)
            {
                PersonnelId = p.Id;
                Tag = p.Tag;
                if(p.Tag!=null)
                    TagId = p.Tag.Id;
                AreaId = p.AreaId ?? 2;//要是AreaId为空就改为四会电厂区域
            }
            return this;
        }

        public LocationAlarm Clone()
        {
            LocationAlarm copy = new LocationAlarm();
            copy = this.CloneObjectByBinary();
            if (this.Tag != null)
            {
                copy.Tag = this.Tag;
            }

            if (this.Personnel != null)
            {
                copy.Personnel = this.Personnel;
            }

            return copy;
        }

        //public LocationAlarmHistory RemoveToHistory()
        //{
        //    LocationAlarmHistory history = new LocationAlarmHistory();
        //    history.Id = this.Id;
        //    history.AlarmType = this.AlarmType;
        //    history.AlarmLevel = this.AlarmLevel;
        //    history.LocationCardId = this.LocationCardId;
        //    history.PersonnelId = this.PersonnelId;
        //    history.Content = this.Content;
        //    history.AlarmTime = this.AlarmTime;
        //    history.AlarmTimeStamp = this.AlarmTimeStamp;
        //    history.HandleTime = this.HandleTime;
        //    history.HandleTimeStamp = this.HandleTimeStamp;
        //    history.Handler = this.Handler;
        //    history.HandleType = this.HandleType;
        //    history.HistoryTime = DateTime.Now;
        //    history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

        //    return history;
        //}

    }
}

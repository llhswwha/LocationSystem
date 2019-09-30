using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.Alarm;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;
using IModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Location.Alarm
{
    /// <summary>
    /// 设备告警
    /// </summary>
    [Serializable]
    public class DevAlarm: IId, IDictEntity,IComparable<DevAlarm>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
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
        [MaxLength(64)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        [Display(Name = "内容")]
        [MaxLength(512)]
        public string Msg { get; set; }

        /// <summary>
        /// 告警级别(字典：报警事件级别,0 未定，1低，2中，3高)
        /// </summary>
        [DataMember]
        [Display(Name = "告警级别")]
        public Abutment_DevAlarmLevel Level { get; set; }

        /// <summary>
        /// 代码(各系统上报的原始编码)
        /// </summary>
        [DataMember]
        [Display(Name = "代码")]
        [MaxLength(32)]
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
        public virtual DevInfo DevInfo { get; set; }

        /// <summary>
        /// 设备说明
        /// </summary>
        [DataMember]
        [Display(Name = "设备说明")]
        [MaxLength(128)]
        public string Device_desc { get; set; }

        private DateTime _alarmTime;

        /// <summary>
        /// 设备告警产生时间
        /// </summary>
        [DataMember]
        [Display(Name = "设备告警产生时间")]
        public DateTime AlarmTime
        {
            get
            {
                return _alarmTime;
            }
            set
            {
                _alarmTime = value;
                AlarmTimeStamp = TimeConvert.ToStamp(value);
            }
        }

        private long _alarmTimeStamp;

        /// <summary>
        /// 告警时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "时间戳")]
        public long AlarmTimeStamp
        {
            get
            {
                return _alarmTimeStamp;
            }
            set
            {
                _alarmTimeStamp = value;
            }
        }

        [NotMapped]
        public string DictKey { get; set; }

        public DevAlarm Clone()
        {
            DevAlarm newItem = new DevAlarm();
            //copy = this.CloneObjectByBinary();
            newItem.Id = this.Id;
            newItem.Abutment_Id = this.Abutment_Id;
            newItem.Title = this.Title;
            newItem.Msg = this.Msg;
            newItem.Level = this.Level;
            newItem.Code = this.Code;
            newItem.Src = this.Src;
            newItem.DevInfoId = this.DevInfoId;
            newItem.Device_desc = this.Device_desc;
            newItem.AlarmTime = this.AlarmTime;
            newItem.AlarmTimeStamp = this.AlarmTimeStamp;
           

            if (this.DevInfo != null)
            {
                newItem.DevInfo = this.DevInfo;
            }
            
            return newItem;
        }

        public DevAlarmHistory RemoveToHistory()
        {
            DevAlarmHistory newItem = new DevAlarmHistory();
            newItem.Id = this.Id;
            newItem.Abutment_Id = this.Abutment_Id;
            newItem.Title = this.Title;
            newItem.Msg = this.Msg;
            newItem.Level = this.Level;
            newItem.Code = this.Code;
            newItem.Src = this.Src;
            newItem.DevInfoId = this.DevInfoId;
            newItem.Device_desc = this.Device_desc;
            newItem.AlarmTime = this.AlarmTime;
            newItem.AlarmTimeStamp = this.AlarmTimeStamp;
            newItem.HistoryTime = DateTime.Now;
            newItem.HistoryTimeStamp = TimeConvert.ToStamp(newItem.HistoryTime);

            return newItem;
        }

        public override string ToString()
        {
            return Msg;
        }

        public int CompareTo(DevAlarm other)
        {
            if (other == null) return -1;
            return other.AlarmTimeStamp.CompareTo(this.AlarmTimeStamp);
        }
    }


    public class DevAlarmInfo 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Abutment_DevAlarmSrc Src { get; set; }
        [DataMember]
        private DateTime _alarmTime;
        [DataMember]
        public DateTime AlarmTime
        {
            get
            {
                return _alarmTime;
            }
            set
            {
                _alarmTime = value;
                AlarmTimeStamp = TimeConvert.ToStamp(value);
            }
        }
        [DataMember]
        private long _alarmTimeStamp;
        [DataMember]
        public long AlarmTimeStamp
        {
            get
            {
                return _alarmTimeStamp;
            }
            set
            {
                _alarmTimeStamp = value;
            }
        }


    }
}

using System;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.ConvertCodes;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Tools;
using Base.Common.Tools;

namespace Location.TModel.Location.Alarm
{
    /// <summary>
    /// 设备告警
    /// </summary>
    [ByName("DevAlarm")]
    [DataContract] [Serializable]
    public class DeviceAlarm:IComparable<DeviceAlarm>
    {
        private Abutment_DevAlarmLevel _level;

        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        //[Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        //[Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 告警级别(字典：报警事件级别,0 未定，1低，2中，3高)
        /// </summary>
        [DataMember]
        public Abutment_DevAlarmLevel Level
        {
            get { return _level; }
            set
            {
                _level = value;
                LevelName = value.ToString();
            }
        }

        [DataMember]
        public string LevelName { get; set; }

        /// <summary>
        /// 代码(各系统上报的原始编码)
        /// </summary>
        [DataMember]
        //[Display(Name = "代码")]
        public string Code { get; set; }

        /// <summary>
        /// 来源(字典：报警事件来源，0 未知，1 视频监控，2 门禁，3消防，11 SIS，12人员定位)
        /// </summary>
        [DataMember]
        //[Display(Name = "来源")]
        public Abutment_DevAlarmSrc Src { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [DataMember]
        [ByName("DevInfoId")]
        public int DevId { get; set; }

        //[DataMember]
        //[ByName("DevInfo")]
        //public DevInfo Dev { get; set; }

        [DataMember]
        public string DevTypeName { get; set; }

        [DataMember]
        public int DevTypeCode { get; set; }

        [DataMember]
        public string DevName { get; set; }

        /// <summary>
        /// 设备说明
        /// </summary>
        [DataMember]
        public string Device_desc { get; set; }

        /// <summary>
        /// 设备告警产生时间
        /// </summary>
        [DataMember]
        //[Display(Name = "设备告警产生时间")]
        [ByName("AlarmTime")]
        public DateTime CreateTime { get { return _createTime; } set
            {
                _createTime = value;
                AlarmTimeStamp = TimeConvert.ToStamp(value);
            } }

        private DateTime _createTime;


        /// <summary>
        /// 告警时间戳
        /// </summary>
        [DataMember]
        //[Display(Name = "时间戳")]
        public long AlarmTimeStamp { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        public DeviceAlarm Clone()
        {
            DeviceAlarm copy = new DeviceAlarm();
            copy = this.CloneObjectByBinary();
            //if (this.Dev != null)
            //{
            //    copy.Dev = this.Dev;
            //}
            
            return copy;
        }

        //public DevAlarmHistory RemoveToHistory()
        //{
        //    DevAlarmHistory history = new DevAlarmHistory();
        //    history.Id = this.Id;
        //    history.Abutment_Id = this.Abutment_Id;
        //    history.Title = this.Title;
        //    history.Msg = this.Msg;
        //    history.Level = this.Level;
        //    history.Code = this.Code;
        //    history.Src = this.Src;
        //    history.DevInfoId = this.DevInfoId;
        //    history.Device_desc = this.Device_desc;
        //    history.AlarmTime = this.AlarmTime;
        //    history.AlarmTimeStamp = this.AlarmTimeStamp;
        //    history.HistoryTime = DateTime.Now;
        //    history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

        //    return history;
        //}

        public DeviceAlarm SetDev(DevInfo dev)
        {
            if (dev == null) return this;
            //Dev = dev;
            DevId = dev.Id;
            DevName = dev.Name;
            DevTypeName = dev.TypeName;
            DevTypeCode = dev.TypeCode;
            AreaId = dev.ParentId==null ?  0 :(int)dev.ParentId;
            return this;
        }

        public override string ToString()
        {
            return string.Format("Id:{0},Name:{1},Title:{2},Msg:{3}", DevId, DevName,Title,Message);
        }

        public int CompareTo(DeviceAlarm other)
        {
            if (other == null) return -1;
            return other.AlarmTimeStamp.CompareTo(this.AlarmTimeStamp);
        }
    }
}

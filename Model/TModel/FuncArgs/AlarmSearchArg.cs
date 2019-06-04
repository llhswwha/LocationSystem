using System;
using System.Runtime.Serialization;
using Location.IModel.FuncArgs;

namespace Location.TModel.FuncArgs
{
    /// <summary>
    /// 告警查询参数
    /// </summary>
    [DataContract]
    [Serializable]
    public class AlarmSearchArg : IAlarmSearchArg
    {
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public string Keyword { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        [DataMember]
        public PageInfo Page { get; set; }
    }

    [DataContract]
    public class PageInfo
    {

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int Size { get; set; }
    }
}


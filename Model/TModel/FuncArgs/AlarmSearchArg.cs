using System;
using System.Runtime.Serialization;
using Location.IModel.FuncArgs;
using DbModel.Tools;

namespace Location.TModel.FuncArgs
{
    /// <summary>
    /// 告警查询参数
    /// </summary>
    [DataContract]
    [Serializable]
    public class AlarmSearchArg : IAlarmSearchArg
    {
        /// <summary>
        /// 是否获取全部
        /// </summary>
        [DataMember]
        public bool IsAll { get; set; }

        [DataMember]
        public string Start { get; set; }

        [DataMember]
        public string End { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public string Keyword { get; set; }

        [DataMember]
        public int AreaId { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        [DataMember]
        public PageInfo Page { get; set; }
    
        /// <summary>
        /// 设备类型
        /// </summary>
        [DataMember]
        public string DevTypeName { get; set; }
    }

    [DataContract]
    [Serializable]
    public class PageInfo
    {
        /// <summary>
        ///第几页
        /// </summary>
        [DataMember]
        public int Number { get; set; }
        /// <summary>
        /// 一页有多少行
        /// </summary>
        [DataMember]
        public int Size { get; set; }
    }
}


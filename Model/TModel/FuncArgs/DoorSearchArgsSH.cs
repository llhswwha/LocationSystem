using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.FuncArgs
{
    /// <summary>
    /// 四会门禁卡历史信息查询字段
    /// </summary>
    [DataContract]
    [Serializable]
    public  class DoorSearchArgsSH
    {
        [DataMember]
        public string device_id { get; set; }
        [DataMember]
        public DateTime startTime { get; set; }
        [DataMember]
        public DateTime endTime { get; set; }
        [DataMember]
        public int pageNo { get; set; }
        [DataMember]
        public int pageSize { get; set; }
    }
}

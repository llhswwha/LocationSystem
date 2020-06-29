using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.FuncArgs
{
    [DataContract]
    [Serializable]
    public  class DoorSearchArgs
    {
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string endTime { get; set; }
        [DataMember]
        public int pageNo { get; set; }
        [DataMember]
        public int pageSize { get; set; }
        [DataMember]
        public string eventType { get; set; }/*必要(事件类型)*/
        [DataMember]
        public string[] personIds { get; set; }
        [DataMember]
        public string[] doorIndexCodes { get; set; }
        [DataMember]
        public string personName { get; set; }

    }
}

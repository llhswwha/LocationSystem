using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TModel.Tools;

namespace TModel.FuncArgs
{
    [DataContract]
    [Serializable]
   public  class TicketSearchArgs
    {
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public DateTime startTime { get; set; }
        [DataMember]
        public DateTime endTime { get; set; }
        [DataMember]
        public int pageIndex { get; set; }
        [DataMember]
        public int pageSize { get; set; }

        public TicketSearchArgs(string v1, DateTime dateTime1, DateTime dateTime2, int v2, int v3)
        {
            this.value = v1;
            this.startTime = dateTime1;
            this.endTime = dateTime2;
            this.pageIndex = v2;
            this.pageIndex = v3;
        }
    }
}

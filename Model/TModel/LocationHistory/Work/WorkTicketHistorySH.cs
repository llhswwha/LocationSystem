using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.LocationHistory.Work
{
    [DataContract]
    public class WorkTicketHistorySH
    {
        [DataMember]
        public bool fromOrder { get; set; }
        [DataMember]
        public int? id { get; set; }
        [DataMember]
        public string ticketCode { get; set; }
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public int state { get; set; }
        [DataMember]
        public string createTime { get; set; }
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string endTime { get; set; }
        [DataMember]
        public int[] workerIds { get; set; } //数组，逗号隔开
        [DataMember]
        public int[] cardIds { get; set; }
        [DataMember]
        public int[] zoneIds { get; set; }
        public int[] doorIds { get; set; }
        [DataMember]
        public string detail { get; set; }  //json,需转化

        public workTicketDetail workTicket { get; set; }

        [DataMember]
        public int rawId { get; set; }
        [DataMember]
        public string ticketName { get; set; }
        [DataMember]
        public int[] authRequests { get; set; }  //数组
        [DataMember]
        public int[] workers { get; set; }   //数组
        [DataMember]
        public int[] cards { get; set; } //数组
        [DataMember]
        public int[] zones { get; set; }//数组
        [DataMember]
        public int[] doors { get; set; }//数组

    }
}

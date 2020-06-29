using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
  public   class TwoTickets
    {
        public bool fromOrder { get; set; }
        public int id { get; set; }
        public string ticketCode { get; set; }
        public int type { get; set; }
        public int state { get; set; }
        public DateTime createTime { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int[] workerIds { get; set; } //数组，逗号隔开
        public int[] cardIds { get; set; }
        public int[] zoneIds { get; set; }
        public int[] doorIds { get; set; }

        public string detail { get; set; }  //json,需转化

        public DetailsSet detailsSet { get; set; } //发送到客户端
        public int rawId { get; set; }
        public string ticketName { get; set; }

        public int[] authRequests { get; set; }  //数组

        public int[] workers { get; set; }   //数组

        public int[] cards { get; set; } //数组

        public int[] zones { get; set; }//数组

        public int[] doors { get; set; }//数组


    }
}

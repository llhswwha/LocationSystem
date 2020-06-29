using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Work
{
  public   class OperationTicketHistorySH:IId
    {
        [DataMember]
        [Display(Name = "操作票Id")]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }
        [DataMember]
        [Display(Name = "")]
        public bool fromOrder { get; set; }
        [DataMember]
        [Display(Name = "")]
        public string ticketCode { get; set; }
        [DataMember]
        [Display(Name = "")]
        public int type { get; set; }
        [DataMember]
        [Display(Name = "")]
        public int state { get; set; }
        [DataMember]
        [Display(Name = "")]
        public DateTime createTime { get; set; }
        [DataMember]
        [Display(Name = "")]
        public DateTime startTime { get; set; }
        [DataMember]
        [Display(Name = "")]
        public DateTime endTime { get; set; }
        [DataMember]
        [Display(Name = "")]
        public string detail { get; set; }  //json,需转化
        public int rawId { get; set; }
        public string ticketName { get; set; }
    }
}

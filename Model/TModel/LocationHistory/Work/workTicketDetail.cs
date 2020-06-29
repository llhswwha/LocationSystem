using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.LocationHistory.Work
{
    [DataContract]
    public class workTicketDetail
    {
        [DataMember]
        public string wtId { get; set; }
        [DataMember]
        public string wtType { get; set; }
        [DataMember]
        public string wtNumber { get; set; }
        [DataMember]
        public string workShift { get; set; }
        [DataMember]
        public string permitDate { get; set; }
        [DataMember]
        public string wtTypeName { get; set; }
        [DataMember]
        public string delayReason { get; set; }
        [DataMember]
        public string receiveDate { get; set; }
        [DataMember]
        public string workContent { get; set; }
        [DataMember]
        public string dutyPersonId { get; set; }
        [DataMember]
        public string dutyPersonName { get; set; }
        [DataMember]
        public string dutySignDate { get; set; }
        [DataMember]
        public string assemblingSet { get; set; }
        [DataMember]
        public string functionMajor { get; set; }
        [DataMember]
        public string removedNumber { get; set; }
        [DataMember]
        public string workShiftName { get; set; }
        [DataMember]
        public string organizationId { get; set; }
        [DataMember]
        public string permitPersonId { get; set; }
        [DataMember]
        public string workFinishDate { get; set;  }
        [DataMember]
        public string workObjectDesc { get; set;}
        [DataMember]
        public string workObjectName { get; set; }
        [DataMember]
        public string countersignDate { get; set; }
        [DataMember]
        public string approveStartDate { get; set; }
        [DataMember]
        public string organizationName { get; set; }
        [DataMember]
        public string planningWorkDate { get; set; }
        [DataMember]
        public string statusLookupCode { get; set; }
        [DataMember]
        public string statusLookupDesc { get; set; }
        [DataMember]
        public string assemblingSetDesc { get; set; }
        [DataMember]
        public string endPermitPersonId { get; set; }
        [DataMember]
        public string overhaulMajorDesc { get; set; }
        [DataMember]
        public string approveWorkPersonId { get; set; }
        [DataMember]
        public string countersignPersonId { get; set; }
        [DataMember]
        public string finishPermitPersonId { get; set; }
        [DataMember]
        public string receivePersonName { get; set; }
        [DataMember]
        public string endPermitPersonName { get; set; }
        [DataMember]
        public List<string> ruleLine { get; set; }

        [DataMember]
        public List<string> approveLime { get; set; }
    }

    public class WorkTicketDetails
    {
        public workTicketDetail workTicket { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
  public  class OptTicket
    {
        public string dept { get; set; }
        public string otType { get; set; }
        public string kksCode { get; set; }
        public string kksDesc { get; set; }
        public string deptDesc { get; set; }
        public string otNumber { get; set; }
        public string opMission { get; set; }
        public DateTime orderDate { get; set; }
        public string workShift { get; set; }
        public string attribute2 { get; set; }
        public string attribute3 { get; set; }
        public string attribute4 { get; set; }
        public string attribute5 { get; set; }
        public string attribute6 { get; set; }
        public string opPersonIdP { get; set; }
        public string otTypeDesc { get; set; }
        public DateTime reportDate { get; set; }
        public string statusDesc { get; set; }
        public string opPersonName { get; set; }
        public string workShiftDesc { get; set; }
        public string organizationId { get; set; }
        public string reportPersonId { get; set; }
        public string verifyPersonId { get; set; }
        public string approvePersonId { get; set; }
        public string organizationName { get; set; }
        public string reportPersonName { get; set; }
        public string statusLookupCode { get; set; }
        public string verifyPersonName { get; set; }
        public string approvePersonName { get; set; }
    }
}

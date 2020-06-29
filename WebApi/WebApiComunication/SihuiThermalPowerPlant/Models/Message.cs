using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    public class Message
    {
        public int total { get; set; }
        public string msg { get; set; }

        public List<TwoTickets> data { get; set; }
    }
}

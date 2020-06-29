using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
  public  class Details
    {
        public List<LinesGet> lines { get; set; }

        public OptTicket optTicket { get; set; }
    }

    public class DetailsSet
    {
        public List<LinesSet> lines { get; set; }

        public OptTicket optTicket { get; set; }

    }
}

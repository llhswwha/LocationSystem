using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.Work;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    public class Message<T>
    {
        public int total { get; set; }
        public string msg { get; set; }

        public List<T> data { get; set; }
    }

}

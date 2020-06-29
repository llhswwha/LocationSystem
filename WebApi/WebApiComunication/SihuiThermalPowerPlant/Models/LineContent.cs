using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    //返回的键值对
   public class LineContent
    {
       public List<KeyValue> Content { get; set; }
    }

    public class KeyValue
    {
        public string key { get; set; }
        public string value { get; set; }

    }
}

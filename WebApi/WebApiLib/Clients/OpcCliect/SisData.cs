using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLib.Clients.OpcCliect
{
 public   class SisData
    {
        public int Id { get; set; }
        /// <summary>
        /// TagName
        /// </summary>
        public string Name { get; set; } 
        public int Type { get; set; }
        public string Desc { get; set; }
        public string Unit { get; set; }
        public string ExDesc { get; set; }
        public int State { get; set; }
       public object ControlSystemName { get; set; }
        public string Value { get; set; }
        public int ROLEType { get; set; }
        public object Key { get; set; }

        public SisData()
        { }

        //1
    }
}

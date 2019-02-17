using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WModel.Location.AreaAndDev
{
    public class Area
    {
        public int id { get; set; }

        public int pId { get; set; }

        public string name { get; set; }

        public Area()
        {
            pId = 0;
            name = "";
        }
    }
}

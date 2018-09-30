using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model.LocationTables;

namespace TransClass.Models
{
    public class DevInfoTrans
    {
        public int total { get; set; }

        public string msg { get; set; }

        public List<DevInfo> data { get; set; }
    }
}

using DbModel.LocationHistory.Door;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.ObjToData
{
   public  class DoorEvents
    {
        public string code { get; set; }
        public string msg { get; set; }
        public DataDoor data { get; set; }
    }
    public class DataDoor
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }

        public List<DoorClick> list { get; set; }
        public int total { get; set; }
        public int totalPage { get; set; }

    }

}

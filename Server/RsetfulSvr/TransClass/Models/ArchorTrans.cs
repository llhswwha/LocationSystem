using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Model;

namespace TransClass.Models
{
    public class ArchorTrans
    {
        public int total { get; set; }

        public string msg { get; set; }

        public List<Archor> data { get; set; }
    }
}

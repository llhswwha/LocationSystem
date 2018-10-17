using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Engine
{
    public class bus_anchor
    {
        [Key]
        public int id { get; set; }

        public string anchor_id { get; set; }

        public int anchor_x { get; set; }

        public int anchor_y { get; set; }

        public int anchor_z { get; set; }

        public int anchor_type { get; set; }

        public int anchor_bno { get; set; }

        public string syn_anchor_id { get; set; }

        public int offset { get; set; }

        public int min_x { get; set; }

        public int max_x { get; set; }

        public int min_y { get; set; }

        public int max_y { get; set; }

        public int min_z { get; set; }

        public int max_z { get; set; }

        public int enabled { get; set; }


    }
}

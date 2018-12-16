using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Engine
{
    public class bus_anchor:IId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

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

        public bus_anchor()
        {
            anchor_id = "";
            anchor_x = 0;
            anchor_y = 0;
            anchor_z = 0;
            anchor_type = 0;
            anchor_bno = 0;
            syn_anchor_id = null;
            offset = 0;
            min_x = 90000000;
            max_x = 90000000;
            min_y = 90000000;
            max_y = 90000000;
            min_z = 90000000;
            max_z = 90000000;
            enabled = 1;
        }
    }
}

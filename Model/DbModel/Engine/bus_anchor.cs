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

        public int? anchor_bno { get; set; }

        //[Unique]
        public string anchor_id { get; set; }

        public int? anchor_type { get; set; }

        public int? anchor_x { get; set; }

        public int? anchor_y { get; set; }

        public int? anchor_z { get; set; }

        public int? enabled { get; set; }

        public int? is_bs { get; set; }

        public int? offset { get; set; }

        public string syn_anchor_id { get; set; }

        public int? is_floor { get; set; }

        public int? status { get; set; }


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
            enabled = 1;
            is_floor = 0;
            status = 0;
            is_bs = 0;
        }

        public override string ToString()
        {
            return anchor_id;
        }
    }
}

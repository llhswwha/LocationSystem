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
    public class bus_anchor_switch_area : IId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        public string area_id { get; set; }

        public int area_no { get; set; }

        public int end_x { get; set; }

        public int end_y { get; set; }
        public int max_z { get; set; }
        public int min_z { get; set; }
        public string color { get; set; }
        public int sort { get; set; }

        public int start_x { get; set; }
        public int start_y { get; set; }
        public int type { get; set; }
        public int leave_rssi_enable { get; set; }

        public override string ToString()
        {
            return area_id;
        }
    }
}

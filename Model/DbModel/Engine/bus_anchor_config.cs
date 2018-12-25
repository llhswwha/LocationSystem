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
    public class bus_anchor_config : IId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        public string anchor_id { get; set; }

        public string anchor_ip { get; set; }

        public int channel { get; set; }

        public int dhcp_enabled { get; set; }

        public int interval { get; set; }

        public string mac_address { get; set; }

        public string net_gate { get; set; }

        public int num { get; set; }

        public string region { get; set; }

        public int seq { get; set; }

        public string server_ip { get; set; }

        public int server_port { get; set; }

        public string subnet_mask { get; set; }

        public int type { get; set; }

        public int lora_switch { get; set; }

        public string lora_syn_param { get; set; }

        public string version_code { get; set; }
    }
}

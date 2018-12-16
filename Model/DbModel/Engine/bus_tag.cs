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
    public class bus_tag:IId
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        public string tag_id { get; set; }

        public int tag_id_dec { get; set; }
    }
}

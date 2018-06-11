using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLocation.Models
{
    /// <summary>
    /// 地图信息
    /// </summary>
    public class Map
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        //public virtual Bound Bound { get; set; }
    }
}
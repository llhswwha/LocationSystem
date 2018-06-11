using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLocation.Models
{
    /// <summary>
    /// 边界信息 地图和区域
    /// </summary>
    public class Bound
    {
        public int Id { get; set; }
        public float MinX { get; set; }

        public float MinY { get; set; }

        public float MinZ { get; set; }

        public float MaxX { get; set; }

        public float MaxY { get; set; }

        public float MaxZ { get; set; }
    }
}
using Location.TModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.AreaAndDev
{
    /// <summary>
    /// Bound的信息
    /// </summary>
    public class AreaPoints
    {
        public int Id { get; set; }

        public List<Point> Points { get; set; }

        public AreaPoints()
        {

        }

        public AreaPoints(PhysicalTopology area)
        {
            Id = area.Id;
            if (area.InitBound != null)
            {
                Points = area.InitBound.Points;
            }
            else
            {
                Points = null;
            }
        }
    }
}

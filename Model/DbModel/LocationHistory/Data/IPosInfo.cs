using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public interface IPosInfo
    {
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }

        long DateTimeStamp { get; set; }
    }
}

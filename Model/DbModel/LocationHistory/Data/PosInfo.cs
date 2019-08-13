using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public class PosInfo
    {
        public int Id;
        public long DateTimeStamp;
        public int PersonnelID;
        public string Code;
        public int AreaId;

        public string PersonnelName;
        public string AreaPath;

        public DateTime DateTime;
    }
}

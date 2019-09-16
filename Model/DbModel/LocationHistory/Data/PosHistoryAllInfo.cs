using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    /// <summary>
    /// 所有人员定位信息
    /// </summary>
    [Serializable]
    public  class PosHistoryAllInfo
    {
        public PosHistoryAllInfo()
        {

        }
        public List<PosInfoList> PosListByArea { get; set; }
        public List<PosInfoList> PosListByPerson { get; set; }
        public List<PosInfoList> PosListByTime { get; set; }
    }

}

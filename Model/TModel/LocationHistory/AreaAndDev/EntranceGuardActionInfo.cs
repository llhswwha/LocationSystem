using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.LocationHistory.AreaAndDev
{
    public class EntranceGuardActionInfo
    {
        //门禁ID
        public int Id { get; set; }

        //门禁名称
        public string Name { get; set; }

        //所属区域ID
        public int AreadId { get; set; }

        //所属区域名称
        public string AreadName { get; set; }

        //门禁卡号
        public string Code { get; set; }

        //刷卡时间
        public DateTime? OperateTime { get; set; }

        //出入时间
        public int nInOutState { get; set; }
    }
}

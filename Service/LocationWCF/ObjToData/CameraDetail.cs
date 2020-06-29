using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations.Services
{
   public class CameraDetail
    {
        /// <summary>
        /// 返回码，0:接口业务处理成功
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 在线状态（0-未知，1-在线，2-离线），扩展字段，暂不使用
        /// </summary>
        public string status { get; set; }
    }
}

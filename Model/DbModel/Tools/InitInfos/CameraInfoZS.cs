using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Tools.InitInfos
{
    public class CameraInfoZS
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 原来名称
        /// </summary>
        public string NormalName { get; set; }

        /// <summary>
        /// 修改后名称
        /// </summary>
        [DataMember]
        public string CurrentName { get; set; }

        /// <summary>
        /// 设备标签
        /// </summary>
        public string DevTag { get; set; }

        /// <summary>
        /// 设备所在区域
        /// </summary>
        public string DevArea { get; set; }

        /// <summary>
        /// IP和端口
        /// </summary>
        public string IpPort { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        /// <summary>
        /// 枪机||球机。。
        /// </summary>

        public string DevType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 检查项列表
    /// </summary>
    public class results
    {
        /// <summary>
        /// 设备kks编码
        /// </summary>
        public string kksCode { get; set; }

        /// <summary>
        /// 巡检内容
        /// </summary>
        public string checkItem { get; set; }

        /// <summary>
        /// 巡检员工号
        /// </summary>
        public string staffCode { get; set; }

        /// <summary>
        /// 巡检时间
        /// </summary>
        public long? checkTime { get; set; }

        /// <summary>
        /// 巡检项ID
        /// </summary>
        public string checkId { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        public string checkResult { get; set; }




        //public string xjx_id { get; set; }
        //public string kkstext { get; set; }

        //public string kkscode_p { get; set; }

        //public string kkstext_p { get; set; }


    }
}

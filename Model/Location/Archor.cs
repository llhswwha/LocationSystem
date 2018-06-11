using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 基站
    /// </summary>
    public class Archor
    {
        /// <summary>
        /// 数据库Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 基站编号
        /// </summary>
        [Display(Name = "基站编号")]
        public string Code { get; set; }

        /// <summary>
        /// 基站名
        /// </summary>
        [Display(Name = "基站名")]
        public string Name { get; set; }

        /// <summary>
        /// 位置X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 位置Y
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 位置Z (?)
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// 类型 : 副、主
        /// </summary>
        [Display(Name = "基站类型")]
        public int Type { get; set; }

        /// <summary>
        /// 自动获取IP
        /// </summary>
        [Display(Name = "自动获取IP")]
        public bool IsAutoIp { get; set; }

        /// <summary>
        /// 基站IP
        /// </summary>
        [Display(Name = "基站IP")]
        public string Ip { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        [Display(Name = "服务器IP")]
        public string ServerIp { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [Display(Name = "端口")]
        public int ServerPort { get; set; }

        /// <summary>
        /// 发射功率
        /// </summary>
        [Display(Name = "发射功率")]
        public double Power { get; set; }

        /// <summary>
        /// 心跳间隔
        /// </summary>
        [Display(Name = "心跳间隔")]
        public double AliveTime { get; set; }
    }
}

using CommunicationClass.SihuiThermalPowerPlant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant
{
    /// <summary>
    /// 用于sis历史数据的，紧凑型
    /// </summary>
    public class SisData_Compact
    {
        [Display(Name = "id")]
        public int id { get; set; }

        [Display(Name = "code")]
        public int code { get; set; }

        [Display(Name = "总数量")]
        public int total { get; set; }

        [Display(Name = "消息内容")]
        public string msg { get; set; }

        [Display(Name = "schema")]
        public sis_compact schema { get; set; }

        [Display(Name = "数据")]
        public List<List<string>> data { get; set; }

        public SisData_Compact()
        {
            msg = "ok";
            schema = new sis_compact();
            data = new List<List<string>>();
        }
    }
}

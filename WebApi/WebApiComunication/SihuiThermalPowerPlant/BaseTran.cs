using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant
{
    public class BaseTran<T>
    {
        [Display(Name = "总数量")]
        public int total { get; set; }

        [Display(Name = "消息内容")]
        public string msg { get; set; }

        [Display(Name = "数据")]
        public List<T> data { get; set; }

        public BaseTran()
        {
            
        }

        public BaseTran(List<T> lst)
        {
            total = lst.Count;
            msg = "ok";
            data = lst;
        } 
    }
}

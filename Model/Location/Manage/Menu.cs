using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu
    {
        public int Id { get; set; }

        [Display(Name = "菜单编号")]
        public string Code { get; set; }

        [Display(Name = "菜单名")]
        public string Name { get; set; }

        [Display(Name = "菜单地址")]
        public string Url { get; set; }

        [Display(Name = "排序")]
        public int Order { get; set; }

        [Display(Name = "上级菜单")]
        public Menu PMenu { get; set; }

        public List<Function> Funcs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Location.IModel;

namespace Location.Model.Manage
{
    /// <summary>
    /// KKS编码信息
    /// </summary>
    public class KKSCode : IKKSCode
    {
        public int Id { get; set; }

        [Display(Name = "序号")]
        [Required]
        public string Serial { get; set; }

        [Display(Name = "设备名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "工艺相关标识")]
        [Required]
        public string Code { get; set; }

        [Display(Name = "上级工艺相关标识")]
        public string ParentCode { get; set; }

        [Display(Name = "设计院编码")]
        public string DesinCode { get; set; }

        [Display(Name = "主类")]
        [Required]
        public string MainType { get; set; }

        [Display(Name = "子类")]
        [Required]
        public string SubType { get; set; }

        [Display(Name = "所属系统")]
        [Required]
        public string System { get; set; }
    }
}

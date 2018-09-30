using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 监控目标（绑定标签的人或者物）
    /// </summary>
    public class Target
    {
        public int Id { get; set; }

        [Display(Name = "目标名称")]
        public string Name { get; set; }

        [Display(Name = "目标编号")]
        public string Code { get; set; }

        public int? TagId { get; set; }

        [Display(Name = "目标标签")]
        public virtual Tag Tag { get; set; }

        [Display(Name = "目标类型")]
        public int Type { get; set; }

        public int? DepId { get; set; }

        [Display(Name = "所属机构")]
        public virtual Department Dep { get; set; }

        [Display(Name = "目标图片")]
        public string ImageFile { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

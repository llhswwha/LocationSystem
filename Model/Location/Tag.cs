using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.IModel.Locations;

namespace Location.Model
{

    /// <summary>
    /// 标签 即（定位卡）
    /// </summary>
    public class Tag: ITag
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="终端编号")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "终端名称")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Describe { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

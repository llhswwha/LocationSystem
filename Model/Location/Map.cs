using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Location.Model.Base;

namespace Location.Model
{
    /// <summary>
    /// 地图信息
    /// </summary>
    public class Map: Bound
    {
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "文件")]
        public string FileName { get; set; }

        [Display(Name = "排序")]
        public int ShowOrder { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        public Department Dep { get; set; }

        /// <summary>
        /// 是否主地图
        /// </summary>
        public bool IsMain { get; set; }

        public virtual List<Area> Areas { get; set; }
    }
}
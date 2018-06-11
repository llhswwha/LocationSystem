using System.ComponentModel.DataAnnotations;
using Location.Model.Base;

namespace Location.Model
{
    /// <summary>
    /// 区域 地图上的一个子部分
    /// </summary>
    public class Area:Bound
    {
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        public virtual Map Map { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Location.IModel.Locations;

namespace Location.Model.Locations
{
    /// <summary>
    ///     配置参数
    /// </summary>
    public class ConfigArg : IConfigArg
    {
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "键")]
        public string Key { get; set; }

        [Display(Name = "值")]
        public string Value { get; set; }

        [Display(Name = "值类型")]
        public string ValueType { get; set; }

        [Display(Name = "描述")]
        public string Describe { get; set; }

        [Display(Name = "配置分类")]
        public string Classify { get; set; }
    }
}
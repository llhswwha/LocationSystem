using System.ComponentModel.DataAnnotations;
using Location.IModel.Locations;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    ///     配置参数
    /// </summary>
    public class ConfigArg : IConfigArg
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [MaxLength(16)]
        public string Name { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        [Display(Name = "键")]
        [MaxLength(32)]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        [Display(Name = "值")]
        [MaxLength(32)]
        public string Value { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        [DataMember]
        [Display(Name = "值类型")]
        [MaxLength(8)]
        public string ValueType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        [Display(Name = "描述")]
        [MaxLength(32)]
        public string Describe { get; set; }

        /// <summary>
        /// 配置分类
        /// </summary>
        [DataMember]
        [Display(Name = "配置分类")]
        [MaxLength(8)]
        public string Classify { get; set; }

        public ConfigArg Clone()
        {
            ConfigArg copy = new ConfigArg();
            copy = this.CloneObjectByBinary();
            return copy;
        }
    }
}
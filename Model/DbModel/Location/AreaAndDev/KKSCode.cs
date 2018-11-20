using System.ComponentModel.DataAnnotations;
using Location.IModel;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// KKS编码信息
    /// </summary>
    public class KKSCode : IKKSCode
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        [Display(Name = "序号")]
        [MaxLength(8)]
        [Required]
        public string Serial { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "设备名称")]
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 工艺相关标识
        /// </summary>
        [DataMember]
        [Display(Name = "工艺相关标识")]
        [MaxLength(32)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 上级工艺相关标识
        /// </summary>
        [DataMember]
        [Display(Name = "上级工艺相关标识")]
        [MaxLength(32)]
        public string ParentCode { get; set; }

        /// <summary>
        /// 设计院编码
        /// </summary>
        [DataMember]
        [Display(Name = "设计院编码")]
        [MaxLength(32)]
        public string DesinCode { get; set; }

        /// <summary>
        /// 主类
        /// </summary>
        [DataMember]
        [Display(Name = "主类")]
        [MaxLength(16)]
        [Required]
        public string MainType { get; set; }

        /// <summary>
        /// 子类
        /// </summary>
        [DataMember]
        [Display(Name = "子类")]
        [MaxLength(32)]
        [Required]
        public string SubType { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        [DataMember]
        [Display(Name = "所属系统")]
        [MaxLength(32)]
        [Required]
        public string System { get; set; }

        public KKSCode Clone()
        {
            return this.CloneObjectByBinary();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 图片
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// 图片
        /// </summary>
        [DataMember]
        [Display(Name = "图片")]
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// 图片信息
        /// </summary>
        [DataMember]
        [Display(Name = "图片信息")]
        [Required]
        public byte[] Info { get; set; }
    }
}

using Location.IModel;
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
    /// 首页图片
    /// </summary>
    [DataContract]
    public class HomePagePicture : IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        [DataMember]
        [Display(Name = "图片名称")]
        [MaxLength(128)]
        public string Name { get; set; }
    }
}

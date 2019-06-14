using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.BaseData
{
    /// <summary>
    /// 获取区域列表
    /// </summary>
    public class zone
    {
        [Key]
        public int dbId { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [Display(Name = "标识")]
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [MaxLength(256)]
        public string name { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        [Display(Name = "KKS编码")]
        [MaxLength(256)]
        public string kks { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        [MaxLength(256)]
        public string description { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        [Display(Name = "X坐标")]
        public float? x { get; set; }

        /// <summary>
        /// y坐标
        /// </summary>
        [Display(Name = "y坐标")]
        public float? y { get; set; }

        /// <summary>
        /// z坐标
        /// </summary>
        [Display(Name = "z坐标")]
        public float? z { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [Display(Name = "父ID")]
        public int? parent_Id { get; set; }

        /// <summary>
        /// 级联关系
        /// </summary>
        [Display(Name = "级联关系")]
        [MaxLength(256)]
        public string path { get; set; }

        [NotMapped]
        public List<zone> zones { get; set; }

        [NotMapped]
        public List<device> devices { get; set; }

        public zone Clone()
        {
            zone copy = new zone();
            copy.id = this.id;
            copy.name = this.name;
            copy.kks = this.kks;
            copy.description = this.description;
            copy.x = this.x;
            copy.y = this.y;
            copy.z = this.z;
            copy.parent_Id = this.parent_Id;
            copy.path = this.path;
            copy.zones = this.zones;
            copy.devices = this.devices;
            


            return copy;
        }
    }
}

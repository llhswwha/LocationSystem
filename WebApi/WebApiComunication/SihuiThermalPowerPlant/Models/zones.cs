using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    /// <summary>
    /// 获取区域列表
    /// </summary>
    public class zones
    {
        /// <summary>
        /// 标识
        /// </summary>
        [Display(Name = "标识")]
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        public string name { get; set; }

        /// <summary>
        /// KKS编码
        /// </summary>
        [Display(Name = "KKS编码")]
        public string kks { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
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
        public int? parent_id { get; set; }

        /// <summary>
        /// 级联关系
        /// </summary>
        [Display(Name = "级联关系")]
        public string path { get; set; }

        public List<zones> lstzon { get; set; }

        public List<devices> lstdev { get; set; }

        public zones Clone()
        {
            zones copy = new zones();
            copy.id = this.id;
            copy.name = this.name;
            copy.kks = this.kks;
            copy.description = this.description;
            copy.x = this.x;
            copy.y = this.y;
            copy.z = this.z;
            copy.parent_id = this.parent_id;
            copy.path = this.path;
            copy.lstzon = this.lstzon;
            copy.lstdev = this.lstdev;


            return copy;
        }
    }
}

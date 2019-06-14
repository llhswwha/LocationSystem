using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.BaseData
{
    public class org
    {
        [Key]
        public int dbId { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(256)]
        public string name { get; set; }

        /// <summary>
        /// 父部门ID,顶级部门的parentId为null
        /// </summary>
        public int? parentId { get; set; }

        /// <summary>
        /// 类型，0本厂，1外委单位
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [MaxLength(512)]
        public string description { get; set; }

        public org Clone()
        {
            org copy = new org();
            copy.id = this.id;
            copy.name = this.name;
            copy.parentId = this.parentId;
            copy.type = this.type;
            copy.description = this.description;

            return copy;
        }
    }
}

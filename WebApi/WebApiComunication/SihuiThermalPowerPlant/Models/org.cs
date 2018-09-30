using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClass.SihuiThermalPowerPlant.Models
{
    public class org
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 父部门ID,顶级部门为0
        /// </summary>
        public int parent_id { get; set; }

        /// <summary>
        /// 类型，0本厂，1外委单位
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string description { get; set; }

        public org Clone()
        {
            org copy = new org();
            copy.id = this.id;
            copy.name = this.name;
            copy.parent_id = this.parent_id;
            copy.type = this.type;
            copy.description = this.description;

            return copy;
        }
    }
}

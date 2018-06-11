using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 监控目标（绑定标签的人或者物）
    /// </summary>
    public class Target
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// 类型：人员，物资
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        public Department Dep { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string ImageFile { get; set; }
    }
}

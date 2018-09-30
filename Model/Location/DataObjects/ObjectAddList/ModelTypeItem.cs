using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model.DataObjects.ObjectAddList
{
    /// <summary>
    /// 设备类型信息
    /// </summary>
    public class ModelTypeItem
    {
        /// <summary>
        ///  大项, 如 机柜、动力设备、装饰
        /// </summary>
        public string Item;
        /// <summary>
        /// 大类, 如 配电柜、办公家具
        /// </summary>
        public string Class;
        /// <summary>
        /// 模型名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 设备类型编号
        /// </summary>
        public string nType;
    }
}

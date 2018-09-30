
using System;
using System.Runtime.Serialization;

namespace Location.TModel.Location.AreaAndDev
{
    [DataContract] [Serializable]
    public class ModelTypeItem
    {
        /// <summary>
        ///  大项, 如 机柜、动力设备、装饰
        /// </summary>
        [DataMember]
        public string Item;
        /// <summary>
        /// 大类, 如 配电柜、办公家具
        /// </summary>
        [DataMember]
        public string Class;
        /// <summary>
        /// 模型名称
        /// </summary>
        [DataMember]
        public string Name;
        /// <summary>
        /// 设备类型编号
        /// </summary>
        [DataMember]
        public string nType;
    }
}

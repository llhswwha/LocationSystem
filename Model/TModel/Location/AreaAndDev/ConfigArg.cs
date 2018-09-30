using System.Runtime.Serialization;
using Location.IModel.Locations;
using Location.TModel.Tools;
using System;

namespace Location.TModel.Location.AreaAndDev
{
    /// <summary>
    ///     配置参数
    /// </summary>
    [DataContract] [Serializable]
    public class ConfigArg : IConfigArg
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        //[Display(Name = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        //[Display(Name = "键")]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        //[Display(Name = "值")]
        public string Value { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        [DataMember]
        //[Display(Name = "值类型")]
        public string ValueType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        //[Display(Name = "描述")]
        public string Describe { get; set; }

        /// <summary>
        /// 配置分类
        /// </summary>
        [DataMember]
        //[Display(Name = "配置分类")]
        public string Classify { get; set; }

        public ConfigArg Clone()
        {
            return this.CloneObjectByBinary();
        }
    }
}
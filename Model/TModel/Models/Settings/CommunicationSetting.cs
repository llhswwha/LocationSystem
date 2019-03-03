using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace TModel.Models.Settings
{
    /// <summary>
    /// 通信相关设置
    /// </summary>
    [XmlType(TypeName = "CommunicationSetting")]
    public class CommunicationSetting
    {

        /// <summary>
        /// CommunicationObject.cs的IP设置
        /// </summary>
        [XmlAttribute]
        public string Ip1 = "127.0.0.1";
        /// <summary>
        /// CommunicationObject.cs的端口设置
        /// </summary>
        [XmlAttribute]
        public string Port1 = "8733";
        /// <summary>
        /// CommunicationCallbackClient.cs的IP设置
        /// </summary>
        [XmlAttribute]
        public string Ip2 = "localhost";
        /// <summary>
        /// CommunicationCallbackClient.cs的端口设置
        /// </summary>
        [XmlAttribute]
        public string Port2 = "8735";
    }
}

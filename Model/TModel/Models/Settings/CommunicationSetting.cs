using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
public enum CommunicationMode
{
    None, Sync, Async, Thread, WebApi, Editor
}
/// <summary>
/// 通信相关设置
/// </summary>
[XmlType(TypeName = "CommunicationSetting")]
public class CommunicationSetting {

    /// <summary>
    /// 上次登陆IP保存
    /// CommunicationObject.cs的IP设置
    /// </summary>
    [XmlAttribute]
    public string Ip1 = "172.16.100.26";
    /// <summary>
    /// 上次登陆端口保存
    /// 【不用显示出来】
    /// CommunicationObject.cs的端口设置
    /// </summary>
    [XmlAttribute]
    public string Port1 = "8733";

    
    ///// <summary>
    ///// CommunicationCallbackClient.cs的IP设置
    ///// </summary>
    //[XmlAttribute]
    //public string Ip2 = "localhost";
    ///// <summary>
    ///// CommunicationCallbackClient.cs的端口设置
    ///// </summary>
    //[XmlAttribute]
    //public string Port2 = "8735";

    [XmlAttribute]
    public CommunicationMode Mode = CommunicationMode.WebApi;

    //1.（信令）服务端设置
    /// <summary>
    /// This is a test server. Don't use in production! The server code is in a zip file in WebRtcNetwork
    /// </summary>
    [XmlAttribute]
    public string SignalingServerUrl = "ws://192.168.1.150:8444/player";
}

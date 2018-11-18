using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;


[XmlType("ArchorList")]
public class ArchorDevList{

    /// <summary>
    /// 区域列表
    /// </summary>
    [XmlElement("Archors")]
    public List<ArchorDev> ArchorList;
}

public class ArchorDev
{
    /// <summary>
    /// 基站ID
    /// </summary>
    [XmlAttribute("ArchorID")]
    public string ArchorID { get; set; }
    /// <summary>
    /// 基站IP
    /// </summary>
    [XmlAttribute("ArchorIP")]
    public string ArchorIp { get; set; }
    /// <summary>
    /// 基站安装区域
    /// </summary>
    [XmlAttribute("InstallArea")]
    public string InstallArea { get; set; }

    [XmlAttribute("IsConnected")]
    public string IsConnected { get; set; }

    /// <summary>
    /// 基站名称
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; }
    /// <summary>
    /// 基站型号
    /// </summary>
    [XmlAttribute("ModelType")]
    public string ModelType { get; set; }

    /// <summary>
    /// 产品序号
    /// </summary>
    [XmlAttribute("SerialNumber")]
    public string SerialNumber { get; set; }


    public override string ToString()
    {
        return string.Format("{0},{1}", ArchorID, ArchorIp);
    }
}

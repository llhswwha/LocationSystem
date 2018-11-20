using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Location.Model.InitInfos
{
    [XmlType("DepList")]
    public class LocationDeviceList
    {
        /// <summary>
        /// 区域列表
        /// </summary>
        [XmlElement("DeviceList")]
        public List<LocationDevices> DepList;
    }
    public class LocationDevices
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name;
        /// <summary>
        /// 区域列表
        /// </summary>
        [XmlElement("Device")]
        public List<LocationDevice> DevList;
    }
    public class LocationDevice
    {
        /// <summary>
        /// 基站名称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name;
        /// <summary>
        /// 基站ID
        /// </summary>
        [XmlAttribute("AnchorId")]
        public string AnchorId;
        /// <summary>
        /// X坐标
        /// </summary>
        [XmlAttribute("XPos")]
        public string XPos;
        /// <summary>
        /// Y坐标
        /// </summary>
        [XmlAttribute("YPos")]
        public string YPos;
        /// <summary>
        /// Z坐标(高度)
        /// </summary>
        [XmlAttribute("ZPos")]
        public string ZPos;

        /// <summary>
        /// 基站IP
        /// </summary>
        [XmlAttribute("IP")]
        public string IP;

        /// <summary>
        /// 厂区绝对位置X
        /// </summary>
        [XmlAttribute("AbsolutePosX")]
        public string AbsolutePosX;

        /// <summary>
        /// 厂区绝对位置Y
        /// </summary>
        [XmlAttribute("AbsolutePosY")]
        public string AbsolutePosY;

        /// <summary>
        /// 厂区绝对位置Z
        /// </summary>
        [XmlAttribute("AbsolutePosZ")]
        public string AbsolutePosZ;

    }
}

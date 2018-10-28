using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Location.Model.InitInfos
{
    [XmlType("DevBackupList")]
    public class DevInfoBackupList
    {
        [XmlElement("DeviceList")]
        public List<DevInfoBackup> DevList;
    }

    public class DevInfoBackup:IComparable<DevInfoBackup>
    {
        //设备信息
        [XmlAttribute("DevId")]
        public string DevId;
        [XmlAttribute("TypeCode")]
        public string TypeCode;
        [XmlAttribute("Name")]
        public string Name;
        [XmlAttribute("ParentName")]
        public string ParentName;
        [XmlAttribute("ParentId")]
        public int ParentId;
        [XmlAttribute("ModelName")]
        public string ModelName;
        [XmlAttribute("KKSCode")]
        public string KKSCode;

        //位置信息
        [XmlAttribute("XPos")]
        public string XPos;
        [XmlAttribute("YPos")]
        public string YPos;
        [XmlAttribute("ZPos")]
        public string ZPos;
        //角度信息
        [XmlAttribute("RotationX")]
        public string RotationX;
        [XmlAttribute("RotationY")]
        public string RotationY;
        [XmlAttribute("RotationZ")]
        public string RotationZ;
        //缩放信息
        [XmlAttribute("ScaleX")]
        public string ScaleX;
        [XmlAttribute("ScaleY")]
        public string ScaleY;
        [XmlAttribute("ScaleZ")]
        public string ScaleZ;

        public int CompareTo(DevInfoBackup other)
        {
            return (this.ParentId + Name).CompareTo(other.ParentId + other.Name);
        }
    }
}
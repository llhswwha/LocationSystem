using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.z_Test.BackUpDevInfo
{
    [XmlType("CameraBackupList")]
    public class CameraInfoBackUpList
    {

        [XmlElement("CameraDevList")]
        public List<CameraInfoBackup> DevList;
    }

    public class CameraInfoBackup
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
        [XmlAttribute("Abutment_DevID")]
        public string Abutment_DevID;
        [XmlAttribute("RtspURL")]
        public string RtspURL;

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


        //摄像头信息
        [XmlAttribute("IP")]
        public string IP;
        [XmlAttribute("UserName")]
        public string UserName;
        [XmlAttribute("PassWord")]
        public string PassWord;
        [XmlAttribute("Port")]
        public string Port;
        [XmlAttribute("CameraIndex")]
        public string CameraIndex;
    }
}


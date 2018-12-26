using DbModel.Location.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.Tools.InitInfos
{
    [XmlType("DoorAccessList")]
    public class DoorAccessList
    {
        /// <summary>
        /// 区域列表
        /// </summary>
        [XmlElement("DoorAccessDevices")]
        public List<DoorAccess> DevList;
    }

    /// <summary>
    /// 门禁信息
    /// </summary>
    public class DoorAccess
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

        /// <summary>
        /// 门禁Id
        /// </summary>
        [XmlAttribute("DoorId")]
        public string DoorId;
        /// <summary>
        /// 设备Id  GUID
        /// </summary>
        [XmlAttribute("Local_DevId")]
        public string Local_DevId;

        /// <summary>
        /// 获取DevInfo(没有ParentId)  (电子设备间|集控楼0m层  外部批量转ParentId)
        /// </summary>
        /// <returns></returns>
        public DevInfo GetDevInfoWithoutParentId()
        {
            DevInfo devInfo = new DevInfo();
            devInfo.Local_DevID = DevId;
            devInfo.Local_TypeCode = TryParseInt(TypeCode);
            devInfo.Name = Name;

            devInfo.ModelName = ModelName;
            devInfo.KKS = KKSCode;

            devInfo.PosX = TryParseFloat(XPos);
            devInfo.PosY = TryParseFloat(YPos);
            devInfo.PosZ = TryParseFloat(ZPos);

            devInfo.RotationX = TryParseFloat(RotationX);
            devInfo.RotationY = TryParseFloat(RotationY);
            devInfo.RotationZ = TryParseFloat(RotationZ);

            devInfo.ScaleX = TryParseFloat(ScaleX);
            devInfo.ScaleY = TryParseFloat(ScaleY);
            devInfo.ScaleZ = TryParseFloat(ScaleZ);
            return devInfo;
        }

        private int TryParseInt(string num)
        {
            try
            {
                int value = int.Parse(num);
                return value;
            }catch(Exception e)
            {
                return 0;
            }
        }
        private float TryParseFloat(string num)
        {
            try
            {
                float value = float.Parse(num);
                return value;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace TModel.Models.Settings
{
    [XmlType(TypeName = "SystemSetting")]
    public class UnitySetting
    {
        [XmlElement]
        public bool IsDebug;
        [XmlElement]
        public bool IsUseService;
        [XmlElement]
        public CinemachineSetting CinemachineSetting;
        [XmlElement]
        public CommunicationSetting CommunicationSetting;
        [XmlElement]
        public VersionSetting VersionSetting;
        [XmlElement]
        public RefreshSetting RefreshSetting;

        public UnitySetting()
        {
            CinemachineSetting = new CinemachineSetting();
            CommunicationSetting = new CommunicationSetting();
            VersionSetting = new VersionSetting();
            RefreshSetting = new RefreshSetting();
        }
    }
}


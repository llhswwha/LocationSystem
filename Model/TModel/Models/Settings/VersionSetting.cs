using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace TModel.Models.Settings
{
    [XmlType(TypeName = "VersionSetting")]
    public class VersionSetting
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [XmlAttribute]
        public string VersionNumber;
    }
}

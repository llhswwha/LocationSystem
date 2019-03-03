using System.Xml.Serialization;

namespace TModel.Models.Settings
{
    [XmlType(TypeName = "CinemachineSetting")]
    public class CinemachineSetting
    {
        [XmlAttribute]
        public float CMvcamFollow_X = 0;
        [XmlAttribute]
        public float CMvcamFollow_Y = 20;
        [XmlAttribute]
        public float CMvcamFollow_Z = -10;
    }
}

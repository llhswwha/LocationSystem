using System.Runtime.Serialization;
using Location.IModel.Locations;
using Location.TModel.ConvertCodes;
using System;
using Location.TModel.Location.Data;
using Location.TModel.Location.Person;
using System.Xml.Serialization;

namespace Location.TModel.Location.AreaAndDev
{

    /// <summary>
    /// 标签 即（定位卡）
    /// </summary>
    [ByName("LocationCard")]
    [DataContract] [Serializable]
    public class Tag: ITag
    {
        [XmlAttribute]
        [DataMember]
        public int Id { get; set; }

        [XmlAttribute]
        [DataMember]
        public string Code { get; set; }

        [XmlAttribute]
        [DataMember]
        public string Name { get; set; }

        [XmlAttribute]
        [DataMember]
        public string Describe { get; set; }

        private Personnel _person = null;

        //[DataMember]
        [XmlIgnore]
        public Personnel Person
        {
            get
            {
                return _person;
            }
            set
            {
                _person = value;
                if (_person != null)
                {
                    PersonId = _person.Id;
                }
            }
        }

        [DataMember]
        public int? PersonId { get; set; }

        [DataMember]
        public TagPosition Pos { get; set; }

        [DataMember]
        public int Power { get; set; }

        [XmlAttribute]
        [DataMember]
        public int PowerState { get; set; }

        [DataMember]
        public string Flag { get; set; }

        [DataMember]
        public int CardRoleId { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

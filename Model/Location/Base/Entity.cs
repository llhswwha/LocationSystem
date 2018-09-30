using Location.IModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Location.Model.Base
{
    [DataContract]
    public class Entity: IEntity
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "名称")]
        public string Name { get; set; }
    }
}
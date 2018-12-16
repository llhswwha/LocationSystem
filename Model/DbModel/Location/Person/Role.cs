using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.IModel;
using Location.TModel.Tools;

namespace DbModel.Location.Person
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role:IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        [DataMember]
        [Display(Name="角色编号")]
        [MaxLength(32)]
        public string Code { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [DataMember]
        [Display(Name="角色名")]
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Role Clone()
        {
            Role copy = new Role();
            copy = this.CloneObjectByBinary();

            return copy;
        }
    }
}

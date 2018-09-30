using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Location.Model
{
    /// <summary>
    /// 登录用户
    /// </summary>
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "登录名")]
        [Required]
        public string LoginName { get; set; }

        [DataMember]
        [Display(Name = "用户名")]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "密码")]
        [Required]
        public string Password { get; set; }

        [DataMember]
        public int? DepId { get; set; }

        [Display(Name = "所属机构")]
        public virtual Department Dep { get; set; }

        //[Display(Name = "可见机构")]
        //public virtual List<Department> VisibleDeps { get; set; }

        public User()
        {
            
        }

        public User(int id)
        {
            this.Id = id;
        }
    }
}

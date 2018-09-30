using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.IModel;

namespace Location.Model
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class Personnel:IEntityNode
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "性别")]
        public string Sex { get; set; }

        [DataMember]
        [Display(Name = "照片")]
        public string Photo { get; set; }

        [DataMember]
        [Display(Name = "出生日期")]
        public string BirthDay { get; set; }

        [DataMember]
        [Display(Name = "民族")]
        public string Nation { get; set; }

        [DataMember]
        [Display(Name = "住址")]
        public string Address { get; set; }

        [DataMember]
        [Display(Name = "标签")]
        public int? TagId { get; set; }
        public virtual Tag Tag { get; set; }

        [DataMember]
        [Display(Name = "所属部门")]
        public int? ParentId { get; set; }

        [Display(Name = "所属部门")]
        public virtual Department Parent { get; set; }

        [DataMember]
        [Display(Name = "工号")]
        public int WorkNumber { get; set; }

        [DataMember]
        [Display(Name = "岗位")]
        public int PstId { get; set; }
        public virtual Post Pst { get; set; }

        [DataMember]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }

        [DataMember]
        [Display(Name = "邮箱")]
        public string MailBox { get; set; }
    }
}

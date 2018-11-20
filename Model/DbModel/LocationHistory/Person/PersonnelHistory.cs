using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.LocationHistory.Person
{
    /// <summary>
    /// 人员历史表
    /// </summary>
    public class PersonnelHistory
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        [Display(Name = "姓名")]
        [MaxLength(16)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        [Display(Name = "性别")]
        public Sexs Sex { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [DataMember]
        [Display(Name = "照片")]
        [MaxLength(128)]
        public string Photo { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember]
        [Display(Name = "出生日期")]
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// 出生日期时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "出生日期时间戳")]
        public long? BirthTimeStamp { get; set; }

        /// <summary>
        /// Nation
        /// </summary>
        [DataMember]
        [Display(Name = "民族")]
        [MaxLength(64)]
        public string Nation { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DataMember]
        [Display(Name = "住址")]
        [MaxLength(512)]
        public string Address { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [DataMember]
        [Display(Name = "工号")]
        public int? WorkNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember]
        [Display(Name = "邮箱")]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        [Display(Name = "电话")]
        [MaxLength(16)]
        public string Phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        [Display(Name = "手机")]
        [MaxLength(16)]
        public string Mobile { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        [Display(Name = "是否启用")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        [Display(Name = "部门")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        [DataMember]
        [Display(Name = "岗位")]
        [MaxLength(16)]
        public string Pst { get; set; }

        /// <summary>
        /// 历史记录产生时间
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录产生时间")]
        public DateTime HistoryTime { get; set; }

        /// <summary>
        /// 历史记录时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "历史记录时间戳")]
        public long HistoryTimeStamp { get; set; }

        public PersonnelHistory Clone()
        {
            PersonnelHistory copy = new PersonnelHistory();
            copy = this.CloneObjectByBinary();

            return copy;

        }
    }
}

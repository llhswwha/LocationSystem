using DbModel.LocationHistory.Person;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;
using DbModel.Location.AreaAndDev;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Location.Person
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class Personnel:INode,IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
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


        //[Display(Name = "标签卡")]
        //public int? LocationCardId { get; set; }

        [Display(Name = "标签卡")]
        [NotMapped]
        public string LocationCardName { get; set; }
        //public LocationCard LocationCard { get; set; }        

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
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// 出生日期时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "出生日期时间戳")]
        public long BirthTimeStamp { get; set; }

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
        [MaxLength(64)]
        public string WorkNumber { get; set; }

        ///// <summary>
        ///// 工号
        ///// </summary>
        //[DataMember]
        //[Display(Name = "工号")]
        //[MaxLength(64)]
        //public string WorkNumberNew { get; set; }

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
        /// Rtsp地址
        /// </summary>
        [DataMember]
        [Display(Name = "Rtsp地址")]
        [MaxLength(512)]
        [NotMapped]//宝信项目需要注释这句话，四会项目需要打开
        public string RtspUrl { get; set; }

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

        [DataMember]
        [Display(Name = "部门")]
        public virtual Department Parent { get; set; }

        [DataMember]
        [Display(Name = "岗位")]
        [MaxLength(16)]
        public string Pst { get; set; }

        public Personnel()
        {
            BirthDay = DateTime.Now;
            BirthTimeStamp = TimeConvert.ToStamp(BirthDay);
            Enabled = true;
            Sex = Sexs.男;
            //Pst = "检修";
        }

        public Personnel(string name,Department parent,string num):this()
        {
            this.Name = name;
            this.ParentId = parent.Id;
            this.WorkNumber = num;
        }

        public Personnel Clone()
        {
            return this.CloneObjectByBinary();
        }

        public PersonnelHistory RemoveToHistory()
        {
            PersonnelHistory history = new PersonnelHistory();
            history.Id = this.Id;
            history.Abutment_Id = this.Abutment_Id;
            history.Name = this.Name;
            history.Sex = this.Sex;
            history.Photo = this.Photo;
            history.BirthDay = this.BirthDay;
            history.BirthTimeStamp = this.BirthTimeStamp;
            history.Nation = this.Nation;
            history.Address = this.Address;
            history.WorkNumber = this.WorkNumber;
            history.Email = this.Email;
            history.Phone = this.Phone;
            history.Mobile = this.Mobile;
            history.Enabled = this.Enabled;
            history.ParentId = this.ParentId;
            
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.ToStamp(history.HistoryTime);

            return history;
        }
    }
}

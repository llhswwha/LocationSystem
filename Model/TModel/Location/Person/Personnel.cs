using System;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.IModel;
using Location.TModel.ConvertCodes;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Tools;

namespace Location.TModel.Location.Person
{
    /// <summary>
    /// 人员信息
    /// </summary>
    [DataContract] [Serializable]
    public class Personnel:INode
    {
        private Tag _tag;

        public Personnel()
        {
            BirthDay = new DateTime(2000, 1, 1);
            BirthTimeStamp = TimeConvert.DateTimeToTimeStamp(BirthDay);
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string Sex { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [DataMember]
        public string Photo { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember]
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// 出生日期时间戳
        /// </summary>
        [DataMember]
        public long BirthTimeStamp { get; set; }

        /// <summary>
        /// Nation
        /// </summary>
        [DataMember]
        public string Nation { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [DataMember]
        public int WorkNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        [ByName("Phone")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public Department Parent { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public int? ParentId { get; set; }


        [DataMember]
        public int? TagId { get; set; }

        [DataMember]
        public virtual Tag Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                if (value != null)
                {
                    TagId = value.Id;
                }
            }
        }

        [DataMember]
        public string Pst { get; set; }

        [DataMember]
        public TagPosition Pos { get; set; }

        public Personnel Clone()
        {
            Personnel copy = new Personnel();
            copy = this.CloneObjectByBinary();

            return copy;
        }

        //public PersonnelHistory RemoveToHistory()
        //{
        //    PersonnelHistory history = new PersonnelHistory();
        //    history.Id = this.Id;
        //    history.Abutment_Id = this.Abutment_Id;
        //    history.Name = this.Name;
        //    history.Sex = this.Sex;
        //    history.Photo = this.Photo;
        //    history.BirthDay = this.BirthDay;
        //    history.BirthTimeStamp = this.BirthTimeStamp;
        //    history.Nation = this.Nation;
        //    history.Address = this.Address;
        //    history.WorkNumber = this.WorkNumber;
        //    history.Email = this.Email;
        //    history.Phone = this.Phone;
        //    history.Mobile = this.Mobile;
        //    history.Enabled = this.Enabled;
        //    history.ParentId = this.ParentId;
            
        //    history.HistoryTime = DateTime.Now;
        //    history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

        //    return history;
        //}

        public override string ToString()
        {
            return Name;
        }
    }
}

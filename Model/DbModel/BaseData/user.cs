using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.BaseData
{
    /*
     {
      "id": 3297,
      "name": "张新立",
      "gender": 0,
      "email": null,
      "mobile": "18680126558",
      "enabled": 1,
      "dep_name": "中电（四会）热电有限责任公司"
    }
    */
    /// <summary>
    /// 获取人员列表
    /// </summary>
    public class user
    {
        [Key]
        public int dbId { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [Display(Name = "标识")]
        public int id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [MaxLength(256)]
        public string name { get; set; }

        /// <summary>
        /// 性别, 0未知，1男，2女
        /// </summary>
        [Display(Name = "性别")]
        public int? gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [MaxLength(256)]
        public string email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [MaxLength(256)]
        public string phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "手机")]
        [MaxLength(256)]
        public string mobile { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool enabled { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "部门名称")]
        [MaxLength(256)]
        public string dep_name { get; set; }//不是dept_name

        
        [XmlIgnore][NotMapped]
        public org parent { get; set; }

        [XmlIgnore]
        [NotMapped]
        public member member { get; set; }

        [XmlIgnore]
        [NotMapped]
        public List<user> sameName { get; set; }

        public void AddUser(user user)
        {
            if (sameName == null)
            {
                sameName = new List<user>();
            }
            sameName.Add(user);
        }

        public void AddUsers(List<user> users)
        {
            if (sameName == null)
            {
                sameName = new List<user>();
            }
            foreach (var u in users)
            {
                if (u == this) continue;
                sameName.Add(u);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }

    public class users
    {
        public string _links { get; set; }

        public int totalCount { get; set; }

        public List<user> data { get; set; }
    }
}

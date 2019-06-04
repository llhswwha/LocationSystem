using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.Manage
{
    /// <summary>
    /// 登陆用户 权限管理用
    /// </summary>
    public class User:IEntity
    {
        public int Id { get; set; }

        [MaxLength(128)]
        [Display(Name = "用户名")]
        public string Name { get; set; }

        [MaxLength(128)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 是否加密
        /// </summary>
        [Display(Name = "是否加密")]
        public bool IsEncrypted { get; set; }

        [MaxLength(128)]
        [Display(Name = "Session")]
        public string Session { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [MaxLength(128)]
        [Display(Name = "权限")]
        public string Authority { get; set; }

        [Display(Name = "结果")]
        public bool Result { get; set; }

        /// <summary>
        /// 客户Ip
        /// </summary>
        [MaxLength(128)]
        [Display(Name = "客户IP")]
        public string ClientIp { get; set; }

        /// <summary>
        /// 客户端口
        /// </summary>
        [Display(Name = "客户端口")]
        public int ClientPort { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        [Display(Name = "登录时间")]
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 心跳包时间
        /// </summary>
        [Display(Name = "心跳包时间")]
        public DateTime LiveTime { get; set; }

        public User()
        {

        }

        public User(string name,string pass,string authority)
        {
            this.Name = name;
            this.Password = pass;
            this.Authority = authority;
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Location.Model
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "登录名")]
        public string LoginName { get; set; }

        [Display(Name = "用户名")]
        public string Name { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "所属机构")]
        public Department Dep { get; set; }

        [Display(Name = "可见机构")]
        public List<Department> SubDeps { get; set; }
    }
}

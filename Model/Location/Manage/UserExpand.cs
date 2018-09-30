using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Location.Model.Manage
{
    public class UserExpand: User
    {
        
        [Display(Name = "原始密码")]
        [NotMapped]
        public string OldPassword { get; set; }

        
        [Display(Name = "新密码")]
        [NotMapped]
        public string NewPassword { get; set; }

        
        [Display(Name = "确认密码")]
        [NotMapped]
        public string NewPassword2 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        public int Id { get; set; }

        [Display(Name="角色编号")]
        public string Code { get; set; }

        [Required]
        [Display(Name="角色名")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

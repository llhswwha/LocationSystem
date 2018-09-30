using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.IModel.Locations;

namespace Location.Model
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class Post: IPost
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "岗位名称")]
        [Required]
        public string Name { get; set; }
    }
}

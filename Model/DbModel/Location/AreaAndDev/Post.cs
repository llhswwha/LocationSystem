using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.IModel.Locations;
using Location.TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class Post: IPost
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [DataMember]
        [Display(Name = "岗位名称")]
        [MaxLength(8)]
        [Required]
        public string Name { get; set; }

        public Post Clone()
        {
            Post copy = new Post();
            copy = this.CloneObjectByBinary();
            
            return copy;
        }
    }
}

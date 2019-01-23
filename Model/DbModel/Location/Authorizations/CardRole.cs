using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.IModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Location.Authorizations
{

    public class CardRole:IEntity
    {
        [NotMapped]
        public bool IsChecked { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [MaxLength(64)]
        [Required]
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

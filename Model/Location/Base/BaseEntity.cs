using System.ComponentModel.DataAnnotations;

namespace Location.Model.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }
    }
}
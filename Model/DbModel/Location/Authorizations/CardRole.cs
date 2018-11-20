using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.IModel;
using System.ComponentModel.DataAnnotations;

namespace DbModel.Location.Authorizations
{

    public class CardRole:IEntity
    {
        public int Id { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

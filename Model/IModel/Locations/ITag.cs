using System;
using System.Collections.Generic;
using System.Text;

namespace Location.IModel.Locations
{
    public interface ITag:IEntity
    {
        string Code { get; set; }

        string Describe { get; set; }
    }
}

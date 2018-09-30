using System;
using System.Collections.Generic;
using System.Text;

namespace Location.IModel.Locations
{
    public interface ITag
    {
        int Id { get; set; }

        string Code { get; set; }

        string Name { get; set; }

        string Describe { get; set; }
    }
}

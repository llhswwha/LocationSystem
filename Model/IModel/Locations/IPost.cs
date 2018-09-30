using System;
using System.Collections.Generic;
using System.Text;

namespace Location.IModel.Locations
{
    public interface IPost
    {
        int Id { get; set; }

        string Name { get; set; }
    }
}

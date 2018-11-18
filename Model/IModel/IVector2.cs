using System;
using System.Collections.Generic;
using System.Text;

namespace IModel
{
    public interface IVector2
    {
        float X { get; set; }

        float Y { get; set; }
    }

    public interface IVector3: IVector2
    {
        float Z { get; set; }
    }
}

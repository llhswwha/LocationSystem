using System;
using System.Collections.Generic;
using System.Text;

namespace Location.IModel.Locations
{
    public interface IConfigArg:IEntity
    {
        string Key { get; set; }
        string Value { get; set; }
        string ValueType { get; set; }
        string Describe { get; set; }
        string Classify { get; set; }
    }
}

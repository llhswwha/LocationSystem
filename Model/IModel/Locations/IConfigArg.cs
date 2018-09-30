using System;
using System.Collections.Generic;
using System.Text;

namespace Location.IModel.Locations
{
    public interface IConfigArg
    {
        int Id { get; set; }
        string Name { get; set; }
        string Key { get; set; }
        string Value { get; set; }
        string ValueType { get; set; }
        string Describe { get; set; }
        string Classify { get; set; }
    }
}

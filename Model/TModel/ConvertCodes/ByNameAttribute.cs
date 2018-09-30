using System;

namespace Location.TModel.ConvertCodes
{
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    /// <summary>
    /// 别名
    /// </summary>
    public class ByNameAttribute:Attribute
    {
        public string Name { get; set; }

        public ByNameAttribute(string name)
        {
            Name = name;
        }
    }
}

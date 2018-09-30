using System;

namespace Location.TModel.ConvertCodes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DbPropertyAttribute:Attribute
    {
        public string Name { get; set; }

        public DbPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}

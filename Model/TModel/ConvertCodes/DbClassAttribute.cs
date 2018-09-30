using System;

namespace Location.TModel.ConvertCodes
{
    /// <summary>
    /// TModel对应的DbModel类，有些在整理过程中改名了。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DbClassAttribute:Attribute
    {
        public string Name { get; set; }

        public DbClassAttribute(string name)
        {
            Name = name;
        }
    }
}

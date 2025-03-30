using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class VmPropertyAttribute:Attribute
    {
        public VmPropertyAttribute()
        {
        }

        public VmPropertyAttribute(string name)
        {
            PropertyName = name;
        }

        public string PropertyName { get; set; }
    }
}

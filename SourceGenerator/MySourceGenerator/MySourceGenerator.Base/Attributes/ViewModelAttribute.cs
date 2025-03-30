using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelAttribute: Attribute
    {
        public ViewModelAttribute()
        {
            
        }
    }
}

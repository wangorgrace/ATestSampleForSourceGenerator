using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute: Attribute
    {
        public CommandAttribute()
        {
            
        }

        public CommandAttribute(string CanExcuteMethodName)
        {
            
        }

        public string CanExecuteMethod { get; set; }

        public string PropertyName {get; set;}
    }
}

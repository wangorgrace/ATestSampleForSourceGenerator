using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MySourceGenerator.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyCallMethodAttribute: Attribute
    {
        public PropertyCallMethodAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }

        public string MethodArgs { get; set; }
    }
}

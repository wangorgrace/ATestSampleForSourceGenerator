using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Models
{
    internal class PropertyToGenerator
    {

        public PropertyToGenerator(string propertyName, string propertyType, string backingField)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            BackingField = backingField;
        }
        public string PropertyName { get; }

        public string PropertyType { get; }

        public string BackingField { get; }

        public bool IsReadOnly { get; }

        public IEnumerable<PropertyMethodCall> MethodsToCall { get; set; }
    }

    internal class PropertyMethodCall
    {
        public PropertyMethodCall(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }

        public string? MethodArgs { get; set; }
    }
}

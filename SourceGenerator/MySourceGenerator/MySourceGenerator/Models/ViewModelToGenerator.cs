using System;
using System.Collections.Generic;
using System.Text;
using MySourceGenerator.Base.Attributes;

namespace MySourceGenerator.Models
{
    internal class ViewModelToGenerator
    {
        public ViewModelToGenerator(string className, string namespaceName)
        {
            ClassName = className;
            NamespaceName = namespaceName;
        }
        public string ClassName { get; }

        public string? ClassAccessModifier { get; set; }

        public string NamespaceName { get; }

        public bool InheritFromViewModelBase { get; set; }

        public List<PropertyToGenerator> PropertiesToGenerate { get; set; }
    }
}

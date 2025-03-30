using System;
using System.Collections.Generic;
using System.Text;
using MySourceGenerator.Models;

namespace MySourceGenerator.Creator
{
    internal static class ClassCreator
    {
        internal static void CreateClass(this ClassBuilder classBuilder, ViewModelToGenerator viewModelToGenerator)
        {
            classBuilder.Append($"partial class {viewModelToGenerator.ClassName}");

            if (viewModelToGenerator.InheritFromViewModelBase)
            {
                classBuilder.Append(" : global::MySourceGenerator.Base.ViewModels.ViewModelBase");
            }

            classBuilder.AppendLine();
            classBuilder.AppendLine("{");
            classBuilder.IncreaseIndent();

        }
    }
}

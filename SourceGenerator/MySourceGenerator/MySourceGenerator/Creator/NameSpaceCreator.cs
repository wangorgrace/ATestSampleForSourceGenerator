using System;
using System.Collections.Generic;
using System.Text;
using MySourceGenerator.Models;

namespace MySourceGenerator.Creator
{
    internal static class NameSpaceCreator
    {
        public static void CreateNameSpace(this ClassBuilder classBuilder, ViewModelToGenerator viewModelToGenerator) 
        {
            classBuilder.AppendLine();
            classBuilder.AppendLine($"namespace {viewModelToGenerator.NamespaceName}");
            classBuilder.AppendLine("{");
            classBuilder.IncreaseIndent();
        }
    }
}

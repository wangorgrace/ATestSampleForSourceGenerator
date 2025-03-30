using System;
using System.Collections.Generic;
using System.Text;
using MySourceGenerator.Models;

namespace MySourceGenerator.Creator
{
    internal static class PropertyCreator
    {
        public static void CreateProperties(this ClassBuilder classBuilder, IEnumerable<PropertyToGenerator> propertiesToGenerate)
        {
            if (propertiesToGenerate is not null)
            {
                foreach (var propertyToGenerator in propertiesToGenerate)
                {
                    CreateProperty(classBuilder, propertyToGenerator);
                }
            }
        }

        private static void CreateProperty(ClassBuilder classBuilder, PropertyToGenerator propertyToGenerator)
        {
            classBuilder.AppendLineBeforeMember();
            classBuilder.Append($"public {propertyToGenerator.PropertyType} {propertyToGenerator.PropertyName}");

            if (propertyToGenerator.IsReadOnly)
            {
                classBuilder.Append($" => {propertyToGenerator.BackingField}");
            }
            else
            {
                classBuilder.AppendLine();
            }

            classBuilder.AppendLine("{");
            classBuilder.IncreaseIndent();
            classBuilder.AppendLine($"get => {propertyToGenerator.BackingField};");
            classBuilder.AppendLine("set");
            classBuilder.AppendLine("{");
            classBuilder.IncreaseIndent();
            classBuilder.AppendLine($"if ({propertyToGenerator.BackingField} != value)");
            classBuilder.AppendLine("{");
            classBuilder.IncreaseIndent();
            classBuilder.AppendLine($"{propertyToGenerator.BackingField} = value;");
            classBuilder.AppendLine($"OnPropertyChanged(\"{propertyToGenerator.PropertyName}\");");

            if (propertyToGenerator.MethodsToCall is not null)
            {
                foreach (var methodToCall in propertyToGenerator.MethodsToCall)
                {
                    classBuilder.AppendLine($"{methodToCall.MethodName}({methodToCall.MethodArgs});");
                }
            }

            classBuilder.DecreaseIndent();
            classBuilder.AppendLine("}");
            classBuilder.DecreaseIndent();
            classBuilder.AppendLine("}");
            classBuilder.DecreaseIndent();
            classBuilder.AppendLine("}");
        }
    }
}

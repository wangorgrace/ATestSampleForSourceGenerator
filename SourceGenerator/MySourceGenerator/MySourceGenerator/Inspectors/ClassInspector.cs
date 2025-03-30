using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using MySourceGenerator.Models;

namespace MySourceGenerator.Inspectors
{

    internal static class ClassInspector
    {
        internal static List<PropertyToGenerator> Inspect(INamedTypeSymbol classSymbol)
        {
            var allMembers = classSymbol.GetMembers();
            List<PropertyToGenerator> propertiesToGenerate = FindPropertiesToGenerate(allMembers.Where(e=>e is IFieldSymbol).Cast<IFieldSymbol>());
            return propertiesToGenerate;

        }

        private static List<PropertyToGenerator> FindPropertiesToGenerate(IEnumerable<IFieldSymbol> fields)
        {
            List<PropertyToGenerator> propertiesToGenerate = new();
            foreach (var field in fields)
            {
                var attributeDatas = field.GetAttributes();
                var propertyAttributeData = attributeDatas.FirstOrDefault(x=>x.AttributeClass.Name == "VmPropertyAttribute");
                if (propertyAttributeData is not null)
                {
                    var propertyType = field.Type.ToString();

                    string propertyName = null;
                    var fieldName = field.Name;

                    foreach (var arg in propertyAttributeData.ConstructorArguments)
                    {
                        propertyName = arg.Value?.ToString();
                    }

                    foreach (var arg in propertyAttributeData.NamedArguments)
                    {
                        if (arg.Key == "PropertyName")
                        {
                            propertyName = arg.Value.Value?.ToString();
                        }
                    }

                    if (propertyName is null)
                    {
                        propertyName = fieldName;
                        if (propertyName.StartsWith("_"))
                        {
                            propertyName = propertyName.Substring(1);
                        }
                        else if (propertyName.StartsWith("m_"))
                        {
                            propertyName = propertyName.Substring(2);
                        }

                        var firstCharacter = propertyName.Substring(0, 1).ToUpper();

                        propertyName = propertyName.Length > 1
                            ? firstCharacter + propertyName.Substring(1)
                            : firstCharacter;
                    }

                    var methodsToCall = new List<PropertyMethodCall>();
                    Debugger.Launch();
                    var propertyCallMethodAttributes = attributeDatas.Where(x => x.AttributeClass?.ToDisplayString() == "MySourceGenerator.Base.Attributes.PropertyCallMethodAttribute").ToList();

                    foreach (var onChangeCallMethodAttribute in propertyCallMethodAttributes)
                    {
                        var methodName = onChangeCallMethodAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();

                        if (methodName is { Length: > 0 })
                        {
                            var methodCall = new PropertyMethodCall(methodName);

                            foreach (var arg in onChangeCallMethodAttribute.NamedArguments)
                            {
                                if (arg.Key == "MethodArgs")
                                {
                                    methodCall.MethodArgs = arg.Value.Value?.ToString();
                                }
                            }

                            methodsToCall.Add(methodCall);
                        }
                    }

                    propertiesToGenerate.Add(new PropertyToGenerator(propertyName, propertyType, fieldName)
                    {
                        MethodsToCall = methodsToCall
                    });
                }
            }
            return propertiesToGenerate;
        }
    }
}

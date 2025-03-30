using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using MySourceGenerator.Models;

namespace MySourceGenerator.Inspectors
{

    internal static class ClassInspector
    {
        internal static (List<PropertyToGenerator>,List<CommandToGenerator>) Inspect(INamedTypeSymbol classSymbol)
        {
            Debugger.Launch();
            var allMembers = classSymbol.GetMembers();
            List<PropertyToGenerator> propertiesToGenerate = FindPropertiesToGenerate(allMembers.Where(e=>e is IFieldSymbol).Cast<IFieldSymbol>());
            List<CommandToGenerator> commandsToGenerate = FindCommandsToGenerate(allMembers.Where(e=>e is IMethodSymbol).Cast<IMethodSymbol>(),allMembers);
            return (propertiesToGenerate,commandsToGenerate);

        }

        private static List<CommandToGenerator> FindCommandsToGenerate(IEnumerable<IMethodSymbol> methods, ImmutableArray<ISymbol> allMembers)
        {
            List<CommandToGenerator> commandsToGenerate = new();
            foreach (var method in methods)
            {
                var methodAttributes = method.GetAttributes();
                var commandAttributeData =
                    methodAttributes.FirstOrDefault(x => x.AttributeClass.ToDisplayString() == "MySourceGenerator.Base.Attributes.CommandAttribute");


                if (commandAttributeData is not null)
                {
                    var executeMethodInfo = new CommandMethod(method.Name)
                    {
                        HasParameter = method.Parameters.Any()
                    };

                    var commandPropertyName = $"{method.Name}Command";
                    var canExecuteMethodName = commandAttributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();

                    foreach (var arg in commandAttributeData.NamedArguments)
                    {
                        if (arg.Key == "CanExecuteMethod")
                        {
                            canExecuteMethodName = arg.Value.Value?.ToString();
                        }
                        else if (arg.Key == "PropertyName")
                        {
                            commandPropertyName = arg.Value.Value?.ToString() ?? commandPropertyName;
                        }
                    }

                    CommandMethod? canExecuteMethodInfo = null;

                    if (canExecuteMethodName is not null)
                    {
                        var canExecuteMethodSymbol = allMembers.OfType<IMethodSymbol>().FirstOrDefault(x => x.Name == canExecuteMethodName);
                        if (canExecuteMethodSymbol is not null)
                        {
                            canExecuteMethodInfo = new CommandMethod(canExecuteMethodSymbol.Name)
                            {
                                HasParameter = canExecuteMethodSymbol.Parameters.Any()
                            };
                        }
                    }

                    commandsToGenerate.Add(
                        new CommandToGenerator(executeMethodInfo, commandPropertyName)
                        {
                            CanExecuteMethod = canExecuteMethodInfo
                        });
                }
            }

            return commandsToGenerate;
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

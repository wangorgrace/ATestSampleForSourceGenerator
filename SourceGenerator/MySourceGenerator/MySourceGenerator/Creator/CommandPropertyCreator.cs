using MySourceGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Creator
{
    internal static class CommandPropertyCreator
    {
        internal static void CreateCommandProperties(this ClassBuilder vmBuilder,
            IEnumerable<CommandToGenerator>? commandsToGenerate,
            string commandType)
        {
            if (commandsToGenerate is not null)
            {
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.AppendLineBeforeMember();
                    vmBuilder.Append($"public ICommand {commandToGenerate.PropertyName} => {commandToGenerate.FieldName} ??= new {commandType}({GetMethodCall(commandToGenerate.ExecuteMethod)}");
                    if (commandToGenerate.CanExecuteMethod is not null)
                    {
                        vmBuilder.Append($", {GetMethodCall(commandToGenerate.CanExecuteMethod)}");
                    }

                    vmBuilder.AppendLine(");");
                }
            }
        }

        private static object GetMethodCall(CommandMethod methodInfo)
        {
            return methodInfo switch
            {
                { HasParameter: true } => $"{methodInfo.Name}",
                { HasParameter: false } => $"_ => {methodInfo.Name}()",
            };
        }
    }
}

using MySourceGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySourceGenerator.Creator
{
    internal static class CommandFieldCreator
    {
        internal static void CreateCommandFields(this ClassBuilder vmBuilder, IEnumerable<CommandToGenerator>? commandsToGenerate)
        {
            if (commandsToGenerate is not null && commandsToGenerate.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.AppendLine($"private ICommand? {commandToGenerate.FieldName};");
                }
            }
        }
    }
}

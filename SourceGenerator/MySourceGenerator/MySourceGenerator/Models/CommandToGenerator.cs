using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Models
{
    internal class CommandToGenerator
    {
        public CommandToGenerator(CommandMethod executeMethod, string propertyName)
        {
            ExecuteMethod = executeMethod;
            PropertyName = propertyName;
            FieldName = $"_{PropertyName.Substring(0, 1).ToLower()}{PropertyName.Substring(1)}";
        }

        public CommandMethod ExecuteMethod { get; }

        public string PropertyName { get; set; }
        public string FieldName { get; }

        public CommandMethod? CanExecuteMethod { get; set; }

        public string CommandType { get; set; } = "DelegateCommand";
    }

    internal class CommandMethod
    {
        public CommandMethod(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool HasParameter { get; set; }
    }
}

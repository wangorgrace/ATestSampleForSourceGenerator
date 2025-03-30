using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGenerator.Creator
{
    internal static class UsingDirectiveCreator
    {
        internal static void CreateUsingDirectives(this ClassBuilder classBuilder, List<string> usingDirectives)
        {
            if(usingDirectives is not null)
            {
                foreach (var usingDirective in usingDirectives)
                {
                    classBuilder.AppendLine($"using {usingDirective};");
                }
            }

            classBuilder.AppendLine("using MySourceGenerator.Base.ViewModels;");
        }
    }
}

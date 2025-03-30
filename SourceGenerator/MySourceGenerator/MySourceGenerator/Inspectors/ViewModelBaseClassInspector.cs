using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace MySourceGenerator.Inspectors
{
    internal class ViewModelBaseClassInspector
    {
        internal static bool Inspect(INamedTypeSymbol viewModelBaseClassSymbol, INamedTypeSymbol viewModelClassSymbol)
        {
            return !ViewModelInheritsAlreadyFromViewModelBase(viewModelClassSymbol, viewModelBaseClassSymbol);
        }

        private static bool ViewModelInheritsAlreadyFromViewModelBase(INamedTypeSymbol viewModelClassSymbol,
            INamedTypeSymbol viewModelBaseClassSymbol)
        {
            var inherits = false;

            var currentBaseType = viewModelClassSymbol.BaseType;

            while (currentBaseType is not null)
            {
                if (currentBaseType.Equals(viewModelBaseClassSymbol, SymbolEqualityComparer.Default)
                    || currentBaseType.GetAttributes().Any(x => x.AttributeClass?.ToDisplayString() == "MySourceGenerator.Base.Attributes.ViewModelAttribute"))
                {
                    inherits = true;
                    break;
                }
                currentBaseType = currentBaseType.BaseType;
            }

            return inherits;
        }
    }
}

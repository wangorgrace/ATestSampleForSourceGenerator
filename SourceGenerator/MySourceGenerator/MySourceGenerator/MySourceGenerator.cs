using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MySourceGenerator.Creator;
using MySourceGenerator.Inspectors;
using MySourceGenerator.Models;

namespace MySourceGenerator
{
    [Generator]
    public class MySourceGenerator: IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var viewModelsToGenerate = context.SyntaxProvider.CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTarget(s),
                    transform: static (s, _) => GetSemanticTarget(s))
                .Where(static target => target is not null);

            context.RegisterSourceOutput(viewModelsToGenerate,
                static (spc, source) => Execute(spc, source));
        }

        private static void Execute(SourceProductionContext spc, ViewModelToGenerator viewModelToGenerate)
        {
            //Debugger.Launch();
            if (viewModelToGenerate is null)
            {
                return;
            }

            var fileName = $"{viewModelToGenerate.NamespaceName}.{viewModelToGenerate.ClassName}.g.cs";
            var classBuilder = new ClassBuilder();
            classBuilder.CreateUsingDirectives(null);
            classBuilder.CreateNameSpace(viewModelToGenerate);
            classBuilder.CreateClass(viewModelToGenerate);
            classBuilder.CreateProperties(viewModelToGenerate.PropertiesToGenerate);

            while (classBuilder.IndentLevel > 1) // Keep the namespace open for a ViewModel interface and/or a factory class
            {
                classBuilder.DecreaseIndent();
                classBuilder.AppendLine("}");
            }

            while (classBuilder.DecreaseIndent())
            {
                classBuilder.AppendLine("}");
            }

            var sourceText = SourceText.From(classBuilder.ToString(), Encoding.UTF8);

            spc.AddSource(fileName, sourceText);
        }

        private static ViewModelToGenerator GetSemanticTarget(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
            var viewModelClassSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            var viewModelBaseClassSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("MySourceGenerator.Base.ViewModels.ViewModelBase");

            var viewModelAttributeData = viewModelClassSymbol?.GetAttributes().FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MySourceGenerator.Base.Attributes.ViewModelAttribute");

            if (viewModelClassSymbol is null
                || viewModelClassSymbol.ContainingNamespace is null
                || viewModelAttributeData is null
                || viewModelBaseClassSymbol is null)
            {
                return null;
            }

            var result =ClassInspector.Inspect(viewModelClassSymbol as INamedTypeSymbol);

            var accessModifier = viewModelClassSymbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                _ => ""
            };
            var viewModelToGenerate = new ViewModelToGenerator(
                viewModelClassSymbol.Name,
                viewModelClassSymbol.ContainingNamespace.ToDisplayString())
            {
                ClassAccessModifier = accessModifier,
                PropertiesToGenerate = result.ToList(),
                InheritFromViewModelBase = ViewModelBaseClassInspector.Inspect(viewModelClassSymbol as INamedTypeSymbol, viewModelBaseClassSymbol)
            };
            return viewModelToGenerate;
        }

        private static bool IsSyntaxTarget(SyntaxNode s) => s is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } };
    }
}

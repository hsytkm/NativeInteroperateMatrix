using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Matrix.SourceGenerator
{
    [Generator]
    public sealed class MatrixGenerator : ISourceGenerator
    {
        private const string _attributeName = nameof(MatrixGenerator) + "Attribute";

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                //System.Diagnostics.Debugger.Launch();
            }
#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation is not CSharpCompilation) return;

            var attrCode = new MatrixGeneratorAttributeTemplate().TransformText();
            context.AddSource(_attributeName + ".cs", attrCode);

            try
            {
                if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

                foreach (var declaration in receiver.Targets)
                {
                    var model = context.Compilation.GetSemanticModel(declaration.SyntaxTree);
                    var typeSymbol = model.GetDeclaredSymbol(declaration);
                    if (typeSymbol is null) continue;

                    var template = new CodeTemplate(declaration)
                    {
                        Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
                    };

                    if (context.CancellationToken.IsCancellationRequested) return;

                    var text = template.TransformText();
                    context.AddSource(typeSymbol.GenerateHintName(), text);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private sealed class SyntaxReceiver : ISyntaxReceiver
        {
            internal List<TypeDeclarationSyntax> Targets { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                static bool HasReadOnlyKind(TypeDeclarationSyntax typeDeclarationSyntax)
                    => typeDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.ReadOnlyKeyword));

                TypeDeclarationSyntax? typeDeclarationSyntax = syntaxNode switch
                {
                    StructDeclarationSyntax structDecl => structDecl,   // for struct
                    RecordDeclarationSyntax recordDecl => recordDecl,   // for record struct
                    _ => null
                };
                if (typeDeclarationSyntax is null)
                    return;

                if (!HasReadOnlyKind(typeDeclarationSyntax))
                    return;

                var attr = typeDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes)
                    .FirstOrDefault(x => x.Name.ToString() is nameof(MatrixGenerator) or _attributeName);
                if (attr is null)
                    return;

                Targets.Add(typeDeclarationSyntax);
            }

        }
    }
}

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Matrix.SourceGenerator
{
    public partial class CodeTemplate
    {
        internal string Namespace { get; set; } = "";
        internal string ClassName { get; }
        internal string ItemTypeName { get; } = "double";


        internal CodeTemplate(StructDeclarationSyntax declaration)
        {
            ClassName = declaration.GetGenericTypeName();
        }

    }
}

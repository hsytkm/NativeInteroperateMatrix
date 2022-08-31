using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Matrix.SourceGenerator
{
    public partial class CodeTemplate
    {
        internal string Namespace { get; set; } = "";
        private string MatrixClassName { get; }
        private string ValueItemTypeName { get; }
        private string ContainerClassName { get; }

        internal CodeTemplate(TypeDeclarationSyntax declaration)
        {
            MatrixClassName = declaration.GetGenericTypeName();     // DoubleMatrix
            ContainerClassName = MatrixClassName + "Container";     // DoubleMatrixContainer
            ValueItemTypeName = GetBuiltInTypeName(MatrixClassName.Replace("Matrix", ""));  // double
        }

        // DoubleMatrix -> double
        private static string GetBuiltInTypeName(string source) => source switch
        {
            "Byte" => "Byte",
            "Int8" => "SByte",
            "Int16" => "Int16",
            "Int32" => "Int32",
            "Int64" => "Int64",
            "Single" => "Single",
            "Double" => "Double",
            _ => source,
        };

    }
}

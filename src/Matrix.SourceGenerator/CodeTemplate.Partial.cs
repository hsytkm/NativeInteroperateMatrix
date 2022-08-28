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
            "Byte" => "byte",
            "Int8" => "sbyte",
            "Int16" => "short",
            "Int32" => "int",
            "Int64" => "long",
            "Single" => "float",
            "Double" => "double",
            _ => source,
        };

    }
}

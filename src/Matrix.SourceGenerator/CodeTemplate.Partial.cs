using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Matrix.SourceGenerator
{
    public partial class CodeTemplate
    {
        internal string Namespace { get; set; } = "";
        private string MatrixClassName { get; }
        private string ValueItemTypeName { get; }
        private string ContainerClassName { get; }

        internal CodeTemplate(StructDeclarationSyntax declaration)
        {
            MatrixClassName = declaration.GetGenericTypeName();     // DoubleMatrix
            ContainerClassName = MatrixClassName + "Container";     // DoubleMatrixContainer
            ValueItemTypeName = GetBuiltInTypeName(MatrixClassName.Replace("Matrix", ""));  // double
        }

        // DoubleMatrix -> double
        private static string GetBuiltInTypeName(string source) => source switch
        {
            "Byte" => "byte",
            "Int16" => "short",
            "Int32" => "int",
            "Int64" => "long",
            "Single" => "float",
            "Double" => "double",
            _ => source,
        };

    }
}

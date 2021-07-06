﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン: 16.0.0.0
//  
//     このファイルへの変更は、正しくない動作の原因になる可能性があり、
//     コードが再生成されると失われます。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Matrix.SourceGenerator
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class CodeTemplate : CodeTemplateBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// <auto-generated>\r\n// THIS (.cs) FILE IS GENERATED BY Matrix.SourceGenerator. D" +
                    "O NOT CHANGE IT.\r\n// </auto-generated>\r\n#nullable enable\r\nusing System;\r\nusing S" +
                    "ystem.Collections.Generic;\r\nusing System.Runtime.InteropServices;\r\n\r\n");
 if (!string.IsNullOrEmpty(Namespace)) { 
            this.Write("namespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write("\r\n");
 } 
            this.Write("{\r\n    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (4 * 4))]\r\n    r" +
                    "eadonly partial struct ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" : IMatrix<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">, IEquatable<");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(@">
    {
        private readonly IntPtr _pointer;
        //private readonly int _allocSize;  // = rows * stride;
        private readonly int _rows;         // height
        private readonly int _columns;      // width
        private readonly int _bytesPerItem;
        private readonly int _stride;

        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write("(IntPtr intPtr, int rows, int columns, int bytesPerItem, int stride)\r\n        {\r\n" +
                    "            if (IntPtr.Size != 8)\r\n                throw new NotSupportedExcepti" +
                    "on();\r\n\r\n            if (bytesPerItem != Marshal.SizeOf<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@">())
                throw new ArgumentException(nameof(bytesPerItem));

            _pointer = intPtr;
            _rows = rows;
            _columns = columns;
            _bytesPerItem = bytesPerItem;
            _stride = stride;
        }

        // IMatrix<T>
        public IntPtr Pointer => _pointer;
        public int Rows => _rows;
        public int Columns => _columns;
        public int BytesPerItem => _bytesPerItem;
        public int BitsPerItem => _bytesPerItem * 8;
        public int Stride => _stride;
        public int Width => _columns;
        public int Height => _rows;
        public int AllocatedSize => _columns * _bytesPerItem * _rows;  // don't use stride
        public bool IsContinuous => (_columns * _bytesPerItem) == _stride;
        public bool IsValid
        {
            get
            {
                if (_pointer == IntPtr.Zero) return false;
                if (_columns <= 0 || _rows <= 0) return false;
                if (_stride < _columns * _bytesPerItem) return false;
                if (AllocatedSize < _columns * _bytesPerItem * _rows) return false;
                return true;    //valid
            }
        }
        public bool IsInvalid => !IsValid;

        public unsafe Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write("> GetRowSpan(int row)\r\n        {\r\n            if (row < 0 || _rows - 1 < row)\r\n  " +
                    "              throw new ArgumentException(\"Invalid row\");\r\n\r\n            var ptr" +
                    " = _pointer + (row * _stride);\r\n            return new Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">(ptr.ToPointer(), _columns);\r\n        }\r\n        public ReadOnlySpan<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@"> GetRoRowSpan(int row) => GetRowSpan(row);

        private IntPtr GetIntPtr(int row, int column)
        {
            if (row < 0 || _rows - 1 < row || column < 0 || _columns - 1 < column)
                throw new ArgumentException(""Out of range."");

            return _pointer + (row * _stride) + (column * _bytesPerItem);
        }
                
        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(" ReadValue(int row, int column)\r\n        {\r\n            if (row < 0 || _rows - 1 " +
                    "< row || column < 0 || _columns - 1 < column)\r\n                throw new Argumen" +
                    "tException(\"Out of range.\");\r\n\r\n            return UnsafeHelper.ReadStructureFro" +
                    "mPtr<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">(GetIntPtr(row, column));\r\n        }\r\n\r\n        public void WriteValue(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@" value, int row, int column)
        {
            if (row < 0 || _rows - 1 < row || column < 0 || _columns - 1 < column)
                throw new ArgumentException(""Out of range."");

            UnsafeHelper.WriteStructureToPtr(GetIntPtr(row, column), value);
        }

        // IEquatable<T>
        public bool Equals(");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" other) => this == other;\r\n        public override bool Equals(object? obj) => (o" +
                    "bj is ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" other) && Equals(other);\r\n        public override int GetHashCode() => HashCode." +
                    "Combine(_pointer, _rows, _columns, _bytesPerItem, _stride);\r\n        public stat" +
                    "ic bool operator ==(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" left, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" right)\r\n             => (left._pointer, left._rows, left._columns, left._bytesPe" +
                    "rItem, left._stride)\r\n                == (right._pointer, right._rows, right._co" +
                    "lumns, right._bytesPerItem, right._stride);\r\n        public static bool operator" +
                    " !=(in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" left, in ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" right) => !(left == right);\r\n\r\n        public override string ToString() => $\"Ro" +
                    "ws={_rows}, Cols={_columns}, Pointer=0x{_pointer:x16}\";\r\n\r\n    }\r\n\r\n    public c" +
                    "lass ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write(" : MatrixContainerBase<");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">\r\n    {\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns, bool initialize = true)\r\n            : base(rows, columns" +
                    ", initialize)\r\n        { }\r\n\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns, IReadOnlyCollection<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@"> items)
            : base(rows, columns, false)
        {
            if (rows * columns != items.Count)
                throw new ArgumentException(""Collection is short."", nameof(items));

            var row = 0;
            var column = 0;
            Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@"> span = Matrix.GetRowSpan(row);

            foreach (var item in items)
            {
                if (column >= columns)
                {
                    column = 0;
                    row++;
                    span = Matrix.GetRowSpan(row);
                }
                span[column++] = item;
            }
        }

        protected override ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" CreateMatrix(IntPtr intPtr, int width, int height, int bytesPerData, int stride)" +
                    "\r\n        {\r\n            return new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write("(intPtr, width, height, bytesPerData, stride);\r\n        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class CodeTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}

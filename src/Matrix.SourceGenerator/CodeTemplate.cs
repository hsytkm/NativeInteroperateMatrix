﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン: 17.0.0.0
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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class CodeTemplate : CodeTemplateBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// <auto-generated>\r\n// THIS (.cs) FILE IS GENERATED BY Matrix.SourceGenerator. D" +
                    "O NOT CHANGE IT.\r\n// </auto-generated>\r\n#nullable enable\r\nusing System;\r\nusing S" +
                    "ystem.Runtime.CompilerServices;\r\nusing System.Runtime.InteropServices;\r\n\r\n");
 if (!string.IsNullOrEmpty(Namespace)) { 
            this.Write("namespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write("\r\n");
 } 
            this.Write("{\r\n    // Do not change the order of the struct because it is the same as C++\r\n  " +
                    "  [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8 + (5 * 4))]\r\n    parti" +
                    "al record struct ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" : IMatrix<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@">
    {
        readonly IntPtr _pointer;
        readonly int _allocSize;
        readonly int _rows;         // height
        readonly int _columns;      // width
        readonly int _bytesPerItem;
        readonly int _stride;

        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write("(IntPtr pointer, int allocSize, int rows, int columns, int bytesPerItem, int stri" +
                    "de)\r\n        {\r\n            if (IntPtr.Size != 8)\r\n                throw new Not" +
                    "SupportedException(\"Must be x64\");\r\n\r\n            if (bytesPerItem != Unsafe.Siz" +
                    "eOf<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">())\r\n                throw new ArgumentException(nameof(bytesPerItem));\r\n\r\n     " +
                    "       _pointer = pointer;\r\n            _allocSize = allocSize;\r\n            _ro" +
                    "ws = rows;\r\n            _columns = columns;\r\n            _bytesPerItem = bytesPe" +
                    "rItem;\r\n            _stride = stride;\r\n        }\r\n\r\n        [MethodImpl(MethodIm" +
                    "plOptions.AggressiveInlining)]\r\n        void ThrowInvalidRow(int row)\r\n        {" +
                    "\r\n            if (row < 0 || _rows - 1 < row)\r\n                throw new Argumen" +
                    "tException($\"Invalid row={row}\");\r\n        }\r\n\r\n        [MethodImpl(MethodImplOp" +
                    "tions.AggressiveInlining)]\r\n        void ThrowInvalidColumn(int column)\r\n       " +
                    " {\r\n            if (column < 0 || _columns - 1 < column)\r\n                throw " +
                    "new ArgumentException($\"Invalid column={column}\");\r\n        }\r\n        \r\n       " +
                    " [MethodImpl(MethodImplOptions.AggressiveInlining)]\r\n        void ThrowInvalidRo" +
                    "wColumn(int row, int column)\r\n        {\r\n            ThrowInvalidRow(row);\r\n    " +
                    "        ThrowInvalidColumn(column);\r\n        }\r\n\r\n        [MethodImpl(MethodImpl" +
                    "Options.AggressiveInlining)]\r\n        IntPtr GetIntPtr(int row, int column)\r\n   " +
                    "     {\r\n            ThrowInvalidRowColumn(row, column);\r\n            return _poi" +
                    "nter + (row * _stride) + (column * _bytesPerItem);\r\n        }\r\n\r\n        // INat" +
                    "iveMemory\r\n        public IntPtr Pointer => _pointer;\r\n        public int Alloca" +
                    "tedSize => _allocSize;\r\n        public int BytesPerItem => _bytesPerItem;\r\n     " +
                    "   public int BitsPerItem => _bytesPerItem * 8;\r\n        public bool IsValid\r\n  " +
                    "      {\r\n            get\r\n            {\r\n                if (_pointer == IntPtr." +
                    "Zero) return false;\r\n                if (_columns <= 0 || _rows <= 0) return fal" +
                    "se;\r\n                if (_stride < _columns * _bytesPerItem) return false;\r\n    " +
                    "            return true;    //valid\r\n            }\r\n        }\r\n\r\n        // IMat" +
                    "rix\r\n        public int Rows => _rows;\r\n        public int Columns => _columns;\r" +
                    "\n        public int Stride => _stride;\r\n        public bool IsContinuous => (_co" +
                    "lumns * _bytesPerItem) == _stride;\r\n\r\n        // IMatrix<T>\r\n        public unsa" +
                    "fe ref ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(" this[int row, int column]\r\n        {\r\n            get\r\n            {\r\n          " +
                    "      IntPtr ptr = GetIntPtr(row, column);\r\n                return ref Unsafe.As" +
                    "Ref<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">(ptr.ToPointer());\r\n            }\r\n        }\r\n\r\n        public unsafe Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write("> AsSpan()\r\n        {\r\n            int size = _rows * _stride;\r\n            retur" +
                    "n new Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">(_pointer.ToPointer(), size);\r\n        }\r\n\r\n        public unsafe Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write("> AsRowSpan(int row)\r\n        {\r\n            ThrowInvalidRow(row);\r\n\r\n           " +
                    " IntPtr ptr = _pointer + (row * _stride);\r\n            return new Span<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">(ptr.ToPointer(), _columns);\r\n        }\r\n\r\n        public override string ToStri" +
                    "ng() => $\"Rows={_rows}, Cols={_columns}, Pointer=0x{_pointer:x16}\";\r\n    }\r\n\r\n  " +
                    "  public /*sealed*/ partial class ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write(" : IMatrixContainer<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">, IDisposable\r\n    {\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write(" Matrix { get; }\r\n        public IMatrix<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write("> MatrixT => (IMatrix<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@">)Matrix;

        readonly record struct PointerSizePair
        {
            public static readonly PointerSizePair Zero = new(IntPtr.Zero, 0);
            public IntPtr Pointer { get; }
            public int Size { get; }
            public PointerSizePair(IntPtr ptr, int size) => (Pointer, Size) = (ptr, size);
            public void Deconstruct(out IntPtr ptr, out int size) => (ptr, size) = (Pointer, Size);
        }

        PointerSizePair _allocatedMemory;
        
        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns, bool initialize)\r\n        {\r\n            int bytesPerData" +
                    " = Unsafe.SizeOf<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@">();
            int stride = columns * bytesPerData;
            _allocatedMemory = Alloc(stride * rows);
            (IntPtr ptr, int allocSize) = _allocatedMemory;

            if (initialize)
            {
                UnsafeUtils.FillZero(ptr, allocSize);
            }
            
            Matrix = new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MatrixClassName));
            this.Write("(ptr, allocSize, rows, columns, bytesPerData, stride);\r\n        }\r\n\r\n        publ" +
                    "ic ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns)\r\n            : this(rows, columns, true)\r\n        { }\r\n\r\n" +
                    "        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns, IEnumerable<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@"> items)
            : this(rows, columns, false)
        {
            int length = rows * columns;
            int written = 0;
            int row = 0;
            int column = 0;
            var span = Matrix.AsRowSpan(0);

            foreach (var item in items)
            {
                if (column >= columns)
                {
                    column = 0;
                    row++;
                    span = Matrix.AsRowSpan(row);
                }
                span[column++] = item;

                if (++written > length)
                    throw new ArgumentException(""items is large."", nameof(items));
            }

            if (!(row == rows - 1 && column == columns))
                throw new ArgumentException(""items is small."", nameof(items));
        }
        
        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns, ReadOnlySpan<");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(@"> items)
            : this(rows, columns, false)
        {
            int length = rows * columns;
            int written = 0;
            int row = 0;
            int column = 0;
            var span = Matrix.AsRowSpan(0);

            foreach (var item in items)
            {
                if (column >= columns)
                {
                    column = 0;
                    row++;
                    span = Matrix.AsRowSpan(row);
                }
                span[column++] = item;

                if (++written > length)
                    throw new ArgumentException(""items is large."", nameof(items));
            }

            if (!(row == rows - 1 && column == columns))
                throw new ArgumentException(""items is small."", nameof(items));
        }
        
");
 if (ValueItemTypeName != "Byte") { 
            this.Write("        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("(int rows, int columns, ReadOnlySpan<byte> items)\r\n            : this(rows, colum" +
                    "ns, MemoryMarshal.Cast<byte, ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ValueItemTypeName));
            this.Write(">(items))\r\n        { }\r\n");
 } 
            this.Write(@"
        static PointerSizePair Alloc(int size)
        {
            IntPtr intPtr;
#if NET6_0_OR_GREATER
            unsafe { intPtr = (IntPtr)NativeMemory.Alloc((nuint)size); }
#else
            intPtr = Marshal.AllocCoTaskMem(size);
#endif
            GC.AddMemoryPressure(size);
            return new(intPtr, size);
        }

        static void Free(in PointerSizePair pair)
        {
#if NET6_0_OR_GREATER
            unsafe { NativeMemory.Free((void*)pair.Pointer); }
#else
            Marshal.FreeCoTaskMem(pair.Pointer);
#endif
            GC.RemoveMemoryPressure(pair.Size);
        }
        
        bool _disposedValue;
        public void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            if (_allocatedMemory != PointerSizePair.Zero)
            {
                Free(_allocatedMemory);
                _allocatedMemory = PointerSizePair.Zero;
            }

            _disposedValue = true;
        }

        ~");
            this.Write(this.ToStringHelper.ToStringWithCulture(ContainerClassName));
            this.Write("() => Dispose(false);\r\n\r\n        public void Dispose()\r\n        {\r\n            Di" +
                    "spose(true);\r\n            GC.SuppressFinalize(this);\r\n        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
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

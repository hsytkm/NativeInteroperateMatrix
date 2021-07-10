using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Nima.Core.Tests.Common
{
    /// <summary>
    /// 初期値あり ctor のテスト（代表して byte 型のみ）
    /// </summary>
    public class X64Test
    {
        [Fact]
        public void X64()
        {
            // Nima は 64bit 想定です。 (StructLayout.Size = 8 にしています)
            Marshal.SizeOf<IntPtr>().Is(8);
            Marshal.SizeOf<nint>().Is(8);
        }

    }
}

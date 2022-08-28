#pragma once
#include "MatrixBase.h"
#include "PixelBgr.h"

namespace NimaNativeCore {

    // 構造体に定義を追加しないこと
    struct ByteMatrix : MatrixBase<unsigned char> { };  // byte in C#

    // 構造体に定義を追加しないこと
    struct Int8Matrix : MatrixBase<char> { };           // sbyte in C#

    // 構造体に定義を追加しないこと
    struct Int16Matrix : MatrixBase<short> { };         // short in C#

    // 構造体に定義を追加しないこと
    struct Int32Matrix : MatrixBase<long> { };          // int in C#

    // 構造体に定義を追加しないこと
    struct Int64Matrix : MatrixBase<long long> { };     // long in C#

    // 構造体に定義を追加しないこと
    struct SingleMatrix : MatrixBase<float> { };        // float in C#

    // 構造体に定義を追加しないこと
    struct DoubleMatrix : MatrixBase<double> { };       // double in C#

    // 構造体に定義を追加しないこと
    struct PixelBgrMatrix : MatrixBase<PixelBgr> { };   // PixelBgr in C#
}

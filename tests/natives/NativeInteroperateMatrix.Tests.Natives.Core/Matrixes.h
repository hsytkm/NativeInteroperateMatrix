#pragma once
#include "MatrixBase.h"
#include "PixelBgr.h"
#include <cstdint>

namespace NimaNativeCore {

    // 構造体に定義を追加しないこと
    struct ByteMatrix : MatrixBase<uint8_t> { };    // byte in C#

    // 構造体に定義を追加しないこと
    struct Int8Matrix : MatrixBase<int8_t> { };     // sbyte in C#

    // 構造体に定義を追加しないこと
    struct Int16Matrix : MatrixBase<int16_t> { };   // short in C#

    // 構造体に定義を追加しないこと
    struct Int32Matrix : MatrixBase<int32_t> { };   // int in C#

    // 構造体に定義を追加しないこと
    struct Int64Matrix : MatrixBase<int64_t> { };   // long in C#

    // 構造体に定義を追加しないこと
    struct SingleMatrix : MatrixBase<float> { };    // float in C#

    // 構造体に定義を追加しないこと
    struct DoubleMatrix : MatrixBase<double> { };   // double in C#

    // 構造体に定義を追加しないこと
    struct PixelBgrMatrix : MatrixBase<PixelBgr> { };   // PixelBgr in C#
}

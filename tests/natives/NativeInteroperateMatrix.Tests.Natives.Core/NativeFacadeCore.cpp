#include "pch.h"
#include "Matrixes.h"

using namespace NimaNativeCore;

// Byte
DllExport int64_t SumByteMatrix(ByteMatrix& matrix)
{
    return matrix.get_sum_int64();
}
DllExport void ClearByteMatrix(ByteMatrix& matrix)
{
    matrix.fill_value(0);
}

// Int16
DllExport int64_t SumInt16Matrix(Int16Matrix& matrix)
{
    return matrix.get_sum_int64();
}
DllExport void ClearInt16Matrix(Int16Matrix& matrix)
{
    matrix.fill_value(0);
}

// Int32
DllExport int64_t SumInt32Matrix(Int32Matrix& matrix)
{
    return matrix.get_sum_int64();
}
DllExport void ClearInt32Matrix(Int32Matrix& matrix)
{
    matrix.fill_value(0);
}

// Int64
DllExport int64_t SumInt64Matrix(Int64Matrix& matrix)
{
    return matrix.get_sum_int64();
}
DllExport void ClearInt64Matrix(Int64Matrix& matrix)
{
    matrix.fill_value(0);
}

// Single
DllExport double SumSingleMatrix(SingleMatrix& matrix)
{
    return matrix.get_sum_double();
}
DllExport void ClearSingleMatrix(SingleMatrix& matrix)
{
    matrix.fill_value(0);
}

// Double
DllExport double SumDoubleMatrix(DoubleMatrix& matrix)
{
    return matrix.get_sum_double();
}
DllExport void ClearDoubleMatrix(DoubleMatrix& matrix)
{
    matrix.fill_value(0);
}

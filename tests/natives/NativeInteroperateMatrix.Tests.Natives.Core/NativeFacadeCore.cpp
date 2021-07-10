#include "pch.h"
#include "Matrixes.h"

using namespace NimaNativeCore;

//
DllExport long long SumInt8Matrix(Int8Matrix* matrix)
{
    return matrix->get_sum_long();
}
DllExport void ClearInt8Matrix(Int8Matrix* matrix)
{
    matrix->fill_value(0);
}

//
DllExport long long SumInt16Matrix(Int16Matrix* matrix)
{
    return matrix->get_sum_long();
}
DllExport void ClearInt16Matrix(Int16Matrix* matrix)
{
    matrix->fill_value(0);
}

//
DllExport long long SumInt32Matrix(Int32Matrix* matrix)
{
    return matrix->get_sum_long();
}
DllExport void ClearInt32Matrix(Int32Matrix* matrix)
{
    matrix->fill_value(0);
}

//
DllExport long long SumInt64Matrix(Int64Matrix* matrix)
{
    return matrix->get_sum_long();
}
DllExport void ClearInt64Matrix(Int64Matrix* matrix)
{
    matrix->fill_value(0);
}

#if false
//
DllExport double SumSingleMatrix(SingleMatrix* matrix)
{
    return matrix->get_sum_double();
}
DllExport void ClearSingleMatrix(SingleMatrix* matrix)
{
    matrix->fill_value(0);
}

//
DllExport double SumDoubleMatrix(DoubleMatrix* matrix)
{
    return matrix->get_sum_double();
}
DllExport void ClearDoubleMatrix(DoubleMatrix* matrix)
{
    matrix->fill_value(0);
}
#endif

using Matrix.SourceGenerator;

namespace Nima;

// SourceGenerator 内で Matrix の Container も一緒に生成しています(手抜き)
[MatrixGenerator]
public readonly partial record struct ByteMatrix : IMatrix<byte> { }

[MatrixGenerator]
public readonly partial record struct Int8Matrix : IMatrix<sbyte> { }

[MatrixGenerator]
public readonly partial record struct Int16Matrix : IMatrix<Int16> { }

[MatrixGenerator]
public readonly partial record struct Int32Matrix : IMatrix<Int32> { }

[MatrixGenerator]
public readonly partial record struct Int64Matrix : IMatrix<Int64> { }

[MatrixGenerator]
public readonly partial record struct SingleMatrix : IMatrix<Single> { }

[MatrixGenerator]
public readonly partial record struct DoubleMatrix : IMatrix<Double> { }


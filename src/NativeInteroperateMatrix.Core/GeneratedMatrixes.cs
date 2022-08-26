using Matrix.SourceGenerator;

namespace Nima;

// SourceGenerator 内で Matrix の Container も一緒に生成しています(手抜き)
[MatrixGenerator]
public readonly partial record struct Int8Matrix { }

[MatrixGenerator]
public readonly partial record struct Int16Matrix { }

[MatrixGenerator]
public readonly partial record struct Int32Matrix { }

[MatrixGenerator]
public readonly partial record struct Int64Matrix { }

[MatrixGenerator]
public readonly partial record struct SingleMatrix { }

[MatrixGenerator]
public readonly partial record struct DoubleMatrix { }


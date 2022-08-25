using Matrix.SourceGenerator;

namespace Nima;

// SourceGenerator 内で Matrix の Container も一緒に生成しています(手抜き)
[MatrixGenerator]
public readonly partial struct Int8Matrix { }

[MatrixGenerator]
public readonly partial struct Int16Matrix { }

[MatrixGenerator]
public readonly partial struct Int32Matrix { }

[MatrixGenerator]
public readonly partial struct Int64Matrix { }

[MatrixGenerator]
public readonly partial struct SingleMatrix { }

[MatrixGenerator]
public readonly partial struct DoubleMatrix { }


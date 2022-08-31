﻿using Matrix.SourceGenerator;

namespace Nima;

[MatrixGenerator]
public readonly partial record struct ByteMatrix : IMatrix<Byte> { }

[MatrixGenerator]
public readonly partial record struct Int8Matrix : IMatrix<SByte> { }

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

#if false   // MatrixGenerator 内で Container も一緒に生成しています(手抜き)
public sealed partial class ByteMatrixContainer : MatrixContainerBase<Byte> { }
public sealed partial class Int8MatrixContainer : MatrixContainerBase<SByte> { }
public sealed partial class Int16MatrixContainer : MatrixContainerBase<Int16> { }
public sealed partial class Int32MatrixContainer : MatrixContainerBase<Int32> { }
public sealed partial class Int64MatrixContainer : MatrixContainerBase<Int64> { }
public sealed partial class SingleMatrixContainer : MatrixContainerBase<Single> { }
public sealed partial class DoubleMatrixContainer : MatrixContainerBase<Double> { }
#endif

# Nima (Native Interoperated Matrix) 

[![GitHub license](https://img.shields.io/github/license/hsytkm/NativeInteroperateMatrix)](https://github.com/hsytkm/NativeInteroperateMatrix/blob/trunk/LICENSE) [![NuGet](https://img.shields.io/nuget/v/Nima.Core?style=flat-square)](https://www.nuget.org/packages/Nima.Core/) [![NuGet](https://img.shields.io/nuget/v/Nima.Imaging.Drawing?style=flat-square)](https://www.nuget.org/packages/Nima.Imaging.Drawing/) [![NuGet](https://img.shields.io/nuget/v/Nima.Imaging.Wpf?style=flat-square)](https://www.nuget.org/packages/Nima.Imaging.Wpf/)

Interoperate 2-dimensional array with natives.

Ver1 用の README です。

## What is this?

.NETのマネージコード と ネイティブコード 間で P/Invoke を介して 2次元配列 を受け渡すことを目的としたライブラリです。

.NET（マネージド側）からアンマネージド領域のメモリを確保し、確保メモリを 2次元配列 として使用/管理します。

本ライブラリでは、アンマネージドメモリ領域上の2次元配列 のことを **Matrix** と呼びます。

## Matrix

**Matrixの種類**

プリミティブ型のサイズに応じた Matrix (struct) を用意しています。

| PrimitiveType | Size (byte) |      Matrix      |
| :-----------: | :---------: | :--------------: |
|     byte      |      1      |    ByteMatrix    |
|     short     |      2      |   Int16Matrix    |
|      int      |      4      |   Int32Matrix    |
|     long      |      8      |   Int64Matrix    |
|     float     |      4      |   SingleMatrix   |
|    double     |      8      |   DoubleMatrix   |
|  PixelBgr24   |      3      | PixelBgr24Matrix |

**Matrixの構造体サイズ**

Matrix (struct) のサイズは プリミティブ型 に関わらず **24Byte固定** となります。

本定義（順序とサイズ）はネイティブコード側と揃える必要があります。

ポインタは 8Byte で管理しており、64bit 環境 のみを想定しています。

|    Member    |  Type  |      Size       |
| :----------: | :----: | :-------------: |
|   Pointer    | IntPtr | **8 (for x64)** |
|     Rows     |  int   |        4        |
|   Columns    |  int   |        4        |
| BytesPerItem |  int   |        4        |
|    Stride    |  int   |        4        |

**配列値の読み書き**

各Matrix は `ref T` を返却する indexer を用意しています。配列値の読み書きに使用して下さい。

```cs
var currentValue = matrix[row, col];
matrix[row, col] = Calcurate(currentValue);
```

## MatrixContainer

各Matrix は 専用の Container (class) を介して確保します。

**コンテナの名称**

各Matrix の末尾に Container を連結した名称のクラスを用意しています。

例）`ByteMatrix` → `ByteMatrixContainer`

**コンテナの生成**

予めアンマネージド メモリを確保する設計となっていますので、コンテナの コンストラクタ に行列数を指定して下さい。

```cs
// アンマネージド領域に int[48,36] を確保（ゼロクリアあり）
var container1 = new Int32MatrixContainer(rows: 48, columns: 36, initialize: true);
```

生成時点で初期値を指定することもできます。

```cs
// アンマネージド領域に byte[2,3] を確保して、初期値を指定
var values = Enumerable.Range(0, 6);
var container2 = new ByteMatrixContainer(2, 3, values);
```

**コンテナの破棄**

`MatrixContainer` は `IDisposable` を継承しています。 アンマネージドメモリを破棄するため、必ず `Dispose()` をコールして下さい。

EOF

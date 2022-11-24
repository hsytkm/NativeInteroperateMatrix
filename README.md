# Nima (Native Interoperated Matrix)

[![GitHub license](https://img.shields.io/github/license/hsytkm/NativeInteroperateMatrix)](https://github.com/hsytkm/NativeInteroperateMatrix/blob/trunk/LICENSE) [![NuGet](https://img.shields.io/nuget/v/Nima.Core?style=flat-square)](https://www.nuget.org/packages/Nima.Core/) [![NuGet](https://img.shields.io/nuget/v/Nima.Imaging.Drawing?style=flat-square)](https://www.nuget.org/packages/Nima.Imaging.Drawing/) [![NuGet](https://img.shields.io/nuget/v/Nima.Imaging.Wpf?style=flat-square)](https://www.nuget.org/packages/Nima.Imaging.Wpf/)

Interoperate 2-dimensional array with natives.

Ver2 用の README です。

[Ver1 はこちら](https://github.com/hsytkm/NativeInteroperateMatrix/blob/trunk/README_ver1.md)

## What is this?

.NETの マネージコード と ネイティブコード 間で P/Invoke を介して 2次元配列 を受け渡すことを目的としたライブラリです。 Ver2 からは 1次元配列 にも対応しています。

本ライブラリでは、アンマネージドメモリ領域上の 2次元配列 を **NativeMatrix** 、1次元配列 を **NativeArray** と呼びます。

## NativeMatrix

2次元配列のネイティブメモリはプリミティブ型に関わらず `NativeMatrix` 構造体で管理します。

### Struct Size

`NativeMatrix` 構造体のサイズは  **28Byte固定** となります。

本定義（順序とサイズ）は 利用者側（C/C++ などのネイティブコード）と揃える必要があります。 64bit 環境 のみを想定していますので、Pointer のサイズは 8Byte となります。

|    Member    |  Type  |      Size       |
| :----------: | :----: | :-------------: |
|   Pointer    | IntPtr | **8 (for x64)** |
| AllocateSize |  int   |        4        |
|     Rows     |  int   |        4        |
|   Columns    |  int   |        4        |
| BytesPerItem |  int   |        4        |
|    Stride    |  int   |        4        |

### Referencing native memory

`NativeMatrix.AsSpan<T>()` から 2次元配列全体のメモリを取得できます。 `Stride` も含まれますので、ゴミデータが不要な場合は `NativeMatrix.AsRowSpan<T>(int row)`  から行ごとの Span を取得して下さい。

### NativeMatrixContainer

`NativeMatrix` は 専用の Container クラスを利用して確保します。

#### プリミティブ型の指定

確保したい 2次元配列のプリミティブ型の末尾に `MatrixContainer` を連結したコンテナクラスを用意しています。

| Type (.NET) | Type (C#)  | Bytes per item |         Container         |
| :---------: | :--------: | :------------: | :-----------------------: |
|    Byte     |    byte    |       1        |    ByteMatrixContainer    |
|    Int16    |   short    |       2        |   Int16MatrixContainer    |
|    Int32    |    int     |       4        |   Int32MatrixContainer    |
|    Int64    |    long    |       8        |   Int64MatrixContainer    |
|   Single    |   float    |       4        |   SingleMatrixContainer   |
|   Double    |   double   |       8        |   DoubleMatrixContainer   |
| PixelBgr24  | PixelBgr24 |       3        | PixelBgr24MatrixContainer |

#### コンテナの生成

インスタンスの生成時に `NativeMatrix` をメモリを確保する設計となっています。 Container のコンストラクタに 行数 と 列数 を指定して下さい。

```cs
// アンマネージド領域に byte[48,36] を確保します（ゼロ初期化あり）
var container = new ByteMatrixContainer(rows: 48, columns: 36, initialize: true);
```

生成時点で初期値を指定することもできます。

```cs
// アンマネージド領域に Int32[2,3] を確保して、初期値を設定します
var values = Enumerable.Range(0, 6);
var container = new Int32MatrixContainer(2, 3, values);
```

#### NativeMatrix の取得

`NativeMatrix` の取得メソッドは、Memory アクセスの排他管理のため Read用 と Write用 に分けています。 読み込み中の書き込み や 書き込み中の書き込み が行われると 、`Nima.InvalidMemoryAccessException` が throw されます。

メソッドから返却される `IDisposable` を `Dispose()` すれば使用中の状態がクリアされます。`NativeMatrix` の使用完了後は必ず `Dispose()` メソッドをコールして下さい。

```cs
// 読み込み用
IDisposable GetMatrixForReading(out NativeMatrix matrix);

// 書き込み用
IDisposable GetMatrixForWriting(out NativeMatrix matrix);
```

#### NativeMatrix の取得2  (dangerous)

ライブラリ内部の参照管理が必要なければ、`DangerousGetNativeMatrix()` から `NativeMatrix` を取得することができます。

```cs
NativeMatrix DangerousGetNativeMatrix();
```

#### コンテナの破棄

`MatrixContainer` は `IDisposable` を継承しています。 アンマネージドメモリを破棄するため、必ず `Dispose()` をコールして下さい。

## NativeArray

1次元配列のネイティブメモリはプリミティブ型に関わらず `NativeArray` 構造体で管理します。

### Struct Size

`NativeArray` 構造体のサイズは  **16Byte固定** となります。

本定義（順序とサイズ）は 利用者側（C/C++ などのネイティブコード）と揃える必要があります。 64bit 環境 のみを想定していますので、Pointer のサイズは 8Byte となります。

|    Member    |  Type  |      Size       |
| :----------: | :----: | :-------------: |
|   Pointer    | IntPtr | **8 (for x64)** |
| AllocateSize |  int   |        4        |
| BytesPerItem |  int   |        4        |

### Referencing native memory

`NativeArray.AsSpan<T>()` から 1次元配列全体のメモリを取得できます。

### NativeArrayContainer

`NativeArray` は 専用の Container クラスを利用して確保します。

#### プリミティブ型の指定

確保したい 2次元配列のプリミティブ型の末尾に `ArrayContainer` を連結したコンテナクラスを用意しています。

| Type (.NET) | Type (C#) | Bytes per item |      Container       |
| :---------: | :-------: | :------------: | :------------------: |
|    Byte     |   byte    |       1        |  ByteArrayContainer  |
|    Int16    |   short   |       2        | Int16ArrayContainer  |
|    Int32    |    int    |       4        | Int32ArrayContainer  |
|    Int64    |   long    |       8        | Int64ArrayContainer  |
|   Single    |   float   |       4        | SingleArrayContainer |
|   Double    |  double   |       8        | DoubleArrayContainer |

#### コンテナの生成

インスタンスの生成時に `NativeArray` をメモリを確保する設計となっています。 Container のコンストラクタに 行数 と 列数 を指定して下さい。

```cs
// アンマネージド領域に byte[144] を確保します（ゼロ初期化あり）
var container = new ByteArrayContainer(length: 144, initialize: true);
```

生成時点で初期値を指定することもできます。

```cs
// アンマネージド領域に Int32[6] を確保して、初期値を設定します
var values = Enumerable.Range(0, 6);
var container = new Int32ArrayContainer(6, values);
```

#### NativeArray の取得

`NativeArray` の取得メソッドは、Memory アクセスの排他管理のため Read用 と Write用 に分けています。 読み込み中の書き込み や 書き込み中の書き込み が行われると 、`Nima.InvalidMemoryAccessException` が throw されます。

メソッドから返却される `IDisposable` を `Dispose()` すれば使用中の状態がクリアされます。`NativeArray` の使用完了後は必ず `Dispose()` メソッドをコールして下さい。

```cs
// 読み込み用
IDisposable GetArrayForReading(out NativeArray array);

// 書き込み用
IDisposable GetArrayForWriting(out NativeArray array);
```

#### NativeArray の取得2 (dangerous)

ライブラリ内部の参照管理が必要なければ、`DangerousGetNativeArray()` から `NativeArray` を取得することができます。

```cs
NativeArray DangerousGetNativeArray();
```

#### コンテナの破棄

`ArrayContainer` は `IDisposable` を継承しています。 アンマネージドメモリを破棄するため、必ず `Dispose()` をコールして下さい。

## 心残り

1. `NativeMatrix`, `NativeArray` の定義メンバの順序に後悔している。アーキテクチャによってサイズが変わる `Pointer` は最後に定義すべきだった。次にコードを触る機会があったら変更したい。
2. プリミティブ型ごとに `NativeMatrix`, `NativeArray` を定義したい。 現状だとコンテナから `NativeMatrix` を読み出しても `ToSpan<T>()` で型を指定する必要があり不便。

EOF

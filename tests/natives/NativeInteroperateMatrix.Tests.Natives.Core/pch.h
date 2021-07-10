// pch.h: プリコンパイル済みヘッダー ファイルです。
// 次のファイルは、その後のビルドのビルド パフォーマンスを向上させるため 1 回だけコンパイルされます。
// コード補完や多くのコード参照機能などの IntelliSense パフォーマンスにも影響します。
// ただし、ここに一覧表示されているファイルは、ビルド間でいずれかが更新されると、すべてが再コンパイルされます。
// 頻繁に更新するファイルをここに追加しないでください。追加すると、パフォーマンス上の利点がなくなります。

#ifndef PCH_H
#define PCH_H

// プリコンパイルするヘッダーをここに追加します
#include "framework.h"

#define DllExport extern "C" __declspec(dllexport)

template <class T> void SafeDelete(T** ppT)
{
    if (*ppT != nullptr)
    {
        delete* ppT;
        *ppT = nullptr;
    }
}

template <class T> void SafeRelease(T** ppT)
{
    if (*ppT)
    {
        (*ppT)->Release();
        *ppT = NULL;
    }
}

template <class T>
T clamp(const T& value, const T& min, const T& max)
{
    return (value <= min) ? min : (max <= value ? max : value);
}

#endif //PCH_H

using System;
using System.Collections;
using System.Collections.Generic;

namespace NativeInteroperateMatrix.Core
{
    public record ValueArray2d<T> : IEnumerable<T>
        where T : struct
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Count => Rows * Columns;

        private readonly T[] _source;

        public ref T this[int row, int column]
        {
            get
            {
                if (IsOutOfRangeVertical(row)) throw new ArgumentOutOfRangeException(nameof(row));
                if (IsOutOfRangeHorizontal(column)) throw new ArgumentOutOfRangeException(nameof(column));
                return ref _source[(row * Columns) + column];
            }
        }
        public ref T this[int i] => ref _source[i];

        public Span<T> GetSpan() => _source.AsSpan();
        public ReadOnlySpan<T> GetRoSpan() => _source.AsSpan();

        public ValueArray2d(int rows, int columns, IEnumerable<T> source)
        {
            (Rows, Columns) = (rows, columns);
            _source = new T[rows * columns];

            int n = 0;
            foreach (var data in source)
            {
                _source[n++] = data;
            }
        }

        protected bool IsOutOfRangeVertical(int row) => row < 0 || Rows <= row;
        protected bool IsOutOfRangeHorizontal(int column) => column < 0 || Columns <= column;

        /// <summary>指定行の要素を全て列挙します</summary>
        public ReadOnlySpan<T> GetRowRoSpan(int row)
        {
            if (IsOutOfRangeVertical(row)) throw new ArgumentOutOfRangeException(nameof(row));
            return _source.AsSpan(row * Columns, Columns);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T data in _source)
                yield return data;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

namespace Nima;

public /*sealed*/ class ByteMatrixContainer : MatrixContainerBase
{
    public ByteMatrixContainer(int rows, int columns, bool initialize = true)
        : base(rows, columns, Unsafe.SizeOf<Byte>(), initialize)
    { }

    public ByteMatrixContainer(int rows, int columns, IEnumerable<Byte> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public ByteMatrixContainer(int rows, int columns, ReadOnlySpan<Byte> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

}

namespace Nima;

public /*sealed*/ class Int64MatrixContainer : MatrixContainerBase
{
    public Int64MatrixContainer(int rows, int columns, bool initialize = true)
        : base(rows, columns, Unsafe.SizeOf<Int64>(), initialize)
    { }

    public Int64MatrixContainer(int rows, int columns, IEnumerable<Int64> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan<Int64>(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan<Int64>(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public Int64MatrixContainer(int rows, int columns, ReadOnlySpan<Int64> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan<Int64>(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan<Int64>(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

}

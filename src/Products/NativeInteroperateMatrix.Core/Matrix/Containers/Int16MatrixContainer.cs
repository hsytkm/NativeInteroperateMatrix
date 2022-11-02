namespace Nima;

public /*sealed*/ class Int16MatrixContainer : MatrixContainerBase
{
    public Int16MatrixContainer(int rows, int columns, bool initialize = true)
        : base(rows, columns, Unsafe.SizeOf<Int16>(), initialize)
    { }

    public Int16MatrixContainer(int rows, int columns, IEnumerable<Int16> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan<Int16>(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan<Int16>(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public Int16MatrixContainer(int rows, int columns, ReadOnlySpan<Int16> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan<Int16>(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan<Int16>(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

}

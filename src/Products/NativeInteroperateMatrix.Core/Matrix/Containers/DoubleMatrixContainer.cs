namespace Nima;

public /*sealed*/ class DoubleMatrixContainer : MatrixContainerBase
{
    public DoubleMatrixContainer(int rows, int columns, bool initialize = true)
        : base(rows, columns, Unsafe.SizeOf<Double>(), initialize)
    { }

    public DoubleMatrixContainer(int rows, int columns, IEnumerable<Double> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan<Double>(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan<Double>(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

    public DoubleMatrixContainer(int rows, int columns, ReadOnlySpan<Double> items)
        : this(rows, columns, false)
    {
        int length = rows * columns;
        int written = 0;
        int row = 0;
        int column = 0;
        var span = Matrix.AsRowSpan<Double>(0);

        foreach (var item in items)
        {
            if (column >= columns)
            {
                column = 0;
                row++;
                span = Matrix.AsRowSpan<Double>(row);
            }
            span[column++] = item;

            if (++written > length)
                throw new ArgumentException("Items is large.", nameof(items));
        }

        if (!(row == rows - 1 && column == columns))
            throw new ArgumentException("Items is small.", nameof(items));
    }

}

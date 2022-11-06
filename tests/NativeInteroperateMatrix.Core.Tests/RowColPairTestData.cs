using System.Collections;

namespace Nima.Core.Tests;

internal class RowColPairTestData : IEnumerable<object[]>
{
    private readonly List<object[]> _testData = new()
    {
        new object[] { 1, 1 },
        new object[] { 1, 2 },
        new object[] { 3, 1 },
        new object[] { 2, 2 },
        new object[] { 4, 5 },
        new object[] { 256, 127 },
        //new object[] { 4000, 3000 },
    };

    public IEnumerator<object[]> GetEnumerator() => _testData.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

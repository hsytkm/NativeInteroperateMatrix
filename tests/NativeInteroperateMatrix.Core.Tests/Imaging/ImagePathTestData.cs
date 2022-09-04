using System.Collections;

namespace Nima.Core.Tests.Imaging;

public class ImagePathTestData : IEnumerable<object[]>
{
    private readonly List<object[]> _testData = new()
    {
        new object[] { @"Assets/image1.bmp" },
        new object[] { @"Assets/image2.bmp" },

        // ◆未テスト
        //new object[] { @"Assets/format24bit.bmp" },
        //new object[] { @"Assets/format8bit.bmp" },
        // ◆未テスト
    };

    public IEnumerator<object[]> GetEnumerator() => _testData.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

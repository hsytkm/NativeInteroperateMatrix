using System;
using System.Collections;
using System.Collections.Generic;

namespace Nima.Core.Tests.Imaging
{
    internal class ImagePathTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _testData = new()
        {
            new object[] { @"Assets/image1.bmp" },
            new object[] { @"Assets/image2.bmp" },
        };

        public IEnumerator<object[]> GetEnumerator() => _testData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

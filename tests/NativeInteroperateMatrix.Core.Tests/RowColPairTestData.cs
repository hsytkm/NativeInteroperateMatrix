using System;
using System.Collections;
using System.Collections.Generic;

namespace Nima.Core.Tests
{
    internal class RowColPairTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _testData = new()
        {
            new object[] { 2, 2 },
            new object[] { 256, 128 },
            new object[] { 4000, 3000 },
        };

        public IEnumerator<object[]> GetEnumerator() => _testData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

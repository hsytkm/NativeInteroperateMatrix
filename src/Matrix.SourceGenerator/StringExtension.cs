using System;
using System.Collections.Generic;

namespace Matrix.SourceGenerator
{
    static class StringExtension
    {
        public static string ToLowerOnlyFirst(this string str) => Char.ToLower(str[0]) + str.Substring(1);

        public static string JoinWithCommas(this IEnumerable<string> ss) => string.Join(", ", ss);

    }
}

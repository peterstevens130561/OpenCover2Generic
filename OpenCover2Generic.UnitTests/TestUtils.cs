using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    static class TestUtils
    {
        internal static void AssertStringsSame(string expected, string actual)
        {
            int len = Math.Min(expected.Length, actual.Length);
            for (int i = 0; i < len; i++)
            {
                if (expected[i] != actual[i])
                {
                    int low = Math.Max(0, i - 10);
                    int len1 = Math.Min(10, low);
                    int len2 = Math.Min(actual.Length - i, 10);
                    int len3 = Math.Min(expected.Length - i, 10);
                    string actualFailed = actual.Substring(low, len1) + "^" + actual.Substring(i, len2);
                    string expectedFailed = expected.Substring(low, len1) + "^" + expected.Substring(i, len3);
                    Assert.Fail($"Strings differ at pos {i} \nexpected='{expectedFailed}' \nactual='{actualFailed}'");
                }
            }
        }
    }
}

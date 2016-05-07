using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class UInt64FilterTests
    {
        #region GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            UInt64Filter filter = new UInt64Filter();
            filter.Value = "18446744073709551615";

            Object actual = filter.GetNumericValue();
            UInt64 expected = 18446744073709551615;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_NotValidValue_ReturnsNull()
        {
            UInt64Filter filter = new UInt64Filter();
            filter.Value = "-1";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class Int64FilterTests
    {
        #region Method: GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            Int64Filter filter = new Int64Filter();
            filter.Value = "-9223372036854775808";

            Object actual = filter.GetNumericValue();
            Int64 expected = -9223372036854775808;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_NotValidValue_ReturnsNull()
        {
            Int64Filter filter = new Int64Filter();
            filter.Value = "9223372036854775808";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

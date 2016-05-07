using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class DoubleFilterTests
    {
        #region GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            DoubleFilter filter = new DoubleFilter();
            filter.Value = "-3.40281540545454";

            Object actual = filter.GetNumericValue();
            Double expected = -3.4028154054545401d;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_NotValidValue_ReturnsNull()
        {
            DoubleFilter filter = new DoubleFilter();
            filter.Value = "3.2f";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

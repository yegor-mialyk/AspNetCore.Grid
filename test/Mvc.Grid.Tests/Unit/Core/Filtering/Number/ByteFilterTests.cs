using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class ByteFilterTests
    {
        #region Method: GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            ByteFilter filter = new ByteFilter();
            filter.Value = "255";

            Object actual = filter.GetNumericValue();
            Byte expected = 255;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_OnNotValidValueReturnsNull()
        {
            ByteFilter filter = new ByteFilter();
            filter.Value = "-1";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

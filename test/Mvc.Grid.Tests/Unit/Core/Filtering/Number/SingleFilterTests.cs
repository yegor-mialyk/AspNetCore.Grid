using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class SingleFilterTests
    {
        #region Method: GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            SingleFilter filter = new SingleFilter();
            filter.Value = "-3.40281540545454";

            Object actual = filter.GetNumericValue();
            Single expected = -3.40281534f;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_OnNotValidValueReturnsNull()
        {
            SingleFilter filter = new SingleFilter();
            filter.Value = "3.2f";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

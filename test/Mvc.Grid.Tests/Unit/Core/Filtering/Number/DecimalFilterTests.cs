using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class DecimalFilterTests
    {
        #region Method: GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            DecimalFilter filter = new DecimalFilter();
            filter.Value = "79228162514264337593543950335";

            Decimal expected = 79228162514264337593543950335M;
            Object actual = filter.GetNumericValue();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_OnNotValidValueReturnsNull()
        {
            DecimalFilter filter = new DecimalFilter();
            filter.Value = "79228162514264337593543950336";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

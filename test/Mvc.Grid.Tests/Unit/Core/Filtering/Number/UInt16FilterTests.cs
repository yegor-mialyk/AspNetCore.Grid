using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class UInt16FilterTests
    {
        #region GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            UInt16Filter filter = new UInt16Filter();
            filter.Value = "65535";

            Object actual = filter.GetNumericValue();
            UInt16 expected = 65535;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_NotValidValue_ReturnsNull()
        {
            UInt16Filter filter = new UInt16Filter();
            filter.Value = "-1";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class Int16FilterTests
    {
        #region GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            Int16Filter filter = new Int16Filter();
            filter.Value = "-32768";

            Object actual = filter.GetNumericValue();
            Int16 expected = -32768;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_NotValidValue_ReturnsNull()
        {
            Int16Filter filter = new Int16Filter();
            filter.Value = "32768";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

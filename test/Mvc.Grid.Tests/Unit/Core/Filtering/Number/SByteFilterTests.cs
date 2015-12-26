using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class SByteFilterTests
    {
        #region Method: GetNumericValue()

        [Fact]
        public void GetNumericValue_ParsesValue()
        {
            SByteFilter filter = new SByteFilter();
            filter.Value = "-128";

            Object actual = filter.GetNumericValue();
            SByte expected = -128;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNumericValue_NotValidValue_ReturnsNull()
        {
            SByteFilter filter = new SByteFilter();
            filter.Value = "128";

            Assert.Null(filter.GetNumericValue());
        }

        #endregion
    }
}

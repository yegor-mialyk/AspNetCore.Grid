using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests
{
    public class GridRowTests
    {
        [Fact]
        public void GridRow_SetsIndex()
        {
            Assert.Equal(3, new GridRow<Object>(new Object(), 3).Index);
        }

        [Fact]
        public void GridRow_SetsModel()
        {
            Object expected = new();
            Object actual = new GridRow<Object>(expected, 0).Model;

            Assert.Same(expected, actual);
        }
    }
}

using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridRowTests
    {
        #region GridRow(T model, Int32 index)

        [Fact]
        public void GridRow_SetsIndex()
        {
            Assert.Equal(3, new GridRow<Object>(null, 3).Index);
        }

        [Fact]
        public void GridRow_SetsModel()
        {
            Object expected = new Object();
            Object actual = new GridRow<Object>(expected, 0).Model;

            Assert.Same(expected, actual);
        }

        #endregion
    }
}

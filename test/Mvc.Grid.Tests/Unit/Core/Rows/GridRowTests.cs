using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridRowTests
    {
        #region Constructor: GridRow(Object model)

        [Fact]
        public void GridRow_SetsModel()
        {
            Object expected = new Object();
            Object actual = new GridRow(expected).Model;

            Assert.Same(expected, actual);
        }

        #endregion
    }
}

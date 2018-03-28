using NSubstitute;
using System;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class BaseGridColumnTests
    {
        private BaseGridColumn<GridModel, String> column;

        public BaseGridColumnTests()
        {
            column = Substitute.ForPartsOf<BaseGridColumn<GridModel, String>>();
        }

        #region IGridColumn.Sort

        [Fact]
        public void IGridColumn_ReturnsSort()
        {
            IGridColumn gridColumn = column;

            Object actual = gridColumn.Filter;
            Object expected = column.Filter;

            Assert.Same(expected, actual);
        }

        #endregion

        #region IGridColumn.Filter

        [Fact]
        public void IGridColumn_ReturnsFilter()
        {
            IGridColumn gridColumn = column;

            Object actual = gridColumn.Filter;
            Object expected = column.Filter;

            Assert.Same(expected, actual);
        }

        #endregion
    }
}

using System;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridTests
    {
        #region IGrid.Columns

        [Fact]
        public void IGridColumns_ReturnsColumns()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            IGridColumns<IGridColumn> actual = (grid as IGrid).Columns;
            IGridColumns<IGridColumn> expected = grid.Columns;

            Assert.Same(expected, actual);
        }

        #endregion

        #region IGrid.Rows

        [Fact]
        public void IGridRows_ReturnsRows()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            IGridRows<Object> actual = (grid as IGrid).Rows;
            IGridRows<Object> expected = grid.Rows;

            Assert.Same(expected, actual);
        }

        #endregion

        #region IGrid.Pager

        [Fact]
        public void IGridPager_ReturnsPager()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            IGridPager actual = ((IGrid)grid).Pager;
            IGridPager expected = grid.Pager;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Grid(IEnumerable<T> source)

        [Fact]
        public void Grid_SetsProcessors()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            Assert.Empty(grid.Processors);
        }

        [Fact]
        public void Grid_SetsSource()
        {
            IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
            IQueryable<GridModel> actual = new Grid<GridModel>(expected).Source;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_SetsColumns()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            GridColumns<GridModel> actual = grid.Columns as GridColumns<GridModel>;
            GridColumns<GridModel> expected = new GridColumns<GridModel>(grid);

            Assert.Same(expected.Grid, actual.Grid);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Grid_SetsRows()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            GridRows<GridModel> actual = grid.Rows as GridRows<GridModel>;
            GridRows<GridModel> expected = new GridRows<GridModel>(grid);

            Assert.Same(expected.Grid, actual.Grid);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

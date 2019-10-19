using System;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridTests
    {
        [Fact]
        public void IGrid_ReturnsColumns()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            Object actual = ((IGrid)grid).Columns;
            Object expected = grid.Columns;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void IGrid_ReturnsRows()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            Object actual = ((IGrid)grid).Rows;
            Object expected = grid.Rows;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void IGrid_ReturnsPager()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            Object? actual = ((IGrid)grid).Pager;
            Object? expected = grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_SetsName()
        {
            Assert.Empty(new Grid<GridModel>(new GridModel[0]).Name);
        }

        [Fact]
        public void Grid_SetsSourceUrl()
        {
            Assert.Empty(new Grid<GridModel>(new GridModel[0]).SourceUrl);
        }

        [Fact]
        public void Grid_SetsFooterPartialViewName()
        {
            Assert.Empty(new Grid<GridModel>(new GridModel[0]).FooterPartialViewName);
        }

        [Fact]
        public void Grid_SetsProcessors()
        {
            Assert.Empty(new Grid<GridModel>(new GridModel[0]).Processors);
        }

        [Fact]
        public void Grid_SetsSource()
        {
            IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
            IQueryable<GridModel> actual = new Grid<GridModel>(expected).Source;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_SetsFilterMode()
        {
            GridFilterMode actual = new Grid<GridModel>(new GridModel[0]).FilterMode;
            GridFilterMode expected = GridFilterMode.Excel;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Grid_SetsEmptyAttributes()
        {
            Assert.Empty(new Grid<GridModel>(new GridModel[0]).Attributes);
        }

        [Fact]
        public void Grid_SetsColumns()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            GridColumns<GridModel> actual = (GridColumns<GridModel>)grid.Columns;
            GridColumns<GridModel> expected = new GridColumns<GridModel>(grid);

            Assert.Same(expected.Grid, actual.Grid);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Grid_SetsMode()
        {
            GridProcessingMode actual = new Grid<GridModel>(new GridModel[0]).Mode;
            GridProcessingMode expected = GridProcessingMode.Automatic;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Grid_SetsRows()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);

            GridRows<GridModel> expected = new GridRows<GridModel>(grid);
            GridRows<GridModel> actual = (GridRows<GridModel>)grid.Rows;

            Assert.Same(expected.Grid, actual.Grid);
            Assert.Equal(expected, actual);
        }
    }
}

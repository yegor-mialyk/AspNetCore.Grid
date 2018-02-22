using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridPagerTests
    {
        private IGrid<GridModel> grid;
        private GridPager<GridModel> pager;

        public GridPagerTests()
        {
            grid = new Grid<GridModel>(new GridModel[0]);
            pager = new GridPager<GridModel>(grid);
            grid.Query = new QueryCollection();
        }

        #region TotalPages

        [Theory]
        [InlineData(0, 20, 0)]
        [InlineData(1, 20, 1)]
        [InlineData(19, 20, 1)]
        [InlineData(20, 20, 1)]
        [InlineData(21, 20, 2)]
        [InlineData(39, 20, 2)]
        [InlineData(40, 20, 2)]
        [InlineData(41, 20, 3)]
        public void TotalPages_ReturnsTotalPages(Int32 total, Int32 rows, Int32 pages)
        {
            pager.RowsPerPage = rows;
            pager.TotalRows = total;

            Int32 actual = pager.TotalPages;
            Int32 expected = pages;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region CurrentPage

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("Grid", "")]
        [InlineData("", "Grid-Page=")]
        [InlineData("", "Grid-Page=2a")]
        [InlineData(null, "Grid-Page=")]
        [InlineData(null, "Grid-Page=2a")]
        [InlineData("Grid", "Grid-Page=")]
        [InlineData("Grid", "Grid-Page=2a")]
        public void CurrentPage_OnInvalidQueryPageUsesCurrentPage(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.Grid.Name = name;
            pager.CurrentPage = 3;
            pager.TotalRows = 100;

            Int32 actual = pager.CurrentPage;
            Int32 expected = 3;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Page=5")]
        [InlineData(null, "Page=5")]
        [InlineData("Grid", "Grid-Page=5")]
        public void CurrentPage_OnGreaterThanTotalPagesReturnsTotalPages(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.RowsPerPage = 25;
            pager.Grid.Name = name;
            pager.TotalRows = 100;

            Int32 actual = pager.CurrentPage;
            Int32 expected = 4;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Page=0")]
        [InlineData("", "Page=-1")]
        [InlineData(null, "Page=0")]
        [InlineData(null, "Page=-1")]
        [InlineData("Grid", "Grid-Page=0")]
        [InlineData("Grid", "Grid-Page=-1")]
        public void CurrentPage_OnLessOrEqualToZeroQueryPageReturnsOne(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.Grid.Name = name;
            pager.CurrentPage = 5;

            Int32 actual = pager.CurrentPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CurrentPage_OnLessOrEqualToZeroCurrentPageReturnsOne(Int32 page)
        {
            pager.CurrentPage = page;

            Int32 actual = pager.CurrentPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Page=2")]
        [InlineData(null, "Page=2")]
        [InlineData("Grid", "Grid-Page=2")]
        public void CurrentPage_SetsCurrentPageFromQuery(String name, String query)
        {
            grid.Query = HttpUtility.ParseQueryString(query);
            grid.Name = name;

            pager.TotalRows = 4 * pager.RowsPerPage;

            Int32 actual = pager.CurrentPage;
            Int32 expected = 2;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region RowsPerPage

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("Grid", "")]
        [InlineData("", "Rows=")]
        [InlineData("", "Rows=2a")]
        [InlineData(null, "Rows=")]
        [InlineData(null, "Rows=2a")]
        [InlineData("Grid", "Grid-Rows=")]
        [InlineData("Grid", "Grid-Rows=2a")]
        public void RowsPerPage_OnInvalidQueryRowsUsesRowsPerPage(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            pager.PageSizes.Clear();
            pager.Grid.Name = name;
            pager.RowsPerPage = 33;
            pager.TotalRows = 500;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 33;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Rows=0")]
        [InlineData("", "Rows=-1")]
        [InlineData(null, "Rows=0")]
        [InlineData(null, "Rows=-1")]
        [InlineData("Grid", "Grid-Rows=0")]
        [InlineData("Grid", "Grid-Rows=-1")]
        public void RowsPerPage_OnLessOrEqualToZeroQueryPageReturnsOne(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            pager.PageSizes.Clear();
            pager.Grid.Name = name;
            pager.RowsPerPage = 5;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void RowsPerPage_OnLessOrEqualToZeroCurrentPageReturnsOne(Int32 rows)
        {
            pager.ShowPageSizes = true;
            pager.RowsPerPage = rows;
            pager.PageSizes.Clear();

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Rows=10", 10)]
        [InlineData("", "Rows=20", 20)]
        [InlineData("", "Rows=60", 10)]
        [InlineData(null, "Rows=10", 10)]
        [InlineData(null, "Rows=20", 20)]
        [InlineData(null, "Rows=60", 10)]
        [InlineData("Grid", "Grid-Rows=10", 10)]
        [InlineData("Grid", "Grid-Rows=20", 20)]
        [InlineData("Grid", "Grid-Rows=60", 10)]
        public void RowsPerPage_AllowsOnlyFromPageSizes(String name, String query, Int32 rows)
        {
            pager.PageSizes = new Dictionary<Int32, String> { [10] = "10", [20] = "20" };
            grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            grid.Name = name;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = rows;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Rows=123")]
        [InlineData(null, "Rows=123")]
        [InlineData("Grid", "Grid-Rows=123")]
        public void RowsPerPage_SetsRowsPerPageFromQuery(String name, String query)
        {
            grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            pager.PageSizes.Clear();
            grid.Name = name;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 123;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RowsPerPage_DoesNotUseQuery()
        {
            grid.Query = HttpUtility.ParseQueryString("Rows=2");
            pager.ShowPageSizes = false;
            pager.RowsPerPage = 1;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FirstDisplayPage

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 2)]
        [InlineData(1, 3, 3)]
        [InlineData(1, 4, 4)]
        [InlineData(1, 5, 5)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(2, 3, 3)]
        [InlineData(2, 4, 4)]
        [InlineData(2, 5, 4)]
        [InlineData(3, 1, 1)]
        [InlineData(3, 2, 1)]
        [InlineData(3, 3, 2)]
        [InlineData(3, 4, 3)]
        [InlineData(3, 5, 3)]
        [InlineData(4, 1, 1)]
        [InlineData(4, 2, 1)]
        [InlineData(4, 3, 2)]
        [InlineData(4, 4, 2)]
        [InlineData(4, 5, 2)]
        [InlineData(5, 1, 1)]
        [InlineData(5, 2, 1)]
        [InlineData(5, 3, 1)]
        [InlineData(5, 4, 1)]
        [InlineData(5, 5, 1)]
        [InlineData(6, 1, 1)]
        [InlineData(6, 2, 1)]
        [InlineData(6, 3, 1)]
        [InlineData(6, 4, 1)]
        [InlineData(6, 5, 1)]
        public void FirstDisplayPage_ReturnsFirstDisplayPage(Int32 display, Int32 page, Int32 first)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString("Page=" + page);
            pager.PagesToDisplay = display;
            pager.RowsPerPage = 1;
            pager.TotalRows = 5;

            Int32 actual = pager.FirstDisplayPage;
            Int32 expected = first;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GridPager(IGrid<T> grid)

        [Fact]
        public void GridPager_SetsGrid()
        {
            IGrid actual = new GridPager<GridModel>(grid).Grid;
            IGrid expected = grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridPager_SetsCurrentPage()
        {
            Int32 actual = new GridPager<GridModel>(grid).CurrentPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsRowsPerPage()
        {
            Int32 actual = new GridPager<GridModel>(grid).RowsPerPage;
            Int32 expected = 20;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsPagesToDisplay()
        {
            Int32 actual = new GridPager<GridModel>(grid).PagesToDisplay;
            Int32 expected = 5;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsDefaultPartialViewName()
        {
            String actual = new GridPager<GridModel>(grid).PartialViewName;
            String expected = "MvcGrid/_Pager";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsProcessorTypeAsPostProcessor()
        {
            GridProcessorType actual = new GridPager<GridModel>(grid).ProcessorType;
            GridProcessorType expected = GridProcessorType.Post;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsDefaultPageSizes()
        {
            Dictionary<Int32, String> expected = new Dictionary<Int32, String> { [10] = "10", [20] = "20", [50] = "50", [100] = "100" };
            Dictionary<Int32, String> actual = new GridPager<GridModel>(grid).PageSizes;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Process(IQueryable<T> items)

        [Fact]
        public void Process_SetsTotalRows()
        {
            pager.Process(new GridModel[100].AsQueryable());

            Int32 actual = pager.TotalRows;
            Int32 expected = 100;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_ReturnsPagedItems()
        {
            IQueryable<GridModel> items = new[] { new GridModel(), new GridModel(), new GridModel() }.AsQueryable();
            pager.RowsPerPage = 1;
            pager.CurrentPage = 2;

            IEnumerable expected = items.Skip(1).Take(1);
            IEnumerable actual = pager.Process(items);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

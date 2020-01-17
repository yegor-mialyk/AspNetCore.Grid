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
        private GridPager<GridModel> pager;

        public GridPagerTests()
        {
            pager = new GridPager<GridModel>(new Grid<GridModel>(Array.Empty<GridModel>()) { Query = new QueryCollection() });
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 1)]
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

        [Theory]
        [InlineData("", "")]
        [InlineData("", "grid-page=")]
        [InlineData("", "grid-page=2a")]
        [InlineData(null, "")]
        [InlineData(null, "grid-page=")]
        [InlineData(null, "grid-page=2a")]
        [InlineData("grid", "")]
        [InlineData("grid", "grid-page=")]
        [InlineData("grid", "grid-page=2a")]
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
        [InlineData("", "page=5")]
        [InlineData(null, "page=5")]
        [InlineData("grid", "grid-page=5")]
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
        [InlineData("", "page=0")]
        [InlineData("", "page=-1")]
        [InlineData(null, "page=0")]
        [InlineData(null, "page=-1")]
        [InlineData("grid", "grid-page=0")]
        [InlineData("grid", "grid-page=-1")]
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
        [InlineData("", "page=2")]
        [InlineData("", "PAGE=2")]
        [InlineData(null, "page=2")]
        [InlineData(null, "PAGE=2")]
        [InlineData("grid", "grid-page=2")]
        [InlineData("grid", "GRID-PAGE=2")]
        public void CurrentPage_SetsCurrentPageFromQuery(String name, String query)
        {
            pager.Grid.Name = name;
            pager.Grid.Query = HttpUtility.ParseQueryString(query);

            pager.TotalRows = 4 * pager.RowsPerPage;

            Int32 actual = pager.CurrentPage;
            Int32 expected = 2;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("grid", "")]
        [InlineData("", "rows=")]
        [InlineData("", "rows=2a")]
        [InlineData(null, "rows=")]
        [InlineData(null, "rows=2a")]
        [InlineData("grid", "grid-rows=")]
        [InlineData("grid", "grid-rows=2a")]
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
        [InlineData("", "rows=-1")]
        [InlineData("", "rows=-10")]
        [InlineData(null, "rows=-1")]
        [InlineData(null, "rows=-10")]
        [InlineData("grid", "grid-rows=-1")]
        [InlineData("grid", "grid-rows=-10")]
        public void RowsPerPage_OnLessThanZeroQueryPageReturnsZero(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            pager.PageSizes.Clear();
            pager.Grid.Name = name;
            pager.RowsPerPage = 5;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void RowsPerPage_OnLessThanZeroCurrentPageReturnsZero(Int32 rows)
        {
            pager.ShowPageSizes = true;
            pager.RowsPerPage = rows;
            pager.PageSizes.Clear();

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "rows=10", 10)]
        [InlineData("", "rows=20", 20)]
        [InlineData("", "rows=60", 10)]
        [InlineData(null, "rows=10", 10)]
        [InlineData(null, "rows=20", 20)]
        [InlineData(null, "rows=60", 10)]
        [InlineData("grid", "grid-rows=10", 10)]
        [InlineData("grid", "grid-rows=20", 20)]
        [InlineData("grid", "grid-rows=60", 10)]
        public void RowsPerPage_AllowsOnlyFromPageSizes(String name, String query, Int32 rows)
        {
            pager.PageSizes = new Dictionary<Int32, String> { [10] = "10", [20] = "20" };
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            pager.Grid.Name = name;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = rows;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "rows=123")]
        [InlineData("", "ROWS=123")]
        [InlineData(null, "rows=123")]
        [InlineData(null, "ROWS=123")]
        [InlineData("grid", "grid-rows=123")]
        [InlineData("grid", "GRID-ROWS=123")]
        public void RowsPerPage_SetsRowsPerPageFromQuery(String name, String query)
        {
            pager.Grid.Query = HttpUtility.ParseQueryString(query);
            pager.ShowPageSizes = true;
            pager.PageSizes.Clear();
            pager.Grid.Name = name;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 123;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RowsPerPage_DoesNotUseQuery()
        {
            pager.Grid.Query = HttpUtility.ParseQueryString("rows=2");
            pager.ShowPageSizes = false;
            pager.RowsPerPage = 1;

            Int32 actual = pager.RowsPerPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

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
            pager.Grid.Query = HttpUtility.ParseQueryString("page=" + page);
            pager.PagesToDisplay = display;
            pager.RowsPerPage = 1;
            pager.TotalRows = 5;

            Int32 actual = pager.FirstDisplayPage;
            Int32 expected = first;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsGrid()
        {
            Object actual = new GridPager<GridModel>(pager.Grid).Grid;
            Object expected = pager.Grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridPager_SetsCssClasses()
        {
            Assert.Empty(new GridPager<GridModel>(pager.Grid).CssClasses);
        }

        [Fact]
        public void GridPager_SetsCurrentPage()
        {
            Int32 actual = new GridPager<GridModel>(pager.Grid).CurrentPage;
            Int32 expected = 1;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsRowsPerPage()
        {
            Int32 actual = new GridPager<GridModel>(pager.Grid).RowsPerPage;
            Int32 expected = 20;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsPagesToDisplay()
        {
            Int32 actual = new GridPager<GridModel>(pager.Grid).PagesToDisplay;
            Int32 expected = 5;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsDefaultPartialViewName()
        {
            String actual = new GridPager<GridModel>(pager.Grid).PartialViewName;
            String expected = "MvcGrid/_Pager";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsProcessorType()
        {
            GridProcessorType actual = new GridPager<GridModel>(pager.Grid).ProcessorType;
            GridProcessorType expected = GridProcessorType.Post;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridPager_SetsDefaultPageSizes()
        {
            Dictionary<Int32, String> expected = new Dictionary<Int32, String> { [10] = "10", [20] = "20", [50] = "50", [100] = "100" };
            Dictionary<Int32, String> actual = new GridPager<GridModel>(pager.Grid).PageSizes;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_SetsTotalRows()
        {
            pager.Process(new GridModel[100].AsQueryable());

            Int32 actual = pager.TotalRows;
            Int32 expected = 100;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_ReturnsAllItems()
        {
            IQueryable<GridModel> items = new[] { new GridModel(), new GridModel(), new GridModel() }.AsQueryable();
            pager.RowsPerPage = 0;
            pager.CurrentPage = 2;

            IEnumerable actual = pager.Process(items);
            IEnumerable expected = items.ToList();

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
    }
}

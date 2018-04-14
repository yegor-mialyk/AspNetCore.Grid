using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class HtmlGridExtensionsTests
    {
        private HtmlGrid<GridModel> htmlGrid;

        public HtmlGridExtensionsTests()
        {
            IGrid<GridModel> grid = new Grid<GridModel>(new GridModel[8]);
            IHtmlHelper html = Substitute.For<IHtmlHelper>();

            htmlGrid = new HtmlGrid<GridModel>(html, grid);
            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        #region Build<T>(this IHtmlGrid<T> html, Action<IGridColumnsOf<T>> builder)

        [Fact]
        public void Build_Columns()
        {
            Action<IGridColumnsOf<GridModel>> columns = Substitute.For<Action<IGridColumnsOf<GridModel>>>();

            htmlGrid.Build(columns);

            columns.Received()(htmlGrid.Grid.Columns);
        }

        [Fact]
        public void Build_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Build(columns => { });

            Assert.Same(expected, actual);
        }

        #endregion

        #region ProcessWith<T>(this IHtmlGrid<T> html, IGridProcessor<T> processor)

        [Fact]
        public void ProcessWith_AddsProcessorToGrid()
        {
            IGridProcessor<GridModel> processor = Substitute.For<IGridProcessor<GridModel>>();
            htmlGrid.Grid.Processors.Clear();
            htmlGrid.ProcessWith(processor);

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = processor;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void ProcessWith_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.ProcessWith(null);

            Assert.Same(expected, actual);
        }

        #endregion

        #region WithSourceUrl<T>(this IHtmlGrid<T> html, String url)

        [Fact]
        public void WithSourceUrl_SetsSourceUrl()
        {
            String actual = htmlGrid.WithSourceUrl("/test/index").Grid.SourceUrl;
            String expected = "/test/index";

            Assert.Same(expected, actual);
        }

        [Fact]
        public void WithSourceUrl_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.WithSourceUrl(null);

            Assert.Same(expected, actual);
        }

        #endregion

        #region MultiFilterable<T>(this IHtmlGrid<T> html)

        [Theory]
        [InlineData(null, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MultiFilterable_SetsIsMulti(Boolean? isMulti, Boolean? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.Filter.IsMulti = isMulti;

            htmlGrid.MultiFilterable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.Filter.IsMulti);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MultiFilterable_SetsIsEnabled(Boolean? isEnabled, Boolean? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.Filter.IsEnabled = isEnabled;

            htmlGrid.MultiFilterable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.Filter.IsEnabled);
        }

        [Fact]
        public void MultiFilterable_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.MultiFilterable();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Filterable<T>(this IHtmlGrid<T> html)

        [Theory]
        [InlineData(null, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Filterable_SetsIsEnabled(Boolean? isEnabled, Boolean? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.Filter.IsEnabled = isEnabled;

            htmlGrid.Filterable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.Filter.IsEnabled);
        }

        [Fact]
        public void Filterable_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Filterable();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Sortable<T>(this IHtmlGrid<T> html)

        [Theory]
        [InlineData(null, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Sortable_SetsIsEnabled(Boolean? isEnabled, Boolean? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.Sort.IsEnabled = isEnabled;

            htmlGrid.Sortable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.Sort.IsEnabled);
        }

        [Fact]
        public void Sortable_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Sortable();

            Assert.Same(expected, actual);
        }

        #endregion

        #region RowAttributed<T>(this IHtmlGrid<T> html, Func<T, Object> htmlAttributes)

        [Fact]
        public void RowAttributed_SetsRowAttributes()
        {
            Func<GridModel, Object> expected = (model) => new { data_id = 1 };
            Func<GridModel, Object> actual = htmlGrid.RowAttributed(expected).Grid.Rows.Attributes;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RowAttributed_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.RowAttributed(null);

            Assert.Same(expected, actual);
        }

        #endregion

        #region RowCss<T>(this IHtmlGrid<T> html, Func<T, String> cssClasses)

        [Fact]
        public void RowCss_SetsRowsCssClasses()
        {
            Func<GridModel, String> expected = (model) => "";
            Func<GridModel, String> actual = htmlGrid.RowCss(expected).Grid.Rows.CssClasses;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RowCss_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.RowCss(null);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Attributed<T>(this IHtmlGrid<T> html, Object htmlAttributes)

        [Fact]
        public void Attributed_SetsAttributes()
        {
            KeyValuePair<String, Object> actual = htmlGrid.Attributed(new { width = 1 }).Grid.Attributes.Single();
            KeyValuePair<String, Object> expected = new KeyValuePair<String, Object>("width", 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Attributed_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Attributed(new { width = 1 });

            Assert.Same(expected, actual);
        }

        #endregion

        #region AppendCss<T>(this IHtmlGrid<T> html, String cssClasses)

        [Theory]
        [InlineData("", "", "")]
        [InlineData("", " ", "")]
        [InlineData("", null, "")]
        [InlineData("", "test", "test")]
        [InlineData("", " test", "test")]
        [InlineData("", "test ", "test")]
        [InlineData("", " test ", "test")]

        [InlineData(" ", "", "")]
        [InlineData(" ", " ", "")]
        [InlineData(" ", null, "")]
        [InlineData(" ", "test", "test")]
        [InlineData(" ", " test", "test")]
        [InlineData(" ", "test ", "test")]
        [InlineData(" ", " test ", "test")]

        [InlineData("first", "", "first")]
        [InlineData("first", null, "first")]
        [InlineData("first", "test", "first test")]
        [InlineData("first", " test", "first test")]
        [InlineData("first", "test ", "first test")]
        [InlineData("first", " test ", "first test")]
        [InlineData("first ", " test ", "first  test")]
        [InlineData(" first ", " test ", "first  test")]
        public void AppendsCss_Classes(String current, String toAppend, String cssClasses)
        {
            htmlGrid.Grid.CssClasses = current;

            String expected = cssClasses;
            String actual = htmlGrid.AppendCss(toAppend).Grid.CssClasses;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AppendsCss_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.AppendCss("column-class");

            Assert.Same(expected, actual);
        }

        #endregion

        #region Css<T>(this IHtmlGrid<T> html, String cssClasses)

        [Fact]
        public void Css_SetsCssClasses()
        {
            String actual = htmlGrid.Css(" table ").Grid.CssClasses;
            String expected = "table";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Css_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Css(null);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Empty<T>(this IHtmlGrid<T> html, String text)

        [Fact]
        public void Empty_SetsEmptyText()
        {
            String actual = htmlGrid.Empty("Text").Grid.EmptyText;
            String expected = "Text";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Empty_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Empty("Text");

            Assert.Same(expected, actual);
        }

        #endregion

        #region Named<T>(this IHtmlGrid<T> html, String name)

        [Fact]
        public void Named_SetsName()
        {
            String actual = htmlGrid.Named("Name").Grid.Name;
            String expected = "Name";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Named_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Named("Name");

            Assert.Same(expected, actual);
        }

        #endregion

        #region Id<T>(this IHtmlGrid<T> html, String id)

        [Fact]
        public void Id_SetsId()
        {
            String actual = htmlGrid.Id("Test").Grid.Id;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Id_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Id("Test");

            Assert.Same(expected, actual);
        }

        #endregion

        #region WithFilterMode<T>(this IHtmlGrid<T> html, GridFilterMode mode)

        [Fact]
        public void WithFilterMode_SetsFilterMode()
        {
            GridFilterMode actual = htmlGrid.WithFilterMode(GridFilterMode.FilterRow).Grid.FilterMode;
            GridFilterMode expected = GridFilterMode.FilterRow;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithFilterMode_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.WithFilterMode(GridFilterMode.HeaderRow);

            Assert.Same(expected, actual);
        }

        #endregion

        #region WithFooter<T>(this IHtmlGrid<T> html, String partialViewName)

        [Fact]
        public void WithFooter_SetsFooterPartialViewName()
        {
            String actual = htmlGrid.WithFooter("Partial").Grid.FooterPartialViewName;
            String expected = "Partial";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithFooter_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.WithFooter("Partial");

            Assert.Same(expected, actual);
        }

        #endregion

        #region Pageable<T>(this IHtmlGrid<T> html, Action<IGridPager<T>> builder)

        [Fact]
        public void Pageable_Builder_DoesNotChangePager()
        {
            IGridPager<GridModel> pager = new GridPager<GridModel>(htmlGrid.Grid);
            htmlGrid.Grid.Pager = pager;

            htmlGrid.Pageable(gridPager => { });

            IGridPager actual = htmlGrid.Grid.Pager;
            IGridPager expected = pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_Builder_CreatesGridPager()
        {
            htmlGrid.Grid.Pager = null;

            htmlGrid.Pageable(pager => { });

            IGridPager<GridModel> expected = new GridPager<GridModel>(htmlGrid.Grid);
            IGridPager<GridModel> actual = htmlGrid.Grid.Pager;

            Assert.Equal(expected.FirstDisplayPage, actual.FirstDisplayPage);
            Assert.Equal(expected.PartialViewName, actual.PartialViewName);
            Assert.Equal(expected.PagesToDisplay, actual.PagesToDisplay);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.RowsPerPage, actual.RowsPerPage);
            Assert.Equal(expected.TotalPages, actual.TotalPages);
            Assert.Equal(expected.TotalRows, actual.TotalRows);
            Assert.Same(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Pageable_Builder_Pager()
        {
            htmlGrid.Grid.Pager = new GridPager<GridModel>(htmlGrid.Grid);
            IGridPager expected = htmlGrid.Grid.Pager;
            Boolean builderCalled = false;

            htmlGrid.Pageable(actual =>
            {
                Assert.Same(expected, actual);
                builderCalled = true;
            });

            Assert.True(builderCalled);
        }

        [Fact]
        public void Pageable_Builder_AddsGridProcessor()
        {
            htmlGrid.Grid.Processors.Clear();

            htmlGrid.Pageable(pager => { });

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_Builder_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Pageable(gridPager => { });

            Assert.Same(expected, actual);
        }

        #endregion

        #region Pageable<T>(this IHtmlGrid<T> html)

        [Fact]
        public void Pageable_DoesNotChangeExistingPager()
        {
            IGridPager<GridModel> pager = new GridPager<GridModel>(htmlGrid.Grid);
            htmlGrid.Grid.Pager = pager;

            htmlGrid.Pageable();

            Object expected = pager;
            Object actual = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_CreatesGridPager()
        {
            htmlGrid.Grid.Pager = null;

            htmlGrid.Pageable();

            IGridPager<GridModel> expected = new GridPager<GridModel>(htmlGrid.Grid);
            IGridPager<GridModel> actual = htmlGrid.Grid.Pager;

            Assert.Equal(expected.FirstDisplayPage, actual.FirstDisplayPage);
            Assert.Equal(expected.PartialViewName, actual.PartialViewName);
            Assert.Equal(expected.PagesToDisplay, actual.PagesToDisplay);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.RowsPerPage, actual.RowsPerPage);
            Assert.Equal(expected.TotalPages, actual.TotalPages);
            Assert.Equal(expected.TotalRows, actual.TotalRows);
            Assert.Same(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Pageable_AddsGridPagerProcessor()
        {
            htmlGrid.Grid.Processors.Clear();

            htmlGrid.Pageable();

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_DoesNotReaddGridProcessor()
        {
            htmlGrid.Grid.Processors.Clear();

            htmlGrid.Pageable();
            htmlGrid.Pageable();

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Pageable();

            Assert.Same(expected, actual);
        }

        #endregion
    }
}

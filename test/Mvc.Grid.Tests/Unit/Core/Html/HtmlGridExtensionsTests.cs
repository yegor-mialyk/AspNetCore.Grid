using Microsoft.AspNetCore.Http;
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
            IHtmlHelper html = Substitute.For<IHtmlHelper>();
            IGrid<GridModel> grid = new Grid<GridModel>(new GridModel[8]);
            html.ViewContext.Returns(new ViewContext { HttpContext = new DefaultHttpContext() });

            htmlGrid = new HtmlGrid<GridModel>(html, grid);
            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        [Fact]
        public void Build_Columns()
        {
            Action<IGridColumnsOf<GridModel>> columns = Substitute.For<Action<IGridColumnsOf<GridModel>>>();

            htmlGrid.Build(columns);

            columns.Received()(htmlGrid.Grid.Columns);
        }

        [Fact]
        public void Build_AddsSortProcessor()
        {
            htmlGrid.Grid.Processors.Clear();

            htmlGrid.Build(columns => { });

            Object expected = htmlGrid.Grid.Sort;
            Object actual = htmlGrid.Grid.Processors.Single();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Build_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Build(columns => { });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingUrl_SetsUrl()
        {
            String actual = htmlGrid.UsingUrl("/test/index").Grid.Url;
            String expected = "/test/index";

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingUrl_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.UsingUrl("");

            Assert.Same(expected, actual);
        }

        [Theory]
        [InlineData(null, GridFilterType.Double)]
        [InlineData(GridFilterType.Single, GridFilterType.Single)]
        [InlineData(GridFilterType.Double, GridFilterType.Double)]
        public void Filterable_SetsType(GridFilterType? type, GridFilterType? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.Filter.Type = type;

            htmlGrid.Filterable(GridFilterType.Double);

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.Filter.Type);
        }

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

        [Fact]
        public void RowAttributed_SetsRowAttributes()
        {
            Func<GridModel, Object>? expected = (model) => new { data_id = 1 };
            Func<GridModel, Object>? actual = htmlGrid.RowAttributed(expected).Grid.Rows.Attributes;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RowAttributed_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.RowAttributed(model => new { });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Attributed_SetsAttributes()
        {
            htmlGrid.Grid.Attributes["width"] = 10;
            htmlGrid.Grid.Attributes["class"] = "test";

            IDictionary<String, Object?> actual = htmlGrid.Attributed(new { width = 1 }).Grid.Attributes;
            IDictionary<String, Object?> expected = new Dictionary<String, Object?> { ["width"] = 1, ["class"] = "test" };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Attributed_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Attributed(new { width = 1 });

            Assert.Same(expected, actual);
        }

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
            htmlGrid.Grid.Attributes["class"] = current;

            String? expected = cssClasses;
            String? actual = htmlGrid.AppendCss(toAppend).Grid.Attributes["class"]?.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AppendsCss_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.AppendCss("column-class");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Css_SetsCssClasses()
        {
            String? expected = "table";
            String? actual = htmlGrid.Css(" table ").Grid.Attributes["class"]?.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Css_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Css("");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Empty_SetsEmptyText()
        {
            String? expected = "Text";
            String? actual = htmlGrid.Empty("Text").Grid.EmptyText;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Empty_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Empty("Text");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Named_SetsName()
        {
            String expected = "Test";
            String actual = htmlGrid.Named("Test").Grid.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Named_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Named("Name");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Id_SetsId()
        {
            String? expected = "Test";
            String? actual = htmlGrid.Id("Test").Grid.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Id_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Id("Test");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingProcessor_AddsProcessorToGrid()
        {
            IGridProcessor<GridModel> processor = Substitute.For<IGridProcessor<GridModel>>();
            htmlGrid.Grid.Processors.Clear();

            htmlGrid.UsingProcessor(processor);

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = processor;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingProcessor_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.UsingProcessor(Substitute.For<IGridProcessor<GridModel>>());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingProcessingMode_SetsProcessingMode()
        {
            GridProcessingMode actual = htmlGrid.UsingProcessingMode(GridProcessingMode.Manual).Grid.Mode;
            GridProcessingMode expected = GridProcessingMode.Manual;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UsingProcessingMode_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.UsingProcessingMode(GridProcessingMode.Manual);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingFilterMode_SetsFilterMode()
        {
            GridFilterMode actual = htmlGrid.UsingFilterMode(GridFilterMode.Row).Grid.FilterMode;
            GridFilterMode expected = GridFilterMode.Row;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UsingFilterMode_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.UsingFilterMode(GridFilterMode.Header);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UsingFooter_SetsFooterPartialViewName()
        {
            String actual = htmlGrid.UsingFooter("Partial").Grid.FooterPartialViewName;
            String expected = "Partial";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UsingFooter_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.UsingFooter("Partial");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_DoesNotChangePager()
        {
            IGridPager<GridModel> pager = new GridPager<GridModel>(htmlGrid.Grid);
            htmlGrid.Grid.Pager = pager;

            htmlGrid.Pageable();

            IGridPager actual = htmlGrid.Grid.Pager;
            IGridPager expected = pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_CreatesGridPager()
        {
            htmlGrid.Grid.Pager = null;

            htmlGrid.Pageable();

            IGridPager<GridModel> expected = new GridPager<GridModel>(htmlGrid.Grid);
            IGridPager<GridModel> actual = htmlGrid.Grid.Pager!;

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
        public void Pageable_ConfiguresPager()
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
        public void Pageable_AddsGridProcessor()
        {
            htmlGrid.Grid.Processors.Clear();

            htmlGrid.Pageable();

            Object? actual = htmlGrid.Grid.Processors.Single();
            Object? expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_ReturnsHtmlGrid()
        {
            Object expected = htmlGrid;
            Object actual = htmlGrid.Pageable();

            Assert.Same(expected, actual);
        }
    }
}

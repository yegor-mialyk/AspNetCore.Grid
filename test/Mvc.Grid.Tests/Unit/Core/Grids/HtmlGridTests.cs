using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.WebEncoders;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class HtmlGridTests
    {
        private HtmlGrid<GridModel> htmlGrid;
        private IGrid<GridModel> grid;
        private IHtmlHelper html;

        public HtmlGridTests()
        {
            html = Substitute.For<IHtmlHelper>();
            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.HttpContext = new DefaultHttpContext();
            grid = new Grid<GridModel>(new GridModel[8]);

            htmlGrid = new HtmlGrid<GridModel>(html, grid);
            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        #region Constructor: HtmlGrid(HtmlHelper html, IGrid<T> grid)

        [Fact]
        public void HtmlGrid_DoesNotChangeExistingQuery()
        {
            IReadableStringCollection query = grid.Query = TestHelper.ParseQuery("");

            Object actual = new HtmlGrid<GridModel>(html, grid).Grid.Query;
            Object expected = query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGridQuery()
        {
            grid.Query = null;

            IReadableStringCollection expected = html.ViewContext.HttpContext.Request.Query;
            IReadableStringCollection actual = new HtmlGrid<GridModel>(html, grid).Grid.Query;

            foreach (String key in expected.Keys)
                Assert.Equal(expected[key], actual[key]);

            Assert.Equal(expected.Keys, actual.Keys);
        }

        [Fact]
        public void HtmlGrid_DoesNotChangeExistingHttpContext()
        {
            ViewContext viewContext = grid.ViewContext = new ViewContext();

            Object actual = new HtmlGrid<GridModel>(html, grid).Grid.ViewContext;
            Object expected = viewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHttpContext()
        {
            grid.ViewContext = null;

            ViewContext actual = new HtmlGrid<GridModel>(html, grid).Grid.ViewContext;
            ViewContext expected = html.ViewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsDefaultPartialViewName()
        {
            String actual = new HtmlGrid<GridModel>(null, grid).PartialViewName;
            String expected = "MvcGrid/_Grid";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHtml()
        {
            IHtmlHelper expected = Substitute.For<IHtmlHelper>();
            IHtmlHelper actual = new HtmlGrid<GridModel>(expected, grid).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGrid()
        {
            IGrid<GridModel> actual = new HtmlGrid<GridModel>(null, grid).Grid;
            IGrid<GridModel> expected = grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Build(Action<IGridColumnsOf<T>> builder)

        [Fact]
        public void Build_BuildsColumns()
        {
            IGridColumnsOf<GridModel> expected = htmlGrid.Grid.Columns;
            Boolean builderCalled = false;

            htmlGrid.Build(actual =>
            {
                Assert.Same(expected, actual);
                builderCalled = true;
            });

            Assert.True(builderCalled);
        }

        [Fact]
        public void Build_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Build(columns => { });
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: ProcessWith(IGridProcessor<T> processor)

        [Fact]
        public void ProcessWith_AddsProcessorToGrid()
        {
            IGridProcessor<GridModel> processor = Substitute.For<IGridProcessor<GridModel>>();
            htmlGrid.Grid.Processors.Clear();
            htmlGrid.ProcessWith(processor);

            IGridProcessor<GridModel> actual = htmlGrid.Grid.Processors.Single();
            IGridProcessor<GridModel> expected = processor;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void ProcessWith_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.ProcessWith(null);
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Filterable(Boolean isFilterable)

        [Theory]
        [InlineData(null, false, false)]
        [InlineData(null, true, true)]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void Filterable_Set_SetsIsFilterable(Boolean? isColumnFilterable, Boolean isGridFilterable, Boolean? expectedIsFilterable)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.IsFilterable = isColumnFilterable;

            htmlGrid.Filterable(isGridFilterable);

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expectedIsFilterable, actual.IsFilterable);
        }

        [Fact]
        public void Filterable_Set_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Filterable(true);
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: MultiFilterable()

        [Theory]
        [InlineData(null, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MultiFilterable_SetsIsMultiFilterable(Boolean? isMultiFilterable, Boolean? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.IsMultiFilterable = isMultiFilterable;

            htmlGrid.MultiFilterable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.IsMultiFilterable);
        }

        [Fact]
        public void MultiFilterable_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.MultiFilterable();
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Filterable()

        [Theory]
        [InlineData(null, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Filterable_SetsIsFilterable(Boolean? isColumnFilterable, Boolean? expected)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.IsFilterable = isColumnFilterable;

            htmlGrid.Filterable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expected, actual.IsFilterable);
        }

        [Fact]
        public void Filterable_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Filterable();
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Sortable(Boolean isSortable)

        [Theory]
        [InlineData(null, false, false)]
        [InlineData(null, true, true)]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void Sortable_Set_SetsIsSortable(Boolean? isColumnSortable, Boolean isGridSortable, Boolean? expectedIsSortable)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.IsSortable = isColumnSortable;

            htmlGrid.Sortable(isGridSortable);

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expectedIsSortable, actual.IsSortable);
        }

        [Fact]
        public void Sortable_Set_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Sortable(true);
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Sortable()

        [Theory]
        [InlineData(null, true)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void Sortable_SetsIsSortableToTrue(Boolean? isColumnSortable, Boolean? expectedIsSortable)
        {
            foreach (IGridColumn column in htmlGrid.Grid.Columns)
                column.IsSortable = isColumnSortable;

            htmlGrid.Sortable();

            foreach (IGridColumn actual in htmlGrid.Grid.Columns)
                Assert.Equal(expectedIsSortable, actual.IsSortable);
        }

        [Fact]
        public void Sortable_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Sortable();
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: RowCss(Func<T, String> cssClasses)

        [Fact]
        public void RowCss_SetsRowsCssClasses()
        {
            Func<GridModel, String> expected = (model) => "";
            Func<GridModel, String> actual = htmlGrid.RowCss(expected).Grid.Rows.CssClasses;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RowCss_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.RowCss(null);
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Css(String cssClasses)

        [Fact]
        public void Css_SetsCssClasses()
        {
            String actual = htmlGrid.Css("table").Grid.CssClasses;
            String expected = "table";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Css_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Css("table");
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Empty(String text)

        [Fact]
        public void Empty_SetsEmptyText()
        {
            String actual = htmlGrid.Empty("Text").Grid.EmptyText;
            String expected = "Text";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Empty_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Empty("Text");
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Named(String name)

        [Fact]
        public void Named_SetsName()
        {
            String actual = htmlGrid.Named("Name").Grid.Name;
            String expected = "Name";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Named_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Named("Name");
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Pageable(Action<IGridPager<T>> builder)

        [Fact]
        public void Pageable_Builder_DoesNotChangeExistingPager()
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
            Assert.Equal(expected.InitialPage, actual.InitialPage);
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.RowsPerPage, actual.RowsPerPage);
            Assert.Equal(expected.TotalPages, actual.TotalPages);
            Assert.Equal(expected.TotalRows, actual.TotalRows);
            Assert.Same(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Pageable_Builder_BuildsPager()
        {
            htmlGrid.Grid.Pager = Substitute.For<IGridPager<GridModel>>();
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
            htmlGrid.Grid.Processors = new List<IGridProcessor<GridModel>>();

            htmlGrid.Pageable(pager => { });

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_Builder_DoesNotReaddGridProcessor()
        {
            htmlGrid.Grid.Processors = new List<IGridProcessor<GridModel>>();

            htmlGrid.Pageable(pager => { });
            htmlGrid.Pageable(pager => { });

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_Builder_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Pageable(gridPager => { });
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Pageable()

        [Fact]
        public void Pageable_DoesNotChangeExistingPager()
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
            IGridPager<GridModel> actual = htmlGrid.Grid.Pager;

            Assert.Equal(expected.FirstDisplayPage, actual.FirstDisplayPage);
            Assert.Equal(expected.PartialViewName, actual.PartialViewName);
            Assert.Equal(expected.PagesToDisplay, actual.PagesToDisplay);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.InitialPage, actual.InitialPage);
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.RowsPerPage, actual.RowsPerPage);
            Assert.Equal(expected.TotalPages, actual.TotalPages);
            Assert.Equal(expected.TotalRows, actual.TotalRows);
            Assert.Same(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Pageable_AddsGridPagerProcessor()
        {
            htmlGrid.Grid.Processors = new List<IGridProcessor<GridModel>>();

            htmlGrid.Pageable();

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_DoesNotReaddGridProcessor()
        {
            htmlGrid.Grid.Processors = new List<IGridProcessor<GridModel>>();

            htmlGrid.Pageable();
            htmlGrid.Pageable();

            Object actual = htmlGrid.Grid.Processors.Single();
            Object expected = htmlGrid.Grid.Pager;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Pageable_ReturnsSameGrid()
        {
            IHtmlGrid<GridModel> actual = htmlGrid.Pageable();
            IHtmlGrid<GridModel> expected = htmlGrid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: WriteTo(TextWriter writer, IHtmlEncoder encoder)

        [Fact]
        public void WriteTo_WritesPartialView()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            html.PartialAsync(htmlGrid.PartialViewName, htmlGrid.Grid, null).Returns(view);
            StringWriter writer = new StringWriter();

            htmlGrid.WriteTo(writer, new HtmlEncoder());

            String actual = writer.GetStringBuilder().ToString();
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

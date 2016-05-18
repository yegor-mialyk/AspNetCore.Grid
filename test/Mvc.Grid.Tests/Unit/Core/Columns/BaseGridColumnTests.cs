using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        #region IGridColumn<T>.Expression

        [Fact]
        public void IGridColumnExpression_ReturnsExpression()
        {
            IGridColumn<GridModel> gridColumn = column;

            Object actual = gridColumn.Expression;
            Object expected = column.Expression;

            Assert.Same(expected, actual);
        }

        #endregion

        #region IFilterableColumn.Filter

        [Fact]
        public void IFilterableColumnFilter_ReturnsFilter()
        {
            IFilterableColumn filterableColumn = column;

            Object actual = filterableColumn.Filter;
            Object expected = column.Filter;

            Assert.Same(expected, actual);
        }

        #endregion

        #region RenderedAs(Func<T, Object> value)

        [Fact]
        public void RenderedAs_SetsRenderValue()
        {
            Func<GridModel, Object> expected = (model) => model.Name;
            Func<GridModel, Object> actual = (column.RenderedAs(expected) as BaseGridColumn<GridModel, String>).RenderValue;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RenderedAs_ReturnsItself()
        {
            IGridColumn actual = column.RenderedAs(model => model.Name);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region MultiFilterable(Boolean isMultiple)

        [Fact]
        public void MultiFilterable_SetsIsMultiFilterable()
        {
            Boolean? actual = column.MultiFilterable(true).IsMultiFilterable;
            Boolean? expected = true;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MultiFilterable_ReturnsItself()
        {
            IGridColumn actual = column.MultiFilterable(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Filterable(Boolean isFilterable)

        [Fact]
        public void Filterable_SetsIsFilterable()
        {
            Boolean? actual = column.Filterable(true).IsFilterable;
            Boolean? expected = true;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Filterable_ReturnsItself()
        {
            IGridColumn actual = column.Filterable(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region FilteredAs(String filterName)

        [Fact]
        public void FilteredAs_SetsFilterName()
        {
            String actual = column.FilteredAs("Numeric").FilterName;
            String expected = "Numeric";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilteredAs_ReturnsItself()
        {
            IGridColumn actual = column.FilteredAs("Numeric");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region InitialSort(GridSortOrder order)

        [Fact]
        public void InitialSort_SetsInitialSortOrder()
        {
            GridSortOrder? actual = column.InitialSort(GridSortOrder.Desc).InitialSortOrder;
            GridSortOrder? expected = GridSortOrder.Desc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InitialSort_ReturnsItself()
        {
            IGridColumn actual = column.InitialSort(GridSortOrder.Desc);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region FirstSort(GridSortOrder order)

        [Fact]
        public void FirstSort_SetsFirstOrder()
        {
            GridSortOrder? actual = column.FirstSort(GridSortOrder.Desc).FirstSortOrder;
            GridSortOrder? expected = GridSortOrder.Desc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstSort_ReturnsItself()
        {
            IGridColumn actual = column.FirstSort(GridSortOrder.Desc);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Sortable(Boolean isSortable)

        [Fact]
        public void Sortable_SetsIsSortable()
        {
            Boolean? actual = column.Sortable(true).IsSortable;
            Boolean? expected = true;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sortable_ReturnsItself()
        {
            IGridColumn actual = column.Sortable(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Encoded(Boolean isEncoded)

        [Fact]
        public void Encoded_SetsIsEncoded()
        {
            Assert.True(column.Encoded(true).IsEncoded);
        }

        [Fact]
        public void Encoded_ReturnsItself()
        {
            IGridColumn actual = column.Encoded(true);
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Formatted(String format)

        [Fact]
        public void Formatted_SetsFormat()
        {
            String actual = column.Formatted("Format").Format;
            String expected = "Format";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Formatted_ReturnsItself()
        {
            IGridColumn actual = column.Formatted("Format");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Css(String cssClasses)

        [Fact]
        public void Css_SetsCssClasses()
        {
            String actual = column.Css("column-class").CssClasses;
            String expected = "column-class";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Css_ReturnsItself()
        {
            IGridColumn actual = column.Css("column-class");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Titled(Object value)

        [Fact]
        public void Titled_SetsHtmlContentTitle()
        {
            IHtmlContent expected = new HtmlString("HtmlContent Title");
            IHtmlContent actual = column.Titled(expected).Title;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Titled_SetsObjectTitle()
        {
            String actual = column.Titled(new Object()).Title.ToString();
            String expected = new Object().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Titled_SetsEmptyTitle()
        {
            Assert.Empty(column.Titled(null).Title.ToString());
        }

        [Fact]
        public void Titled_SetsTitle()
        {
            String actual = column.Titled("Title").Title.ToString();
            String expected = "Title";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Titled_ReturnsItself()
        {
            IGridColumn actual = column.Titled("Title");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Named(String name)

        [Fact]
        public void Named_SetsName()
        {
            String actual = column.Named("Name").Name;
            String expected = "Name";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Named_ReturnsItself()
        {
            IGridColumn actual = column.Named("Name");
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnTests
    {
        private IGridFilters filters;
        private IGrid<GridModel> grid;
        private GridColumn<GridModel, Object> column;

        public GridColumnTests()
        {
            filters = Substitute.For<IGridFilters>();
            grid = new Grid<GridModel>(new GridModel[0]);

            column = new GridColumn<GridModel, Object>(grid, model => model.Name);

            grid.Query = new QueryCollection();
            grid.ViewContext = new ViewContext();
            grid.ViewContext.HttpContext = Substitute.For<HttpContext>();
            grid.ViewContext.HttpContext.RequestServices.GetService<IGridFilters>().Returns(filters);
        }

        #region Filter

        [Fact]
        public void Filter_NoFilters_Throws()
        {
            grid.ViewContext.HttpContext.RequestServices.GetService<IGridFilters>().Returns(null as IGridFilters);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => column.Filter);
            Assert.Equal($"No service for type '{typeof(IGridFilters).FullName}' has been registered.", exception.Message);
        }

        [Fact]
        public void Filter_ReturnsFromGridFilters()
        {
            GridColumnFilter<GridModel, Object> filter = new GridColumnFilter<GridModel, Object>(column);
            filters.GetFilter(column).Returns(filter);

            Object actual = column.Filter;
            Object expected = filter;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Filter_Get_Caches()
        {
            GridColumnFilter<GridModel, Object> filter = new GridColumnFilter<GridModel, Object>(column);
            filters.GetFilter(column).Returns(filter);

            IGridColumnFilter<GridModel, Object> cachedFilter = column.Filter;
            filter = new GridColumnFilter<GridModel, Object>(column);

            filters.GetFilter(column).Returns(filter);

            Object expected = cachedFilter;
            Object actual = column.Filter;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Filter_Set_Caches()
        {
            GridColumnFilter<GridModel, Object> filter = new GridColumnFilter<GridModel, Object>(column);

            column.Filter = new GridColumnFilter<GridModel, Object>(column);
            column.Filter = filter;

            IGridColumnFilter<GridModel, Object> actual = column.Filter;
            IGridColumnFilter<GridModel, Object> expected = filter;

            Assert.Same(expected, actual);
        }

        #endregion

        #region GridColumn(IGrid<T> grid, Expression<Func<T, TValue>> expression)

        [Fact]
        public void GridColumn_SetsGrid()
        {
            IGrid actual = new GridColumn<GridModel, Object>(grid, model => model.Name).Grid;
            IGrid expected = grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsIsEncoded()
        {
            Assert.True(new GridColumn<GridModel, Object>(grid, model => model.Name).IsEncoded);
        }

        [Fact]
        public void GridColumn_SetsExpression()
        {
            Expression<Func<GridModel, String>> expected = (model) => model.Name;
            Expression<Func<GridModel, String>> actual = new GridColumn<GridModel, String>(grid, expected).Expression;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_NotMemberExpression_SetsEmptyTitle()
        {
            Assert.Empty(new GridColumn<GridModel, Object>(grid, model => model.ToString()).Title.ToString());
        }

        [Fact]
        public void GridColumn_NoDisplayAttribute_SetsEmptyTitle()
        {
            Assert.Empty(new GridColumn<GridModel, Object>(grid, model => model.Name).Title.ToString());
        }

        [Fact]
        public void GridColumn_DisplayAttribute_SetsTitleFromDisplayName()
        {
            DisplayAttribute display = typeof(GridModel).GetProperty("Text").GetCustomAttribute<DisplayAttribute>();
            column = new GridColumn<GridModel, Object>(grid, model => model.Text);

            String actual = column.Title.ToString();
            String expected = display.GetName();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_DisplayAttribute_SetsTitleFromDisplayShortName()
        {
            DisplayAttribute display = typeof(GridModel).GetProperty("ShortText").GetCustomAttribute<DisplayAttribute>();
            column = new GridColumn<GridModel, Object>(grid, model => model.ShortText);

            String expected = display.GetShortName();
            String actual = column.Title.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsExpressionValueAsCompiledExpression()
        {
            GridModel model = new GridModel { Name = "TestName" };

            String actual = column.ExpressionValue(model) as String;
            String expected = "TestName";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsProcessorTypeAsPreProcessor()
        {
            GridProcessorType actual = new GridColumn<GridModel, Object>(grid, model => model.Name).ProcessorType;
            GridProcessorType expected = GridProcessorType.Pre;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsDefaultSort()
        {
            GridColumn<GridModel, String> column = new GridColumn<GridModel, String>(grid, model => model.Name);

            IGridColumnSort<GridModel, String> expected = new GridColumnSort<GridModel, String>(column);
            IGridColumnSort<GridModel, String> actual = column.Sort;

            Assert.Same(column, actual.Column);
            Assert.Null(actual.InitialOrder);
            Assert.Null(actual.FirstOrder);
            Assert.Null(actual.IsEnabled);
            Assert.Null(actual.Order);
        }

        [Fact]
        public void GridColumn_SetsNameFromExpression()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            String actual = new GridColumn<GridModel, String>(grid, expression).Name;
            String expected = ExpressionHelper.GetExpressionText(expression);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Process(IQueryable<T> items)

        [Fact]
        public void Process_ReturnsFilteredAndSortedItems()
        {
            column.Filter = Substitute.For<IGridColumnFilter<GridModel, Object>>();
            column.Sort = Substitute.For<IGridColumnSort<GridModel, Object>>();

            IQueryable<GridModel> filteredItems = new GridModel[2].AsQueryable();
            IQueryable<GridModel> sortedItems = new GridModel[2].AsQueryable();
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();

            column.Sort.Apply(filteredItems).Returns(sortedItems);
            column.Filter.Apply(items).Returns(filteredItems);

            IQueryable<GridModel> actual = column.Process(items);
            IQueryable<GridModel> expected = sortedItems;

            Assert.Same(expected, actual);
        }

        #endregion

        #region ValueFor(IGridRow<Object> row)

        [Fact]
        public void ValueFor_NullReferenceInExpressionValue_ReturnsEmpty()
        {
            column.ExpressionValue = (model) => model.Name;

            String actual = column.ValueFor(new GridRow<Object>(null)).ToString();

            Assert.Empty(actual);
        }

        [Fact]
        public void ValueFor_NullReferenceInRenderValue_ReturnsEmpty()
        {
            column.RenderValue = (model) => model.Name;

            String actual = column.ValueFor(new GridRow<Object>(null)).ToString();

            Assert.Empty(actual);
        }

        [Fact]
        public void ValueFor_ExpressionValue_ThrowsNotNullReferenceException()
        {
            column.ExpressionValue = (model) => Int32.Parse("Zero");

            Assert.Throws<FormatException>(() => column.ValueFor(new GridRow<Object>(null)));
        }

        [Fact]
        public void ValueFor_RenderValue_ThrowsNotNullReferenceException()
        {
            column.RenderValue = (model) => Int32.Parse("Zero");

            Assert.Throws<FormatException>(() => column.ValueFor(new GridRow<Object>(null)));
        }

        [Theory]
        [InlineData(null, "For {0}", true, "")]
        [InlineData(null, "For {0}", false, "")]
        [InlineData("<name>", null, true, "<name>")]
        [InlineData("<name>", null, false, "<name>")]
        [InlineData("<name>", "For <{0}>", true, "<name>")]
        [InlineData("<name>", "For <{0}>", false, "<name>")]
        public void ValueFor_RenderValue_Html(String value, String format, Boolean isEncoded, String renderedValue)
        {
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Content = value == null ? null : new HtmlString(value) });
            column.RenderValue = (model) => model.Content;
            column.ExpressionValue = null;
            column.IsEncoded = isEncoded;
            column.Format = format;

            String actual = column.ValueFor(row).ToString();
            String expected = renderedValue;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "For {0}", true, "")]
        [InlineData(null, "For {0}", false, "")]
        [InlineData("<name>", null, true, "<name>")]
        [InlineData("<name>", null, false, "<name>")]
        [InlineData("<name>", "For <{0}>", true, "<name>")]
        [InlineData("<name>", "For <{0}>", false, "<name>")]
        public void ValueFor_ExpressionValue_Html(String value, String format, Boolean isEncoded, String expressionValue)
        {
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Content = value == null ? null : new HtmlString(value) });
            column = new GridColumn<GridModel, Object>(grid, model => model.Content);
            column.IsEncoded = isEncoded;
            column.Format = format;

            String actual = column.ValueFor(row).ToString();
            String expected = expressionValue;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "For {0}", true, "")]
        [InlineData(null, "For {0}", false, "")]
        [InlineData("<name>", null, false, "<name>")]
        [InlineData("<name>", null, true, "&lt;name&gt;")]
        [InlineData("<name>", "For <{0}>", false, "For <<name>>")]
        [InlineData("<name>", "For <{0}>", true, "For &lt;&lt;name&gt;&gt;")]
        public void ValueFor_RenderValue(String value, String format, Boolean isEncoded, String renderedValue)
        {
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Name = value });
            column.RenderValue = (model) => model.Name;
            column.ExpressionValue = null;
            column.IsEncoded = isEncoded;
            column.Format = format;

            String actual = column.ValueFor(row).ToString();
            String expected = renderedValue;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "For {0}", true, "")]
        [InlineData(null, "For {0}", false, "")]
        [InlineData("<name>", null, false, "<name>")]
        [InlineData("<name>", null, true, "&lt;name&gt;")]
        [InlineData("<name>", "For <{0}>", false, "For <<name>>")]
        [InlineData("<name>", "For <{0}>", true, "For &lt;&lt;name&gt;&gt;")]
        public void ValueFor_ExpressionValue(String value, String format, Boolean isEncoded, String expressionValue)
        {
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Name = value });
            column.IsEncoded = isEncoded;
            column.Format = format;

            String actual = column.ValueFor(row).ToString();
            String expected = expressionValue;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

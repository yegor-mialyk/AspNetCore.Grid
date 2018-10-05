using Microsoft.AspNetCore.Html;
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
        private GridColumn<GridModel, Object> column;

        public GridColumnTests()
        {
            IGrid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);
            column = new GridColumn<GridModel, Object>(grid, model => model.Name);
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

        #region GridColumn(IGrid<T> grid, Expression<Func<T, TValue>> expression)

        [Fact]
        public void GridColumn_SetsGrid()
        {
            Object actual = new GridColumn<GridModel, Int32>(column.Grid, model => 0).Grid;
            Object expected = column.Grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsIsEncoded()
        {
            Assert.True(new GridColumn<GridModel, Int32>(column.Grid, model => 1).IsEncoded);
        }

        [Fact]
        public void GridColumn_SetsExpression()
        {
            Object actual = new GridColumn<GridModel, Object>(column.Grid, column.Expression).Expression;
            Object expected = column.Expression;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_NotMemberExpression_SetsEmptyTitle()
        {
            Assert.Empty(new GridColumn<GridModel, Int32>(column.Grid, model => 1).Title.ToString());
        }

        [Fact]
        public void GridColumn_NoDisplayAttribute_SetsEmptyTitle()
        {
            Assert.Empty(new GridColumn<GridModel, Object>(column.Grid, model => model.Name).Title.ToString());
        }

        [Fact]
        public void GridColumn_DisplayAttribute_SetsTitleFromDisplayName()
        {
            DisplayAttribute display = typeof(GridModel).GetProperty("Text").GetCustomAttribute<DisplayAttribute>();
            column = new GridColumn<GridModel, Object>(column.Grid, model => model.Text);

            String actual = column.Title.ToString();
            String expected = display.GetName();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_DisplayAttribute_SetsTitleFromDisplayShortName()
        {
            DisplayAttribute display = typeof(GridModel).GetProperty("ShortText").GetCustomAttribute<DisplayAttribute>();
            column = new GridColumn<GridModel, Object>(column.Grid, model => model.ShortText);

            String expected = display.GetShortName();
            String actual = column.Title.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsExpressionValue()
        {
            GridModel model = new GridModel { Name = "Testing name" };

            Object actual = column.ExpressionValue(model);
            Object expected = "Testing name";

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsPreProcessorType()
        {
            GridProcessorType actual = new GridColumn<GridModel, Object>(column.Grid, model => 0).ProcessorType;
            GridProcessorType expected = GridProcessorType.Pre;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsDefaultSort()
        {
            column = new GridColumn<GridModel, Object>(column.Grid, model => model.Name);

            IGridColumnSort<GridModel, Object> actual = column.Sort;

            Assert.Same(column, actual.Column);
            Assert.Null(actual.InitialOrder);
            Assert.Null(actual.FirstOrder);
            Assert.Null(actual.IsEnabled);
            Assert.Null(actual.Order);
        }

        [Fact]
        public void GridColumn_SetsNameFromUnderscoredExpression()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.UnderScored_Field;

            String actual = new GridColumn<GridModel, String>(column.Grid, expression).Name;
            String expected = "under-scored-field";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsNameFromExpression()
        {
            Expression<Func<GridModel, Boolean?>> expression = (model) => model.NIsChecked;

            String actual = new GridColumn<GridModel, Boolean?>(column.Grid, expression).Name;
            String expected = "n-is-checked";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsDefaultFilter()
        {
            column = new GridColumn<GridModel, Object>(column.Grid, model => model.Name);

            IGridColumnFilter<GridModel, Object> actual = column.Filter;

            Assert.Equal(column, actual.Column);
            Assert.Null(actual.IsEnabled);
            Assert.Null(actual.Operator);
            Assert.Null(actual.IsMulti);
            Assert.Null(actual.Second);
            Assert.Null(actual.First);
            Assert.Null(actual.Name);
        }

        #endregion

        #region Process(IQueryable<T> items)

        [Fact]
        public void Process_ReturnsFilteredAndSortedItems()
        {
            column.Filter = Substitute.For<IGridColumnFilter<GridModel, Object>>();
            column.Sort = Substitute.For<IGridColumnSort<GridModel, Object>>();

            IQueryable<GridModel> filtered = new GridModel[2].AsQueryable();
            IQueryable<GridModel> sorted = new GridModel[2].AsQueryable();
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();

            column.Filter.Apply(items).Returns(filtered);
            column.Sort.Apply(filtered).Returns(sorted);

            Object actual = column.Process(items);
            Object expected = sorted;

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
            column = new GridColumn<GridModel, Object>(column.Grid, model => model.Content);
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

        [Fact]
        public void ValueFor_BadValue_EnumExpressionValue()
        {
            GridColumn<GridModel, TestEnum> enumColumn = new GridColumn<GridModel, TestEnum>(column.Grid, model => model.Enum);
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Enum = (TestEnum)2 });

            String actual = enumColumn.ValueFor(row).ToString();
            String expected = "2";

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.First, "1st")]
        [InlineData(TestEnum.Second, "2nd")]
        public void ValueFor_NullableEnumExpressionValue(TestEnum value, String expressionValue)
        {
            GridColumn<GridModel, TestEnum?> enumColumn = new GridColumn<GridModel, TestEnum?>(column.Grid, model => model.Enum);
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Enum = value });

            String actual = enumColumn.ValueFor(row).ToString();
            String expected = expressionValue;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.First, "1st")]
        [InlineData(TestEnum.Second, "2nd")]
        public void ValueFor_EnumExpressionValue(TestEnum value, String expressionValue)
        {
            GridColumn<GridModel, TestEnum> enumColumn = new GridColumn<GridModel, TestEnum>(column.Grid, model => model.Enum);
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Enum = value });

            String actual = enumColumn.ValueFor(row).ToString();
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

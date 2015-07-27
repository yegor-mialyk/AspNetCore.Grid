using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.WebUtilities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnTests : IDisposable
    {
        private GridColumn<GridModel, Object> column;
        private static IGridFilters oldFilters;
        private IGrid<GridModel> grid;

        static GridColumnTests()
        {
            oldFilters = MvcGrid.Filters;
        }
        public GridColumnTests()
        {
            IDictionary<String, String[]> query = new Dictionary<String, String[]>();
            grid = Substitute.For<IGrid<GridModel>>();
            grid.Query = new ReadableStringCollection(query);
            grid.Name = "Grid";

            column = new GridColumn<GridModel, Object>(grid, model => model.Name);

            MvcGrid.Filters = Substitute.For<IGridFilters>();
        }
        public void Dispose()
        {
            MvcGrid.Filters = oldFilters;
        }

        #region Property: SortOrder

        [Fact]
        public void SortOrder_DoesNotChangeSortOrderAfterItsSet()
        {
            grid.Query = TestHelper.ParseQuery("Grid-Sort=Name&Grid-Order=Asc");

            column.SortOrder = null;

            Assert.Null(column.SortOrder);
        }

        [Theory]
        [InlineData("Grid-Sort=Name&Grid-Order=", "Name", GridSortOrder.Desc, null)]
        [InlineData("Grid-Order=Desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("Grid-Sort=Name&Grid-Order=Asc", "Name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData("Grid-Sort=Name&Grid-Order=Desc", "Name", GridSortOrder.Asc, GridSortOrder.Desc)]
        public void SortOrder_GetsSortOrderFromQuery(String query, String name, GridSortOrder? initialOrder, GridSortOrder? expected)
        {
            grid.Query = TestHelper.ParseQuery(query);
            column.InitialSortOrder = initialOrder;
            column.Name = name;

            GridSortOrder? actual = column.SortOrder;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Grid-Sort=Name&Grid-Order=", "Grid-Sort=Name&Grid-Order=Desc", null)]
        [InlineData("Grid-Sort=Name&Grid-Order=Asc", "Grid-Sort=Name&Grid-Order=Desc", GridSortOrder.Asc)]
        [InlineData("Grid-Sort=Name&Grid-Order=Desc", "Grid-Sort=Name&Grid-Order=Asc", GridSortOrder.Desc)]
        public void SortOrder_DoesNotChangeSortOrderAfterFirstGet(String initialQuery, String changedQuery, GridSortOrder? expected)
        {
            grid.Query = TestHelper.ParseQuery(initialQuery);
            GridSortOrder? sortOrder = column.SortOrder;

            grid.Query = TestHelper.ParseQuery(changedQuery);

            GridSortOrder? actual = column.SortOrder;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Grid-Order=Desc", "", GridSortOrder.Asc)]
        [InlineData("Gride-Sort=Name&Grid-Order=Asc", "Name", GridSortOrder.Asc)]
        [InlineData("RGrid-Sort=Name&Grid-Order=Desc", "Name", GridSortOrder.Desc)]
        public void SortOrder_OnNotSpecifiedSortOrderSetsInitialSortOrder(String query, String name, GridSortOrder? initialOrder)
        {
            grid.Query = TestHelper.ParseQuery(query);
            column.InitialSortOrder = initialOrder;
            column.Name = name;

            GridSortOrder? actual = column.SortOrder;
            GridSortOrder? expected = initialOrder;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Property: Filter

        [Fact]
        public void Filter_SetsFilterThenItsNotSet()
        {
            IGridColumnFilter<GridModel> filter = Substitute.For<IGridColumnFilter<GridModel>>();
            MvcGrid.Filters.GetFilter(column).Returns(filter);

            Object actual = column.Filter;
            Object expected = filter;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Filter_DoesNotChangeFilterAfterFirstGet()
        {
            IGridColumnFilter<GridModel> filter = Substitute.For<IGridColumnFilter<GridModel>>();
            MvcGrid.Filters.GetFilter(column).Returns(filter);

            IGridColumnFilter<GridModel> currentFilter = column.Filter;
            filter = Substitute.For<IGridColumnFilter<GridModel>>();
            MvcGrid.Filters.GetFilter(column).Returns(filter);

            Object expected = currentFilter;
            Object actual = column.Filter;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Filter_DoesNotChangeFilterAfterItsSet()
        {
            IGridColumnFilter<GridModel> filter = Substitute.For<IGridColumnFilter<GridModel>>();
            MvcGrid.Filters.GetFilter(column).Returns(filter);

            column.Filter = null;

            Assert.Null(column.Filter);
        }

        #endregion

        #region Constructor: GridColumn(IGrid<T> grid, Expression<Func<T, TValue>> expression)

        [Fact]
        public void GridColumn_SetsGrid()
        {
            IGrid actual = new GridColumn<GridModel, Object>(grid, model => model.Name).Grid;
            IGrid expected = grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_SetsIsEncodedToTrue()
        {
            column = new GridColumn<GridModel, Object>(grid, model => model.Name);

            Assert.True(column.IsEncoded);
        }

        [Fact]
        public void GridColumn_SetsExpression()
        {
            Expression<Func<GridModel, String>> expected = (model) => model.Name;
            Expression<Func<GridModel, String>> actual = new GridColumn<GridModel, String>(grid, expected).Expression;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumn_OnNonMemberExpressionSetsTitleToNull()
        {
            column = new GridColumn<GridModel, Object>(grid, model => model.ToString());

            Assert.Null(column.Title);
        }

        [Fact]
        public void GridColumn_OnPropertyWithoutDisplayAttributeSetsTitleToNull()
        {
            column = new GridColumn<GridModel, Object>(grid, model => model.Name);

            Assert.Null(column.Title);
        }

        [Fact]
        public void GridColumn_SetsTitleFromDisplayAttribute()
        {
            DisplayAttribute display = typeof(GridModel).GetProperty("Text").GetCustomAttribute<DisplayAttribute>();
            column = new GridColumn<GridModel, Object>(grid, model => model.Text);

            String expected = display.GetName();
            String actual = column.Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNullForEnum()
        {
            AssertFilterNameFor(model => model.EnumField, null);
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForSByte()
        {
            AssertFilterNameFor(model => model.SByteField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForByte()
        {
            AssertFilterNameFor(model => model.ByteField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForInt16()
        {
            AssertFilterNameFor(model => model.Int16Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForUInt16()
        {
            AssertFilterNameFor(model => model.UInt16Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForInt32()
        {
            AssertFilterNameFor(model => model.Int32Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForUInt32()
        {
            AssertFilterNameFor(model => model.UInt32Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForInt64()
        {
            AssertFilterNameFor(model => model.Int64Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForUInt64()
        {
            AssertFilterNameFor(model => model.UInt64Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForSingle()
        {
            AssertFilterNameFor(model => model.SingleField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForDouble()
        {
            AssertFilterNameFor(model => model.DoubleField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForDecimal()
        {
            AssertFilterNameFor(model => model.DecimalField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsBooleanForBoolean()
        {
            AssertFilterNameFor(model => model.BooleanField, "Boolean");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsDateCellForDateTime()
        {
            AssertFilterNameFor(model => model.DateTimeField, "Date");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNullForNullableEnum()
        {
            AssertFilterNameFor(model => model.NullableEnumField, null);
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableSByte()
        {
            AssertFilterNameFor(model => model.NullableSByteField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableByte()
        {
            AssertFilterNameFor(model => model.NullableByteField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableInt16()
        {
            AssertFilterNameFor(model => model.NullableInt16Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableUInt16()
        {
            AssertFilterNameFor(model => model.NullableUInt16Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableInt32()
        {
            AssertFilterNameFor(model => model.NullableInt32Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableUInt32()
        {
            AssertFilterNameFor(model => model.NullableUInt32Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableInt64()
        {
            AssertFilterNameFor(model => model.NullableInt64Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableUInt64()
        {
            AssertFilterNameFor(model => model.NullableUInt64Field, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableSingle()
        {
            AssertFilterNameFor(model => model.NullableSingleField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableDouble()
        {
            AssertFilterNameFor(model => model.NullableDoubleField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsNumberForNullableDecimal()
        {
            AssertFilterNameFor(model => model.NullableDecimalField, "Number");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsBooleanForNullableBoolean()
        {
            AssertFilterNameFor(model => model.NullableBooleanField, "Boolean");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsDateForNullableDateTime()
        {
            AssertFilterNameFor(model => model.NullableDateTimeField, "Date");
        }

        [Fact]
        public void AddProperty_SetsFilterNameAsTextForOtherTypes()
        {
            AssertFilterNameFor(model => model.StringField, "Text");
        }

        [Fact]
        public void GridColumn_SetsExpressionValueAsCompiledExpression()
        {
            GridModel gridModel = new GridModel { Name = "TestName" };

            String actual = column.ExpressionValue(gridModel) as String;
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
        public void GridColumn_OnNonMemberExpressionIsNotSortable()
        {
            Boolean? actual = new GridColumn<GridModel, String>(grid, model => model.ToString()).IsSortable;
            Boolean? expected = false;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_OnMemberExpressionIsSortableIsNull()
        {
            GridColumn<GridModel, Int32> column = new GridColumn<GridModel, Int32>(grid, model => model.Sum);

            Assert.Null(column.IsSortable);
        }

        [Fact]
        public void GridColumn_OnNonMemberExpressionIsNotFilterable()
        {
            Boolean? actual = new GridColumn<GridModel, String>(grid, model => model.ToString()).IsFilterable;
            Boolean? expected = false;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumn_OnMemberExpressionIsFilterableIsNull()
        {
            GridColumn<GridModel, Int32> column = new GridColumn<GridModel, Int32>(grid, model => model.Sum);

            Assert.Null(column.IsFilterable);
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

        #region Method: Process(IQueryable<T> items)

        [Fact]
        public void Process_IfFilterIsNullReturnsItems()
        {
            column.Filter = null;
            column.IsSortable = false;
            column.IsFilterable = true;
            column.SortOrder = GridSortOrder.Desc;

            IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
            IQueryable<GridModel> actual = column.Process(expected);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_IfNotFilterableReturnsItems()
        {
            column.IsSortable = false;
            column.IsFilterable = false;
            column.SortOrder = GridSortOrder.Desc;
            column.Filter = Substitute.For<IGridColumnFilter<GridModel>>();

            IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
            IQueryable<GridModel> actual = column.Process(expected);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_ReturnsFilteredItems()
        {
            column.IsSortable = false;
            column.IsFilterable = true;
            column.SortOrder = GridSortOrder.Desc;
            column.Filter = Substitute.For<IGridColumnFilter<GridModel>>();

            IQueryable<GridModel> filteredItems = new GridModel[2].AsQueryable();
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();
            column.Filter.Process(items).Returns(filteredItems);

            IQueryable<GridModel> actual = column.Process(items);
            IQueryable<GridModel> expected = filteredItems;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_IfSortOrderIsNullReturnsItems()
        {
            column.IsFilterable = false;
            column.IsSortable = true;
            column.SortOrder = null;

            IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
            IQueryable<GridModel> actual = column.Process(expected);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_IfNotSortableReturnsItems()
        {
            column.IsSortable = false;
            column.IsFilterable = false;
            column.SortOrder = GridSortOrder.Desc;

            IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
            IQueryable<GridModel> actual = column.Process(expected);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_ReturnsItemsSortedInAscendingOrder()
        {
            column.IsSortable = true;
            column.IsFilterable = false;
            column.SortOrder = GridSortOrder.Asc;
            GridModel[] items = { new GridModel { Name = "B" }, new GridModel { Name = "A" }};

            IEnumerable<GridModel> expected = items.OrderBy(model => model.Name);
            IEnumerable<GridModel> actual = column.Process(items.AsQueryable());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_ReturnsItemsSortedInDescendingOrder()
        {
            column.IsSortable = true;
            column.IsFilterable = false;
            column.SortOrder = GridSortOrder.Desc;
            GridModel[] items = { new GridModel { Name = "A" }, new GridModel { Name = "B" } };

            IEnumerable<GridModel> expected = items.OrderByDescending(model => model.Name);
            IEnumerable<GridModel> actual = column.Process(items.AsQueryable());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_ReturnsFilteredAndSortedItems()
        {
            column.IsSortable = true;
            column.IsFilterable = true;
            column.SortOrder = GridSortOrder.Desc;
            column.Filter = Substitute.For<IGridColumnFilter<GridModel>>();
            IQueryable<GridModel> items = new [] { new GridModel { Name = "A" }, new GridModel { Name = "B" }, new GridModel { Name = "C" } }.AsQueryable();
            column.Filter.Process(items).Returns(items.Where(model => model.Name != "A").ToList().AsQueryable());

            IEnumerable<GridModel> expected = items.Where(model => model.Name != "A").OrderByDescending(model => model.Name);
            IEnumerable<GridModel> actual = column.Process(items);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: ValueFor(IGridRow<Object> row)

        [Fact]
        public void ValueFor_UsesExpressionValueToGetValue()
        {
            column.ExpressionValue = (model) => "TestValue";

            String actual = column.ValueFor(new GridRow<Object>(null)).ToString();
            String expected = "TestValue";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValueFor_OnNotNullRenderValueUsesRenderValueToGetValue()
        {
            column.RenderValue = (model) => "RenderValue";
            column.ExpressionValue = (model) => "ExpressionValue";

            String actual = column.ValueFor(new GridRow<Object>(null)).ToString();
            String expected = "RenderValue";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValueFor_OnNullReferenceInExpressionValueReturnsEmpty()
        {
            column.ExpressionValue = (model) => (null as String).Length;

            String actual = column.ValueFor(new GridRow<Object>(null)).ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValueFor_OnNullReferenceInRenderValueReturnsEmpty()
        {
            column.RenderValue = (model) => (null as String).Length.ToString();
            column.ExpressionValue = (model) => "ExpressionValue";

            String actual = column.ValueFor(new GridRow<Object>(null)).ToString();
            String expected = "";

            Assert.Equal(expected, actual);
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
            column.RenderValue = (model) => Int32.Parse("Zero").ToString();

            Assert.Throws<FormatException>(() => column.ValueFor(new GridRow<Object>(null)));
        }

        [Theory]
        [InlineData(null, "For {0}", true, "")]
        [InlineData(null, "For {0}", false, "")]
        [InlineData("<name>", null, false, "<name>")]
        [InlineData("<name>", null, true, "&lt;name&gt;")]
        [InlineData("<name>", "For <{0}>", false, "For <<name>>")]
        [InlineData("<name>", "For <{0}>", true, "For &lt;&lt;name&gt;&gt;")]
        public void ValueFor_GetsRenderValue(String value, String format, Boolean isEncoded, String expected)
        {
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Name = value });
            column.RenderValue = (model) => model.Name;
            column.ExpressionValue = null;
            column.IsEncoded = isEncoded;
            column.Format = format;

            String actual = column.ValueFor(row).ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "For {0}", true, "")]
        [InlineData(null, "For {0}", false, "")]
        [InlineData("<name>", null, false, "<name>")]
        [InlineData("<name>", null, true, "&lt;name&gt;")]
        [InlineData("<name>", "For <{0}>", false, "For <<name>>")]
        [InlineData("<name>", "For <{0}>", true, "For &lt;&lt;name&gt;&gt;")]
        public void ValueFor_GetsExpressionValue(String value, String format, Boolean isEncoded, String expected)
        {
            IGridRow<GridModel> row = new GridRow<GridModel>(new GridModel { Name = value });
            column.IsEncoded = isEncoded;
            column.Format = format;

            String actual = column.ValueFor(row).ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Test helpers

        private void AssertFilterNameFor<TValue>(Expression<Func<AllTypesModel, TValue>> property, String expected)
        {
            IGrid<AllTypesModel> grid = Substitute.For<IGrid<AllTypesModel>>();
            grid.Query = TestHelper.ParseQuery("");

            String actual = new GridColumn<AllTypesModel, TValue>(grid, property).FilterName;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

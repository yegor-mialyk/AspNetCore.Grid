using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridFiltersTests
    {
        private GridFilters filters;
        private IGridColumn<GridModel, String> column;

        public GridFiltersTests()
        {
            column = new GridColumn<GridModel, String>(new Grid<GridModel>(new GridModel[0]), model => model.Name);
            column.Grid.Query = new QueryCollection();
            filters = new GridFilters();
        }

        #region GridFilters()

        [Theory]
        [InlineData(typeof(SByte), "Equals", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "NotEquals", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "LessThan", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "GreaterThan", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "LessThanOrEqual", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "GreaterThanOrEqual", typeof(SByteFilter))]

        [InlineData(typeof(Byte), "Equals", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "NotEquals", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "LessThan", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "GreaterThan", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "LessThanOrEqual", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "GreaterThanOrEqual", typeof(ByteFilter))]

        [InlineData(typeof(Int16), "Equals", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "NotEquals", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "LessThan", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "GreaterThan", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "LessThanOrEqual", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "GreaterThanOrEqual", typeof(Int16Filter))]

        [InlineData(typeof(UInt16), "Equals", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "NotEquals", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "LessThan", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "GreaterThan", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "LessThanOrEqual", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "GreaterThanOrEqual", typeof(UInt16Filter))]

        [InlineData(typeof(Int32), "Equals", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "NotEquals", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "LessThan", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "GreaterThan", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "LessThanOrEqual", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "GreaterThanOrEqual", typeof(Int32Filter))]

        [InlineData(typeof(UInt32), "Equals", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "NotEquals", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "LessThan", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "GreaterThan", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "LessThanOrEqual", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "GreaterThanOrEqual", typeof(UInt32Filter))]

        [InlineData(typeof(Int64), "Equals", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "NotEquals", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "LessThan", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "GreaterThan", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "LessThanOrEqual", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "GreaterThanOrEqual", typeof(Int64Filter))]

        [InlineData(typeof(UInt64), "Equals", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "NotEquals", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "LessThan", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "GreaterThan", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "LessThanOrEqual", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "GreaterThanOrEqual", typeof(UInt64Filter))]

        [InlineData(typeof(Single), "Equals", typeof(SingleFilter))]
        [InlineData(typeof(Single), "NotEquals", typeof(SingleFilter))]
        [InlineData(typeof(Single), "LessThan", typeof(SingleFilter))]
        [InlineData(typeof(Single), "GreaterThan", typeof(SingleFilter))]
        [InlineData(typeof(Single), "LessThanOrEqual", typeof(SingleFilter))]
        [InlineData(typeof(Single), "GreaterThanOrEqual", typeof(SingleFilter))]

        [InlineData(typeof(Double), "Equals", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "NotEquals", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "LessThan", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "GreaterThan", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "LessThanOrEqual", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "GreaterThanOrEqual", typeof(DoubleFilter))]

        [InlineData(typeof(Decimal), "Equals", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "NotEquals", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "LessThan", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "GreaterThan", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "LessThanOrEqual", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "GreaterThanOrEqual", typeof(DecimalFilter))]

        [InlineData(typeof(DateTime), "Equals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "NotEquals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "EarlierThan", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "LaterThan", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "EarlierThanOrEqual", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "LaterThanOrEqual", typeof(DateTimeFilter))]

        [InlineData(typeof(Boolean), "Equals", typeof(BooleanFilter))]

        [InlineData(typeof(String), "Equals", typeof(StringEqualsFilter))]
        [InlineData(typeof(String), "NotEquals", typeof(StringNotEqualsFilter))]
        [InlineData(typeof(String), "Contains", typeof(StringContainsFilter))]
        [InlineData(typeof(String), "EndsWith", typeof(StringEndsWithFilter))]
        [InlineData(typeof(String), "StartsWith", typeof(StringStartsWithFilter))]
        public void GridFilters_RegistersDefaultFilters(Type type, String name, Type filter)
        {
            Type actual = new GridFilters().Table[type][name];
            Type expected = filter;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetFilter<T, TValue>(IGridColumn<T, TValue> column)

        [Theory]
        [InlineData("", "Name-Equals")]
        [InlineData("", "Name=Equals")]
        [InlineData("", "Name-=Equals")]
        [InlineData("", "Name-Op=Equals")]
        [InlineData(null, "Name-Equals")]
        [InlineData(null, "Name=Equals")]
        [InlineData(null, "Name-=Equals")]
        [InlineData(null, "Name-Op=Equals")]
        [InlineData("Grid", "Grid-Name-Equals")]
        [InlineData("Grid", "Grid-Name=Equals")]
        [InlineData("Grid", "Grid-Name-=Equals")]
        [InlineData("Grid", "Grid-Name-Op=Equals")]
        public void GetFilter_NotFoundFilter_SetsSecondFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("", "Name-Contains=a&Name-Equals=b&Name-Op=And")]
        [InlineData(null, "Name-Contains=a&Name-Equals=b&Name-Op=And")]
        [InlineData("Grid", "Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=And")]
        public void GetFilter_NotFoundValueType_SetsSecondFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);
            GridColumn<GridModel, Object> testColumn = new GridColumn<GridModel, Object>(column.Grid, model => model.Name);

            Assert.Null(filters.GetFilter(testColumn).Second);
        }

        [Theory]
        [InlineData("", "Name-Eq=a&Name-Eq=b&Name-Op=And")]
        [InlineData(null, "Name-Eq=a&Name-Eq=b&Name-Op=And")]
        [InlineData("", "Name-Contains=a&Name-Eq=b&Name-Op=And")]
        [InlineData(null, "Name-Contains=a&Name-Eq=b&Name-Op=And")]
        [InlineData("Grid", "Grid-Name-Eq=a&Grid-Name-Eq=b&Grid-Name-Op=And")]
        [InlineData("Grid", "Grid-Name-Contains=a&Grid-Name-Eq=b&Grid-Name-Op=And")]
        public void GetFilter_NotFoundFilterType_SetsSecondFilterToNull(String name, String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);
            column.Grid.Name = name;

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("", "Name-Eq=a&Name-Equals=b", "b")]
        [InlineData("", "Name-Equals=a&Name-Equals=b", "b")]
        [InlineData("", "Name-Contains=a&Name-Equals=", "")]
        [InlineData("", "Name-Equals=a&Name-Equals=", "")]
        [InlineData("", "Name-Contains=a&Name-Equals=ba", "ba")]
        [InlineData("", "Name-Contains=a&Name-Equals=b&Name-Op=Or", "b")]
        [InlineData(null, "Name-Eq=a&Name-Equals=b", "b")]
        [InlineData(null, "Name-Equals=a&Name-Equals=b", "b")]
        [InlineData(null, "Name-Contains=a&Name-Equals=", "")]
        [InlineData(null, "Name-Equals=a&Name-Equals=", "")]
        [InlineData(null, "Name-Contains=a&Name-Equals=ba", "ba")]
        [InlineData(null, "Name-Contains=a&Name-Equals=b&Name-Op=Or", "b")]
        [InlineData("Grid", "Grid-Name-Eq=a&Grid-Name-Equals=b", "b")]
        [InlineData("Grid", "Grid-Name-Equals=a&Grid-Name-Equals=b", "b")]
        [InlineData("Grid", "Grid-Name-Contains=a&Grid-Name-Equals=", "")]
        [InlineData("Grid", "Grid-Name-Equals=a&Grid-Name-Equals=", "")]
        [InlineData("Grid", "Grid-Name-Contains=a&Grid-Name-Equals=ba", "ba")]
        [InlineData("Grid", "Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=Or", "b")]
        public void GetFilter_SetsSecondFilter(String name, String query, String value)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            IGridFilter actual = filters.GetFilter(column).Second;

            Assert.Equal(typeof(StringEqualsFilter), actual.GetType());
            Assert.Equal("Equals", actual.Type);
            Assert.Equal(value, actual.Value);
        }

        [Theory]
        [InlineData("", "Name=Equals")]
        [InlineData("", "Name-=Equals")]
        [InlineData("", "Name-Op=Equals")]
        [InlineData(null, "Name=Equals")]
        [InlineData(null, "Name-=Equals")]
        [InlineData(null, "Name-Op=Equals")]
        [InlineData("Grid", "Grid-Name=Equals")]
        [InlineData("Grid", "Grid-Name-=Equals")]
        [InlineData("Grid", "Grid-Name-Op=Equals")]
        public void GetFilter_NotFoundFilter_SetsFirstFilterToNull(String name, String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);
            column.Grid.Name = name;

            Assert.Null(filters.GetFilter(column).First);
        }

        [Fact]
        public void GetFilter_NotFoundValueType_SetsFirstFilterToNull()
        {
            column.Grid.Query = HttpUtility.ParseQueryString("Name-Contains=a&Name-Equals=b&Name-Op=And");
            GridColumn<GridModel, Object> testColumn = new GridColumn<GridModel, Object>(column.Grid, model => model.Name);

            Assert.Null(filters.GetFilter(testColumn).First);
        }

        [Theory]
        [InlineData("", "Name-Eq=a&Name-Eq=b&Name-Op=And")]
        [InlineData("", "Name-Eq=a&Name-Contains=b&Name-Op=And")]
        [InlineData(null, "Name-Eq=a&Name-Eq=b&Name-Op=And")]
        [InlineData(null, "Name-Eq=a&Name-Contains=b&Name-Op=And")]
        [InlineData("Grid", "Grid-Name-Eq=a&Grid-Name-Eq=b&Grid-Name-Op=And")]
        [InlineData("Grid", "Grid-Name-Eq=a&Grid-Name-Contains=b&Grid-Name-Op=And")]
        public void GetFilter_NotFoundFilterType_SetsFirstFilterToNull(String name, String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);
            column.Grid.Name = name;

            Assert.Null(filters.GetFilter(column).First);
        }

        [Theory]
        [InlineData("", "Name-Equals=a&Name-Eq=b", "a")]
        [InlineData("", "Name-Equals=&Name-Equals=b", "")]
        [InlineData("", "Name-Equals=&Name-Contains=b", "")]
        [InlineData("", "Name-Equals=a&Name-Contains=b", "a")]
        [InlineData("", "Name-Equals=a&Name-Equals=b", "a")]
        [InlineData("", "Name-Equals=a&Name-Contains=b&Name-Op=Or", "a")]
        [InlineData(null, "Name-Equals=a&Name-Eq=b", "a")]
        [InlineData(null, "Name-Equals=&Name-Equals=b", "")]
        [InlineData(null, "Name-Equals=&Name-Contains=b", "")]
        [InlineData(null, "Name-Equals=a&Name-Contains=b", "a")]
        [InlineData(null, "Name-Equals=a&Name-Equals=b", "a")]
        [InlineData(null, "Name-Equals=a&Name-Contains=b&Name-Op=Or", "a")]
        [InlineData("Grid", "Grid-Name-Equals=a&Grid-Name-Eq=b", "a")]
        [InlineData("Grid", "Grid-Name-Equals=&Grid-Name-Equals=b", "")]
        [InlineData("Grid", "Grid-Name-Equals=&Grid-Name-Contains=b", "")]
        [InlineData("Grid", "Grid-Name-Equals=a&Grid-Name-Contains=b", "a")]
        [InlineData("Grid", "Grid-Name-Equals=a&Grid-Name-Equals=b", "a")]
        [InlineData("Grid", "Grid-Name-Equals=a&Grid-Name-Contains=b&Grid-Name-Op=Or", "a")]
        public void GetFilter_SetsFirstFilter(String name, String query, String value)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            IGridFilter actual = filters.GetFilter(column).First;

            Assert.Equal(typeof(StringEqualsFilter), actual.GetType());
            Assert.Equal("Equals", actual.Type);
            Assert.Equal(value, actual.Value);
        }

        [Theory]
        [InlineData("", "Name-Op", "")]
        [InlineData("", "Name-Op=", "")]
        [InlineData("", "Name-Op=Or", "Or")]
        [InlineData("", "Name-Op=And", "And")]
        [InlineData("", "Name-Op-And=And", null)]
        [InlineData("", "Name-Op=And&Name-Op=Or", "And")]
        [InlineData(null, "Name-Op", "")]
        [InlineData(null, "Name-Op=", "")]
        [InlineData(null, "Name-Op=Or", "Or")]
        [InlineData(null, "Name-Op=And", "And")]
        [InlineData(null, "Name-Op-And=And", null)]
        [InlineData(null, "Name-Op=And&Name-Op=Or", "And")]
        [InlineData("Grid", "Grid-Name-Op", "")]
        [InlineData("Grid", "Grid-Name-Op=", "")]
        [InlineData("Grid", "Grid-Name-Op=Or", "Or")]
        [InlineData("Grid", "Grid-Name-Op=And", "And")]
        [InlineData("Grid", "Grid-Name-Op-And=And", null)]
        [InlineData("Grid", "Grid-Name-Op=And&Grid-Name-Op=Or", "And")]
        public void GetFilter_SetsOperatorFromQuery(String name, String query, String filterOperator)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            String actual = filters.GetFilter(column).Operator;
            String expected = filterOperator;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFilter_SetsColumn()
        {
            IGridColumn actual = filters.GetFilter(column).Column;
            IGridColumn expected = column;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GetFilter_SetsFilterNameForEnum()
        {
            AssertFilterNameFor(model => model.EnumField, null);
        }

        [Fact]
        public void GetFilter_SetsFilterNameForSByte()
        {
            AssertFilterNameFor(model => model.SByteField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForByte()
        {
            AssertFilterNameFor(model => model.ByteField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForInt16()
        {
            AssertFilterNameFor(model => model.Int16Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForUInt16()
        {
            AssertFilterNameFor(model => model.UInt16Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForInt32()
        {
            AssertFilterNameFor(model => model.Int32Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForUInt32()
        {
            AssertFilterNameFor(model => model.UInt32Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForInt64()
        {
            AssertFilterNameFor(model => model.Int64Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForUInt64()
        {
            AssertFilterNameFor(model => model.UInt64Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForSingle()
        {
            AssertFilterNameFor(model => model.SingleField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForDouble()
        {
            AssertFilterNameFor(model => model.DoubleField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForDecimal()
        {
            AssertFilterNameFor(model => model.DecimalField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForString()
        {
            AssertFilterNameFor(model => model.StringField, "Text");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForBoolean()
        {
            AssertFilterNameFor(model => model.BooleanField, "Boolean");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForDateTime()
        {
            AssertFilterNameFor(model => model.DateTimeField, "Date");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableEnum()
        {
            AssertFilterNameFor(model => model.NullableEnumField, null);
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableSByte()
        {
            AssertFilterNameFor(model => model.NullableSByteField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableByte()
        {
            AssertFilterNameFor(model => model.NullableByteField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableInt16()
        {
            AssertFilterNameFor(model => model.NullableInt16Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableUInt16()
        {
            AssertFilterNameFor(model => model.NullableUInt16Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableInt32()
        {
            AssertFilterNameFor(model => model.NullableInt32Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableUInt32()
        {
            AssertFilterNameFor(model => model.NullableUInt32Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableInt64()
        {
            AssertFilterNameFor(model => model.NullableInt64Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableUInt64()
        {
            AssertFilterNameFor(model => model.NullableUInt64Field, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableSingle()
        {
            AssertFilterNameFor(model => model.NullableSingleField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableDouble()
        {
            AssertFilterNameFor(model => model.NullableDoubleField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableDecimal()
        {
            AssertFilterNameFor(model => model.NullableDecimalField, "Number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableBoolean()
        {
            AssertFilterNameFor(model => model.NullableBooleanField, "Boolean");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableDateTime()
        {
            AssertFilterNameFor(model => model.NullableDateTimeField, "Date");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForOtherTypes()
        {
            AssertFilterNameFor(model => model, null);
        }

        #endregion

        #region Register(Type forType, String filterType, Type filter)

        [Fact]
        public void Register_FilterForExistingType()
        {
            Dictionary<String, Type> filter = new Dictionary<String, Type> { { "Test", typeof(Object) } };
            filters.Table[typeof(Object)] = filter;

            filters.Register(typeof(Object), "TestFilter", typeof(String));

            Type actual = filter["TestFilter"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_NullableFilterTypeForExistingType()
        {
            Dictionary<String, Type> filter = new Dictionary<String, Type> { { "Test", typeof(Object) } };
            filters.Table[typeof(Int32)] = filter;

            filters.Register(typeof(Int32?), "TestFilter", typeof(String));

            Type actual = filter["TestFilter"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_Overrides_NullableFilter()
        {
            Dictionary<String, Type> filter = new Dictionary<String, Type> { { "Test", typeof(Object) } };
            filters.Table[typeof(Int32)] = filter;

            filters.Register(typeof(Int32?), "Test", typeof(String));

            Type actual = filter["Test"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_Overrides_Filter()
        {
            Dictionary<String, Type> filter = new Dictionary<String, Type> { { "Test", typeof(Object) } };
            filters.Table[typeof(Int32)] = filter;

            filters.Register(typeof(Int32), "Test", typeof(String));

            Type actual = filter["Test"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_NullableTypeAsNotNullable()
        {
            filters.Register(typeof(Int32?), "Test", typeof(String));

            Type actual = filters.Table[typeof(Int32)]["Test"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_FilterForNewType()
        {
            filters.Register(typeof(Object), "Test", typeof(String));

            Type actual = filters.Table[typeof(Object)]["Test"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Unregister(Type forType, String filterType)

        [Fact]
        public void Unregister_ExistingFilter()
        {
            Dictionary<String, Type> filter = new Dictionary<String, Type> { { "Test", null } };
            filters.Table[typeof(Object)] = filter;

            filters.Unregister(typeof(Object), "Test");

            Assert.Empty(filter);
        }

        [Fact]
        public void Unregister_CanBeCalledOnNotExistingFilter()
        {
            filters.Unregister(typeof(Object), "Test");
        }

        #endregion

        #region Test helpers

        private void AssertFilterNameFor<TValue>(Expression<Func<AllTypesModel, TValue>> property, String name)
        {
            Grid<AllTypesModel> grid = new Grid<AllTypesModel>(new AllTypesModel[0]) { Query = new QueryCollection() };

            String actual = filters.GetFilter(new GridColumn<AllTypesModel, TValue>(grid, property)).Name;
            String expected = name;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

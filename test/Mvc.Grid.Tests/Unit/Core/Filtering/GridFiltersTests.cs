using System;
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
            filters = new GridFilters();
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);
            column = new GridColumn<GridModel, String>(grid, model => model.Name);
        }

        #region GridFilters()

        [Theory]
        [InlineData(typeof(SByte), "equals", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "not-equals", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "less-than", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "greater-than", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "less-than-or-equal", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "greater-than-or-equal", typeof(SByteFilter))]

        [InlineData(typeof(Byte), "equals", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "not-equals", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "less-than", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "greater-than", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "less-than-or-equal", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "greater-than-or-equal", typeof(ByteFilter))]

        [InlineData(typeof(Int16), "equals", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "not-equals", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "less-than", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "greater-than", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "less-than-or-equal", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "greater-than-or-equal", typeof(Int16Filter))]

        [InlineData(typeof(UInt16), "equals", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "not-equals", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "less-than", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "greater-than", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "less-than-or-equal", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "greater-than-or-equal", typeof(UInt16Filter))]

        [InlineData(typeof(Int32), "equals", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "not-equals", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "less-than", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "greater-than", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "less-than-or-equal", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "greater-than-or-equal", typeof(Int32Filter))]

        [InlineData(typeof(UInt32), "equals", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "not-equals", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "less-than", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "greater-than", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "less-than-or-equal", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "greater-than-or-equal", typeof(UInt32Filter))]

        [InlineData(typeof(Int64), "equals", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "not-equals", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "less-than", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "greater-than", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "less-than-or-equal", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "greater-than-or-equal", typeof(Int64Filter))]

        [InlineData(typeof(UInt64), "equals", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "not-equals", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "less-than", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "greater-than", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "less-than-or-equal", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "greater-than-or-equal", typeof(UInt64Filter))]

        [InlineData(typeof(Single), "equals", typeof(SingleFilter))]
        [InlineData(typeof(Single), "not-equals", typeof(SingleFilter))]
        [InlineData(typeof(Single), "less-than", typeof(SingleFilter))]
        [InlineData(typeof(Single), "greater-than", typeof(SingleFilter))]
        [InlineData(typeof(Single), "less-than-or-equal", typeof(SingleFilter))]
        [InlineData(typeof(Single), "greater-than-or-equal", typeof(SingleFilter))]

        [InlineData(typeof(Double), "equals", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "not-equals", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "less-than", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "greater-than", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "less-than-or-equal", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "greater-than-or-equal", typeof(DoubleFilter))]

        [InlineData(typeof(Decimal), "equals", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "not-equals", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "less-than", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "greater-than", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "less-than-or-equal", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "greater-than-or-equal", typeof(DecimalFilter))]

        [InlineData(typeof(DateTime), "equals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "not-equals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "earlier-than", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "later-than", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "earlier-than-or-equal", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "later-than-or-equal", typeof(DateTimeFilter))]

        [InlineData(typeof(Boolean), "equals", typeof(BooleanFilter))]

        [InlineData(typeof(String), "equals", typeof(StringEqualsFilter))]
        [InlineData(typeof(String), "not-equals", typeof(StringNotEqualsFilter))]
        [InlineData(typeof(String), "contains", typeof(StringContainsFilter))]
        [InlineData(typeof(String), "ends-with", typeof(StringEndsWithFilter))]
        [InlineData(typeof(String), "starts-with", typeof(StringStartsWithFilter))]
        public void GridFilters_RegistersDefaultFilters(Type type, String name, Type filter)
        {
            Type actual = new GridFilters().Table[type][name];
            Type expected = filter;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetFilter<T, TValue>(IGridColumn<T, TValue> column)

        [Theory]
        [InlineData("", "name-equals=a&name=equals")]
        [InlineData("", "name-equals=a&name-=equals")]
        [InlineData("", "name-equals=a&name-op=equals")]
        [InlineData(null, "name-equals=a&name=equals")]
        [InlineData(null, "name-equals=a&name-=equals")]
        [InlineData(null, "name-equals=a&name-op=equals")]
        [InlineData("grid", "grid-name-equals=a&grid-name=equals")]
        [InlineData("grid", "grid-name-equals=a&grid-name-=equals")]
        [InlineData("grid", "grid-name-equals=a&grid-name-op=equals")]
        public void GetFilter_NotFoundFilter_SetsSecondFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("", "name-contains=a&name-equals=b&name-op=and")]
        [InlineData(null, "name-contains=a&name-equals=b&name-op=and")]
        [InlineData("grid", "grid-name-contains=a&grid-name-equals=b&grid-name-op=and")]
        public void GetFilter_NotFoundValueType_SetsSecondFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);
            GridColumn<GridModel, Object> testColumn = new GridColumn<GridModel, Object>(column.Grid, model => model.Name);

            Assert.Null(filters.GetFilter(testColumn).Second);
        }

        [Theory]
        [InlineData("", "name-equals=a&name-eq=b&name-op=and")]
        [InlineData(null, "name-equals=a&name-eq=b&name-op=and")]
        [InlineData("grid", "grid-name-equals=a&grid-name-eq=b&grid-name-op=and")]
        public void GetFilter_NotFoundFilterType_SetsSecondFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("", "name-eq=a&name-equals=b", "b")]
        [InlineData("", "name-equals=a&name-equals=b", "b")]
        [InlineData("", "name-contains=a&name-equals=", "")]
        [InlineData("", "name-equals=a&name-equals=", "")]
        [InlineData("", "name-contains=a&name-equals=ba", "ba")]
        [InlineData("", "name-contains=a&name-equals=b&name-op=or", "b")]
        [InlineData("", "NAME-CONTAINS=A&NAME-EQUALS=B&NAME-OP=OR", "B")]
        [InlineData(null, "name-eq=a&name-equals=b", "b")]
        [InlineData(null, "name-equals=a&name-equals=b", "b")]
        [InlineData(null, "name-contains=a&name-equals=", "")]
        [InlineData(null, "name-equals=a&name-equals=", "")]
        [InlineData(null, "name-contains=a&name-equals=ba", "ba")]
        [InlineData(null, "name-contains=a&name-equals=b&name-op=or", "b")]
        [InlineData(null, "NAME-CONTAINS=A&NAME-EQUALS=B&NAME-OP=OR", "B")]
        [InlineData("grid", "grid-name-eq=a&grid-name-equals=b", "b")]
        [InlineData("grid", "grid-name-equals=a&grid-name-equals=b", "b")]
        [InlineData("grid", "grid-name-contains=a&grid-name-equals=", "")]
        [InlineData("grid", "grid-name-equals=a&grid-name-equals=", "")]
        [InlineData("grid", "grid-name-contains=a&grid-name-equals=ba", "ba")]
        [InlineData("grid", "grid-name-contains=a&grid-name-equals=b&grid-name-op=or", "b")]
        [InlineData("grid", "GRID-NAME-CONTAINS=A&GRID-NAME-EQUALS=B&GRID-NAME-OP=OR", "B")]
        public void GetFilter_SetsSecondFilter(String name, String query, String value)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            IGridFilter actual = filters.GetFilter(column).Second;

            Assert.Equal(typeof(StringEqualsFilter), actual.GetType());
            Assert.Equal("equals", actual.Type);
            Assert.Equal(value, actual.Value);
        }

        [Theory]
        [InlineData("", "name=equals")]
        [InlineData("", "name-=equals")]
        [InlineData("", "name-op=equals")]
        [InlineData(null, "name=equals")]
        [InlineData(null, "name-=equals")]
        [InlineData(null, "name-op=equals")]
        [InlineData("grid", "grid-name=equals")]
        [InlineData("grid", "grid-name-=equals")]
        [InlineData("grid", "grid-name-op=equals")]
        public void GetFilter_NotFoundFilter_SetsFirstFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).First);
        }

        [Theory]
        [InlineData("", "name-contains=a&name-equals=b&name-op=and")]
        [InlineData(null, "name-contains=a&name-equals=b&name-op=and")]
        [InlineData("grid", "name-contains=a&name-equals=b&name-op=and")]
        public void GetFilter_NotFoundValueType_SetsFirstFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);
            GridColumn<GridModel, Object> testColumn = new GridColumn<GridModel, Object>(column.Grid, model => model.Name);

            Assert.Null(filters.GetFilter(testColumn).First);
        }

        [Theory]
        [InlineData("", "name-eq=a&name-eq=b&name-op=and")]
        [InlineData("", "name-eq=a&name-contains=b&name-op=and")]
        [InlineData(null, "name-eq=a&name-eq=b&name-op=and")]
        [InlineData(null, "name-eq=a&name-contains=b&name-op=and")]
        [InlineData("grid", "grid-name-eq=a&grid-name-eq=b&grid-name-op=and")]
        [InlineData("grid", "grid-name-eq=a&grid-name-contains=b&grid-name-op=and")]
        public void GetFilter_NotFoundFilterType_SetsFirstFilterToNull(String name, String query)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).First);
        }

        [Theory]
        [InlineData("", "name-equals=a&name-eq=b", "a")]
        [InlineData("", "name-equals=&name-equals=b", "")]
        [InlineData("", "name-equals=&name-contains=b", "")]
        [InlineData("", "name-equals=a&name-contains=b", "a")]
        [InlineData("", "name-equals=a&name-equals=b", "a")]
        [InlineData("", "name-equals=a&name-contains=b&name-op=or", "a")]
        [InlineData("", "NAME-EQUALS=A&NAME-CONTAINS=B&NAME-OP=OR", "A")]
        [InlineData(null, "name-equals=a&name-eq=b", "a")]
        [InlineData(null, "name-equals=&name-equals=b", "")]
        [InlineData(null, "name-equals=&name-contains=b", "")]
        [InlineData(null, "name-equals=a&name-contains=b", "a")]
        [InlineData(null, "name-equals=a&name-equals=b", "a")]
        [InlineData(null, "name-equals=a&name-contains=b&name-op=or", "a")]
        [InlineData(null, "NAME-EQUALS=A&NAME-CONTAINS=B&NAME-OP=OR", "A")]
        [InlineData("grid", "grid-name-equals=a&grid-name-eq=b", "a")]
        [InlineData("grid", "grid-name-equals=&grid-name-equals=b", "")]
        [InlineData("grid", "grid-name-equals=&grid-name-contains=b", "")]
        [InlineData("grid", "grid-name-equals=a&grid-name-contains=b", "a")]
        [InlineData("grid", "grid-name-equals=a&grid-name-equals=b", "a")]
        [InlineData("grid", "grid-name-equals=a&grid-name-contains=b&grid-name-op=or", "a")]
        [InlineData("grid", "GRID-NAME-EQUALS=A&GRID-NAME-CONTAINS=B&GRID-NAME-OP=OR", "A")]
        public void GetFilter_SetsFirstFilter(String name, String query, String value)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            IGridFilter actual = filters.GetFilter(column).First;

            Assert.Equal(typeof(StringEqualsFilter), actual.GetType());
            Assert.Equal("equals", actual.Type);
            Assert.Equal(value, actual.Value);
        }

        [Theory]
        [InlineData("", "name-op", "")]
        [InlineData("", "name-op=", "")]
        [InlineData("", "name-op=or", "or")]
        [InlineData("", "name-op=and", "and")]
        [InlineData("", "name-op-and=and", null)]
        [InlineData("", "name-op=and&name-op=or", "and")]
        [InlineData("", "NAME-OP=AND&NAME-OP=OR", "and")]
        [InlineData(null, "name-op", "")]
        [InlineData(null, "name-op=", "")]
        [InlineData(null, "name-op=or", "or")]
        [InlineData(null, "name-op=and", "and")]
        [InlineData(null, "name-op-and=and", null)]
        [InlineData(null, "name-op=and&name-op=or", "and")]
        [InlineData(null, "NAME-OP=AND&NAME-OP=OR", "and")]
        [InlineData("grid", "grid-name-op", "")]
        [InlineData("grid", "grid-name-op=", "")]
        [InlineData("grid", "grid-name-op=or", "or")]
        [InlineData("grid", "grid-name-op=and", "and")]
        [InlineData("grid", "grid-name-op-and=and", null)]
        [InlineData("grid", "grid-name-op=and&grid-name-op=or", "and")]
        [InlineData("grid", "GRID-NAME-OP=AND&GRID-NAME-OP=OR", "and")]
        public void GetFilter_SetsOperatorFromQuery(String name, String query, String op)
        {
            column.Grid.Name = name;
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            String actual = filters.GetFilter(column).Operator;
            String expected = op;

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
            AssertFilterNameFor(model => model.SByteField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForByte()
        {
            AssertFilterNameFor(model => model.ByteField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForInt16()
        {
            AssertFilterNameFor(model => model.Int16Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForUInt16()
        {
            AssertFilterNameFor(model => model.UInt16Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForInt32()
        {
            AssertFilterNameFor(model => model.Int32Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForUInt32()
        {
            AssertFilterNameFor(model => model.UInt32Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForInt64()
        {
            AssertFilterNameFor(model => model.Int64Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForUInt64()
        {
            AssertFilterNameFor(model => model.UInt64Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForSingle()
        {
            AssertFilterNameFor(model => model.SingleField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForDouble()
        {
            AssertFilterNameFor(model => model.DoubleField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForDecimal()
        {
            AssertFilterNameFor(model => model.DecimalField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForString()
        {
            AssertFilterNameFor(model => model.StringField, "text");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForBoolean()
        {
            AssertFilterNameFor(model => model.BooleanField, "boolean");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForDateTime()
        {
            AssertFilterNameFor(model => model.DateTimeField, "date");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableEnum()
        {
            AssertFilterNameFor(model => model.NullableEnumField, null);
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableSByte()
        {
            AssertFilterNameFor(model => model.NullableSByteField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableByte()
        {
            AssertFilterNameFor(model => model.NullableByteField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableInt16()
        {
            AssertFilterNameFor(model => model.NullableInt16Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableUInt16()
        {
            AssertFilterNameFor(model => model.NullableUInt16Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableInt32()
        {
            AssertFilterNameFor(model => model.NullableInt32Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableUInt32()
        {
            AssertFilterNameFor(model => model.NullableUInt32Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableInt64()
        {
            AssertFilterNameFor(model => model.NullableInt64Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableUInt64()
        {
            AssertFilterNameFor(model => model.NullableUInt64Field, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableSingle()
        {
            AssertFilterNameFor(model => model.NullableSingleField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableDouble()
        {
            AssertFilterNameFor(model => model.NullableDoubleField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableDecimal()
        {
            AssertFilterNameFor(model => model.NullableDecimalField, "number");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableBoolean()
        {
            AssertFilterNameFor(model => model.NullableBooleanField, "boolean");
        }

        [Fact]
        public void GetFilter_SetsFilterNameForNullableDateTime()
        {
            AssertFilterNameFor(model => model.NullableDateTimeField, "date");
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
            filters.Register(typeof(Int32), "test", typeof(Object));
            filters.Register(typeof(Int32), "test-filter", typeof(String));

            Type actual = filters.Table[typeof(Int32)]["TEST-FILTER"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_NullableFilterTypeForExistingType()
        {
            filters.Register(typeof(Int32), "test", typeof(Object));
            filters.Register(typeof(Int32?), "test-filter", typeof(String));

            Type actual = filters.Table[typeof(Int32)]["TEST-FILTER"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_Overrides_NullableFilter()
        {
            filters.Register(typeof(Int32), "test-filter", typeof(Object));
            filters.Register(typeof(Int32?), "TEST-filter", typeof(String));

            Type actual = filters.Table[typeof(Int32)]["TEST-FILTER"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_Overrides_Filter()
        {
            filters.Register(typeof(Int32), "test-filter", typeof(Object));
            filters.Register(typeof(Int32), "TEST-filter", typeof(String));

            Type actual = filters.Table[typeof(Int32)]["TEST-FILTER"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_NullableTypeAsNotNullable()
        {
            filters.Register(typeof(Int32?), "test", typeof(String));

            Type actual = filters.Table[typeof(Int32)]["TEST"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_FilterForNewType()
        {
            filters.Register(typeof(Object), "test", typeof(String));

            Type actual = filters.Table[typeof(Object)]["TEST"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Unregister(Type forType, String filterType)

        [Fact]
        public void Unregister_ExistingFilter()
        {
            filters.Register(typeof(Object), "test", typeof(String));

            filters.Unregister(typeof(Object), "TEST");

            Assert.Empty(filters.Table[typeof(Object)]);
        }

        [Fact]
        public void Unregister_CanBeCalledOnNotExistingFilter()
        {
            filters.Unregister(typeof(Object), "test");
        }

        #endregion

        #region Test helpers

        private void AssertFilterNameFor<TValue>(Expression<Func<AllTypesModel, TValue>> property, String name)
        {
            Grid<AllTypesModel> grid = new Grid<AllTypesModel>(new AllTypesModel[0]);

            String actual = filters.GetFilter(new GridColumn<AllTypesModel, TValue>(grid, property)).Name;
            String expected = name;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

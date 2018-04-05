using System;
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
            Assert.IsType(filter, new GridFilters().GetFilter(type, name));
        }

        #endregion

        #region IGridFilter GetFilter(Type forType, String filterType)

        [Fact]
        public void GetFilter_NotFoundForType_ReturnsNull()
        {
            Assert.Null(filters.GetFilter(typeof(Object), "equals"));
        }

        [Fact]
        public void GetFilter_NotFoundFilterType_ReturnsNull()
        {
            Assert.Null(filters.GetFilter(typeof(String), "less-than"));
        }

        [Fact]
        public void GetFilter_ForType()
        {
            IGridFilter actual = filters.GetFilter(typeof(String), "CONTAINS");

            Assert.IsType<StringContainsFilter>(actual);
            Assert.Equal("contains", actual.Type);
        }

        #endregion

        #region Register(Type forType, String filterType, Type filter)

        [Fact]
        public void Register_FilterForExistingType()
        {
            filters.Register(typeof(Int32), "TEST", typeof(Object));
            filters.Register(typeof(Int32), "TEST-FILTER", typeof(StringEqualsFilter));

            Assert.IsType<StringEqualsFilter>(filters.GetFilter(typeof(Int32), "test-filter"));
        }

        [Fact]
        public void Register_NullableFilterTypeForExistingType()
        {
            filters.Register(typeof(Int32), "TEST", typeof(Object));
            filters.Register(typeof(Int32?), "TEST-FILTER", typeof(StringEqualsFilter));

            Assert.IsType<StringEqualsFilter>(filters.GetFilter(typeof(Int32), "test-filter"));
        }

        [Fact]
        public void Register_Overrides_NullableFilter()
        {
            filters.Register(typeof(Int32), "test-filter", typeof(Object));
            filters.Register(typeof(Int32?), "TEST-filter", typeof(Int32Filter));

            Assert.IsType<Int32Filter>(filters.GetFilter(typeof(Int32), "test-filter"));
        }

        [Fact]
        public void Register_Overrides_Filter()
        {
            filters.Register(typeof(Int32), "test-filter", typeof(Object));
            filters.Register(typeof(Int32), "TEST-filter", typeof(Int32Filter));

            Assert.IsType<Int32Filter>(filters.GetFilter(typeof(Int32), "test-filter"));
        }

        [Fact]
        public void Register_NullableTypeAsNotNullable()
        {
            filters.Register(typeof(Int32?), "TEST", typeof(Int32Filter));

            Assert.IsType<Int32Filter>(filters.GetFilter(typeof(Int32), "test"));
        }

        [Fact]
        public void Register_FilterForNewType()
        {
            filters.Register(typeof(Object), "test", typeof(Int32Filter));

            Assert.IsType<Int32Filter>(filters.GetFilter(typeof(Object), "test"));
        }

        #endregion

        #region Unregister(Type forType, String filterType)

        [Fact]
        public void Unregister_ExistingFilter()
        {
            filters.Register(typeof(Object), "test", typeof(StringEqualsFilter));

            filters.Unregister(typeof(Object), "TEST");

            Assert.Null(filters.GetFilter(typeof(Object), "test"));
        }

        [Fact]
        public void Unregister_CanBeCalledOnNotExistingFilter()
        {
            filters.Register(typeof(Object), "test", typeof(StringEqualsFilter));

            filters.Unregister(typeof(Object), "test");
            filters.Unregister(typeof(Object), "test");

            Assert.Null(filters.GetFilter(typeof(Object), "test"));
        }

        #endregion
    }
}

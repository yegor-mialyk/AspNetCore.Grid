using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridFiltersTests
    {
        private IGridColumn<GridModel> column;
        private GridFilters filters;

        public GridFiltersTests()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            column = Substitute.For<IGridColumn<GridModel>>();
            column.Grid.Query = new QueryCollection();
            column.Expression.Returns(expression);
            column.IsMultiFilterable = true;
            column.Grid.Name = "Grid";
            column.Name = "Name";

            filters = new GridFilters();
        }

        #region GridFilters()

        [Theory]
        [InlineData(typeof(SByte), "Equals", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "LessThan", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "GreaterThan", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "LessThanOrEqual", typeof(SByteFilter))]
        [InlineData(typeof(SByte), "GreaterThanOrEqual", typeof(SByteFilter))]

        [InlineData(typeof(Byte), "Equals", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "LessThan", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "GreaterThan", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "LessThanOrEqual", typeof(ByteFilter))]
        [InlineData(typeof(Byte), "GreaterThanOrEqual", typeof(ByteFilter))]

        [InlineData(typeof(Int16), "Equals", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "LessThan", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "GreaterThan", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "LessThanOrEqual", typeof(Int16Filter))]
        [InlineData(typeof(Int16), "GreaterThanOrEqual", typeof(Int16Filter))]

        [InlineData(typeof(UInt16), "Equals", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "LessThan", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "GreaterThan", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "LessThanOrEqual", typeof(UInt16Filter))]
        [InlineData(typeof(UInt16), "GreaterThanOrEqual", typeof(UInt16Filter))]

        [InlineData(typeof(Int32), "Equals", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "LessThan", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "GreaterThan", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "LessThanOrEqual", typeof(Int32Filter))]
        [InlineData(typeof(Int32), "GreaterThanOrEqual", typeof(Int32Filter))]

        [InlineData(typeof(UInt32), "Equals", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "LessThan", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "GreaterThan", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "LessThanOrEqual", typeof(UInt32Filter))]
        [InlineData(typeof(UInt32), "GreaterThanOrEqual", typeof(UInt32Filter))]

        [InlineData(typeof(Int64), "Equals", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "LessThan", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "GreaterThan", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "LessThanOrEqual", typeof(Int64Filter))]
        [InlineData(typeof(Int64), "GreaterThanOrEqual", typeof(Int64Filter))]

        [InlineData(typeof(UInt64), "Equals", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "LessThan", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "GreaterThan", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "LessThanOrEqual", typeof(UInt64Filter))]
        [InlineData(typeof(UInt64), "GreaterThanOrEqual", typeof(UInt64Filter))]

        [InlineData(typeof(Single), "Equals", typeof(SingleFilter))]
        [InlineData(typeof(Single), "LessThan", typeof(SingleFilter))]
        [InlineData(typeof(Single), "GreaterThan", typeof(SingleFilter))]
        [InlineData(typeof(Single), "LessThanOrEqual", typeof(SingleFilter))]
        [InlineData(typeof(Single), "GreaterThanOrEqual", typeof(SingleFilter))]

        [InlineData(typeof(Double), "Equals", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "LessThan", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "GreaterThan", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "LessThanOrEqual", typeof(DoubleFilter))]
        [InlineData(typeof(Double), "GreaterThanOrEqual", typeof(DoubleFilter))]

        [InlineData(typeof(Decimal), "Equals", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "LessThan", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "GreaterThan", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "LessThanOrEqual", typeof(DecimalFilter))]
        [InlineData(typeof(Decimal), "GreaterThanOrEqual", typeof(DecimalFilter))]

        [InlineData(typeof(DateTime), "Equals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "LessThan", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "GreaterThan", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "LessThanOrEqual", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "GreaterThanOrEqual", typeof(DateTimeFilter))]

        [InlineData(typeof(Boolean), "Equals", typeof(BooleanFilter))]

        [InlineData(typeof(String), "Equals", typeof(StringEqualsFilter))]
        [InlineData(typeof(String), "Contains", typeof(StringContainsFilter))]
        [InlineData(typeof(String), "EndsWith", typeof(StringEndsWithFilter))]
        [InlineData(typeof(String), "StartsWith", typeof(StringStartsWithFilter))]
        public void GridFilters_RegistersDefaultFilters(Type type, String name, Type expected)
        {
            GridFilters filters = new GridFilters();

            Type actual = filters.Table[type][name];

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetFilter<T>(IGridColumn<T> column)

        [Fact]
        public void GetFilter_NotMultiFilterable_SetsSecondFilterToNull()
        {
            column.Grid.Query = HttpUtility.ParseQueryString("Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=Or");
            column.IsMultiFilterable = false;

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("Grid-Name-Equals")]
        [InlineData("Grid-Name=Equals")]
        [InlineData("Grid-Name-=Equals")]
        [InlineData("Grid-Name-Op=Equals")]
        public void GetFilter_NotFoundFilter_SetsSecondFilterToNull(String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Fact]
        public void GetFilter_NotFoundValueType_SetsSecondFilterToNull()
        {
            column.Grid.Query = HttpUtility.ParseQueryString("Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=And");
            Expression<Func<GridModel, Object>> expression = (model) => model.Name;
            column.Expression.Returns(expression);

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("Grid-Name-Eq=a&Grid-Name-Eq=b&Grid-Name-Op=And")]
        [InlineData("Grid-Name-Contains=a&Grid-Name-Eq=b&Grid-Name-Op=And")]
        public void GetFilter_NotFoundFilterType_SetsSecondFilterToNull(String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).Second);
        }

        [Theory]
        [InlineData("Grid-Name-Eq=a&Grid-Name-Equals=b", "Equals", "b")]
        [InlineData("Grid-Name-Equals=a&Grid-Name-Equals=", "Equals", "")]
        [InlineData("Grid-Name-Equals=a&Grid-Name-Equals=b", "Equals", "b")]
        [InlineData("Grid-Name-Contains=a&Grid-Name-Equals=", "Equals", "")]
        [InlineData("Grid-Name-Contains=a&Grid-Name-Equals=b", "Equals", "b")]
        [InlineData("Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=Or", "Equals", "b")]
        public void GetFilter_SetsSecondFilter(String query, String type, String value)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            IGridFilter actual = filters.GetFilter(column).Second;

            Assert.Equal(typeof(StringEqualsFilter), actual.GetType());
            Assert.Equal(value, actual.Value);
            Assert.Equal(type, actual.Type);
        }

        [Theory]
        [InlineData("Grid-Name-Equals")]
        [InlineData("Grid-Name=Equals")]
        [InlineData("Grid-Name-=Equals")]
        [InlineData("Grid-Name-Op=Equals")]
        public void GetFilter_NotFoundFilter_SetsFirstFilterToNull(String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).First);
        }

        [Fact]
        public void GetFilter_NotFoundValueType_SetsFirstFilterToNull()
        {
            column.Grid.Query = HttpUtility.ParseQueryString("Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=And");
            Expression<Func<GridModel, Object>> expression = (model) => model.Name;
            column.Expression.Returns(expression);

            Assert.Null(filters.GetFilter(column).First);
        }

        [Theory]
        [InlineData("Grid-Name-Eq=a&Grid-Name-Eq=b&Grid-Name-Op=And")]
        [InlineData("Grid-Name-Eq=a&Grid-Name-Contains=b&Grid-Name-Op=And")]
        public void GetFilter_NotFoundFilterType_SetsFirstFilterToNull(String query)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(filters.GetFilter(column).First);
        }

        [Theory]
        [InlineData("Grid-Name-Equals=a&Grid-Name-Eq=b", "Equals", "a")]
        [InlineData("Grid-Name-Equals=&Grid-Name-Equals=b", "Equals", "")]
        [InlineData("Grid-Name-Equals=a&Grid-Name-Equals=b", "Equals", "a")]
        [InlineData("Grid-Name-Equals=&Grid-Name-Contains=b", "Equals", "")]
        [InlineData("Grid-Name-Equals=a&Grid-Name-Contains=b", "Equals", "a")]
        [InlineData("Grid-Name-Equals=a&Grid-Name-Contains=b&Grid-Name-Op=Or", "Equals", "a")]
        public void GetFilter_SetsFirstFilter(String query, String type, String value)
        {
            column.Grid.Query = HttpUtility.ParseQueryString(query);

            IGridFilter actual = filters.GetFilter(column).First;

            Assert.Equal(typeof(StringEqualsFilter), actual.GetType());
            Assert.Equal(value, actual.Value);
            Assert.Equal(type, actual.Type);
        }

        [Fact]
        public void GetFilter_OnNotMultiFilterableColumnSetsOperatorToNull()
        {
            column.Grid.Query = HttpUtility.ParseQueryString("Grid-Name-Contains=a&Grid-Name-Equals=b&Grid-Name-Op=Or");
            column.IsMultiFilterable = false;

            Assert.Null(filters.GetFilter(column).Operator);
        }

        [Theory]
        [InlineData("Grid-Name-Op=", "")]
        [InlineData("Grid-Name-Op", null)]
        [InlineData("Grid-Name-Op=Or", "Or")]
        [InlineData("Grid-Name-Op=And", "And")]
        [InlineData("Grid-Name-Op-And=And", null)]
        [InlineData("Grid-Name-Op=And&Grid-Name-Op=Or", "And")]
        public void GetFilter_SetsOperatorFromQuery(String query, String filterOperator)
        {
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
        public void GetFilter_ReturnsGridColumnFilter()
        {
            Type expected = typeof(GridColumnFilter<GridModel>);
            Type actual = filters.GetFilter(column).GetType();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Register(Type forType, String filterType, Type filter)

        [Fact]
        public void Register_FilterForExistingType()
        {
            IDictionary<String, Type> filterPairs = new Dictionary<String, Type> { { "Test", typeof(Object) } };
            filters.Table.Add(typeof(Object), filterPairs);

            filters.Register(typeof(Object), "TestFilter", typeof(String));

            Type actual = filterPairs["TestFilter"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_NullableFilterTypeForExistingType()
        {
            IDictionary<String, Type> filterPairs = new Dictionary<String, Type> { { "Test", typeof(Object) } };

            filters.Table.Remove(typeof(Int32));
            filters.Table.Add(typeof(Int32), filterPairs);

            filters.Register(typeof(Int32?), "TestFilter", typeof(String));

            Type actual = filterPairs["TestFilter"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_Overrides_NullableFilter()
        {
            IDictionary<String, Type> filterPairs = new Dictionary<String, Type> { { "Test", typeof(Object) } };

            filters.Table.Remove(typeof(Int32));
            filters.Table.Add(typeof(Int32), filterPairs);

            filters.Register(typeof(Int32?), "Test", typeof(String));

            Type actual = filterPairs["Test"];
            Type expected = typeof(String);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Register_Overrides_Filter()
        {
            IDictionary<String, Type> filterPairs = new Dictionary<String, Type> { { "Test", typeof(Object) } };

            filters.Table.Remove(typeof(Int32));
            filters.Table.Add(typeof(Int32), filterPairs);

            filters.Register(typeof(Int32), "Test", typeof(String));

            Type actual = filterPairs["Test"];
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
            IDictionary<String, Type> filterPairs = new Dictionary<String, Type> { { "Test", null } };
            filters.Table.Add(typeof(Object), filterPairs);

            filters.Unregister(typeof(Object), "Test");

            Assert.Empty(filterPairs);
        }

        [Fact]
        public void Unregister_CanBeCalledOnNotExistingFilter()
        {
            filters.Unregister(typeof(Object), "Test");
        }

        #endregion
    }
}

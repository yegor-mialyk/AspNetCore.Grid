using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Linq;
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

        [Fact]
        public void GridFilters_SetEmptyBooleanText()
        {
            Assert.Empty(new GridFilters().BooleanEmptyOptionText());
        }

        [Fact]
        public void GridFilters_SetFalseBooleanText()
        {
            Assert.Equal("False", new GridFilters().BooleanFalseOptionText());
        }

        [Fact]
        public void GridFilters_SetTrueBooleanText()
        {
            Assert.Equal("True", new GridFilters().BooleanTrueOptionText());
        }

        [Theory]
        [InlineData(typeof(SByte), "equals", typeof(NumberFilter<SByte>))]
        [InlineData(typeof(SByte), "not-equals", typeof(NumberFilter<SByte>))]
        [InlineData(typeof(SByte), "less-than", typeof(NumberFilter<SByte>))]
        [InlineData(typeof(SByte), "greater-than", typeof(NumberFilter<SByte>))]
        [InlineData(typeof(SByte), "less-than-or-equal", typeof(NumberFilter<SByte>))]
        [InlineData(typeof(SByte), "greater-than-or-equal", typeof(NumberFilter<SByte>))]

        [InlineData(typeof(Byte), "equals", typeof(NumberFilter<Byte>))]
        [InlineData(typeof(Byte), "not-equals", typeof(NumberFilter<Byte>))]
        [InlineData(typeof(Byte), "less-than", typeof(NumberFilter<Byte>))]
        [InlineData(typeof(Byte), "greater-than", typeof(NumberFilter<Byte>))]
        [InlineData(typeof(Byte), "less-than-or-equal", typeof(NumberFilter<Byte>))]
        [InlineData(typeof(Byte), "greater-than-or-equal", typeof(NumberFilter<Byte>))]

        [InlineData(typeof(Int16), "equals", typeof(NumberFilter<Int16>))]
        [InlineData(typeof(Int16), "not-equals", typeof(NumberFilter<Int16>))]
        [InlineData(typeof(Int16), "less-than", typeof(NumberFilter<Int16>))]
        [InlineData(typeof(Int16), "greater-than", typeof(NumberFilter<Int16>))]
        [InlineData(typeof(Int16), "less-than-or-equal", typeof(NumberFilter<Int16>))]
        [InlineData(typeof(Int16), "greater-than-or-equal", typeof(NumberFilter<Int16>))]

        [InlineData(typeof(UInt16), "equals", typeof(NumberFilter<UInt16>))]
        [InlineData(typeof(UInt16), "not-equals", typeof(NumberFilter<UInt16>))]
        [InlineData(typeof(UInt16), "less-than", typeof(NumberFilter<UInt16>))]
        [InlineData(typeof(UInt16), "greater-than", typeof(NumberFilter<UInt16>))]
        [InlineData(typeof(UInt16), "less-than-or-equal", typeof(NumberFilter<UInt16>))]
        [InlineData(typeof(UInt16), "greater-than-or-equal", typeof(NumberFilter<UInt16>))]

        [InlineData(typeof(Int32), "equals", typeof(NumberFilter<Int32>))]
        [InlineData(typeof(Int32), "not-equals", typeof(NumberFilter<Int32>))]
        [InlineData(typeof(Int32), "less-than", typeof(NumberFilter<Int32>))]
        [InlineData(typeof(Int32), "greater-than", typeof(NumberFilter<Int32>))]
        [InlineData(typeof(Int32), "less-than-or-equal", typeof(NumberFilter<Int32>))]
        [InlineData(typeof(Int32), "greater-than-or-equal", typeof(NumberFilter<Int32>))]

        [InlineData(typeof(UInt32), "equals", typeof(NumberFilter<UInt32>))]
        [InlineData(typeof(UInt32), "not-equals", typeof(NumberFilter<UInt32>))]
        [InlineData(typeof(UInt32), "less-than", typeof(NumberFilter<UInt32>))]
        [InlineData(typeof(UInt32), "greater-than", typeof(NumberFilter<UInt32>))]
        [InlineData(typeof(UInt32), "less-than-or-equal", typeof(NumberFilter<UInt32>))]
        [InlineData(typeof(UInt32), "greater-than-or-equal", typeof(NumberFilter<UInt32>))]

        [InlineData(typeof(Int64), "equals", typeof(NumberFilter<Int64>))]
        [InlineData(typeof(Int64), "not-equals", typeof(NumberFilter<Int64>))]
        [InlineData(typeof(Int64), "less-than", typeof(NumberFilter<Int64>))]
        [InlineData(typeof(Int64), "greater-than", typeof(NumberFilter<Int64>))]
        [InlineData(typeof(Int64), "less-than-or-equal", typeof(NumberFilter<Int64>))]
        [InlineData(typeof(Int64), "greater-than-or-equal", typeof(NumberFilter<Int64>))]

        [InlineData(typeof(UInt64), "equals", typeof(NumberFilter<UInt64>))]
        [InlineData(typeof(UInt64), "not-equals", typeof(NumberFilter<UInt64>))]
        [InlineData(typeof(UInt64), "less-than", typeof(NumberFilter<UInt64>))]
        [InlineData(typeof(UInt64), "greater-than", typeof(NumberFilter<UInt64>))]
        [InlineData(typeof(UInt64), "less-than-or-equal", typeof(NumberFilter<UInt64>))]
        [InlineData(typeof(UInt64), "greater-than-or-equal", typeof(NumberFilter<UInt64>))]

        [InlineData(typeof(Single), "equals", typeof(NumberFilter<Single>))]
        [InlineData(typeof(Single), "not-equals", typeof(NumberFilter<Single>))]
        [InlineData(typeof(Single), "less-than", typeof(NumberFilter<Single>))]
        [InlineData(typeof(Single), "greater-than", typeof(NumberFilter<Single>))]
        [InlineData(typeof(Single), "less-than-or-equal", typeof(NumberFilter<Single>))]
        [InlineData(typeof(Single), "greater-than-or-equal", typeof(NumberFilter<Single>))]

        [InlineData(typeof(Double), "equals", typeof(NumberFilter<Double>))]
        [InlineData(typeof(Double), "not-equals", typeof(NumberFilter<Double>))]
        [InlineData(typeof(Double), "less-than", typeof(NumberFilter<Double>))]
        [InlineData(typeof(Double), "greater-than", typeof(NumberFilter<Double>))]
        [InlineData(typeof(Double), "less-than-or-equal", typeof(NumberFilter<Double>))]
        [InlineData(typeof(Double), "greater-than-or-equal", typeof(NumberFilter<Double>))]

        [InlineData(typeof(Decimal), "equals", typeof(NumberFilter<Decimal>))]
        [InlineData(typeof(Decimal), "not-equals", typeof(NumberFilter<Decimal>))]
        [InlineData(typeof(Decimal), "less-than", typeof(NumberFilter<Decimal>))]
        [InlineData(typeof(Decimal), "greater-than", typeof(NumberFilter<Decimal>))]
        [InlineData(typeof(Decimal), "less-than-or-equal", typeof(NumberFilter<Decimal>))]
        [InlineData(typeof(Decimal), "greater-than-or-equal", typeof(NumberFilter<Decimal>))]

        [InlineData(typeof(DateTime), "equals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "not-equals", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "earlier-than", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "later-than", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "earlier-than-or-equal", typeof(DateTimeFilter))]
        [InlineData(typeof(DateTime), "later-than-or-equal", typeof(DateTimeFilter))]

        [InlineData(typeof(Boolean), "equals", typeof(BooleanFilter))]
        [InlineData(typeof(Boolean), "not-equals", typeof(BooleanFilter))]

        [InlineData(typeof(String), "equals", typeof(StringEqualsFilter))]
        [InlineData(typeof(String), "not-equals", typeof(StringNotEqualsFilter))]
        [InlineData(typeof(String), "contains", typeof(StringContainsFilter))]
        [InlineData(typeof(String), "ends-with", typeof(StringEndsWithFilter))]
        [InlineData(typeof(String), "starts-with", typeof(StringStartsWithFilter))]

        [InlineData(typeof(Enum), "equals", typeof(EnumFilter))]
        [InlineData(typeof(Enum), "not-equals", typeof(EnumFilter))]

        [InlineData(typeof(Guid), "equals", typeof(GuidFilter))]
        [InlineData(typeof(Guid), "not-equals", typeof(GuidFilter))]
        public void GridFilters_RegistersDefaultFilters(Type type, String method, Type filter)
        {
            Assert.IsType(filter, new GridFilters().GetFilter(type, method));
        }

        #endregion

        #region GetFilter(Type type, String method)

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
        public void GetFilter_ForNullableType()
        {
            IGridFilter actual = filters.GetFilter(typeof(Int32?), "EQUALS");

            Assert.IsType<NumberFilter<Int32>>(actual);
            Assert.Equal("equals", actual.Method);
        }

        [Fact]
        public void GetFilter_ForSpecificEnumType()
        {
            filters.Register(typeof(TestEnum), "equals", typeof(StringEqualsFilter));

            IGridFilter actual = filters.GetFilter(typeof(TestEnum), "EQUALS");

            Assert.IsType<StringEqualsFilter>(actual);
            Assert.Equal("equals", actual.Method);
        }

        [Fact]
        public void GetFilter_ForEnumType()
        {
            IGridFilter actual = filters.GetFilter(typeof(TestEnum), "EQUALS");

            Assert.Equal("equals", actual.Method);
            Assert.IsType<EnumFilter>(actual);
        }

        [Fact]
        public void GetFilter_ForType()
        {
            IGridFilter actual = filters.GetFilter(typeof(String), "CONTAINS");

            Assert.IsType<StringContainsFilter>(actual);
            Assert.Equal("contains", actual.Method);
        }

        #endregion

        #region GetFilterOptions<T, TValue>(IGridColumn<T, TValue> column)

        [Fact]
        public void GetFilterOptions_ForBoolean()
        {
            SelectListItem[] actual = filters.GetFilterOptions<GridModel, Boolean>(null).ToArray();

            Assert.Equal(3, actual.Length);
            Assert.Equal("", actual[0].Value);
            Assert.Equal("true", actual[1].Value);
            Assert.Equal("false", actual[2].Value);
            Assert.Equal(filters.BooleanEmptyOptionText(), actual[0].Text);
            Assert.Equal(filters.BooleanTrueOptionText(), actual[1].Text);
            Assert.Equal(filters.BooleanFalseOptionText(), actual[2].Text);
        }

        [Fact]
        public void GetFilterOptions_ForNullableBoolean()
        {
            SelectListItem[] actual = filters.GetFilterOptions<GridModel, Boolean?>(null).ToArray();

            Assert.Equal(3, actual.Length);
            Assert.Equal("", actual[0].Value);
            Assert.Equal("true", actual[1].Value);
            Assert.Equal("false", actual[2].Value);
            Assert.Equal(filters.BooleanEmptyOptionText(), actual[0].Text);
            Assert.Equal(filters.BooleanTrueOptionText(), actual[1].Text);
            Assert.Equal(filters.BooleanFalseOptionText(), actual[2].Text);
        }

        [Fact]
        public void GetFilterOptions_NullViewContext_ForEnum()
        {
            IGridColumn<GridModel, TestEnum> enumColumn = new GridColumn<GridModel, TestEnum>(column.Grid, model => TestEnum.First);

            SelectListItem[] actual = filters.GetFilterOptions(enumColumn).ToArray();

            Assert.Single(actual);
            Assert.Null(actual[0].Text);
            Assert.Null(actual[0].Value);
        }

        [Fact]
        public void GetFilterOptions_ForEnum()
        {
            IHtmlHelper helper = Substitute.For<IHtmlHelper>();
            column.Grid.ViewContext = new ViewContext { HttpContext = Substitute.For<HttpContext>() };
            column.Grid.ViewContext.HttpContext.RequestServices.GetService<IHtmlHelper>().Returns(helper);
            helper.GetEnumSelectList(typeof(TestEnum)).Returns(new[] { new SelectListItem { Value = "0", Text = "Test" } });
            IGridColumn<GridModel, TestEnum> enumColumn = new GridColumn<GridModel, TestEnum>(column.Grid, model => TestEnum.First);

            SelectListItem[] actual = filters.GetFilterOptions(enumColumn).ToArray();

            Assert.Equal(2, actual.Length);

            Assert.Null(actual[0].Text);
            Assert.Null(actual[0].Value);
            Assert.Equal("0", actual[1].Value);
            Assert.Equal("Test", actual[1].Text);
        }

        [Fact]
        public void GetFilterOptions_ForOtherTypes()
        {
            Assert.Empty(filters.GetFilterOptions<GridModel, String>(null));
        }

        #endregion

        #region Register(Type type, String method, Type filter)

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
            filters.Register(typeof(Int32?), "TEST-filter", typeof(NumberFilter<Int32>));

            Assert.IsType<NumberFilter<Int32>>(filters.GetFilter(typeof(Int32), "test-filter"));
        }

        [Fact]
        public void Register_Overrides_Filter()
        {
            filters.Register(typeof(Int32), "test-filter", typeof(Object));
            filters.Register(typeof(Int32), "TEST-filter", typeof(NumberFilter<Int32>));

            Assert.IsType<NumberFilter<Int32>>(filters.GetFilter(typeof(Int32), "test-filter"));
        }

        [Fact]
        public void Register_NullableTypeAsNotNullable()
        {
            filters.Register(typeof(Int32?), "TEST", typeof(NumberFilter<Int32>));

            Assert.IsType<NumberFilter<Int32>>(filters.GetFilter(typeof(Int32), "test"));
        }

        [Fact]
        public void Register_FilterForNewType()
        {
            filters.Register(typeof(Object), "test", typeof(NumberFilter<Int32>));

            Assert.IsType<NumberFilter<Int32>>(filters.GetFilter(typeof(Object), "test"));
        }

        #endregion

        #region Unregister(Type type, String method)

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

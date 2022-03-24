using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;

namespace NonFactors.Mvc.Grid;

public class GridColumnFilterTests
{
    private GridColumnFilter<GridModel, String?> filter;
    private IQueryable<GridModel> items;

    public GridColumnFilterTests()
    {
        Grid<GridModel> grid = new(Array.Empty<GridModel>());
        GridColumn<GridModel, String?> column = new(grid, model => model.Name);

        filter = new GridColumnFilter<GridModel, String?>(column) { IsEnabled = true };

        items = new[]
        {
            new GridModel { Name = "aa", NSum = 10, Sum = 40 },
            new GridModel { Name = "Aa", NSum = 15, Sum = 35 },
            new GridModel { Name = "AA", NSum = 20, Sum = 35 },
            new GridModel { Name = "bb", NSum = 20, Sum = 30 },
            new GridModel { Name = "Bb", NSum = 25, Sum = 25 },
            new GridModel { Name = "BB", NSum = 30, Sum = 15 },
            new GridModel { Name = null, NSum = 30, Sum = 20 },
            new GridModel { Name = "Cc", NSum = null, Sum = 10 }
        }.AsQueryable();
    }

    [Fact]
    public void Options_Set_Caches()
    {
        Object expected = filter.Options = new List<SelectListItem>();
        Object actual = filter.Options;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Options_Get_Disabled()
    {
        filter.IsEnabled = false;

        Assert.Empty(filter.Options);
    }

    [Fact]
    public void Options_Get_FromFilters()
    {
        filter.Column.Grid.ViewContext = new ViewContext();
        IGridFilters filters = Substitute.For<IGridFilters>();
        filters.OptionsFor(filter.Column).Returns(new[] { new SelectListItem() });
        filter.Column.Grid.ViewContext.HttpContext = Substitute.For<HttpContext>();
        filter.Column.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IGridFilters)).Returns(filters);

        Object expected = filters.OptionsFor(filter.Column);
        Object actual = filter.Options;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Options_Get_Caches()
    {
        filter.Column.Grid.ViewContext = new ViewContext();
        IGridFilters filters = Substitute.For<IGridFilters>();
        filters.OptionsFor(filter.Column).Returns(Array.Empty<SelectListItem>());
        filter.Column.Grid.ViewContext.HttpContext = Substitute.For<HttpContext>();
        filter.Column.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IGridFilters)).Returns(filters);

        Object options = filter.Options;

        filters.OptionsFor(filter.Column).Returns(Array.Empty<SelectListItem>());

        Assert.Same(options, filter.Options);
    }

    [Fact]
    public void Operator_Set_Caches()
    {
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-op=and");

        filter.Operator = "or";

        Assert.Equal("or", filter.Operator);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(false, null)]
    [InlineData(null, GridFilterType.Single)]
    [InlineData(false, GridFilterType.Double)]
    public void Operator_Get_Disabled(Boolean? isEnabled, GridFilterType? type)
    {
        filter.Type = type;
        filter.IsEnabled = isEnabled;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-op=and");

        Assert.Null(filter.Operator);
    }

    [Fact]
    public void Operator_Get_NoQuery()
    {
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = null;

        Assert.Null(filter.Operator);
    }

    [Theory]
    [InlineData("", "", null)]
    [InlineData("", "op", null)]
    [InlineData("", "name-op", "")]
    [InlineData("", "name-op=", "")]
    [InlineData("", "name-op=or", "or")]
    [InlineData("", "name-op=and", "and")]
    [InlineData("", "name-op-and=and", null)]
    [InlineData("", "name-op=and&name-op=or", "and")]
    [InlineData("", "NAME-OP=AND&NAME-OP=OR", "and")]
    [InlineData("grid", "", null)]
    [InlineData("grid", "name-op", null)]
    [InlineData("grid", "grid-name-op", "")]
    [InlineData("grid", "grid-name-op=", "")]
    [InlineData("grid", "grid-name-op=or", "or")]
    [InlineData("grid", "grid-name-op=and", "and")]
    [InlineData("grid", "grid-name-op-and=and", null)]
    [InlineData("grid", "grid-name-op=and&grid-name-op=or", "and")]
    [InlineData("grid", "GRID-NAME-OP=AND&GRID-NAME-OP=OR", "and")]
    public void Operator_Get_FromQuery(String name, String query, String? op)
    {
        filter.Column.Grid.Name = name;
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString(query);

        Assert.Equal(op, filter.Operator);
    }

    [Fact]
    public void Operator_Get_Caches()
    {
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-op=or");

        String? op = filter.Operator;

        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-op=and");

        Assert.Equal(op, filter.Operator);
    }

    [Fact]
    public void First_Set_Caches()
    {
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-contains=a");

        filter.First = null;

        Assert.Null(filter.First);
    }

    [Fact]
    public void First_Get_Disabled()
    {
        filter.IsEnabled = false;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-equals=a");

        Assert.Null(filter.First);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "name=a")]
    [InlineData("", "name-=a")]
    [InlineData("", "name-eq=a")]
    [InlineData("grid", "")]
    [InlineData("grid", "grid-")]
    [InlineData("grid", "grid-name=a")]
    [InlineData("grid", "grid-name-=a")]
    [InlineData("grid", "grid-name-eq=a")]
    public void First_Get_NotFoundReturnNull(String name, String query)
    {
        filter.Column.Grid.Name = name;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString(query);

        Assert.Null(filter.First);
    }

    [Theory]
    [InlineData(GridFilterType.Multi)]
    [InlineData(GridFilterType.Single)]
    [InlineData(GridFilterType.Double)]
    public void First_Get_NullQuery(GridFilterType type)
    {
        filter.Type = type;
        filter.Column.Grid.Query = null;

        Assert.Null(filter.First);
    }

    [Theory]
    [InlineData("", "name-equals=a&name-eq=b", "a")]
    [InlineData("", "name-equals=&name-equals=b", "")]
    [InlineData("", "name-equals=&name-contains=b", "")]
    [InlineData("", "name-equals=a&name-contains=b", "a")]
    [InlineData("", "name-equals=a&name-equals=b", "a")]
    [InlineData("", "name-equals=a&name-contains=b&name-op=or", "a")]
    [InlineData("", "NAME-EQUALS=A&NAME-CONTAINS=B&NAME-OP=OR", "A")]
    [InlineData("grid", "grid-name-equals=a&grid-name-eq=b", "a")]
    [InlineData("grid", "grid-name-equals=&grid-name-equals=b", "")]
    [InlineData("grid", "grid-name-equals=&grid-name-contains=b", "")]
    [InlineData("grid", "grid-name-equals=a&grid-name-contains=b", "a")]
    [InlineData("grid", "grid-name-equals=a&grid-name-equals=b", "a")]
    [InlineData("grid", "grid-name-equals=a&grid-name-contains=b&grid-name-op=or", "a")]
    [InlineData("grid", "GRID-NAME-EQUALS=A&GRID-NAME-CONTAINS=B&GRID-NAME-OP=OR", "A")]
    public void First_Get_FromQuery(String name, String query, String value)
    {
        filter.Column.Grid.Name = name;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString(query);

        IGridFilter actual = filter.First!;

        Assert.Equal("equals", actual.Method);
        Assert.IsType<StringFilter>(actual);
        Assert.Equal(value, actual.Values);
    }

    [Fact]
    public void First_Get_SkipsOperation()
    {
        filter.Column.Grid.ViewContext = new ViewContext();
        IGridFilters filters = Substitute.For<IGridFilters>();
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-op=test");
        filter.Column.Grid.ViewContext.HttpContext = Substitute.For<HttpContext>();
        filter.Column.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IGridFilters)).Returns(filters);
        filters.Create(Arg.Any<Type>(), Arg.Any<String>(), Arg.Any<StringValues>()).Returns(Substitute.For<IGridFilter>());

        Assert.Null(filter.First);
    }

    [Theory]
    [InlineData("", "name-equals=a&name-equals=b", "a,b")]
    [InlineData("grid", "grid-name-equals=a&grid-name-equals=b", "a,b")]
    public void First_Get_MultiFilter(String name, String query, String value)
    {
        filter.Column.Grid.Name = name;
        filter.Type = GridFilterType.Multi;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString(query);

        IGridFilter actual = filter.First!;

        Assert.Equal("equals", actual.Method);
        Assert.IsType<StringFilter>(actual);
        Assert.Equal(value, actual.Values);
    }

    [Fact]
    public void First_Get_Caches()
    {
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-contains=a");

        IGridFilter? expected = filter.First;

        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-equals=b");

        IGridFilter actual = filter.First!;

        Assert.Equal("contains", actual.Method);
        Assert.IsType<StringFilter>(actual);
        Assert.Equal("a", actual.Values);
        Assert.Same(expected, actual);
    }

    [Fact]
    public void Second_Set_Caches()
    {
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-contains=a&name-equals=b");

        filter.Second = null;

        Assert.Null(filter.Second);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(false, null)]
    [InlineData(null, GridFilterType.Single)]
    [InlineData(false, GridFilterType.Double)]
    public void Second_Get_Disabled(Boolean? isEnabled, GridFilterType? type)
    {
        filter.Type = type;
        filter.IsEnabled = isEnabled;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-equals=a&name-equals=b");

        Assert.Null(filter.Second);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "name=a")]
    [InlineData("", "name-=a")]
    [InlineData("", "name-eq=a")]
    [InlineData("", "name-equals=a")]
    [InlineData("", "name-equals=a&")]
    [InlineData("", "name-equals=a&name=b")]
    [InlineData("", "name-equals=a&name-=b")]
    [InlineData("", "name-equals=a&name-eq=b")]
    [InlineData("grid", "")]
    [InlineData("grid", "grid-")]
    [InlineData("grid", "grid-name=a")]
    [InlineData("grid", "grid-name-=a")]
    [InlineData("grid", "grid-name-eq=a")]
    [InlineData("grid", "grid-name-equals=a")]
    [InlineData("grid", "grid-name-equals=a&")]
    [InlineData("grid", "grid-name-equals=a&grid-name=b")]
    [InlineData("grid", "grid-name-equals=a&grid-name-=b")]
    [InlineData("grid", "grid-name-equals=a&grid-name-eq=b")]
    public void Second_Get_NotFoundReturnNull(String name, String query)
    {
        filter.Column.Grid.Name = name;
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString(query);

        Assert.Null(filter.Second);
    }

    [Theory]
    [InlineData("", "name-eq=a&name-equals=b", "b")]
    [InlineData("", "name-equals=a&name-equals=b", "b")]
    [InlineData("", "name-contains=a&name-equals=", "")]
    [InlineData("", "name-equals=a&name-equals=", "")]
    [InlineData("", "name-contains=a&name-equals=ba", "ba")]
    [InlineData("", "name-contains=a&name-equals=b&name-op=or", "b")]
    [InlineData("", "NAME-CONTAINS=A&NAME-EQUALS=B&NAME-OP=OR", "B")]
    [InlineData("grid", "grid-name-eq=a&grid-name-equals=b", "b")]
    [InlineData("grid", "grid-name-equals=a&grid-name-equals=b", "b")]
    [InlineData("grid", "grid-name-contains=a&grid-name-equals=", "")]
    [InlineData("grid", "grid-name-equals=a&grid-name-equals=", "")]
    [InlineData("grid", "grid-name-contains=a&grid-name-equals=ba", "ba")]
    [InlineData("grid", "grid-name-contains=a&grid-name-equals=b&grid-name-op=or", "b")]
    [InlineData("grid", "GRID-NAME-CONTAINS=A&GRID-NAME-EQUALS=B&GRID-NAME-OP=OR", "B")]
    public void Second_Get_FromQuery(String name, String query, String value)
    {
        filter.Column.Grid.Name = name;
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString(query);

        IGridFilter actual = filter.Second!;

        Assert.Equal("equals", actual.Method);
        Assert.IsType<StringFilter>(actual);
        Assert.Equal(value, actual.Values);
    }

    [Fact]
    public void Second_Get_SkipsOperation()
    {
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.ViewContext = new ViewContext();
        IGridFilters filters = Substitute.For<IGridFilters>();
        filter.Column.Grid.ViewContext.HttpContext = Substitute.For<HttpContext>();
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-equals=test&name-op=test");
        filter.Column.Grid.ViewContext.HttpContext.RequestServices.GetService(typeof(IGridFilters)).Returns(filters);
        filters.Create(Arg.Any<Type>(), Arg.Any<String>(), Arg.Any<StringValues>()).Returns(Substitute.For<IGridFilter>());

        Assert.Null(filter.Second);
    }
    [Fact]
    public void Second_Get_Caches()
    {
        filter.Type = GridFilterType.Double;
        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-contains=a&name-equals=b");

        IGridFilter? expected = filter.Second;

        filter.Column.Grid.Query = HttpUtility.ParseQueryString("name-starts-with=d&name-ends-with=e");

        IGridFilter actual = filter.Second!;

        Assert.Equal("equals", actual.Method);
        Assert.IsType<StringFilter>(actual);
        Assert.Equal("b", actual.Values);
        Assert.Same(expected, actual);
    }

    [Fact]
    public void GridColumnFilter_SetsColumn()
    {
        Object actual = new GridColumnFilter<GridModel, String?>(filter.Column).Column;
        Object expected = filter.Column;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void GridColumnFilter_SetsNameForEnum()
    {
        AssertFilterNameFor(model => model.EnumField, "default");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForSByte()
    {
        AssertFilterNameFor(model => model.SByteField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForByte()
    {
        AssertFilterNameFor(model => model.ByteField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForInt16()
    {
        AssertFilterNameFor(model => model.Int16Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForUInt16()
    {
        AssertFilterNameFor(model => model.UInt16Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForInt32()
    {
        AssertFilterNameFor(model => model.Int32Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForUInt32()
    {
        AssertFilterNameFor(model => model.UInt32Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForInt64()
    {
        AssertFilterNameFor(model => model.Int64Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForUInt64()
    {
        AssertFilterNameFor(model => model.UInt64Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForSingle()
    {
        AssertFilterNameFor(model => model.SingleField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForDouble()
    {
        AssertFilterNameFor(model => model.DoubleField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForDecimal()
    {
        AssertFilterNameFor(model => model.DecimalField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForString()
    {
        AssertFilterNameFor(model => model.StringField, "text");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForStringList()
    {
        AssertFilterNameFor(model => model.NullableListField, "text");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForStringArray()
    {
        AssertFilterNameFor(model => model.NullableArrayField, "text");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForStringEnumerable()
    {
        AssertFilterNameFor(model => model.NullableEnumerableField, "text");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNotEnumerableGenericType()
    {
        AssertFilterNameFor(model => model.GridColumnField, "default");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForBoolean()
    {
        AssertFilterNameFor(model => model.BooleanField, "default");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForDateTime()
    {
        AssertFilterNameFor(model => model.DateTimeField, "date");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForGuid()
    {
        AssertFilterNameFor(model => model.GuidField, "guid");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableEnum()
    {
        AssertFilterNameFor(model => model.NullableEnumField, "default");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableSByte()
    {
        AssertFilterNameFor(model => model.NullableSByteField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableByte()
    {
        AssertFilterNameFor(model => model.NullableByteField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableInt16()
    {
        AssertFilterNameFor(model => model.NullableInt16Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableUInt16()
    {
        AssertFilterNameFor(model => model.NullableUInt16Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableInt32()
    {
        AssertFilterNameFor(model => model.NullableInt32Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableUInt32()
    {
        AssertFilterNameFor(model => model.NullableUInt32Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableInt64()
    {
        AssertFilterNameFor(model => model.NullableInt64Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableUInt64()
    {
        AssertFilterNameFor(model => model.NullableUInt64Field, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableSingle()
    {
        AssertFilterNameFor(model => model.NullableSingleField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableDouble()
    {
        AssertFilterNameFor(model => model.NullableDoubleField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableDecimal()
    {
        AssertFilterNameFor(model => model.NullableDecimalField, "number");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableBoolean()
    {
        AssertFilterNameFor(model => model.NullableBooleanField, "default");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableDateTime()
    {
        AssertFilterNameFor(model => model.NullableDateTimeField, "date");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForNullableGuid()
    {
        AssertFilterNameFor(model => model.NullableGuidField, "guid");
    }

    [Fact]
    public void GridColumnFilter_SetsNameForOtherTypes()
    {
        AssertFilterNameFor(model => model, "default");
    }

    [Fact]
    public void GridColumnFilter_SetsDefaultMethod()
    {
        Assert.Empty(new GridColumnFilter<GridModel, String?>(filter.Column).DefaultMethod);
    }

    [Fact]
    public void GridColumnFilter_NotMemberExpression_IsNotEnabled()
    {
        GridColumn<GridModel, String?> column = new(filter.Column.Grid, model => model.ToString());

        Assert.False(new GridColumnFilter<GridModel, String?>(column).IsEnabled);
    }

    [Fact]
    public void GridColumnFilter_MemberExpression_IsEnabledNull()
    {
        GridColumn<GridModel, String?> column = new(filter.Column.Grid, model => model.Name);

        Assert.Null(new GridColumnFilter<GridModel, String?>(column).IsEnabled);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    public void Apply_NotEnabled_ReturnsSameItems(Boolean? isEnabled)
    {
        filter.IsEnabled = isEnabled;
        filter.Type = GridFilterType.Double;
        filter.First = new StringFilter { Values = "A" };

        Assert.Equal(items, filter.Apply(items));
    }

    [Fact]
    public void Apply_NoFilters_ReturnsSameItems()
    {
        filter.First = null;
        filter.Second = null;
        filter.IsEnabled = true;
        filter.Type = GridFilterType.Double;

        Assert.Equal(items, filter.Apply(items));
    }

    [Fact]
    public void Apply_NullAppliedFilter_ReturnsSameItems()
    {
        filter.Operator = "or";
        filter.Type = GridFilterType.Double;
        filter.First = Substitute.For<IGridFilter>();
        filter.Second = Substitute.For<IGridFilter>();

        Assert.Equal(items, filter.Apply(items));
    }

    [Theory]
    [InlineData("and")]
    [InlineData("AND")]
    public void Apply_UsingAndOperator(String op)
    {
        filter.Operator = op;
        filter.Type = GridFilterType.Double;
        filter.First = new StringFilter { Method = "contains", Values = "a" };
        filter.Second = new StringFilter { Method = "contains", Values = "A" };

        IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains('a') && item.Name.Contains('A'));
        IQueryable actual = filter.Apply(items);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("or")]
    [InlineData("OR")]
    public void Apply_UsingOrOperator(String op)
    {
        filter.Operator = op;
        filter.Type = GridFilterType.Double;
        filter.First = new StringFilter { Method = "contains", Values = "a" };
        filter.Second = new StringFilter { Method = "contains", Values = "bB" };

        IQueryable expected = items.Where(item => item.Name != null && (item.Name.Contains('a') || item.Name.Contains("bB")));
        IQueryable actual = filter.Apply(items);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("xor")]
    public void Apply_InvalidOperator_FirstFilter(String? op)
    {
        filter.Operator = op;
        filter.Type = GridFilterType.Double;
        filter.First = new StringFilter { Method = "contains", Values = "a" };
        filter.Second = new StringFilter { Method = "contains", Values = "BB" };

        IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains('a'));
        IQueryable actual = filter.Apply(items);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_NoSecondFilter()
    {
        filter.Second = null;
        filter.Operator = "or";
        filter.Type = GridFilterType.Double;
        filter.First = new StringFilter { Method = "contains", Values = "a" };

        IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains('a'));
        IQueryable actual = filter.Apply(items);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_FirstFilter()
    {
        filter.Operator = "or";
        filter.Type = GridFilterType.Single;
        filter.First = new StringFilter { Method = "contains", Values = "a" };
        filter.Second = new StringFilter { Method = "contains", Values = "bb" };

        IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains('a'));
        IQueryable actual = filter.Apply(items);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_FiltersByExpressions()
    {
        GridColumn<GridModel, Int32?> testColumn = new(new Grid<GridModel>(Array.Empty<GridModel>()), model => model.NSum);
        GridColumnFilter<GridModel, Int32?> testFilter = new(testColumn)
        {
            Second = new NumberFilter<Int32> { Method = "greater-than", Values = "25" },
            First = new NumberFilter<Int32> { Method = "equals", Values = "10" },
            Type = GridFilterType.Double,
            IsEnabled = true,
            Operator = "or"
        };

        IQueryable expected = items.Where(item => item.NSum == 10 || item.NSum > 25);
        IQueryable actual = testFilter.Apply(items);

        Assert.Equal(expected, actual);
    }

    private void AssertFilterNameFor<TValue>(Expression<Func<GridModel, TValue>> property, String name)
    {
        Grid<GridModel> grid = new(Array.Empty<GridModel>());

        Assert.Equal(name, new GridColumnFilter<GridModel, TValue>(new GridColumn<GridModel, TValue>(grid, property)).Name);
    }
}

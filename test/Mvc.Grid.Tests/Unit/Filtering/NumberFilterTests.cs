using Microsoft.Extensions.Primitives;
using System.Collections;

namespace NonFactors.Mvc.Grid;

public class NumberFilterTests
{
    private Expression<Func<GridModel, Int32?>> nSumExpression;
    private Expression<Func<GridModel, Int32>> sumExpression;
    private IQueryable<GridModel> items;

    public NumberFilterTests()
    {
        items = new[]
        {
            new GridModel(),
            new GridModel { NSum = 1, Sum = 2 },
            new GridModel { NSum = 2, Sum = 1 }
        }.AsQueryable();

        nSumExpression = model => model.NSum;
        sumExpression = model => model.Sum;
    }

    [Theory]
    [InlineData("test")]
    [InlineData("79228162514264337593543950336")]
    [InlineData("-79228162514264337593543950336")]
    public void Apply_BadDecimalValue_ReturnsNull(String value)
    {
        NumberFilter<Decimal> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("test")]
    [InlineData("1.8076931348623157E+308")]
    [InlineData("-1.8076931348623157E+308")]
    public void Apply_BadDoubleValue_ReturnsNull(String value)
    {
        NumberFilter<Double> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("test")]
    [InlineData("3.50282347E+38")]
    [InlineData("-3.50282347E+38")]
    public void Apply_BadSingleValue_ReturnsNull(String value)
    {
        NumberFilter<Single> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("test")]
    [InlineData("9223372036854775808")]
    [InlineData("-9223372036854775809")]
    public void Apply_BadInt64Value_ReturnsNull(String value)
    {
        NumberFilter<Int64> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("test")]
    [InlineData("18446744073709551616")]
    public void Apply_BadUInt64Value_ReturnsNull(String value)
    {
        NumberFilter<UInt64> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("test")]
    [InlineData("2147483648")]
    [InlineData("-2147483649")]
    public void Apply_BadInt32Value_ReturnsNull(String value)
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("test")]
    [InlineData("4294967296")]
    public void Apply_BadUInt32Value_ReturnsNull(String value)
    {
        NumberFilter<UInt32> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("test")]
    [InlineData("32768")]
    [InlineData("-32769")]
    public void Apply_BadInt16Value_ReturnsNull(String value)
    {
        NumberFilter<Int16> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("test")]
    [InlineData("65536")]
    public void Apply_BadUInt16Value_ReturnsNull(String value)
    {
        NumberFilter<UInt16> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("128")]
    [InlineData("-129")]
    [InlineData("test")]
    public void Apply_BadSByteValue_ReturnsNull(String value)
    {
        NumberFilter<SByte> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("256")]
    [InlineData("test")]
    public void Apply_BadByteValue_ReturnsNull(String value)
    {
        NumberFilter<Byte> filter = new() { Method = "equals", Values = value };

        Assert.Null(filter.Apply(sumExpression.Body));
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NullableEqualsFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = value };

        IEnumerable expected = items.Where(model => model.NSum == number);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableEqualsFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = new[] { "", "1" } };

        IEnumerable expected = items.Where(model => model.NSum == null || model.NSum == 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_EqualsFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = value };

        IEnumerable expected = items.Where(model => model.Sum == number);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleEqualsFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = new[] { "1", "2" } };

        IEnumerable expected = items.Where(model => model.Sum == 1 || model.Sum == 2);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NullableNotEqualsFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "not-equals", Values = value };

        IEnumerable expected = items.Where(model => model.NSum != number);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableNotEqualsFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "not-equals", Values = new[] { "", "1" } };

        IEnumerable expected = items.Where(model => model.NSum != null && model.NSum != 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NotEqualsFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "not-equals", Values = value };

        IEnumerable expected = items.Where(model => model.Sum != number);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNotEqualsFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "not-equals", Values = new[] { "1", "2" } };

        IEnumerable expected = items.Where(model => model.Sum != 1 && model.Sum != 2);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NullableLessThanFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "less-than", Values = value };

        IEnumerable expected = items.Where(model => model.NSum < number);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableLessThanFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "less-than", Values = new[] { "", "1" } };

        IEnumerable expected = items.Where(model => model.NSum < 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_LessThanFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "less-than", Values = value };

        IEnumerable expected = items.Where(model => model.Sum < number);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleLessThanFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "less-than", Values = new[] { "1", "2" } };

        IEnumerable expected = items.Where(model => model.Sum < 2);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NullableGreaterThanFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than", Values = value };

        IEnumerable expected = items.Where(model => model.NSum > number);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableGreaterThanFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than", Values = new[] { "", "1" } };

        IEnumerable expected = items.Where(model => model.NSum > 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_GreaterThanFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than", Values = value };

        IEnumerable expected = items.Where(model => model.Sum > number);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleGreaterThanFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than", Values = new[] { "1", "2" } };

        IEnumerable expected = items.Where(model => model.Sum > 1);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NullableLessThanOrEqualFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "less-than-or-equal", Values = value };

        IEnumerable expected = items.Where(model => model.NSum <= number);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableLessThanOrEqualFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "less-than-or-equal", Values = new[] { "", "1" } };

        IEnumerable expected = items.Where(model => model.NSum <= 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_LessThanOrEqualFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "less-than-or-equal", Values = value };

        IEnumerable expected = items.Where(model => model.Sum <= number);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleLessThanOrEqualFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "less-than-or-equal", Values = new[] { "0", "1" } };

        IEnumerable expected = items.Where(model => model.Sum <= 1);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_NullableGreaterThanOrEqualFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than-or-equal", Values = value };

        IEnumerable actual = items.Where(nSumExpression, filter);
        IEnumerable expected = items.Where(model => model.NSum >= number);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableGreaterThanOrEqualFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than-or-equal", Values = new[] { "", "1" } };

        IEnumerable expected = items.Where(model => model.NSum >= 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Apply_GreaterThanOrEqualFilter(String value, Int32? number)
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than-or-equal", Values = value };

        IEnumerable actual = items.Where(sumExpression, filter);
        IEnumerable expected = items.Where(model => model.Sum >= number);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleGreaterThanOrEqualFilter()
    {
        NumberFilter<Int32> filter = new() { Method = "greater-than-or-equal", Values = new[] { "1", "2" } };

        IEnumerable expected = items.Where(model => model.Sum >= 1);
        IEnumerable actual = items.Where(sumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleWithBadValues()
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = new[] { "", "test", "1" } };

        IEnumerable expected = items.Where(model => model.NSum == null || model.NSum == 1);
        IEnumerable actual = items.Where(nSumExpression, filter);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_EmptyValue_ReturnsNull()
    {
        NumberFilter<Int32> filter = new() { Method = "equals", Values = StringValues.Empty };

        Assert.Null(filter.Apply(nSumExpression.Body));
    }

    [Fact]
    public void Apply_BadMethod_ReturnsNull()
    {
        Assert.Null(new NumberFilter<Int32> { Method = "test", Values = "1" }.Apply(sumExpression.Body));
    }
}

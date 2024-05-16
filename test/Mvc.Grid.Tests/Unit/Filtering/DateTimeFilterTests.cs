using Microsoft.Extensions.Primitives;
using System.Collections;

namespace NonFactors.Mvc.Grid;

public class DateTimeFilterTests
{
    private IQueryable<GridModel> items;
    private DateTimeFilter filter;

    public DateTimeFilterTests()
    {
        items = new[]
        {
            new GridModel { Date = new DateTime(2013, 01, 01), NDate = null },
            new GridModel { Date = new DateTime(2014, 01, 01), NDate = new DateTime(2015, 01, 01) },
            new GridModel { Date = new DateTime(2015, 01, 01), NDate = new DateTime(2014, 01, 01) }
        }.AsQueryable();

        filter = new DateTimeFilter();
    }

    [Fact]
    public void Apply_BadDateOnlyValue_ReturnsItems()
    {
        Expression<Func<GridModel, DateOnly>> expression = model => model.DateOnly;
        filter.Values = "Test";

        Assert.Null(filter.Apply(expression.Body));
    }

    [Fact]
    public void Apply_BadDateTimeValue_ReturnsItems()
    {
        Expression<Func<GridModel, DateTime>> expression = model => model.Date;
        filter.Values = "Test";

        Assert.Null(filter.Apply(expression.Body));
    }

    [Fact]
    public void Apply_BadTimeOnlyValue_ReturnsItems()
    {
        Expression<Func<GridModel, TimeOnly>> expression = model => model.TimeOnly;
        filter.Values = "Test";

        Assert.Null(filter.Apply(expression.Body));
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NullableEqualsFilter(String value)
    {
        filter.Values = value;
        filter.Method = "equals";

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate == (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableEqualsFilter()
    {
        filter.Method = "equals";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate == null || model.NDate == new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_EqualsFilter(String value)
    {
        filter.Values = value;
        filter.Method = "equals";

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date == (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleEqualsFilter()
    {
        filter.Method = "equals";
        filter.Values = new[] { "2013-01-01", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date == new DateTime(2013, 1, 1) || model.Date == new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NullableNotEqualsFilter(String value)
    {
        filter.Values = value;
        filter.Method = "not-equals";

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate != (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableNotEqualsFilter()
    {
        filter.Method = "not-equals";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate != null && model.NDate != new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NotEqualsFilter(String value)
    {
        filter.Values = value;
        filter.Method = "not-equals";

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date != (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNotEqualsFilter()
    {
        filter.Method = "not-equals";
        filter.Values = new[] { "2013-01-01", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date != new DateTime(2013, 1, 1) && model.Date != new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NullableEarlierThanFilter(String value)
    {
        filter.Values = value;
        filter.Method = "earlier-than";

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate < (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableEarlierThanFilter()
    {
        filter.Method = "earlier-than";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate < new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_EarlierThanFilter(String value)
    {
        filter.Values = value;
        filter.Method = "earlier-than";

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date < (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleEarlierThanFilter()
    {
        filter.Method = "earlier-than";
        filter.Values = new[] { "2013-01-01", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date < new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NullableLaterThanFilter(String value)
    {
        filter.Values = value;
        filter.Method = "later-than";

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate > (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableLaterThanFilter()
    {
        filter.Method = "later-than";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate > new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_LaterThanFilter(String value)
    {
        filter.Values = value;
        filter.Method = "later-than";

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date > (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleLaterThanFilter()
    {
        filter.Method = "later-than";
        filter.Values = new[] { "2013-01-01", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date > new DateTime(2013, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NullableEarlierThanOrEqualFilter(String value)
    {
        filter.Values = value;
        filter.Method = "earlier-than-or-equal";

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate <= (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableEarlierThanOrEqualFilter()
    {
        filter.Method = "earlier-than-or-equal";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate <= new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_EarlierThanOrEqualFilter(String value)
    {
        filter.Values = value;
        filter.Method = "earlier-than-or-equal";

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date <= (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleEarlierThanOrEqualFilter()
    {
        filter.Method = "earlier-than-or-equal";
        filter.Values = new[] { "2013-01-01", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date <= new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_NullableLaterThanOrEqualFilter(String value)
    {
        filter.Values = value;
        filter.Method = "later-than-or-equal";

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate >= (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleNullableLaterThanOrEqualFilter()
    {
        filter.Method = "later-than-or-equal";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate >= new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2014-01-01")]
    public void Apply_LaterThanOrEqualFilter(String value)
    {
        filter.Values = value;
        filter.Method = "later-than-or-equal";

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date >= (String.IsNullOrEmpty(value) ? null : DateTime.Parse(value)));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleLaterThanOrEqualFilter()
    {
        filter.Method = "later-than-or-equal";
        filter.Values = new[] { "", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.Date, filter);
        IEnumerable expected = items.Where(model => model.Date >= new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_MultipleWithBadValues()
    {
        filter.Method = "equals";
        filter.Values = new[] { "", "test", "2014-01-01" };

        IEnumerable actual = items.Where(model => model.NDate, filter);
        IEnumerable expected = items.Where(model => model.NDate == null || model.NDate == new DateTime(2014, 1, 1));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Apply_EmptyValue_ReturnsNull()
    {
        Expression<Func<GridModel, DateTime>> expression = model => model.Date;
        filter.Values = StringValues.Empty;
        filter.Method = "equals";

        Assert.Null(filter.Apply(expression.Body));
    }

    [Fact]
    public void Apply_BadMethod_ReturnsNull()
    {
        Expression<Func<GridModel, DateTime>> expression = model => model.Date;
        filter.Values = "2014-01-01";
        filter.Method = "test";

        Assert.Null(filter.Apply(expression.Body));
    }
}

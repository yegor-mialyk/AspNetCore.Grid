using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class DateTimeFilterTests : BaseGridFilterTests
    {
        private Expression<Func<GridModel, DateTime?>> nDateExpression;
        private Expression<Func<GridModel, DateTime>> dateExpression;
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

            nDateExpression = (model) => model.NDate;
            dateExpression = (model) => model.Date;
            filter = new DateTimeFilter();
        }

        #region Apply(Expression expression)

        [Fact]
        public void Apply_BadValue_ReturnsItems()
        {
            filter.Value = "Test";

            Assert.Null(filter.Apply(dateExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NullableEqualsFilter(String value)
        {
            filter.Value = value;
            filter.Method = "equals";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate == (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_EqualsFilter(String value)
        {
            filter.Value = value;
            filter.Method = "equals";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date == (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NullableNotEqualsFilter(String value)
        {
            filter.Value = value;
            filter.Method = "not-equals";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate != (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NotEqualsFilter(String value)
        {
            filter.Value = value;
            filter.Method = "not-equals";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date != (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NullableEarlierThanFilter(String value)
        {
            filter.Value = value;
            filter.Method = "earlier-than";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate < (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_EalierThanFilter(String value)
        {
            filter.Value = value;
            filter.Method = "earlier-than";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date < (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NullableLaterThanFilter(String value)
        {
            filter.Value = value;
            filter.Method = "later-than";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate > (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_LaterThanFilter(String value)
        {
            filter.Value = value;
            filter.Method = "later-than";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date > (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NullableEarlierThanOrEqualFilter(String value)
        {
            filter.Value = value;
            filter.Method = "earlier-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate <= (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_EarlierThanOrEqualFilter(String value)
        {
            filter.Value = value;
            filter.Method = "earlier-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date <= (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_NullableLaterThanOrEqualFilter(String value)
        {
            filter.Value = value;
            filter.Method = "later-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate >= (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2014-01-01")]
        public void Apply_LaterThanOrEqualFilter(String value)
        {
            filter.Value = value;
            filter.Method = "later-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date >= (String.IsNullOrEmpty(value) ? null : (DateTime?)DateTime.Parse(value)));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Method = "test";
            filter.Value = "2014-01-01";

            Assert.Null(filter.Apply(dateExpression.Body));
        }

        #endregion
    }
}

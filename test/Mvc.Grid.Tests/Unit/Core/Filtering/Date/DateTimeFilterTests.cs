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
        public void Apply_NotDateTimeValue_ReturnsItems()
        {
            filter.Value = "Test";

            Assert.Null(filter.Apply(dateExpression.Body));
        }

        [Fact]
        public void Apply_NullableEqualsFilter()
        {
            filter.Method = "equals";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.NDate == new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EqualsFilter()
        {
            filter.Method = "equals";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.Date == new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableNotEqualsFilter()
        {
            filter.Method = "not-equals";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.NDate != new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotEqualsFilter()
        {
            filter.Method = "not-equals";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.Date != new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableEarlierThanFilter()
        {
            filter.Method = "earlier-than";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.NDate < new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EalierThanFilter()
        {
            filter.Method = "earlier-than";
            filter.Value = new DateTime(2014, 01, 01).ToString("d");

            IEnumerable expected = items.Where(model => model.Date < new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLaterThanFilter()
        {
            filter.Method = "later-than";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.NDate > new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LaterThanFilter()
        {
            filter.Method = "later-than";
            filter.Value = new DateTime(2014, 01, 01).ToString("D");

            IEnumerable expected = items.Where(model => model.Date > new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableEarlierThanOrEqualFilter()
        {
            filter.Method = "earlier-than-or-equal";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.NDate <= new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EarlierThanOrEqualFilter()
        {
            filter.Method = "earlier-than-or-equal";
            filter.Value = new DateTime(2014, 01, 01).ToString("d");

            IEnumerable expected = items.Where(model => model.Date <= new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLaterThanOrEqualFilter()
        {
            filter.Method = "later-than-or-equal";
            filter.Value = new DateTime(2014, 01, 01).ToString();

            IEnumerable expected = items.Where(model => model.NDate >= new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LaterThanOrEqualFilter()
        {
            filter.Method = "later-than-or-equal";
            filter.Value = new DateTime(2014, 01, 01).ToString("d");

            IEnumerable expected = items.Where(model => model.Date >= new DateTime(2014, 01, 01));
            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Method = "test";
            filter.Value = new DateTime(2014, 01, 01).ToString("d");

            Assert.Null(filter.Apply(dateExpression.Body));
        }

        #endregion
    }
}

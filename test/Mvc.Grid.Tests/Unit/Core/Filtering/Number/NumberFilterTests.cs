using NSubstitute;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class NumberFilterTests : BaseGridFilterTests
    {
        private Expression<Func<GridModel, Int32?>> nSumExpression;
        private Expression<Func<GridModel, Int32>> sumExpression;
        private IQueryable<GridModel> items;
        private NumberFilter filter;

        public NumberFilterTests()
        {
            items = new[]
            {
                new GridModel(),
                new GridModel { NSum = 1, Sum = 2 },
                new GridModel { NSum = 2, Sum = 1 }
            }.AsQueryable();

            filter = Substitute.ForPartsOf<NumberFilter>();
            nSumExpression = (model) => model.NSum;
            sumExpression = (model) => model.Sum;
        }

        #region Apply(Expression expression)

        [Fact]
        public void Apply_NullNumericValue_ReturnsNull()
        {
            filter.GetNumericValue().Returns(null);

            Assert.Null(filter.Apply(sumExpression.Body));
        }

        [Fact]
        public void Apply_NullableEqualsFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "Equals";

            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);
            IEnumerable expected = items.Where(model => model.NSum == 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EqualsFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "Equals";

            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);
            IEnumerable expected = items.Where(model => model.Sum == 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableNotEqualsFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "NotEquals";

            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);
            IEnumerable expected = items.Where(model => model.NSum != 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotEqualsFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "NotEquals";

            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);
            IEnumerable expected = items.Where(model => model.Sum != 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLessThanFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "LessThan";

            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);
            IEnumerable expected = items.Where(model => model.NSum < 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LessThanFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "LessThan";

            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);
            IEnumerable expected = items.Where(model => model.Sum < 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableGreaterThanFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "GreaterThan";

            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);
            IEnumerable expected = items.Where(model => model.NSum > 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_GreaterThanFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "GreaterThan";

            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);
            IEnumerable expected = items.Where(model => model.Sum > 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLessThanOrEqualFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "LessThanOrEqual";

            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);
            IEnumerable expected = items.Where(model => model.NSum <= 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LessThanOrEqualFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "LessThanOrEqual";

            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);
            IEnumerable expected = items.Where(model => model.Sum <= 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableGreaterThanOrEqualFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "GreaterThanOrEqual";

            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);
            IEnumerable expected = items.Where(model => model.NSum >= 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_GreaterThanOrEqualFilter()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "GreaterThanOrEqual";

            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);
            IEnumerable expected = items.Where(model => model.Sum >= 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_OnNotSupportedFilterTypeReturnsNull()
        {
            filter.GetNumericValue().Returns(1);
            filter.Type = "Test";

            Assert.Null(filter.Apply(sumExpression.Body));
        }

        #endregion
    }
}

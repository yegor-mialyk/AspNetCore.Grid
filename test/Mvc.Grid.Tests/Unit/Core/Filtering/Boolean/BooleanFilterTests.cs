using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class BooleanFilterTests : BaseGridFilterTests
    {
        private BooleanFilter filter;
        private IQueryable<GridModel> items;
        private Expression<Func<GridModel, Boolean>> booleanExpression;
        private Expression<Func<GridModel, Boolean?>> nBooleanExpression;

        public BooleanFilterTests()
        {
            items = new[]
            {
                new GridModel(),
                new GridModel { IsChecked = true, NIsChecked = false },
                new GridModel { IsChecked = false, NIsChecked = true }
            }.AsQueryable();

            filter = new BooleanFilter();
            booleanExpression = (model) => model.IsChecked;
            nBooleanExpression = (model) => model.NIsChecked;
        }

        #region Apply(Expression expression)

        [Fact]
        public void Apply_BadValue_ReturnsNull()
        {
            filter.Value = "Test";
            filter.Method = "equals";

            Assert.Null(filter.Apply(booleanExpression.Body));
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_NullableEqualsFilter(String value, Boolean? isChecked)
        {
            filter.Value = value;
            filter.Method = "equals";

            IEnumerable expected = items.Where(model => model.NIsChecked == isChecked);
            IEnumerable actual = Filter(items, filter.Apply(nBooleanExpression.Body), nBooleanExpression);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_EqualsFilter(String value, Boolean? isChecked)
        {
            filter.Value = value;
            filter.Method = "equals";

            IEnumerable expected = items.Where(model => model.IsChecked == isChecked);
            IEnumerable actual = Filter(items, filter.Apply(booleanExpression.Body), booleanExpression);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_NullableNotEqualsFilter(String value, Boolean? isChecked)
        {
            filter.Value = value;
            filter.Method = "not-equals";

            IEnumerable expected = items.Where(model => model.NIsChecked != isChecked);
            IEnumerable actual = Filter(items, filter.Apply(nBooleanExpression.Body), nBooleanExpression);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_NotEqualsFilter(String value, Boolean? isChecked)
        {
            filter.Value = value;
            filter.Method = "not-equals";

            IEnumerable expected = items.Where(model => model.IsChecked != isChecked);
            IEnumerable actual = Filter(items, filter.Apply(booleanExpression.Body), booleanExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Value = "false";
            filter.Method = "test";

            Assert.Null(filter.Apply(booleanExpression.Body));
        }

        #endregion
    }
}

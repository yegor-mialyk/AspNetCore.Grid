using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class BooleanFilterTests : BaseGridFilterTests
    {
        private IQueryable<GridModel> items;
        private BooleanFilter filter;

        public BooleanFilterTests()
        {
            items = new[]
            {
                new GridModel(),
                new GridModel { IsChecked = true, NIsChecked = false },
                new GridModel { IsChecked = false, NIsChecked = true }
            }.AsQueryable();

            filter = new BooleanFilter();
        }

        #region Apply(Expression expression)

        [Fact]
        public void Apply_NotBooleanValue_ReturnsNull()
        {
            Expression<Func<GridModel, Boolean>> expression = (model) => model.IsChecked;
            filter.Value = "Test";

            Assert.Null(filter.Apply(expression.Body));
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        public void Apply_NullableEqualsFilter(String value, Boolean isChecked)
        {
            Expression<Func<GridModel, Boolean?>> expression = (model) => model.NIsChecked;
            filter.Value = value;

            IEnumerable actual = Filter(items, filter.Apply(expression.Body), expression);
            IEnumerable expected = items.Where(model => model.NIsChecked == isChecked);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        public void Apply_EqualsFilter(String value, Boolean isChecked)
        {
            Expression<Func<GridModel, Boolean?>> expression = (model) => model.IsChecked;
            filter.Value = value;

            IEnumerable actual = Filter(items, filter.Apply(expression.Body), expression);
            IEnumerable expected = items.Where(model => model.IsChecked == isChecked);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class EnumFilterTests : BaseGridFilterTests
    {
        private Expression<Func<GridModel, TestEnum?>> nEnumExpression;
        private Expression<Func<GridModel, TestEnum>> enumExpression;
        private IQueryable<GridModel> items;
        private EnumFilter filter;

        public EnumFilterTests()
        {
            items = new[]
            {
                new GridModel(),
                new GridModel { Enum = TestEnum.First },
                new GridModel { Enum = TestEnum.Second },
                new GridModel { Enum = TestEnum.First, NEnum = TestEnum.Second },
                new GridModel { Enum = TestEnum.Second, NEnum = TestEnum.First }
            }.AsQueryable();

            nEnumExpression = (model) => model.NEnum;
            enumExpression = (model) => model.Enum;
            filter = new EnumFilter();
        }

        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        public void Apply_BadValue_ReturnsNull(String value)
        {
            filter.Value = value;
            filter.Method = "equals";

            Assert.Null(filter.Apply(enumExpression.Body));
        }

        [Fact]
        public void Apply_NullableEqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "equals";

            IEnumerable expected = items.Where(model => model.NEnum == TestEnum.Second);
            IEnumerable actual = Filter(items, filter.Apply(nEnumExpression.Body), nEnumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "equals";

            IEnumerable expected = items.Where(model => model.Enum == TestEnum.Second);
            IEnumerable actual = Filter(items, filter.Apply(enumExpression.Body), enumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableNotEqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "not-equals";

            IEnumerable expected = items.Where(model => model.NEnum != TestEnum.Second);
            IEnumerable actual = Filter(items, filter.Apply(nEnumExpression.Body), nEnumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotEqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "not-equals";

            IEnumerable expected = items.Where(model => model.Enum != TestEnum.Second);
            IEnumerable actual = Filter(items, filter.Apply(enumExpression.Body), enumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Value = "0";
            filter.Method = "test";

            Assert.Null(filter.Apply(enumExpression.Body));
        }

        #endregion
    }
}

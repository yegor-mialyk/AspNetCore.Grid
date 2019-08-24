using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class EnumFilterTests
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

        [Fact]
        public void Apply_BadValue_ReturnsNull()
        {
            filter.Method = "equals";
            filter.Values = new[] { "test" };

            Assert.Null(filter.Apply(enumExpression.Body));
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("1", TestEnum.Second)]
        public void Apply_NullableEqualsFilter(String value, TestEnum? test)
        {
            filter.Method = "equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(nEnumExpression, filter);
            IEnumerable expected = items.Where(model => model.NEnum == test);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("1", TestEnum.Second)]
        public void Apply_EqualsFilter(String value, TestEnum? test)
        {
            filter.Method = "equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(enumExpression, filter);
            IEnumerable expected = items.Where(model => model.Enum == test);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("1", TestEnum.Second)]
        public void Apply_NullableNotEqualsFilter(String value, TestEnum? test)
        {
            filter.Method = "not-equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(nEnumExpression, filter);
            IEnumerable expected = items.Where(model => model.NEnum != test);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("1", TestEnum.Second)]
        public void Apply_NotEqualsFilter(String value, TestEnum? test)
        {
            filter.Method = "not-equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(enumExpression, filter);
            IEnumerable expected = items.Where(model => model.Enum != test);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Method = "test";
            filter.Values = new[] { "0" };

            Assert.Null(filter.Apply(enumExpression.Body));
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringContainsFilterTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;

            Assert.Null(new StringContainsFilter { Method = "contains", Values = new[] { value } }.Apply(expression));
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            StringContainsFilter filter = new StringContainsFilter { Method = "contains", Values = new[] { "Est" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().Contains("EST"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersMultipleItems()
        {
            StringContainsFilter filter = new StringContainsFilter { Method = "contains", Values = new[] { "", "Est" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;

            Assert.Null(filter.Apply(expression));
        }
    }
}

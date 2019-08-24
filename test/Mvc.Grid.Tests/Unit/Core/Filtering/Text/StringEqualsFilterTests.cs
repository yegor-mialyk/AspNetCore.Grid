using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringEqualsFilterTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_FiltersEmptyOrNullValues(String value)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { value } };

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "" },
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name == null || model.Name == "");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new [] { "Test" } };

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TEST2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper() == "TEST");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersMultipleItems()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { "Test", "TES" } };

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "TES" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TEST2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name.ToUpper() == "TES" || model.Name.ToUpper() == "TEST"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }
    }
}

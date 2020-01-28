using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringNotEqualsFilterTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_FiltersNotEmptyAndNotNullValues(String value)
        {
            StringNotEqualsFilter filter = new StringNotEqualsFilter { Method = "not-equals", Values = new[] { value } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "" },
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name != "");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingOriginalCaseFilter()
        {
            StringNotEqualsFilter filter = new StringNotEqualsFilter { Method = "not-equals", Values = new[] { "test" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != "test");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingUpperCaseFilter()
        {
            StringNotEqualsFilter filter = new StringNotEqualsFilter { Method = "not-equals", Values = new[] { "test" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Upper;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name == null || model.Name.ToUpper() != "TEST");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingLowerCaseFilter()
        {
            StringNotEqualsFilter filter = new StringNotEqualsFilter { Method = "not-equals", Values = new[] { "TEST" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Lower;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name == null || model.Name.ToLower() != "test");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultiFilter()
        {
            StringNotEqualsFilter filter = new StringNotEqualsFilter { Method = "not-equals", Values = new[] { "test", "Test2" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != "test" && model.Name != "Test2");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }
    }
}

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
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { value } };
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

            IQueryable expected = items.Where(model => model.Name == null || model.Name == "");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingOriginalCaseFilter()
        {
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { "test" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name == "test");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingUpperCaseFilter()
        {
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { "test" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Upper;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper() == "TEST");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingLowerCaseFilter()
        {
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { "TEST" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Lower;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToLower() == "test");
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultiFilter()
        {
            StringEqualsFilter filter = new StringEqualsFilter { Method = "equals", Values = new[] { "test", "Test2" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name == "test" || model.Name == "Test2"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }
    }
}

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

            Assert.Null(new StringContainsFilter { Method = "contains", Values = new[] { value, "1" } }.Apply(expression));
        }

        [Fact]
        public void Apply_UsingOriginalCaseFilter()
        {
            StringContainsFilter filter = new StringContainsFilter { Method = "contains", Values = "es" };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.Contains("es"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingUpperCaseFilter()
        {
            StringContainsFilter filter = new StringContainsFilter { Method = "contains", Values = "es" };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Upper;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().Contains("ES"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingLowerCaseFilter()
        {
            StringContainsFilter filter = new StringContainsFilter { Method = "contains", Values = "ES" };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Lower;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToLower().Contains("es"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultiFilter()
        {
            StringContainsFilter filter = new StringContainsFilter { Method = "contains", Values = new[] { "Te", "es" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name.Contains("Te") || model.Name.Contains("es")));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringEndsWithFilterTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;

            Assert.Null(new StringEndsWithFilter { Method = "ends-with", Values = new[] { value, "1" } }.Apply(expression));
        }

        [Fact]
        public void Apply_UsingOriginalCaseFilter()
        {
            StringEndsWithFilter filter = new StringEndsWithFilter { Method = "ends-with", Values = "est" };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TESTEr" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.EndsWith("est"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingUpperCaseFilter()
        {
            StringEndsWithFilter filter = new StringEndsWithFilter { Method = "ends-with", Values = "est" };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Upper;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TESTEr" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().EndsWith("EST"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingLowerCaseFilter()
        {
            StringEndsWithFilter filter = new StringEndsWithFilter { Method = "ends-with", Values = "EST" };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Lower;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TESTEr" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToLower().EndsWith("est"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultiFilter()
        {
            StringEndsWithFilter filter = new StringEndsWithFilter { Method = "ends-with", Values = new[] { "t", "Er" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TESTEr" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name.EndsWith("t") || model.Name.EndsWith("Er")));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringEndsWithFilterTests
    {
        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            Assert.Null(new StringEndsWithFilter { Method = "ends-with", Values = value }.Apply(expression));
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            StringEndsWithFilter filter = new StringEndsWithFilter { Method = "ends-with", Values = "est" };
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().EndsWith("TEST"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersMultipleItems()
        {
            StringEndsWithFilter filter = new StringEndsWithFilter { Method = "ends-with", Values = new[] { "est", "ER" } };
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TESTEr" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name.ToUpper().EndsWith("TEST") || model.Name.ToUpper().EndsWith("ER")));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

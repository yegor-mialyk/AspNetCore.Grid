using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringStartsWithFilterTests
    {
        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            Assert.Null(new StringStartsWithFilter { Method = "starts-with", Values = value }.Apply(expression));
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            StringStartsWithFilter filter = new StringStartsWithFilter { Method = "starts-with", Values = "Test" };
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TTEST2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().StartsWith("TEST"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersMultipleItems()
        {
            StringStartsWithFilter filter = new StringStartsWithFilter { Method = "starts-with", Values = new[] { "Test", "tt" } };
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TTEST2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name.ToUpper().StartsWith("TT") || model.Name.ToUpper().StartsWith("TEST")));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

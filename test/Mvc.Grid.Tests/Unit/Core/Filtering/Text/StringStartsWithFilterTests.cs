using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringStartsWithFilterTests : BaseGridFilterTests
    {
        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;

            Assert.Null(new StringStartsWithFilter { Value = value }.Apply(expression));
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringStartsWithFilter filter = new StringStartsWithFilter();
            filter.Value = "Test";

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "TTEST2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().StartsWith("TEST"));
            IQueryable actual = Filter(items, filter.Apply(expression.Body), expression);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

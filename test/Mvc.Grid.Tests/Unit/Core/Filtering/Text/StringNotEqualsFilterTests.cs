using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Extensions;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringNotEqualsFilterTests : BaseGridFilterTests
    {
        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_FiltersNotEmptyAndNotNullValues(String value)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringNotEqualsFilter filter = new StringNotEqualsFilter();
            filter.Value = value;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "" },
                new GridModel { Name = null },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name != "");
            IQueryable actual = Filter(items, filter.Apply(expression.Body), expression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersItemsByIgnoringCase()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringNotEqualsFilter filter = new StringNotEqualsFilter();
            filter.Value = "Test";

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "" },
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "Test2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name == null || model.Name.ToUpper() != "TEST");
            IQueryable actual = Filter(items, filter.Apply(expression.Body), expression);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

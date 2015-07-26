using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringEndsWithFilterTests : BaseGridFilterTests
    {
        #region Method: Apply(Expression expression)

        [Fact]
        public void Apply_FiltersItemsWithCaseInsensitiveComparison()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            StringEndsWithFilter filter = new StringEndsWithFilter();
            filter.Value = "Test";

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "Test" },
                new GridModel { Name = "TEST2" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().EndsWith("TEST"));
            IQueryable actual = Filter(items, filter.Apply(expression.Body), expression);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class StringStartsWithFilterTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Apply_NullOrEmptyValue_ReturnsNull(String value)
        {
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;

            Assert.Null(new StringStartsWithFilter { Method = "starts-with", Values = new[] { value, "1" } }.Apply(expression));
        }

        [Fact]
        public void Apply_UsingOriginalCaseFilter()
        {
            StringStartsWithFilter filter = new StringStartsWithFilter { Method = "starts-with", Values = new[] { "tes" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "ttes" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TTESTE" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.StartsWith("tes"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingUpperCaseFilter()
        {
            StringStartsWithFilter filter = new StringStartsWithFilter { Method = "starts-with", Values = new[] { "tt" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Upper;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "ttes" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TTESTE" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToUpper().StartsWith("TT"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingLowerCaseFilter()
        {
            StringStartsWithFilter filter = new StringStartsWithFilter { Method = "starts-with", Values = new[] { "TE" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Lower;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "TEST" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TTESTE" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && model.Name.ToLower().StartsWith("te"));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultiFilter()
        {
            StringStartsWithFilter filter = new StringStartsWithFilter { Method = "starts-with", Values = new[] { "te", "TT" } };
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;
            filter.Case = GridFilterCase.Original;

            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = null },
                new GridModel { Name = "Tes" },
                new GridModel { Name = "test" },
                new GridModel { Name = "ttes" },
                new GridModel { Name = "TESTE" },
                new GridModel { Name = "TTESTE" }
            }.AsQueryable();

            IQueryable expected = items.Where(model => model.Name != null && (model.Name.StartsWith("te") || model.Name.StartsWith("TT")));
            IQueryable actual = items.Where(expression, filter);

            Assert.Equal(expected, actual);
        }
    }
}

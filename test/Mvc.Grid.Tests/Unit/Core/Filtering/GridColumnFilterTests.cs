using NSubstitute;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnFilterTests
    {
        private GridColumnFilter<GridModel> filter;
        private IQueryable<GridModel> items;

        public GridColumnFilterTests()
        {
            filter = new GridColumnFilter<GridModel>();
            filter.Column = Substitute.For<IGridColumn<GridModel>>();

            items = new[]
            {
                new GridModel { Name = "AA", NSum = 10, Sum = 40 },
                new GridModel { Name = "BB", NSum = 20, Sum = 30 },
                new GridModel { Name = null, NSum = 30, Sum = 20 },
                new GridModel { Name = "CC", NSum = null, Sum = 10 }
            }.AsQueryable();
        }

        #region Process(IQueryable<T> items)

        [Fact]
        public void Process_NoFilters_ReturnsSameItems()
        {
            filter.First = null;
            filter.Second = null;

            IQueryable actual = filter.Process(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_NullAppliedFilter_ReturnsSameItems()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Column.Expression.Returns(expression);

            filter.Second = Substitute.For<IGridFilter>();
            filter.First = Substitute.For<IGridFilter>();

            IQueryable actual = filter.Process(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Prcoess_OnNullSecondFilterAndNullFirstFiltersExpressionReturnsSameItems()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Column.Expression.Returns(expression);

            filter.First = Substitute.For<IGridFilter>();
            filter.Second = null;

            IQueryable actual = filter.Process(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Prcoess_OnNullFirstFilterAndNullSecondFiltersExpressionReturnsSameItems()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Column.Expression.Returns(expression);

            filter.Second = Substitute.For<IGridFilter>();
            filter.First = null;

            IQueryable actual = filter.Process(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Process_UsingAndOperator()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Second = new StringContainsFilter { Value = "A" };
            filter.First = new StringContainsFilter { Value = "AA" };
            filter.Column.Expression.Returns(expression);
            filter.Operator = "And";

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("AA") && item.Name.Contains("A"));
            IQueryable actual = filter.Process(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_UsingOrOperator()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Second = new StringContainsFilter { Value = "A" };
            filter.First = new StringContainsFilter { Value = "BB" };
            filter.Column.Expression.Returns(expression);
            filter.Operator = "Or";

            IQueryable expected = items.Where(item => item.Name != null && (item.Name.Contains("A") || item.Name.Contains("BB")));
            IQueryable actual = filter.Process(items);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("or")]
        [InlineData("and")]
        public void Process_OnInvalidOperatorUsesOnlyFirstFilter(String op)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Second = new StringContainsFilter { Value = "A" };
            filter.First = new StringContainsFilter { Value = "BB" };
            filter.Column.Expression.Returns(expression);
            filter.Operator = op;

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("BB"));
            IQueryable actual = filter.Process(items);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("or")]
        [InlineData("and")]
        public void Process_OnInvalidOperatorAndFirstFilterNullUsesSecondFilter(String op)
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            filter.Second = new StringContainsFilter { Value = "A" };
            filter.Column.Expression.Returns(expression);
            filter.Operator = op;
            filter.First = null;

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("A"));
            IQueryable actual = filter.Process(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Process_FiltersNullableExpressions()
        {
            Expression<Func<GridModel, Int32?>> expression = (model) => model.NSum;
            filter.Second = new Int32Filter { Type = "GreaterThan", Value = "25" };
            filter.First = new Int32Filter { Type = "Equals", Value = "10" };
            filter.Column.Expression.Returns(expression);
            filter.Operator = "Or";

            IQueryable expected = items.Where(item => item.NSum == 10 || item.NSum > 25);
            IQueryable actual = filter.Process(items);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

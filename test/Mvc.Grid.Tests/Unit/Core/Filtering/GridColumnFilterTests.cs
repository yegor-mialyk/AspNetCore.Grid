using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnFilterTests
    {
        private GridColumnFilter<GridModel, String> filter;
        private IQueryable<GridModel> items;

        public GridColumnFilterTests()
        {
            Grid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);
            GridColumn<GridModel, String> column = new GridColumn<GridModel, String>(grid, model => model.Name);

            filter = new GridColumnFilter<GridModel, String>(column);
            filter.IsEnabled = true;

            items = new[]
            {
                new GridModel { Name = "AA", NSum = 10, Sum = 40 },
                new GridModel { Name = "BB", NSum = 20, Sum = 30 },
                new GridModel { Name = null, NSum = 30, Sum = 20 },
                new GridModel { Name = "CC", NSum = null, Sum = 10 }
            }.AsQueryable();
        }

        #region GridColumnFilter(IGridColumn<T, TValue> column)

        [Fact]
        public void GridColumnFilter_SetsColumn()
        {
            IGridColumn<GridModel, String> expected = new GridColumn<GridModel, String>(filter.Column.Grid, model => model.Name);
            IGridColumn<GridModel, String> actual = new GridColumnFilter<GridModel, String>(expected).Column;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumnFilter_NotMemberExpression_IsNotEnabled()
        {
            IGridColumn<GridModel, String> column = new GridColumn<GridModel, String>(filter.Column.Grid, model => model.ToString());

            Assert.False(new GridColumnFilter<GridModel, String>(column).IsEnabled);
        }

        [Fact]
        public void GridColumnFilter_MemberExpression_IsEnabledNull()
        {
            IGridColumn<GridModel, String> column = new GridColumn<GridModel, String>(filter.Column.Grid, model => model.Name);

            Assert.Null(new GridColumnFilter<GridModel, String>(column).IsEnabled);
        }

        #endregion

        #region Apply(IQueryable<T> items)

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        public void Apply_NotEnabled_ReturnsSameItems(Boolean? isEnabled)
        {
            filter.IsMulti = true;
            filter.IsEnabled = isEnabled;
            filter.First = new StringContainsFilter { Value = "A" };

            IQueryable actual = filter.Apply(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_NoFilters_ReturnsSameItems()
        {
            filter.First = null;
            filter.Second = null;
            filter.IsMulti = true;
            filter.IsEnabled = true;

            IQueryable actual = filter.Apply(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_NullAppliedFilter_ReturnsSameItems()
        {
            filter.IsMulti = true;
            filter.Operator = "Or";
            filter.First = Substitute.For<IGridFilter>();
            filter.Second = Substitute.For<IGridFilter>();

            IQueryable actual = filter.Apply(items);
            IQueryable expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_UsingAndOperator()
        {
            filter.IsMulti = true;
            filter.Operator = "And";
            filter.First = new StringContainsFilter { Value = "A" };
            filter.Second = new StringContainsFilter { Value = "AA" };

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("A") && item.Name.Contains("AA"));
            IQueryable actual = filter.Apply(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingOrOperator()
        {
            filter.IsMulti = true;
            filter.Operator = "Or";
            filter.First = new StringContainsFilter { Value = "A" };
            filter.Second = new StringContainsFilter { Value = "BB" };

            IQueryable expected = items.Where(item => item.Name != null && (item.Name.Contains("A") || item.Name.Contains("BB")));
            IQueryable actual = filter.Apply(items);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("or")]
        [InlineData("and")]
        public void Apply_InvalidOperator_FirstFilter(String op)
        {
            filter.Operator = op;
            filter.IsMulti = true;
            filter.First = new StringContainsFilter { Value = "A" };
            filter.Second = new StringContainsFilter { Value = "BB" };

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("A"));
            IQueryable actual = filter.Apply(items);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("or")]
        [InlineData("and")]
        public void Apply_InvalidOperator_SecondFilter(String op)
        {
            filter.Operator = op;
            filter.IsMulti = true;
            filter.First = Substitute.For<IGridFilter>();
            filter.Second = new StringContainsFilter { Value = "A" };

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("A"));
            IQueryable actual = filter.Apply(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FirstFilter()
        {
            filter.IsMulti = false;
            filter.Operator = "Or";
            filter.First = new StringContainsFilter { Value = "A" };
            filter.Second = new StringContainsFilter { Value = "BB" };

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("A"));
            IQueryable actual = filter.Apply(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_SecondFilter()
        {
            filter.IsMulti = false;
            filter.Operator = "Or";
            filter.First = Substitute.For<IGridFilter>();
            filter.Second = new StringContainsFilter { Value = "BB" };

            IQueryable expected = items.Where(item => item.Name != null && item.Name.Contains("BB"));
            IQueryable actual = filter.Apply(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_FiltersByExpressions()
        {
            GridColumn<GridModel, Int32?> testColumn = new GridColumn<GridModel, Int32?>(new Grid<GridModel>(new GridModel[0]), model => model.NSum);
            GridColumnFilter<GridModel, Int32?> testFilter = new GridColumnFilter<GridModel, Int32?>(testColumn);
            testFilter.Second = new Int32Filter { Type = "GreaterThan", Value = "25" };
            testFilter.First = new Int32Filter { Type = "Equals", Value = "10" };
            testFilter.IsEnabled = true;
            testFilter.Operator = "Or";
            testFilter.IsMulti = true;

            IQueryable expected = items.Where(item => item.NSum == 10 || item.NSum > 25);
            IQueryable actual = testFilter.Apply(items);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

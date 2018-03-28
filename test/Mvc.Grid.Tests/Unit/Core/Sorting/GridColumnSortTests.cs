using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnSortTests
    {
        private GridColumnSort<GridModel, Object> sort;

        public GridColumnSortTests()
        {
            IGrid<GridModel> grid = new Grid<GridModel>(new GridModel[0]);
            sort = new GridColumnSort<GridModel, Object>(new GridColumn<GridModel, Object>(grid, model => model.Name));
        }

        #region Order

        [Fact]
        public void Order_Set_Caches()
        {
            sort.Column.Grid.Query = HttpUtility.ParseQueryString("Sort=Name&Order=Asc");

            sort.Order = null;

            Assert.Null(sort.Order);
        }

        [Theory]
        [InlineData("", "Sort=Name&Order=", "Name", GridSortOrder.Desc, null)]
        [InlineData("", "Order=Desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("", "Sort=Name&Order=Asc", "Name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData("", "Sort=Name&Order=Desc", "Name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData(null, "Sort=Name&Order=", "Name", GridSortOrder.Desc, null)]
        [InlineData(null, "Order=Desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData(null, "Sort=Name&Order=Asc", "Name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData(null, "Sort=Name&Order=Desc", "Name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("Grid", "Grid-Sort=Name&Grid-Order=", "Name", GridSortOrder.Desc, null)]
        [InlineData("Grid", "Grid-Order=Desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("Grid", "Grid-Sort=Name&Grid-Order=Asc", "Name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData("Grid", "Grid-Sort=Name&Grid-Order=Desc", "Name", GridSortOrder.Asc, GridSortOrder.Desc)]
        public void Order_ReturnsFromQuery(String gridName, String query, String name, GridSortOrder? initialOrder, GridSortOrder? order)
        {
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(query);
            sort.Column.Grid.Name = gridName;
            sort.InitialOrder = initialOrder;
            sort.Column.Name = name;

            GridSortOrder? actual = sort.Order;
            GridSortOrder? expected = order;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "Sort=Name&Order=", "Sort=Name&Order=Desc")]
        [InlineData("", "Sort=Name&Order=Asc", "Sort=Name&Order=Desc")]
        [InlineData("", "Sort=Name&Order=Desc", "Sort=Name&Order=Asc")]
        [InlineData(null, "Sort=Name&Order=", "Sort=Name&Order=Desc")]
        [InlineData(null, "Sort=Name&Order=Asc", "Sort=Name&Order=Desc")]
        [InlineData(null, "Sort=Name&Order=Desc", "Sort=Name&Order=Asc")]
        [InlineData("Grid", "Grid-Sort=Name&Grid-Order=", "Grid-Sort=Name&Grid-Order=Desc")]
        [InlineData("Grid", "Grid-Sort=Name&Grid-Order=Asc", "Grid-Sort=Name&Grid-Order=Desc")]
        [InlineData("Grid", "Grid-Sort=Name&Grid-Order=Desc", "Grid-Sort=Name&Grid-Order=Asc")]
        public void Order_Get_Caches(String gridName, String initialQuery, String changedQuery)
        {
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(initialQuery);
            sort.Column.Grid.Name = gridName;

            GridSortOrder? order = sort.Order;

            sort.Column.Grid.Query = HttpUtility.ParseQueryString(changedQuery);

            GridSortOrder? actual = sort.Order;
            GridSortOrder? expected = order;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "-Order=Desc", "", GridSortOrder.Asc)]
        [InlineData("", "e-Sort=Name&Grid-Order=Asc", "Name", GridSortOrder.Asc)]
        [InlineData("", "R-Sort=Name&Grid-Order=Desc", "Name", GridSortOrder.Desc)]
        [InlineData(null, "-Order=Desc", "", GridSortOrder.Asc)]
        [InlineData(null, "e-Sort=Name&Grid-Order=Asc", "Name", GridSortOrder.Asc)]
        [InlineData(null, "R-Sort=Name&Grid-Order=Desc", "Name", GridSortOrder.Desc)]
        [InlineData("Grid", "Grid-Order=Desc", "", GridSortOrder.Asc)]
        [InlineData("Grid", "Gride-Sort=Name&Grid-Order=Asc", "Name", GridSortOrder.Asc)]
        [InlineData("Grid", "RGrid-Sort=Name&Grid-Order=Desc", "Name", GridSortOrder.Desc)]
        public void Order_NotFound_ReturnsInitialSortOrder(String gridName, String query, String name, GridSortOrder? initialOrder)
        {
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(query);
            sort.Column.Grid.Name = gridName;
            sort.InitialOrder = initialOrder;
            sort.Column.Name = name;

            GridSortOrder? actual = sort.Order;
            GridSortOrder? expected = initialOrder;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GridColumnSort<GridModel, Object>(IGridColumn<T, TValue> column)

        [Fact]
        public void GridColumnSort_SetsColumn()
        {
            IGridColumn<GridModel, String> expected = new GridColumn<GridModel, String>(null, model => model.Name);
            IGridColumn<GridModel, String> actual = new GridColumnSort<GridModel, String>(expected).Column;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumnSort_NotMemberExpression_IsNotEnabled()
        {
            IGridColumn<GridModel, String> column = new GridColumn<GridModel, String>(null, model => model.ToString());

            Assert.False(new GridColumnSort<GridModel, String>(column).IsEnabled);
        }

        [Fact]
        public void GridColumnSort_MemberExpression_IsEnabledNull()
        {
            IGridColumn<GridModel, String> column = new GridColumn<GridModel, String>(null, model => model.Name);

            Assert.Null(new GridColumnSort<GridModel, String>(column).IsEnabled);
        }

        #endregion

        #region Apply(IQueryable<T> items)

        [Fact]
        public void Apply_NoOrder_ReturnsSameItems()
        {
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();
            sort.IsEnabled = true;
            sort.Order = null;

            IQueryable<GridModel> actual = sort.Apply(items);
            IQueryable<GridModel> expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_NotEnabled_ReturnsSameItems()
        {
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();
            sort.Order = GridSortOrder.Desc;
            sort.IsEnabled = false;

            IQueryable<GridModel> actual = sort.Apply(items);
            IQueryable<GridModel> expected = items;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_ReturnsItemsSortedInAscendingOrder()
        {
            IQueryable<GridModel> items = new[] { new GridModel { Name = "B" }, new GridModel { Name = "A" } }.AsQueryable();
            sort.Order = GridSortOrder.Asc;
            sort.IsEnabled = true;

            IEnumerable<GridModel> expected = items.OrderBy(model => model.Name);
            IEnumerable<GridModel> actual = sort.Apply(items.AsQueryable());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_ReturnsItemsSortedInDescendingOrder()
        {
            IQueryable<GridModel> items = new[] { new GridModel { Name = "B" }, new GridModel { Name = "A" } }.AsQueryable();
            sort.Order = GridSortOrder.Desc;
            sort.IsEnabled = true;

            IEnumerable<GridModel> expected = items.OrderByDescending(model => model.Name);
            IEnumerable<GridModel> actual = sort.Apply(items.AsQueryable());

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

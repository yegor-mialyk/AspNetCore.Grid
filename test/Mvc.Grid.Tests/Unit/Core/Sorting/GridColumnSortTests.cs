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
            GridColumn<GridModel, Object> column = new GridColumn<GridModel, Object>(grid, model => model.Name);

            sort = new GridColumnSort<GridModel, Object>(column) { IsEnabled = true };
        }

        #region Order

        [Fact]
        public void Order_Set_Caches()
        {
            sort.Column.Grid.Query = HttpUtility.ParseQueryString("sort=name&order=asc");

            sort.Order = null;

            Assert.Null(sort.Order);
        }

        [Theory]
        [InlineData("", "sort=name&order=", "name", GridSortOrder.Desc, null)]
        [InlineData("", "order=desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("", "sort=name&order=asc", "name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData("", "sort=name&order=desc", "name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("", "SORT=NAME&ORDER=DESC", "name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData(null, "sort=name&order=", "name", GridSortOrder.Desc, null)]
        [InlineData(null, "order=desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData(null, "sort=name&order=asc", "name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData(null, "sort=name&order=desc", "name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData(null, "SORT=NAME&ORDER=DESC", "name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("grid", "grid-sort=name&grid-order=", "name", GridSortOrder.Desc, null)]
        [InlineData("grid", "grid-order=desc", null, GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("grid", "grid-sort=name&grid-order=asc", "name", GridSortOrder.Desc, GridSortOrder.Asc)]
        [InlineData("grid", "grid-sort=name&grid-order=desc", "name", GridSortOrder.Asc, GridSortOrder.Desc)]
        [InlineData("grid", "GRID-SORT=NAME&GRID-ORDER=DESC", "name", GridSortOrder.Asc, GridSortOrder.Desc)]
        public void Order_ReturnsFromQuery(String gridName, String query, String name, GridSortOrder? initialOrder, GridSortOrder? order)
        {
            sort.Column.Name = name;
            sort.InitialOrder = initialOrder;
            sort.Column.Grid.Name = gridName;
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(query);

            GridSortOrder? actual = sort.Order;
            GridSortOrder? expected = order;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Order_Get_Disabled()
        {
            sort.IsEnabled = false;
            sort.Column.Grid.Query = HttpUtility.ParseQueryString("sort=name&order=asc");

            Assert.Null(sort.Order);
        }

        [Theory]
        [InlineData("", "sort=name&order=", "sort=name&order=desc")]
        [InlineData("", "sort=name&order=asc", "sort=name&order=desc")]
        [InlineData("", "sort=name&order=desc", "sort=name&order=asc")]
        [InlineData(null, "sort=name&order=", "sort=name&order=desc")]
        [InlineData(null, "sort=name&order=asc", "sort=name&order=desc")]
        [InlineData(null, "sort=name&order=desc", "sort=name&order=asc")]
        [InlineData("grid", "grid-sort=name&grid-order=", "grid-sort=name&grid-order=desc")]
        [InlineData("grid", "grid-sort=name&grid-order=asc", "grid-sort=name&grid-order=desc")]
        [InlineData("grid", "grid-sort=name&grid-order=desc", "grid-sort=name&grid-order=asc")]
        public void Order_Get_Caches(String name, String initialQuery, String changedQuery)
        {
            sort.Column.Grid.Name = name;
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(initialQuery);

            GridSortOrder? order = sort.Order;

            sort.Column.Grid.Query = HttpUtility.ParseQueryString(changedQuery);

            GridSortOrder? actual = sort.Order;
            GridSortOrder? expected = order;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "order=", "", GridSortOrder.Asc)]
        [InlineData("", "-order=desc", "", GridSortOrder.Asc)]
        [InlineData("", "e-sort=name&grid-order=asc", "name", GridSortOrder.Asc)]
        [InlineData("", "r-sort=name&grid-order=desc", "name", GridSortOrder.Desc)]
        [InlineData(null, "order=", "", GridSortOrder.Asc)]
        [InlineData(null, "-order=desc", "", GridSortOrder.Asc)]
        [InlineData(null, "e-sort=name&grid-order=asc", "name", GridSortOrder.Asc)]
        [InlineData(null, "r-sort=name&grid-order=desc", "name", GridSortOrder.Desc)]
        [InlineData("grid", "grid-order=", "", GridSortOrder.Asc)]
        [InlineData("grid", "grid-order=desc", "", GridSortOrder.Asc)]
        [InlineData("grid", "gride-sort=name&grid-order=asc", "name", GridSortOrder.Asc)]
        [InlineData("grid", "rgrid-sort=name&grid-order=desc", "name", GridSortOrder.Desc)]
        public void Order_NotFound_ReturnsInitialSortOrder(String gridName, String query, String name, GridSortOrder? initialOrder)
        {
            sort.Column.Name = name;
            sort.InitialOrder = initialOrder;
            sort.Column.Grid.Name = gridName;
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(query);

            GridSortOrder? expected = initialOrder;
            GridSortOrder? actual = sort.Order;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "sort=test&order=test")]
        [InlineData(null, "sort=test&order=test")]
        [InlineData("grid", "grid-sort=test&grid-order=test")]
        public void Order_OtherColumn_ReturnsNull(String name, String query)
        {
            sort.Column.Name = "this";
            sort.Column.Grid.Name = name;
            sort.InitialOrder = GridSortOrder.Asc;
            sort.Column.Grid.Query = HttpUtility.ParseQueryString(query);

            Assert.Null(sort.Order);
        }

        #endregion

        #region GridColumnSort<GridModel, Object>(IGridColumn<T, TValue> column)

        [Fact]
        public void GridColumnSort_SetsColumn()
        {
            IGridColumn<GridModel, String> expected = new GridColumn<GridModel, String>(sort.Column.Grid, model => model.Name);
            IGridColumn<GridModel, String> actual = new GridColumnSort<GridModel, String>(expected).Column;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumnSort_NotMemberExpression_IsNotEnabled()
        {
            IGridColumn<GridModel, String> column = new GridColumn<GridModel, String>(sort.Column.Grid, model => model.ToString());

            Assert.False(new GridColumnSort<GridModel, String>(column).IsEnabled);
        }

        [Fact]
        public void GridColumnSort_MemberExpression_IsEnabledNull()
        {
            IGridColumn<GridModel, String> column = new GridColumn<GridModel, String>(sort.Column.Grid, model => model.Name);

            Assert.Null(new GridColumnSort<GridModel, String>(column).IsEnabled);
        }

        #endregion

        #region Apply(IQueryable<T> items)

        [Fact]
        public void Apply_NoOrder_ReturnsSameItems()
        {
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();
            sort.Order = null;

            Object expected = items;
            Object actual = sort.Apply(items);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_NotEnabled_ReturnsSameItems()
        {
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();
            sort.Order = GridSortOrder.Desc;
            sort.IsEnabled = false;

            Object expected = items;
            Object actual = sort.Apply(items);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Apply_ReturnsItemsSortedInAscendingOrder()
        {
            IQueryable<GridModel> items = new[] { new GridModel { Name = "b" }, new GridModel { Name = "a" } }.AsQueryable();
            sort.Order = GridSortOrder.Asc;

            IEnumerable<GridModel> expected = items.OrderBy(model => model.Name);
            IEnumerable<GridModel> actual = sort.Apply(items.AsQueryable());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_ReturnsItemsSortedInDescendingOrder()
        {
            IQueryable<GridModel> items = new[] { new GridModel { Name = "b" }, new GridModel { Name = "a" } }.AsQueryable();
            sort.Order = GridSortOrder.Desc;

            IEnumerable<GridModel> expected = items.OrderByDescending(model => model.Name);
            IEnumerable<GridModel> actual = sort.Apply(items.AsQueryable());

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}

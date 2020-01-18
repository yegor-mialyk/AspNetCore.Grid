using System;
using System.Linq;
using NSubstitute;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnSortTests
    {
        private GridColumnSort<GridModel, Object?> sort;

        public GridColumnSortTests()
        {
            IGrid<GridModel> grid = new Grid<GridModel>(Array.Empty<GridModel>());
            GridColumn<GridModel, Object?> column = new GridColumn<GridModel, Object?>(grid, model => model.Name);

            sort = new GridColumnSort<GridModel, Object?>(column) { IsEnabled = true };
        }

        [Fact]
        public void Index_ReturnsFromGridSort()
        {
            sort.Column.Grid.Sort = Substitute.For<IGridSort<GridModel>>();
            sort.Column.Grid.Sort[sort.Column].Returns((2, GridSortOrder.Desc));

            Int32? actual = sort.Index;
            Int32? expected = 2;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Order_ReturnsFromGridSort()
        {
            sort.Column.Grid.Sort = Substitute.For<IGridSort<GridModel>>();
            sort.Column.Grid.Sort[sort.Column].Returns((0, GridSortOrder.Desc));

            GridSortOrder? expected = GridSortOrder.Desc;
            GridSortOrder? actual = sort.Order;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GridColumnSort_SetsColumn()
        {
            IGridColumn<GridModel, String?> expected = new GridColumn<GridModel, String?>(sort.Column.Grid, model => model.Name);
            IGridColumn<GridModel, String?> actual = new GridColumnSort<GridModel, String?>(expected).Column;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GridColumnSort_NotMemberExpression_IsNotEnabled()
        {
            IGridColumn<GridModel, String?> column = new GridColumn<GridModel, String?>(sort.Column.Grid, model => model.ToString());

            Assert.False(new GridColumnSort<GridModel, String?>(column).IsEnabled);
        }

        [Fact]
        public void GridColumnSort_MemberExpression_IsEnabledNull()
        {
            IGridColumn<GridModel, String?> column = new GridColumn<GridModel, String?>(sort.Column.Grid, model => model.Name);

            Assert.Null(new GridColumnSort<GridModel, String?>(column).IsEnabled);
        }

        [Fact]
        public void GridColumnSort_SetsFirstOrder()
        {
            GridSortOrder actual = new GridColumnSort<GridModel, Object?>(sort.Column).FirstOrder;
            GridSortOrder expected = GridSortOrder.Asc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void By_ReturnsSameItems()
        {
            IQueryable<GridModel> items = new GridModel[2].AsQueryable();
            sort.IsEnabled = false;

            Object expected = items;
            Object actual = sort.By(items);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void By_AscendingOrder()
        {
            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "b" },
                new GridModel { Name = "a" }
            }.AsQueryable();

            sort.Column.Grid.Sort = Substitute.For<IGridSort<GridModel>>();
            sort.Column.Grid.Sort[sort.Column].Returns((0, GridSortOrder.Asc));

            IQueryable<GridModel> expected = items.OrderBy(sort.Column.Expression);
            IQueryable<GridModel> actual = sort.By(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void By_DescendingOrder()
        {
            IQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "a" },
                new GridModel { Name = "b" }
            }.AsQueryable();

            sort.Column.Grid.Sort = Substitute.For<IGridSort<GridModel>>();
            sort.Column.Grid.Sort[sort.Column].Returns((0, GridSortOrder.Desc));

            IQueryable<GridModel> expected = items.OrderByDescending(sort.Column.Expression);
            IQueryable<GridModel> actual = sort.By(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThenBy_ReturnsSameItems()
        {
            IOrderedQueryable<GridModel> items = new GridModel[2].AsQueryable().OrderBy(item => item.ShortText);
            sort.IsEnabled = false;

            Object expected = items;
            Object actual = sort.ThenBy(items);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void ThenBy_AscendingOrder()
        {
            IOrderedQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "b", ShortText = "c" },
                new GridModel { Name = "a", ShortText = "c" }
            }.AsQueryable().OrderBy(item => item.ShortText);

            sort.Column.Grid.Sort = Substitute.For<IGridSort<GridModel>>();
            sort.Column.Grid.Sort[sort.Column].Returns((1, GridSortOrder.Asc));

            IQueryable<GridModel> expected = items.ThenBy(item => item.Name);
            IQueryable<GridModel> actual = sort.ThenBy(items);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThenBy_DescendingOrder()
        {
            IOrderedQueryable<GridModel> items = new[]
            {
                new GridModel { Name = "a", ShortText = "c" },
                new GridModel { Name = "b", ShortText = "c" }
            }.AsQueryable().OrderBy(item => item.ShortText);

            sort.Column.Grid.Sort = Substitute.For<IGridSort<GridModel>>();
            sort.Column.Grid.Sort[sort.Column].Returns((1, GridSortOrder.Desc));

            IQueryable<GridModel> expected = items.ThenByDescending(item => item.Name);
            IQueryable<GridModel> actual = sort.ThenBy(items);

            Assert.Equal(expected, actual);
        }
    }
}

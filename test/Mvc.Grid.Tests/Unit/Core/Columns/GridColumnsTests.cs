using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnsTests
    {
        private GridColumns<GridModel> columns;

        public GridColumnsTests()
        {
            columns = new GridColumns<GridModel>(new Grid<GridModel>(new GridModel[0]));
        }

        #region GridColumns(IGrid<T> grid)

        [Fact]
        public void GridColumns_SetsGrid()
        {
            Object actual = new GridColumns<GridModel>(columns.Grid).Grid;
            Object expected = columns.Grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Add()

        [Fact]
        public void Add_GridColumn()
        {
            columns.Add();

            GridColumn<GridModel, Object> expected = new GridColumn<GridModel, Object>(columns.Grid, model => null);
            GridColumn<GridModel, Object> actual = columns.Single() as GridColumn<GridModel, Object>;

            Assert.Equal(expected.Filter.IsEnabled, actual.Filter.IsEnabled);
            Assert.Equal(expected.Title.ToString(), actual.Title.ToString());
            Assert.Equal(expected.Sort.IsEnabled, actual.Sort.IsEnabled);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.Filter.Type, actual.Filter.Type);
            Assert.Equal(expected.Filter.Name, actual.Filter.Name);
            Assert.Null(actual.Expression.Compile().Invoke(null));
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.Sort.Order, actual.Sort.Order);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        #endregion

        #region Add<TValue>(Expression<Func<T, TValue>> expression)

        [Fact]
        public void Add_Expression_GridColumn()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            columns.Add(expression);

            GridColumn<GridModel, String> expected = new GridColumn<GridModel, String>(columns.Grid, expression);
            GridColumn<GridModel, String> actual = columns.Single() as GridColumn<GridModel, String>;

            Assert.Equal(expected.Filter.IsEnabled, actual.Filter.IsEnabled);
            Assert.Equal(expected.Title.ToString(), actual.Title.ToString());
            Assert.Equal(expected.Sort.IsEnabled, actual.Sort.IsEnabled);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.Filter.Type, actual.Filter.Type);
            Assert.Equal(expected.Filter.Name, actual.Filter.Name);
            Assert.Equal(expected.Expression, actual.Expression);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.Sort.Order, actual.Sort.Order);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Add_GridColumnProcessor()
        {
            columns.Add(model => model.Name);

            Object expected = columns.Single();
            Object actual = columns.Grid.Processors.Single();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Add_ReturnsAddedColumn()
        {
            Object actual = columns.Add(model => model.Name);
            Object expected = columns.Single();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Insert(Int32 index)

        [Fact]
        public void Insert_GridColumn()
        {
            columns.Add(model => 0);
            columns.Insert(0, model => 1);

            GridColumn<GridModel, Int32> expected = new GridColumn<GridModel, Int32>(columns.Grid, model => 1);
            GridColumn<GridModel, Int32> actual = columns.First() as GridColumn<GridModel, Int32>;

            Assert.Equal(expected.Filter.IsEnabled, actual.Filter.IsEnabled);
            Assert.Equal(expected.Title.ToString(), actual.Title.ToString());
            Assert.Equal(expected.Sort.IsEnabled, actual.Sort.IsEnabled);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(1, actual.Expression.Compile().Invoke(null));
            Assert.Equal(expected.Filter.Type, actual.Filter.Type);
            Assert.Equal(expected.Filter.Name, actual.Filter.Name);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.Sort.Order, actual.Sort.Order);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        #endregion

        #region Insert<TValue>(Int32 index, Expression<Func<T, TValue>> expression)

        [Fact]
        public void Insert_Expression_GridColumn()
        {
            Expression<Func<GridModel, Int32>> expression = (model) => model.Sum;
            columns.Add(model => model.Name);
            columns.Insert(0, expression);

            GridColumn<GridModel, Int32> expected = new GridColumn<GridModel, Int32>(columns.Grid, expression);
            GridColumn<GridModel, Int32> actual = columns.First() as GridColumn<GridModel, Int32>;

            Assert.Equal(expected.Filter.IsEnabled, actual.Filter.IsEnabled);
            Assert.Equal(expected.Title.ToString(), actual.Title.ToString());
            Assert.Equal(expected.Sort.IsEnabled, actual.Sort.IsEnabled);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.Filter.Type, actual.Filter.Type);
            Assert.Equal(expected.Filter.Name, actual.Filter.Name);
            Assert.Equal(expected.Expression, actual.Expression);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.Sort.Order, actual.Sort.Order);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Insert_GridColumnProcessor()
        {
            columns.Insert(0, model => model.Name);

            Object actual = columns.Grid.Processors.Single();
            Object expected = columns.Single();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Insert_ReturnsInsertedColumn()
        {
            Object actual = columns.Insert(0, model => model.Name);
            Object expected = columns.Single();

            Assert.Same(expected, actual);
        }

        #endregion
    }
}

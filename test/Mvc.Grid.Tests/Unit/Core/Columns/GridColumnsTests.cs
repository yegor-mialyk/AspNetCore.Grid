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
            columns = new GridColumns<GridModel>(new Grid<GridModel>(Array.Empty<GridModel>()));
        }

        [Fact]
        public void GridColumns_SetsGrid()
        {
            Object actual = new GridColumns<GridModel>(columns.Grid).Grid;
            Object expected = columns.Grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Add_GridColumn()
        {
            IGridColumn<GridModel, Object> expected = new GridColumn<GridModel, Object>(columns.Grid, model => "");
            IGridColumn<GridModel, Object> actual = columns.Add();

            Assert.Equal("", actual.Expression.Compile().Invoke(new GridModel()));
            Assert.Equal(expected.Filter.IsEnabled, actual.Filter.IsEnabled);
            Assert.Equal(expected.Title.ToString(), actual.Title.ToString());
            Assert.Equal(expected.Sort.IsEnabled, actual.Sort.IsEnabled);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.Filter.Type, actual.Filter.Type);
            Assert.Equal(expected.Filter.Name, actual.Filter.Name);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.Sort.Order, actual.Sort.Order);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Add_Expression_GridColumn()
        {
            Expression<Func<GridModel, String?>> expression = (model) => model.Name;

            IGridColumn<GridModel, String?> expected = new GridColumn<GridModel, String?>(columns.Grid, expression);
            IGridColumn<GridModel, String?> actual = columns.Add(expression);

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

        [Fact]
        public void Insert_GridColumn()
        {
            columns.Add(model => 0);

            IGridColumn<GridModel, Int32> expected = new GridColumn<GridModel, Int32>(columns.Grid, model => 1);
            IGridColumn<GridModel, Object> actual = columns.Insert(0);

            Assert.Equal("", actual.Expression.Compile().Invoke(new GridModel()));
            Assert.Equal(expected.Filter.IsEnabled, actual.Filter.IsEnabled);
            Assert.Equal(expected.Title.ToString(), actual.Title.ToString());
            Assert.Equal(expected.Sort.IsEnabled, actual.Sort.IsEnabled);
            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.Filter.Type, actual.Filter.Type);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.Sort.Order, actual.Sort.Order);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
            Assert.Empty(actual.Filter.Name);
        }

        [Fact]
        public void Insert_Expression_GridColumn()
        {
            Expression<Func<GridModel, Int32>> expression = (model) => model.Sum;
            columns.Add(model => model.Name);

            IGridColumn<GridModel, Int32> expected = new GridColumn<GridModel, Int32>(columns.Grid, expression);
            IGridColumn<GridModel, Int32> actual = columns.Insert(0, expression);

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
    }
}
